using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using asf.cms.model;

namespace asf.cms.widgets
{
    public class WidgetParser
    {
        //The expression is of the form: #${COMMAND_NAME[:CONFIGURATION_PARAM[=VALUE]][:CONFIGURATION_PARAM[=VALUE][...] }

        private static Regex widgetCommand;
        private static Regex keysCommand = new Regex("\\#\\$");
        private static IDictionary CommandsList = new Dictionary<string, string>();
        private static IDictionary<string, List<string>> ExpressionsByUser = new Dictionary<string, List<string>>();


        private static void BuildCommandsList()
        {
            string[] commands = { "news", "announce", "video_player", "counter", "map", "modification","recuperaciones","audit" };

            CommandsList.Add(commands[0], "all,publications,sections,section_id=,publication_id=");
            CommandsList.Add(commands[1], "NA");
            CommandsList.Add(commands[2], "NA");
            CommandsList.Add(commands[3], "this,global,publication_id=");
            CommandsList.Add(commands[4], "all");
            CommandsList.Add(commands[5], "this,global,section_id=,publication_id=");
            CommandsList.Add(commands[6], "list,reference_id=");
            CommandsList.Add(commands[7], "list,search,index_permalink=");

            ExpressionsByUser.Add("ADMIN", new List<string>(commands));
            ExpressionsByUser.Add("USER", new List<string>(commands));
            ExpressionsByUser.Add("COLABORATOR", new List<string>());
            ExpressionsByUser.Add("RECUPERACIONES", new List<string>());


            for (int allowed=1; allowed<commands.Length; allowed++){
                ExpressionsByUser["RECUPERACIONES"].Add(commands[allowed]);
                ExpressionsByUser["COLABORATOR"].Add(commands[allowed]);
                ExpressionsByUser["RECUPERACIONES"].Add(commands[allowed]);
            }
        }

