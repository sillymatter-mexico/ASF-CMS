using System;
using System.Collections.Generic;
using asf.cms.model;
using asf.cms.dal;
using log4net;
using Newtonsoft.Json;

namespace asf.cms.bll
{
    public enum ModificationType
    {
        CREATE,
        MODIFY,
        DELETE,
        RESTORE
    }

    public enum ModificationTargetType
    {
        SECTION,
        PUBLICATION
    }

    public class ModificationLog
    {
        private static ILog Log = LogManager.GetLogger("ModificationLog");

        private ModificationLogVO modificationLog = new ModificationLogVO();
        private List<ModificationLogField> fieldChanges = new List<ModificationLogField>();

        private ModificationLog(ModificationLogVO log)
        {
            modificationLog = log;
            fieldChanges = String.IsNullOrEmpty(log.FieldChanges) ? fieldChanges : ModificationLogField.ListFromJson(log.FieldChanges);
        }

        #region Accessors
        public int Id
        {
            get { return modificationLog.Id; }
        }

        public ModificationType Type
        {
            get { return StringToModificationType(modificationLog.Type); }
        }

        public ModificationTargetType TargetType
        {
            get { return StringToModificationTargetType(modificationLog.TargetType); }
        }

        public int TargetId
        {
            get { return modificationLog.TargetId; }
        }

        public string UserName
        {
            get { return modificationLog.UserName; }
        }

        public DateTime Created
        {
            get { return modificationLog.Created; }
        }

        public string Permalink
        {
            get { return modificationLog.Permalink; }
        }

        public List<ModificationLogField> FieldChanges
        {
            get { return fieldChanges; }
        }

        public int HistoricId
        {
            get { return modificationLog.HistoricId; }
        }
        #endregion

        public string TypeReadable()
        {
            switch(Type)
            {
                case ModificationType.CREATE:
                    return "creado";
                case ModificationType.DELETE:
                    return "eliminado";
                case ModificationType.MODIFY:
                    return "modificado";
                case ModificationType.RESTORE:
                    return "restaurado";
                default:
                    return "acción desconocida (" + Type.ToString() + ")";
            }
        }

        public string TargetTypeReadable()
        {
            switch(TargetType)
            {
                case ModificationTargetType.PUBLICATION:
                    return "publicación";
                case ModificationTargetType.SECTION:
                    return "sección";
                default:
                    return "objetivo desconocido (" + TargetType.ToString() + ")";
            }
        }

        public static bool AddSectionRegistry(ModificationType type, SectionVO target, UserVO user, SectionVO oldSection = null)
        {
            List<ModificationLogField> delta = null;

            if (type == ModificationType.MODIFY && oldSection != null)
            {
                delta = CreateSectionDelta(target, oldSection);
            }

            return AddRegistry(ModificationTargetType.SECTION, type, target.Id, target.Permalink, user.Username);
        }

        public static bool AddSectionRegistry(ModificationType type, Section target, UserVO user, Section oldSection = null)
        {
            List<ModificationLogField> delta = null;

            if (type == ModificationType.MODIFY && oldSection != null)
            {
                delta = CreateSectionDelta(target, oldSection);
            }

            return AddRegistry(ModificationTargetType.SECTION, type, target.SectionId, target.Permalink, user.Username, delta);
        }

        public static bool AddPublicationRegistry(ModificationType type, PublicationVO target, UserVO user, bool useHistoric = true)
        {
            return AddPublicationRegistry(type, target, user, null, useHistoric);
        }

