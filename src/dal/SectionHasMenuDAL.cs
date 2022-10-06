using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;
using asf.cms.exception;

namespace asf.cms.dal
{
    public class SectionHasMenuDAL:DAL<SectionHasMenuVO>
    {
        public SectionHasMenuVO GetById(String menuKey, int sectionId)
        {
            try
            {
                SectionHasMenuIdVO shmidvo = new SectionHasMenuIdVO();
                shmidvo.MenuKey = menuKey;
                shmidvo.SectionId = sectionId;
                return this.GetById(shmidvo);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<SectionHasMenuVO> GetByMenu(String menuKey)      
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("menukey", ExpresionStringHelper.replaceEscapeCharacter(menuKey));
                return this.list("select shm from SectionHasMenuVO as shm where shm.id.MenuKey=:menukey order by shm.Position", param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public List<SectionHasMenuVO> UpdateMenu(string menuKey, List<SectionHasMenuVO> items)
        {
            try
            {
                return this.UpdateList(items, "delete SectionHasMenuVO shm where shm.id.MenuKey='" + ExpresionStringHelper.replaceEscapeCharacter(menuKey) + "'");
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }
    }
}
