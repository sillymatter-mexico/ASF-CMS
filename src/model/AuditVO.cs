using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.util;

namespace asf.cms.model
{
    public class AuditVO:IComparable
    {
        public virtual int Id
        {
            set;
            get;
        }
        public virtual String Title
        {
            set;
            get;
        }
        public virtual String FileAnchor
        {
            set;
            get;
        }
        public virtual int AuditReportId
        {
            set;
            get;
        }
        public virtual int AuditEntityId
        {
            set;
            get;
        }
        public virtual int AuditTypeId
        {
            set;
            get;
        }
        public virtual string Code
        {
            set;
            get;
        }
        public virtual int  Number
        {
            set;
            get;
        }
        public virtual string FilePath
        {
            set;
            get;
        }
        public virtual string FileMime
        {
            set;
            get;
        }
        public virtual string IndexPosition
        {
            set;
            get;
        }
        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            AuditVO segundo = ((AuditVO)obj);

            string[] uno = this.IndexPosition.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string[] dos = segundo.IndexPosition.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            int tomo1 = Encoder.romanToInt(uno[0]);
            int tomo2 = Encoder.romanToInt(dos[0]);
            if (tomo1 < tomo2)
                return -1;
            else if (tomo2 < tomo1)
                return 1;
            int i = 0;
            for (i = 1; i < dos.Length; i++)
            {
                int intuno;
                int intdos;
                if (i >= uno.Length)
                    return -1;
                if (int.TryParse(uno[i], out intuno) && int.TryParse(dos[i], out intdos))
                {
                    if (intuno < intdos)
                        return -1;
                    else if (intdos < intuno)
                        return 1;
                }
                else if (uno[i].CompareTo(dos[i]) < 0)
                    return -1;
                else if (dos[i].CompareTo(uno[i]) < 0)
                    return 1;

            }
            if (i <= uno.Length)
                return 1;
            return 0;
        }

        #endregion

    }
}