        public static bool AddPublicationRegistry(ModificationType type, PublicationVO target, UserVO user, PublicationVO oldPublication, bool useHistoric = true)
        {
            List<ModificationLogField> delta = null;
            int? recoveryId = null;

            if (type == ModificationType.MODIFY)
            {
                if (oldPublication == null && useHistoric)
                    oldPublication = GetLatestHistoricPublication(target.Id);
                else if (oldPublication == null)
                    oldPublication = GetCurrentPublication(target.Id);
                if (oldPublication != null)
                    delta = CreatePublicationDelta(target, oldPublication);
            }
            if (type == ModificationType.RESTORE)
            {
                if (oldPublication == null)
                {
                    Log.Error("Error al crear registro de recuperación de publicación. No se especifico la publicación histórica.");
                    return false;
                }
                recoveryId = oldPublication.Id;
            }

            return AddRegistry(ModificationTargetType.PUBLICATION, type, target.Id, target.Permalink, user.Username, delta, recoveryId);
        }

        public static bool AddPublicationRegistry(ModificationType type, int publicationId, string username, bool useHistoric = true)
        {
            PublicationDAL pDAL = new PublicationDAL();
            PublicationVO pvo = pDAL.GetById(publicationId),
                          oldPvo = null;
            List<ModificationLogField> delta = null;

            if (type == ModificationType.MODIFY)
            {
                if (useHistoric)
                    oldPvo = GetLatestHistoricPublication(publicationId);
                else
                    oldPvo = GetCurrentPublication(publicationId);
                delta = CreatePublicationDelta(pvo, oldPvo);
            }

            return AddRegistry(ModificationTargetType.PUBLICATION, type, publicationId, pvo.Permalink, username);
        }

        public static bool AddPublicationRegistry(ModificationType type, EspecialesVO target, UserVO user, EspecialesVO oldPublication = null)
        {
            List<ModificationLogField> delta = null;
            if (type == ModificationType.MODIFY && oldPublication != null)
            {
                delta = CreatePublicationDelta(target, oldPublication);
            }

            return AddRegistry(ModificationTargetType.PUBLICATION, type, target.Id, target.Permalink, user.Username, delta);
        }

        protected static bool AddRegistry(ModificationTargetType targetType, ModificationType type, int targetId, string permalink, string username, List<ModificationLogField> delta = null, int? recoverId = null)
        {
            ModificationLogVO mod = new ModificationLogVO();

            mod.Created = DateTime.Now;

            mod.UserName = username;

            mod.TargetType = targetType.ToString();
            mod.TargetId = targetId;
            mod.Permalink = permalink;

            mod.Type = type.ToString();

            mod.FieldChanges = delta == null ? null : ModificationLogField.ListToJson(delta);
            mod.HistoricId = recoverId == null ? 0 : (int)recoverId;

            try
            {
                ModificationLogDAL mDAL = new ModificationLogDAL();
                mod = mDAL.Insert(mod);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error al crear el registro de modification.", ex);
                return false;
            }
        }

        protected static List<ModificationLogField> CreateSectionDelta(SectionVO current, SectionVO previous)
        {
            string[] properties = new string[] { "IsMain", "RedirectTo", "Permalink", "Created", "ParentSectionId", "Updated", "Position", "RedirectOptions", "SitemapExclude", "Active", "CssClass" };
            return CreateDelta(current, previous, properties);
        }

        protected static List<ModificationLogField> CreateSectionDelta(Section current, Section previous)
        {
            string[] properties = new string[] { "IsMain", "RedirectTo", "Permalink", "Created", "ParentSectionId", "Updated", "Position", "RedirectOptions", "SitemapExclude", "Active", "CssClass" };
            return CreateDelta(current, previous, properties);
        }

        protected static List<ModificationLogField> CreatePublicationDelta(PublicationVO current, PublicationVO previous)
        {
            string[] properties = new string[] { "SectionId", "LanguageId", "Title", "Published", "IsMain", "Content", "Unpublished", "Status", "NewsContent", "Position", "Active", "Meta", "SitemapExclude", "CssClass" };
            List<ModificationLogField> delta = CreateDelta(current, previous, properties);

            return delta;
        }

