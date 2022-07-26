using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;

namespace asf.cms.widgets
{
    public enum WidgetNames
    {
        news = 1,
        announce = 2,
        video_player = 3,
        counter = 4,
        map = 5,
        modification = 6,
        recuperaciones = 7, 
        audit=8

    }

    public class WidgetConfigurationVO
    {

        public string FullCommand { get; set; }

        private string cmdName;
        public string CommandName 
        {
            get
            {
                return cmdName;
            }
            set
            {
                cmdName = value;
            }
        }

        public WidgetNames CommandStrongName
        {
            get 
            {
                return (WidgetNames)Enum.Parse(typeof(WidgetNames), cmdName);
            }
        }


        public List<WidgetConfigurationParamVO> ConfigurationParams { get; set; }

        /// <summary>
        /// Builds a configuration VO from a content, using the provided regex
        /// </summary>
        /// <param name="rawWidgetConfiguration"></param>
        /// <param name="?"></param>
        public WidgetConfigurationVO()
        {
            ConfigurationParams = new List<WidgetConfigurationParamVO>();
        }

    }

}