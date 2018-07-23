namespace PostoWeb
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Net;
    using System.Collections;
    public class Email
    {
        public static string SendEmail(string Sender, string EmailPassword, string To, string CC, string CCO, string Subject, string Body, bool IsBodyHtml, string Url, string FileName, string Host, int Port, bool EnableSSL, bool EnableTLS)
        {
            try
            {
                // Specify the file to be attached and sent.
                // Create a message and set up the recipients.

                MailMessage message = new MailMessage(Sender, To, Subject, Body);
                message.IsBodyHtml = IsBodyHtml;

                if (!string.IsNullOrEmpty(CC))
                {
                    message.CC.Add(CC);
                }

                if (!string.IsNullOrEmpty(CCO))
                {
                    message.Bcc.Add(CCO);
                }

                if (!string.IsNullOrEmpty(Url))
                {
                    var stream = new WebClient().OpenRead(Url);
                    Attachment data = new Attachment(stream, MediaTypeNames.Application.Pdf);

                    data.Name = FileName;

                    message.Attachments.Add(data);
                }

                if (EnableTLS) {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }
                

                //Send the message.
                SmtpClient client = new SmtpClient();
                client.Host = Host;
                client.EnableSsl = EnableSSL;
                
                

                client.Credentials = new NetworkCredential(Sender, EmailPassword);
                client.Port = Port;

                client.Send(message);
                return "E-mail enviado com sucesso";

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}