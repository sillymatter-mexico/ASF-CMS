using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.widgets
{
    public class WidgetConfigurationParamVO
    {
        public string ConfigurationParamName { get; set; }
        public string ConfigurationParamValue { get; set; }

        public WidgetConfigurationParamVO(string rawConfigParamName)
        {
            string[] configParamNames = rawConfigParamName.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

            ConfigurationParamName = configParamNames[0];
            if (configParamNames.Length > 1)
            {
                ConfigurationParamValue = configParamNames[1];
            }
        }
    }
}