using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace asf.cms.dal
{
    public class AuditDAL : DAL<AuditVO>
    {

        public IList<AuditResultVO> GetAuditWithFileFromPreload(int year)
        {
            String query = "select tipo_audit as TypeName, clave_audit as SectorName , cast(numero_audit as unsigned) as Number, " +
                "titulo_audit as Title, siglas_ente as EntityName, file_path as File, file_anchor as Page " +
            " from preload_audit_report par where par.file_path is not null and anio='" + year + "' limit 0,200";
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.SetResultTransformer(Transformers.AliasToBean(typeof(AuditResultVO)));
                IList<AuditResultVO> Lista = iquery.List<AuditResultVO>();
                return Lista;
            }

        }
        public IList<AuditResultVO> GetAuditWithoutFileFromPreload(int year)
        {
            String query = "select tipo_audit as TypeName, clave_audit as SectorName , cast(numero_audit as unsigned) as Number, " +
                "titulo_audit as Title, siglas_ente as EntityName, file_path as File, file_anchor as Page " +
            " from preload_audit_report par where par.file_path is null and anio='" + year + "';";
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.SetResultTransformer(Transformers.AliasToBean(typeof(AuditResultVO)));
                IList<AuditResultVO> Lista = iquery.List<AuditResultVO>();
                return Lista;
            }

        }
        public IList<AuditResultVO> GetOrphanFilesFromPreload(int year)
        {
            String query = "select paf.file_path as File, paf.file_anchor as Page, paf.original_match as Title, paf.audit_key as SectorName"+
            " from preload_audit_file paf left join "+
            " preload_audit_report par on  par.parsed_code=paf.parsed_code where par.parsed_code is null order by paf.file_path;";
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.SetResultTransformer(Transformers.AliasToBean(typeof(AuditResultVO)));
                IList<AuditResultVO> Lista = iquery.List<AuditResultVO>();
                return Lista;
            }

        }


        public IList<AuditResultVO> SearchAudit(String filtro)
        {
            String query = "select  `year` as `Year`, audit_type.id as TypeId, audit_type.name as TypeName, audit_entity.id as EntityId,"+
               " audit_entity.name as EntityName, audit_sector.id as SectorId , audit_sector.name as SectorName, audit.title as Title, number as Number, "+
               " file_path as File, file_anchor as Page, gf.id as GrupoFuncionalId, gf.name as GrupoFuncionalName" +
               " from audit_report,audit_entity, audit_type, audit_sector, audit left join "+
               " grupo_funcional gf on audit.grupo_funcional_id=gf.id"+
               " where audit.audit_report_id=audit_report.id"+
               " and audit.audit_entity_id=audit_entity.id"+
               " and audit_sector.id=audit_entity.sector_id"+
               " and audit.audit_type_id=audit_type.id " + 
               " and audit_report.published=1 "+filtro;

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.SetResultTransformer(Transformers.AliasToBean(typeof(AuditResultVO)));
                IList<AuditResultVO> Lista = iquery.List<AuditResultVO>();
                return Lista;
            }
        }
        public IList<AuditVO> GetByYear(int year)
        {
            string query = "select avo from AuditVO avo, AuditReportVO arvo "
            +"where avo.AuditReportId=arvo.Id and arvo.Year='" + year + "'";
            return this.list(query);
        }
        public void DeletePreload()
        {
            ExecuteUpdate("delete from preload_audit_report;");
            ExecuteUpdate("delete from preload_audit_file;");

        }
        public bool DeleteByReport(int auditReportId)
        {
            ExecuteUpdate("update publication set content ='' where permalink in (select index_permalink from audit_report where id='" + auditReportId + "');");
            return ExecuteUpdate("delete from audit where audit_report_id='" + auditReportId + "'");
           
        }
        public bool ExecuteUpdate(String query)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                iquery.ExecuteUpdate();
            }
            return true;
        }
        public bool DetectEntities(int year)
        {
            string query = "update preload_audit_report, audit_entity set entity_id=id, entity_exists=1 where " +
            " clave_ente=entity_key and clave_dependencia=dep_key and clave_tipo_ente=type_key and anio='" + year + "';";
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ISQLQuery iquery = session.CreateSQLQuery(query);
                int entityFinded = iquery.ExecuteUpdate();
           }
            return true;
        }
        public bool DetectSectors(int year)
        {
            String query="update preload_audit_report, audit_sector set sector_id=id, sector_exists=1 where "+
            "trim(nombre_sector)=trim(audit_sector.name) and anio='"+year+"';";
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {

                ISQLQuery iquery = session.CreateSQLQuery(query);
                int sectorsFinded = iquery.ExecuteUpdate();
           }
            return true;

        }
        public bool DetectGroups(int year)
        {
            String query = "update preload_audit_report, grupo_funcional set group_id=id, group_exists=1 where " +
            "trim(group_code)=trim(grupo_funcional.code) and anio='" + year + "';";
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {

                ISQLQuery iquery = session.CreateSQLQuery(query);
                int groupsFinded = iquery.ExecuteUpdate();
            }
            return true;

        }
        public bool DetectFiles(int year)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {

                string query = "update preload_audit_report pr, preload_audit_file pf " +
                "set pr.file_anchor=pf.file_anchor, pr.file_path=pf.file_path where anio='" + year + "' and " +
                "pf.parsed_code=pr.parsed_code and pr.file_path is null;";
                ISQLQuery iquery = session.CreateSQLQuery(query);
                int entityFinded = iquery.ExecuteUpdate();
            }
            return true;
        }
        public bool AuditLoad(int year)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                 String query = "insert ignore into audit_type (id,name)" +
                " select distinct id_tipo_audit, tipo_audit from preload_audit_report where anio='"+year+"' order by id_tipo_audit;";
                ISQLQuery iquery = session.CreateSQLQuery(query);
                int auditTypesInserted=iquery.ExecuteUpdate();

                query = "insert into audit_sector(asf_key, name) " +
                   " select distinct '', nombre_sector " +
                   " from preload_audit_report where sector_id is null and anio='" + year + "';";
                iquery = session.CreateSQLQuery(query);
                int sectorInserted = iquery.ExecuteUpdate();

                query = "update preload_audit_report, audit_sector set sector_id=id where " +
                         " nombre_sector=audit_sector.name and anio='" + year + "';";
                iquery = session.CreateSQLQuery(query);
                int sectors = iquery.ExecuteUpdate();

                query = "insert into audit_entity(entity_key, dep_key, type_key,short_name, name,sector_id) " +
                   " select distinct clave_ente, clave_dependencia,clave_tipo_ente,siglas_ente, nombre_ente, sector_id " +
                   " from preload_audit_report where entity_id is null and anio='"+year+"';";
                iquery = session.CreateSQLQuery(query);
                int entityInserted = iquery.ExecuteUpdate();

                query = "update preload_audit_report, audit_entity set entity_id=id where " +
                " clave_ente=entity_key and clave_dependencia=dep_key and clave_tipo_ente=type_key and anio='"+year+"';";
                iquery = session.CreateSQLQuery(query);
                int entitys = iquery.ExecuteUpdate();

                query = "insert into grupo_funcional(code, name) " +
                   " select distinct group_code, group_name" +
                   " from preload_audit_report where group_id is null "+
                   " and (group_code is not null or group_name is not null)"+
                   " and (group_code!='' or group_name!='') and anio='" + year + "';";
                iquery = session.CreateSQLQuery(query);
                int groupInserted = iquery.ExecuteUpdate();


                query = "update preload_audit_report, grupo_funcional gr set group_id=gr.id where " +
                " gr.code=group_code and gr.name=group_name and anio='" + year + "';";
                iquery = session.CreateSQLQuery(query);
                int grupos = iquery.ExecuteUpdate();


                query = "insert into audit (title, audit_report_id, audit_entity_id, audit_type_id, code, file_path, file_anchor, file_mime, number,index_position, grupo_funcional_id) select " +
                "titulo_audit,audit_report.id,entity_id,id_tipo_audit,clave_audit, file_path, file_anchor,'application/pdf',numero_audit, num_r,group_id " +
                "from preload_audit_report, audit_report where anio=`year` and anio='"+year+"'; ";
                iquery = session.CreateSQLQuery(query);
                int auditInserted = iquery.ExecuteUpdate();
            }
            
            return true;
        }
    }
}
