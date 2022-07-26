using asf.cms.dal;
using asf.cms.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace asf.cms.bll
{
    public class PublicationLinkAccess
    {
        public static void IncreaseLinkCounter(string url)
        {
            try
            {
                Regex scheme = new Regex("^https?://.*");

                if (url[0] == '/')
                    url = "https://www.asf.gob.mx" + url;
                if (!scheme.IsMatch(url))
                    url = "https://" + url;

                Uri uri = new Uri(url);
                DateTime current = DateTime.Now.Date;

                PublicationLinkAccessDAL plaDAL = new PublicationLinkAccessDAL();

                PublicationLinkAccessIdVO linkAccessId = new PublicationLinkAccessIdVO();
                linkAccessId.AccessDate = current;
                linkAccessId.AccessUrl = (ConfigurationManager.AppSettings.Get("LinkCaptureUseScheme") == "true" ? uri.Scheme + "://" : "") +
                    uri.Host + 
                    (uri.IsDefaultPort ? "" : ":" + uri.Port) + 
                    uri.AbsolutePath +
                    (ConfigurationManager.AppSettings.Get("LinkCaptureUseParams") == "true" ? uri.Query : "");

                PublicationLinkAccessVO linkAccess = plaDAL.GetById(linkAccessId);

                if(linkAccess != null)
                {
                    linkAccess.HitCount++;
                    plaDAL.Update(linkAccess);
                }
                else
                {
                    linkAccess = new PublicationLinkAccessVO();
                    linkAccess.PublicationLinkAccessId = linkAccessId;
                    linkAccess.HitCount = 1;
                    plaDAL.Insert(linkAccess);
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("PublicationController").Error("Error procesing link counter", ex);
            }
        }

        public static IList<PublicationLinkAccessResultVO> GetPublicationLinkAccessResults()
        {
            PublicationLinkAccessDAL plaDAL = new PublicationLinkAccessDAL();
            return plaDAL.GetPublicationLinkAccessResults();
        }
    }
}