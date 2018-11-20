using Method;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Atualizador
{
    [XmlRoot("Configuracao")]
    public class ConfiguracaoXml
    {
        private const string Folder = "cfg";
        private const string File = "Atualizador.xml";
        private const string Key = "BG6NMtwakaZzehJhBg3cRTmCySPhUdFW";

        public ConfiguracaoXml()
        {
            Porta = "5432";
        }

        /// <summary>
        /// Servidor a ser configurado
        /// </summary>
        [XmlElement("Servidor")]
        public string Servidor { get; set; }

        /// <summary>
        /// Porta a ser configurada
        /// </summary>
        [XmlElement("Porta")]
        public string Porta { get; set; }

        /// <summary>
        /// Banco a ser configurado
        /// </summary>
        [XmlElement("Banco")]
        public string Banco { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [XmlElement("Usuario")]
        public string Usuario { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [XmlElement("Senha")]
        public string Senha { get; set; }

        /// <summary>
        /// Local do diretorio que serão extraidos os arquivos
        /// </summary>
        [XmlElement("LocalDiretorio")]
        public string LocalDiretorio { get; set; }

        /// <summary>
        /// Leitor de bombas
        /// </summary>
        [XmlElement("LeitorBomba")]
        public bool LeitorBomba { get; set; }

        /// <summary>
        /// Posto web
        /// </summary>
        [XmlElement("PostoWeb")]
        public bool PostoWeb { get; set; }

        /// <summary>
        /// Carrega as informações do xml
        /// </summary>
        /// <returns>Retorna o arquivo de configuração</returns>
        public static ConfiguracaoXml CarregarConfiguracao()
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
                XmlSerializer serializer = new XmlSerializer(typeof(ConfiguracaoXml));
                sR = new StreamReader(path);
                ConfiguracaoXml config = (ConfiguracaoXml)serializer.Deserialize(sR);
                sR.Close();
                config.Senha = Criptografia.Decrypt(config.Senha, Key);
                return config;
            }
            catch (Exception)
            {
                if (sR != null)
                {
                    sR.Close();
                }

                ConfiguracaoXml nova = new ConfiguracaoXml();

                nova.GravarConfiguracao();
                return nova;
            }
        }

        /// <summary>
        /// Grava o arquivo de configuração
        /// </summary>
        public void GravarConfiguracao()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfiguracaoXml));
            string path = string.Format("{0}/{1}/{2}", Environment.CurrentDirectory, Folder, File);
            StreamWriter sW = new StreamWriter(path);
            if (Senha != null)
            {
                Senha = Criptografia.Encrypt(Senha, Key);
            }
            serializer.Serialize(sW, this);
            sW.Close();
        }
    }
}