        /// <summary>
        /// Dynamically creates the regular expression that will detect widget commands
        /// </summary>
        static WidgetParser()
        {
            BuildCommandsList();
            StringBuilder sbRegex = new StringBuilder();
            sbRegex.Append("\\#\\$\\{(");

            StringBuilder validCommands = new StringBuilder();
            int cont = 0;
            foreach (string key in CommandsList.Keys)
            {
                string configParamExpression = "";

                if (CommandsList[key].ToString() != "NA")
                {
                    StringBuilder configParams = new StringBuilder();
                    string[] configParamsArr = CommandsList[key].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    int innerCont = 0;
                    foreach (string cParam in configParamsArr)
                    {
                        //If we expect a value for each config param, accept it

                        if (cParam[cParam.Length - 1] == '=')
                        {
                            configParams.Append(cParam + "[\\w\\W]+?");
                        }
                        else
                        {
                            configParams.Append(cParam);
                        }

                        if (innerCont++ != configParamsArr.Length - 1)
                        {
                            configParams.Append("|");
                        }
                    }

                    configParamExpression = "(\\:(" + configParams + "){1})*";
                }

                validCommands.Append(key + configParamExpression);

                if (cont++ != CommandsList.Count - 1)
                {
                    validCommands.Append("|");
                }
            }

            sbRegex.Append(validCommands);
            sbRegex.Append("){1}\\}");

            widgetCommand = new Regex(sbRegex.ToString(), RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Returns the parse result for a widget expression, specifying the errors
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static WidgetParseResult ContainsWidgetExpression(string content, string currentUser)
        {
            WidgetParseResult wpr = new WidgetParseResult();
            List<string> matches = new List<string>();
            
            MatchCollection mcs = widgetCommand.Matches(content);
            List<string> matchedGroups = new List<string>();

            foreach (Match m in mcs)
            {
                if (m.Groups.Count > 0)
                {
                    foreach (Group g in m.Groups)
                    {
                        if (g.Value.Contains("#$") && g.Success)
                        {
                            if (!IsUserWidgetAllowed(g.Value, currentUser))
                            {
                                AddForbiddenWidgetMessage(wpr.ErrorsInExpression, g.Value);                   
                            }
                            else
                            {
                                matches.Add(g.Value);
                            }
                        }
                    }
                }
            }

            //If we have errors at this point, return immediatly, forbidden widgets for the user had been used!
            if (wpr.ErrorsInExpression.Count > 0)
            {
                return wpr;
            }

            //Now count the keys
            mcs = keysCommand.Matches(content);
            int keys = 0;
            foreach (Match m in mcs)
            {
                if (m.Groups.Count > 0)
                {
                    foreach (Group g in m.Groups)
                    {
                        if (g.Value.Contains("#$") && g.Success)
                        {
                            keys++;
                        }
                    }
                }
            }

            if (keys != matches.Count) 
            { 
                foreach(string mt in matches)
                {
                    content = content.Replace(mt, string.Empty);
                }
                CalculateErrorsInExpression(wpr.ErrorsInExpression, content);
            }
            else
            {
                wpr.ContainsWidgetExpression = true;
            }
            
            return wpr;
        }

        /// <summary>
        /// Adds an error message to the list stating that the provided widget expression is forbidden (due to the user type)
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="widgetExpression"></param>
        private static void AddForbiddenWidgetMessage(List<string> errors, string widgetExpression)
        {
            errors.Add(string.Format("La expresion '{0}' no es permitida", widgetExpression));
        }

        /// <summary>
        /// Returns true if the provided user can use the provided widget expression
        /// </summary>
        /// <param name="widgetExpression"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        private static bool IsUserWidgetAllowed(string widgetExpression, string username)
        {
            UserVO uvo = asf.cms.bll.User.GetUser(username).user;

            foreach (String allowedWidget in ExpressionsByUser[uvo.Type])
            {
                if (widgetExpression.Contains(allowedWidget))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Calculates the errors in a content and adds them to the list of errors
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="content"></param>
        private static void CalculateErrorsInExpression(List<string> errors, string content)
        {

            int indexOfOpen = content.IndexOf("#$");

            while (indexOfOpen >= 0)
            {

                int indexOfClose = content.IndexOf("}");
                if (indexOfClose >= 0)
                {
                    indexOfClose -= indexOfOpen;
                }
                else
                {
                    indexOfClose = 5;
                }
                string wdgt = content.Substring(indexOfOpen, indexOfClose + 1);
                errors.Add(string.Format("La expresion para widget '{0}..' contiene errores, por favor verifiquela", wdgt));

                content = content.Remove(indexOfOpen, indexOfClose+1);
                indexOfOpen = content.IndexOf("#$");
            }
        }

        /// <summary>
        /// Get the list of configuration VOs from a raw content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<WidgetConfigurationVO> GetConfigurationFromContent(string content)
        {
            List<WidgetConfigurationVO> wcVOs = new List<WidgetConfigurationVO>();

            MatchCollection mcs = widgetCommand.Matches(content);
            List<string> matchedGroups = new List<string>();

            foreach (Match m in mcs)
            {
                if (m.Groups.Count > 0)
                {
                    foreach (Group g in m.Groups)
                    {
                        if (g.Success && g.Value.Contains("#$"))
                        {
                            WidgetConfigurationVO wcVO = new WidgetConfigurationVO();
                            wcVO.FullCommand = g.Value;
                            string rawValue = g.Value.Replace("#${", "");
                            rawValue = rawValue.Replace("}", "");
                            string[] configParams = rawValue.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                            for (int i = 0; i < configParams.Length; i++)
                            {
                                if (i == 0)
                                {
                                    wcVO.CommandName = configParams[i];
                                }
                                else
                                {
                                    WidgetConfigurationParamVO wcpvo = new WidgetConfigurationParamVO(configParams[i]);
                                    wcVO.ConfigurationParams.Add(wcpvo);
                                }
                            }

                            wcVOs.Add(wcVO);

                        }
                    }
                }
            }

            return wcVOs;
        }

    }

}