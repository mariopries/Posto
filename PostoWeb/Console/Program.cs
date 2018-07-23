using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostoWeb;

namespace TesteConsole
{
    class Program
    {
        static void Main(string[] args)
        {


            #region Variaveis dos testes

            string returnText;
            bool valido;

            #endregion

            #region Criptografia
            /*
                Console.WriteLine("----Inicio--Criptografia----");

                string secretKey = "1f352c073b6108d72d9810a30914dff4";

                string decryptedText;
                string encryptedText;
                
                decryptedText = "Este texto está criptografado";
                returnText =  Criptografia.Encrypt(decryptedText, secretKey);
                Console.WriteLine(returnText);
                Console.ReadKey();
            
            //testes aqui
                encryptedText = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPFNEVENvbnRhc1JlY2ViZXJSZXN1bWlkbyB4bWxucz0iUG9zdG9XZWIiPgoJPENhck5ybz40Nzk3MjA8L0Nhck5ybz4KCTxDbGlDb2Q+MDwvQ2xpQ29kPgoJPFRjYkNvZD4wPC9UY2JDb2Q+Cgk8QmFuQ29kPjA8L0JhbkNvZD4KCTxDYXJWYWxFc3RpbWFkbz4gICAgICA3LjIwPC9DYXJWYWxFc3RpbWFkbz4KCTxDYXJWYWxSZWNlYmlkbz4gICAgICA3LjIwPC9DYXJWYWxSZWNlYmlkbz4KCTxDYXJWYWxEZXNjb250bz4gICAgICAwLjAwPC9DYXJWYWxEZXNjb250bz4KCTxDYXJWYWxKdXJvPiAgICAgIDAuMDA8L0NhclZhbEp1cm8+Cgk8Q2FyVmFsT3V0QWNyZXNjaW1vcz4gICAgICAwLjAwPC9DYXJWYWxPdXRBY3Jlc2NpbW9zPgoJPENhckluZEF0aXZvPlRSVUU8L0NhckluZEF0aXZvPgoJPENhck9icz5HZXJhZG8gcGVsbyBQQUYtRUNGPC9DYXJPYnM+Cgk8Q2FyTnJvRG9jQ29icmFuY2E+MjMzNzg8L0Nhck5yb0RvY0NvYnJhbmNhPgoJPENhck5yb0RvY1NlcXVlbmNpYT4wPC9DYXJOcm9Eb2NTZXF1ZW5jaWE+Cgk8Q2FySW5kQmFpeGFkbz5UUlVFPC9DYXJJbmRCYWl4YWRvPgoJPENhckluZEltcERvY0NvYnJhbmNhPkZBTFNFPC9DYXJJbmRJbXBEb2NDb2JyYW5jYT4KCTxDYXJJbmRJbXBGYXR1cmE+RkFMU0U8L0NhckluZEltcEZhdHVyYT4KCTxDYXJEZXNFZmVCYWl4YT4gICAgICAwLjAwPC9DYXJEZXNFZmVCYWl4YT4KCTxDYXJDYWl4YT45NDU3PC9DYXJDYWl4YT4KCTxDYXJGYXRGb2lTb21TYWxDbGllbnRlPlRSVUU8L0NhckZhdEZvaVNvbVNhbENsaWVudGU+Cgk8Q2FyTm9zc29Ocm8vPgoJPENhclZhbERlc1BhcmFCb2xldG8+ICAgICAgICAwLjAwPC9DYXJWYWxEZXNQYXJhQm9sZXRvPgoJPENhclRpdEluY29icmF2ZWw+RkFMU0U8L0NhclRpdEluY29icmF2ZWw+Cgk8Q2FyTnJvTmZpUmVmZXJlbmNpYWRhPjA8L0Nhck5yb05maVJlZmVyZW5jaWFkYT4KCTxDYXJOZmlUbmZDb2Q+MDwvQ2FyTmZpVG5mQ29kPgoJPENhck5maVNldGFkYT5OPC9DYXJOZmlTZXRhZGE+Cgk8Q2FyVmFsVGF4Q2FydGFvPiAgICAgMC4wMDwvQ2FyVmFsVGF4Q2FydGFvPgoJPENhclBlclRheENhcnRhbz4gMC4wMDwvQ2FyUGVyVGF4Q2FydGFvPgoJPENhckNwZ0NvZD4xPC9DYXJDcGdDb2Q+Cgk8Q2FyVGlwQ2FydGFvLz4KCTxDYXJOcm9WZW5kYT4xOTExODcwPC9DYXJOcm9WZW5kYT4KCTxDYXJQbGFjYS8+Cgk8Q2FyQ2xpTW90Q29kPjA8L0NhckNsaU1vdENvZD4KCTxDYXJOcm9GZWNGaW5hbmNlaXJvPjA8L0Nhck5yb0ZlY0ZpbmFuY2Vpcm8+Cgk8Q2FyTnJvUmVtZXNzYT4wPC9DYXJOcm9SZW1lc3NhPgoJPENhclRpcENEQz5GQUxTRTwvQ2FyVGlwQ0RDPgoJPENhck5yb1ZpbkNEQz4wPC9DYXJOcm9WaW5DREM+Cgk8Q2FyU2Vzc2FvSWQ+MDwvQ2FyU2Vzc2FvSWQ+Cgk8Q2FyVGVmTnJvQXV0b3JpemFjYW8vPgoJPENhck5yb1BhcmNlbGFzPjA8L0Nhck5yb1BhcmNlbGFzPgoJPENhclBhcmNlbGFBdHVhbD4gICAwPC9DYXJQYXJjZWxhQXR1YWw+CjwvU0RUQ29udGFzUmVjZWJlclJlc3VtaWRvPgo=";
                returnText = Criptografia.Decrypt(encryptedText, secretKey);
                Console.WriteLine(returnText);
                Console.ReadKey();

                Console.WriteLine("----Fim-----Criptografia----");
            */
            #endregion

            #region Base64
            /*
                Console.WriteLine("----Inicio--Base64----");
                //returnText = Base64Convert.convertStringToBase64("este é um texto normal que foi encodado");
                //Console.WriteLine(returnText);
                returnText = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iSVNPLTg4NTktMSI/Pgo8U0RUUmFtb0F0aXZpZGFkZT4KCTxSYW1Db2Q+MTwvUmFtQ29kPgoJPFJhbVdlYi8+Cgk8UmFtRGVzPlNFUlZJx09TPC9SYW1EZXM+Cgk8UmFtVGltZVN0YW1wLz4KPC9TRFRSYW1vQXRpdmlkYWRlPgo=";
                returnText = Base64Convert.convertBase64ToString(returnText);
                Console.WriteLine(returnText);
                Console.WriteLine("----Fim-----Base64----");
                Console.ReadKey();
            */
            #endregion

            #region Inscrição Estadual
            /*
                Console.WriteLine("----Inicio--Insc.Estadual----");
                valido = Util.ValidarInscricaoEstadual("SC", "255771576");
                Console.WriteLine(valido);
                Console.WriteLine("----Fim-----Insc.Estadual----");
                Console.ReadKey();
            */
            #endregion

            #region Envio de Email
            
            Console.WriteLine("----Inicio--Envio de Email----"); // 
            returnText = PostoWeb.Email.SendEmail("douglas.method@gmail.com", "", "douglasdossantos22@gmail.com", "", "",
                "Teste de email", "<b> Hi there</b>", true, "http://srvev3/PostoWebdesenvolvimento/arltr001.aspx?1,%3C%3fxml+version+%3d+%221.0%22+encoding+%3d+%22ISO-8859-1%22%3f%3E%0a%0d%0a%3Ctitulos%3E%0a%09%3Ctitulo%3E8113%3C%2ftitulo%3E%0a%3C%2ftitulos%3E%0a", 
                "Boleto.pdf", "smtp.gmail.com", 587, true, true);

            Console.WriteLine(returnText);
            Console.WriteLine("----Fim-----Envio de Email----");
            Console.ReadKey();
            
            #endregion
        }
    }
}
