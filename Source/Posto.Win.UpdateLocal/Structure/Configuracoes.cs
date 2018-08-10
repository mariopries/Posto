using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Posto.Win.UpdateLocal.Estrutura
{
    [XmlRoot("ConfiguracoesXml")]
    public class ConfiguracoesXml
    {
        private const string Folder = "Config";
        private const string File = "Config.xml";

        /// <summary>
        /// Diretório local da instalação do posto
        /// </summary>
        [XmlElement("Local")]
        public string Local { get; set; }

        /// <summary>
        /// Diretório server da instalação do posto
        /// </summary>
        [XmlElement("Servidor")]
        public string Servidor { get; set; }

        /// <summary>
        /// Carrega as informações do xml
        /// </summary>
        /// <returns>Retorna o arquivo de configuração</returns>
        public static ConfiguracoesXml CarregarConfiguracao()
        {
            if (!Directory.Exists(Folder)) //Se o diretório não existir...
            {
                //Criamos um com o nome folder
                Directory.CreateDirectory(Folder);
            }

            string path = string.Format("{0}/{1}/{2}", Environment.CurrentDirectory, Folder, File);
            StreamReader sR = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfiguracoesXml));
                sR = new StreamReader(path);
                ConfiguracoesXml config = (ConfiguracoesXml)serializer.Deserialize(sR);
                sR.Close();

                return config;
            }
            catch (Exception)
            {
                if (sR != null)
                {
                    sR.Close();
                }

                ConfiguracoesXml nova = new ConfiguracoesXml();

                nova.GravarConfiguracao();
                return nova;
            }
        }

        /// <summary>
        /// Grava o arquivo de configuração
        /// </summary>
        public void GravarConfiguracao()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfiguracoesXml));
            string path = string.Format("{0}/{1}/{2}", Environment.CurrentDirectory, Folder, File);
            StreamWriter sW = new StreamWriter(path);
            serializer.Serialize(sW, this);
            sW.Close();
        }
    }

    public class Configuracoes
    {
        private string _local;
        private string _servidor;

        public string Local
        {
            get { return _local; }
            set { if (_local != value) { _local = value; } }
        }
        public string Servidor
        {
            get { return _servidor; }
            set { if (_servidor != value) { _servidor = value; } }
        }
    }

}
