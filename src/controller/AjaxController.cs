using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.bll;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Configuration;

namespace asf.cms.controller
{
    public class AjaxController : Controller
    {
        protected static string MSG_BASE = "Mensaje de forma de contacto ASF CMS\n----\nDE: {0}\nMENSAJE:\n{1}\n----";  

        public AjaxController(HttpContext Context) : base(Context)
        {
        }

        public void SendMail()
        {
            SiteContent sc = new SiteContent();
            try
            {
                int charLimit = 100;
                int.TryParse(ConfigurationManager.AppSettings.Get("SMTPUserCharLimit"), out charLimit);

                string from = Request["messageFormFrom"];
                string message = Request["messageFormMessage"];
                string to = Request["messageTo"];

                MailAddress m = new MailAddress(from);

                if (String.IsNullOrEmpty(message))
                    throw new Exception("Mensaje vacio.");

                message = Regex.Replace(message, "<.*?>", String.Empty);

                if (message.Length > charLimit)
                    throw new Exception("Mensaje demasiado largo.");

                if (sc.SendContactFormEmail(string.Format(MSG_BASE, from, message), to))
                    SendJSON("{\"msg\":\"Correo enviado correctamente\", \"result\": \"true\" }");
                else
                    throw new Exception("Error al enviar correo");
            }
            catch (Exception e)
            {
                SendJSON("{\"msg\":\"" + e.Message +  "\", \"result\": \"false\"}");
            }
        }

        public void IncreaseLinkCounter()
        {
            string urlString = Request["accessUrl"];
            PublicationLinkAccess.IncreaseLinkCounter(urlString);

            SendJSON("{\"msg\":\"OK\", \"result\": \"true\"}");
        }
    }
}