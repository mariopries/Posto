using log4net;
using Posto.Win.Update.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Posto.Win.Update.Infraestrutura
{
    public class Ftp
    {
        #region Gerenciador de log

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constantes

        private const string Url = "ftp.methodinformatica.com.br";

        private const string Usuario = "methodinformatica";

        private const string Senha = "method172202";

        #endregion

        #region Variaveis

        private string Local;

        private long FileSize;

        #endregion

        #region Propriedades

        private AtualizarModel _atualizar;

        #endregion

        public Ftp()
        {
            Local = ConfigurationManager.AppSettings["LocalPosto"];
        }

        public string[] GetFileList(string path)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + Url + "/" + path));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(Usuario, Senha);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception e)
            {
                log.Error(e);
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                downloadFiles = null;
                return downloadFiles;
            }
        }

        public long GetFileSize(string path)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + Url + "/" + path);
                request.Proxy = null;
                request.Credentials = new NetworkCredential(Usuario, Senha);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                FileSize = response.ContentLength;
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            return FileSize;
        }

        public byte[] Download(string path, AtualizarModel atualizar)
        {

            _atualizar = atualizar;
            byte[] buffer = new byte[2048];
            int read;
            try
            {
                GetFileSize(path);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + Url + "/" + path);
                request.Credentials = new NetworkCredential(Usuario, Senha);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                using (MemoryStream ms = new MemoryStream())
                {
                    Stream ftpStream = request.GetResponse().GetResponseStream();
                    var Progresso = 0;
                    while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);

                        Progresso += read;
                        var Porcentagem = ((double)Progresso / FileSize) * 100;

                        if (Porcentagem > 100)
                        {
                            _atualizar.MensagemStatus = "Finalizando o download...";
                        }
                        else
                        {
                            _atualizar.MensagemStatus = "Baixando aquivos... ( " + (Porcentagem / 100).ToString("P1") + " )";
                        }

                    }
                    return ms.ToArray();
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            return buffer;
        }
    }
}