        protected static List<ModificationLogField> CreatePublicationDelta(EspecialesVO current, EspecialesVO previous)
        {
            string[] properties = new string[] { "SectionId", "LanguageId", "Title", "Published", "IsMain", "Content", "Unpublished", "Status", "NewsContent", "Position", "Active", "Meta", "SitemapExclude", "CssClass" };
            List<ModificationLogField> delta = CreateDelta(current, previous, properties);

            return delta;
        }

        protected static List<ModificationLogField> CreateDelta(object current, object previous, string[] properties)
        {
            if (current.GetType() != previous.GetType())
                throw new ArgumentException("Cannot create delta for objects of different types.");

            List<ModificationLogField> delta = new List<ModificationLogField>();
            Type type = current.GetType();

            foreach (string p in properties)
            {
                object cValue = type.GetProperty(p).GetValue(current, null),
                       pValue = type.GetProperty(p).GetValue(previous, null);
                if (cValue != null && pValue != null)
                {
                    if (!cValue.Equals(pValue))
                        delta.Add(new ModificationLogField(p, pValue, cValue, cValue.GetType().Name));
                } else
                {
                    if (cValue != null && cValue != pValue)
                        delta.Add(new ModificationLogField(p, pValue, cValue, cValue.GetType().Name));
                }
            }

            return delta;
        }

        protected static PublicationVO GetLatestHistoricPublication(int publicationId)
        {
            PublicationDAL pDAL = new PublicationDAL();
            IList<PublicationVO> historic = pDAL.GetHistoric(publicationId, 1);
            if (historic.Count > 0)
                return historic[0];
            else
                return null;
        }

        protected static PublicationVO GetCurrentPublication(int publicationId)
        {
            PublicationDAL pDAL = new PublicationDAL();
            return pDAL.GetById(publicationId);
        }

        public static List<ModificationLog> GetAll(DateTime start, DateTime end)
        {
            ModificationLogDAL mlDAL = new ModificationLogDAL();
            List<ModificationLogVO> mlVOS = mlDAL.ListAll();
            List<ModificationLog> ml = new List<ModificationLog>();
            foreach (ModificationLogVO mlVO in mlVOS)
                ml.Add(new ModificationLog(mlVO));
            return ml;
        }

        public static List<ModificationLog> Filter(DateTime start, DateTime end, string type, string target)
        {
            ModificationLogDAL mlDAL = new ModificationLogDAL();
            List<ModificationLogVO> mlVOS = new List<ModificationLogVO>();

            if (type == null && target == null)
                mlVOS = mlDAL.List(start, end);
            else if (type != null && target != null)
                mlVOS = mlDAL.GetByTypeAndTarget(type, target, start, end);
            else if (type != null)
                mlVOS = mlDAL.GetByType(type, start, end);
            else if (target != null)
                mlVOS = mlDAL.GetByTarget(target, start, end);

            List<ModificationLog> ml = new List<ModificationLog>();
            foreach (ModificationLogVO mlVO in mlVOS)
                ml.Add(new ModificationLog(mlVO));
            return ml;
        }

        public static string ListToJson(List<ModificationLogVO> list)
        {
            return JsonConvert.SerializeObject(list);
        }

        private static ModificationType StringToModificationType(string type)
        {
            if(ModificationType.CREATE.ToString() == type)
                return ModificationType.CREATE;
            if(ModificationType.DELETE.ToString() == type)
                return ModificationType.DELETE;
            if (ModificationType.RESTORE.ToString() == type)
                return ModificationType.RESTORE;
            if(ModificationType.MODIFY.ToString() == type)
                return ModificationType.MODIFY;

            throw new Exception("Tipo de modificación invalido.");
        }

        private static ModificationTargetType StringToModificationTargetType(string targetType)
        {
            if (ModificationTargetType.PUBLICATION.ToString() == targetType)
                return ModificationTargetType.PUBLICATION;
            if (ModificationTargetType.SECTION.ToString() == targetType)
                return ModificationTargetType.SECTION;
            throw new Exception("Tipo de objetivo de modificación invalido.");
        }
    }
}