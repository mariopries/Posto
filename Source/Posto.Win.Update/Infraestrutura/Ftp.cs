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
        public string Url = "ftp.methodinformatica.com.br";

        public string Usuario = "methodinformatica";

        public string Senha = "method172202";

        public string Local;

        public Ftp() 
        {
            this.Local = ConfigurationManager.AppSettings["LocalPosto"];
        }

        public string[] GetFileList(string path)
        {
            string[] downloadFiles;
            StringBuilder result    = new StringBuilder();
            WebResponse response    = null;
            StreamReader reader     = null;

            try
            {
                FtpWebRequest reqFTP;
                reqFTP             = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + Url + "/" + path));
                reqFTP.UseBinary   = true;
                reqFTP.Credentials = new NetworkCredential(Usuario, Senha);
                reqFTP.Method      = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy       = null;
                reqFTP.KeepAlive   = false;
                reqFTP.UsePassive  = false;
                response           = reqFTP.GetResponse();
                reader             = new StreamReader(response.GetResponseStream());
                string line        = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
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


        public byte[] Download(string path)
        {            
            byte[] buffer = new byte[2048];
            int read;


            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + Url + "/" + path);
            request.Credentials = new NetworkCredential(this.Usuario, this.Senha);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            using (MemoryStream ms = new MemoryStream())
            {
                Stream ftpStream = request.GetResponse().GetResponseStream();

                while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }

            return buffer;
        }
    }
}
