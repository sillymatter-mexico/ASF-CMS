using System;

namespace asf.cms.model
{
    public class ModificationLogVO
    {
        public virtual int Id { set; get; }
        public virtual string TargetType { set; get; }
        public virtual int TargetId { set; get; }
        public virtual string UserName { set; get; }
        public virtual DateTime Created { set; get; }
        public virtual string Type { set; get; }
        public virtual string Permalink { set; get; }
        public virtual string FieldChanges { set; get; }
        public virtual int HistoricId { set; get; }
    }
}