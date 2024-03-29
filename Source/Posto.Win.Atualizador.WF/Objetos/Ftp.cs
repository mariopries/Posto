﻿using Atualizador.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace Atualizador.Objetos
{
    public class Ftp
    {
        #region Gerenciador de log

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constantes

        private const string Url = "ftp.methodinformatica.com.br";

        private const string Usuario = "methodinformatica";

        private const string Senha = "Meth0d@172202";

        #endregion

        #region Variaveis

        private string Local;

        private long FileSize;
        private DateTime FileModified;

        #endregion

        #region Propriedades

        private TelaPrincipal _telaPrincipal;
        private AtualizarModel _atualizarmodel;

        #endregion

        #region Construtor
        public Ftp()
        {
            Local = ConfigurationManager.AppSettings["LocalPosto"];
        }
        #endregion

        #region Objetos
        public TelaPrincipal TelaPrincipal
        {
            get { return _telaPrincipal; }
            set { if (_telaPrincipal != value) { _telaPrincipal = value; } }
        }
        public AtualizarModel AtualizarModel
        {
            get { return _atualizarmodel; }
            set { if (_atualizarmodel != value) { _atualizarmodel = value; } }
        }
        #endregion

        #region Funçoes

        public string[] GetFileList(string path)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                HttpWebRequest.DefaultCachePolicy = policy;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + Url + "/" + path));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(Usuario, Senha);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.CachePolicy = noCachePolicy;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = true;
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
                HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                HttpWebRequest.DefaultCachePolicy = policy;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + Url + "/" + path);
                request.Proxy = null;
                request.Credentials = new NetworkCredential(Usuario, Senha);
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                request.CachePolicy = noCachePolicy;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                FileSize = response.ContentLength;
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            return FileSize;
        }

        public DateTime GetFileLasModified(string path)
        {
            try
            {
                HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                HttpWebRequest.DefaultCachePolicy = policy;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + Url + "/" + path);
                request.Proxy = null;
                request.Credentials = new NetworkCredential(Usuario, Senha);
                request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                request.CachePolicy = noCachePolicy;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                FileModified = response.LastModified;
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            return FileModified;
        }
        public byte[] Download(string path, TelaPrincipal telaPrincipal)
        {
            TelaPrincipal = telaPrincipal;
            AtualizarModel = TelaPrincipal.AbaAtualizacao.AtualizarModel;

            byte[] buffer = new byte[32 * 1024];
            int read;
            try
            {
                GetFileSize(path);

                HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                HttpWebRequest.DefaultCachePolicy = policy;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + Url + "/" + path);
                request.Credentials = new NetworkCredential(Usuario, Senha);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.CachePolicy = noCachePolicy;

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
                            TelaPrincipal.AbaAtualizacao.LabelContent = "Finalizando o download...";
                        }
                        else
                        {
                            TelaPrincipal.AbaAtualizacao.ProgressoBarra1 = Math.Min((int)Porcentagem, 100);
                            TelaPrincipal.AbaAtualizacao.LabelContent = "Baixando aquivos... ( " + (Porcentagem / 100).ToString("P1") + " )";
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

        #endregion
    }
}
