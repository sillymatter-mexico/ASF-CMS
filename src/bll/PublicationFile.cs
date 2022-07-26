using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.dal;
using asf.cms.util;
using System.IO;
namespace asf.cms.bll
{
    public class PublicationFile
    {
        FileVO file;
        public PublicationFile(string permalink, string name, string path)
        {
            file = new FileVO();
            file.Name = name;
            file.Path = path ;
            file.Mime = Encoder.getMime(Path.GetExtension(name));
   //         File.AppendAllText("h:\\proyectos\\einsNull\\cmsDemo\\cmsDemo\\uploads\\x.txt","\n"+ file.Name + "|" + file.Mime + "|" + file.Path + "|" + file.PublicationId);
        }
        public bool Insert()
        {
            PublicationVO pvo = Publication.GetByPermalink(HttpContext.Current.Session["permalink"].ToString());
            FileDAL fdal = new FileDAL();
            if (pvo != null)
            {
                file.PublicationId = pvo.Id;
                file.Title = pvo.Title;
            }
            fdal.Insert(file);
            return true;
        }
        public bool CopyTo(PublicationFile to)
        {
            PublicationVO pvo = Publication.GetByPermalink(HttpContext.Current.Session["permalink"].ToString());
            FileDAL fdal = new FileDAL();
            if (pvo != null)
            {
                to.file.PublicationId = pvo.Id;
                to.file.Title = pvo.Title;
            }
            fdal.Insert(to.file);

            return true;
        }
        public bool RenameTo(PublicationFile to)
        {
            FileDAL fdal = new FileDAL();
            FileVO fvo = fdal.getByPath(file.Path);
            fvo.Name = to.file.Name;
            fvo.Mime = to.file.Mime;
            fvo.Path = to.file.Path;
            fdal.Update(fvo);
            return true;
        }
        public bool Delete()
        {
            FileDAL fdal = new FileDAL();
            fdal.Delete(fdal.getByPath(file.Path));
            return true;
        }
        public bool DeleteDir()
        {
            return true;
        }
        public bool Update()
        {
            return true;
        }
        public static int UpdateTitle(int fileId,string title)
        {
            FileDAL fdal=new FileDAL();
            return fdal.UpdateTitle(fileId,title);
        }

        public static bool SetMain(int publicationId, int fileId)
        {
            FileDAL fdal = new FileDAL();
            List<FileVO> files = Publication.GetFiles(publicationId);
            int mainFileId = -1;

            foreach(FileVO f in files)
            {
                if (f.IsMain == 1)
                    mainFileId = f.Id;
            }

            foreach (FileVO f in files)
            {
                if (f.IsMain == 1)
                {
                    f.IsMain = 0;
                    fdal.Update(f);
                }
                if(f.Id == fileId && fileId != mainFileId)
                {
                    f.IsMain = 1;
                    fdal.Update(f);
                }
            }
            return true;
        }
    }
}
