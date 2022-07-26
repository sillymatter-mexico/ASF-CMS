using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.widgets
{
    public class WidgetParseResult
    {

        private List<String> m_ErrorsInExpression = new List<string>();

        public bool ContainsWidgetExpression { get; set; }
        public List<string> ErrorsInExpression
        {
            get
            {
                return m_ErrorsInExpression;
            }
            set
            {
                m_ErrorsInExpression = value;
            }
        }
    }
}