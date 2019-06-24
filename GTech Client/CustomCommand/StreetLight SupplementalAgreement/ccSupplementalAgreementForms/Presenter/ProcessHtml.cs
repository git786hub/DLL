using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using GTechnology.Oncor.CustomAPI.View;
using GTechnology.Oncor.CustomAPI.Model;
using Intergraph.GTechnology.API;
using ADODB;
using System.Text.RegularExpressions;
using System.Threading;
using System.ComponentModel;

namespace GTechnology.Oncor.CustomAPI.Presenter
{
    public class ProcessHtml
    {
        public ProcessHtml()
        {

        }
        /// <summary>
        /// Get all image tags in the html template.
        /// </summary>
        /// <param name="strHtml">Content of the html template</param>
        /// <returns></returns>
        public List<string> GetImagesInHtmlString(string strHtml)
        {
            List<string> images = new List<string>();
            try
            {
                string pattern = @"<(img)\b[^>]*>";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(strHtml);

                for (int i = 0, l = matches.Count; i < l; i++)
                {
                    images.Add(matches[i].Value);
                }
            }
            catch
            {
                throw;
            }

            return images;
        }

        /// <summary>
        /// Replace all image tags with label tags.
        /// </summary>
        /// <param name="lstImages">List of image tags</param>
        /// <param name="strReplace">label tag to replace</param>
        /// <param name="strContent">Content of the html template</param>
        public string ReplaceImagesTagsWithLabels(List<string> lstImages, string strContent)
        {
            string strReplace = "<label for='Active WR' Style='font-size:11.0pt;color:red'><i>[[Active WR]]</i></label>";
            foreach (string img in lstImages)
            {
                if (img.Contains("Active WR"))
                {
                    strReplace = "<label for='Active WR' Style='font-size:11.0pt;color:red'><i>[[Active WR]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Customer]"))
                {
                    strReplace = "<label for='Active Customer' Style='font-size:11.0pt;color:red'><i>[[Active Customer]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Agreement"))
                {
                    strReplace = "<label for='Active Agreement' Style='font-size:11.0pt;color:red'><i>[[Active Agreement]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Customer Description"))
                {
                    strReplace = "<label for='Active Customer Description' Style='font-size:11.0pt;color:red'><i>[[Active Customer Description]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Month"))
                {
                    strReplace = "<label for='Active Month' Style='font-size:11.0pt;color:red'><i>[[Active Month]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Year"))
                {
                    strReplace = "<label for='Active Year' Style='font-size:11.0pt;color:red'><i>[[Active Year]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Day"))
                {
                    strReplace = "<label for='Active Day' Style='font-size:11.0pt;color:red'><i>[[Active Day]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Service Date"))
                {
                    strReplace = "<label for='Active Service Date' Style='font-size:11.0pt;color:red'><i>[[Active Service Date]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Customer Agent"))
                {
                    strReplace = "<label for='Active Customer Agent' Style='font-size:11.0pt;color:red'><i>[[Active Customer Agent]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Comments"))
                {
                    strReplace = "<label for='Active Comments' Style='font-size:11.0pt;color:red'><i>[[Active Comments]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active Cost"))
                {
                    strReplace = "<label for='Active Cost' Style='font-size:11.0pt;color:red'><i>[[Active Cost]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("For CIAC purposes only pursuant to Section"))
                {
                    strReplace = "<label for='For CIAC purposes' Style='font-size:11.0pt;color:black'><i>For CIAC purposes only pursuant to Section (4) above.</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("Active City"))
                {
                    strReplace = "<label for='Active City' Style='font-size:11.0pt;color:red'><i>[[Active City]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("AADD"))
                {
                    strReplace = "<label for='AADD' Style='font-size:11.0pt;color:red'><i>[[AADD]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("ARemove"))
                {
                    strReplace = "<label for='ARemove' Style='font-size:11.0pt;color:red'><i>[[ARemove]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
                else if (img.Contains("ACost"))
                {
                    strReplace = "<label for='ACost' Style='font-size:11.0pt;color:red'><i>[[ACost]]</i></label>";
                    strContent = strContent.Replace(img, strReplace);
                }
            }

            return strContent;
        }


        /// <summary>
        /// Get <tr> tag of the datagridview from the html template.
        /// </summary>
        /// <param name="strHtml">Content of the html template</param>
        /// <returns></returns>
        public List<string> GetRowsOfGridInHtmlString(string strHtml)
        {
            List<string> lstDataRows = new List<string>();
            try
            {
                MatchCollection matches = Regex.Matches(strHtml, @"(?<1><tr for='DataRows'[^>]*>\s*<td.*?</tr>)",
              RegexOptions.Singleline | RegexOptions.IgnoreCase);

                for (int i = 0, l = matches.Count; i < l; i++)
                {
                    lstDataRows.Add(matches[i].Value);
                }

                return lstDataRows;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Get <tr> tag of the datagridview from the html template.
        /// </summary>
        /// <param name="strHtml">Content of the html template</param>
        /// <returns></returns>
        public string GetRowofComments(string strHtml)
        {
            List<string> lstDataRows = new List<string>();
            try
            {
                MatchCollection matches = Regex.Matches(strHtml, @"(?<1><tr for='Comments TR'[^>]*>\s*<td.*?</tr>)",
              RegexOptions.Singleline | RegexOptions.IgnoreCase);

                for (int i = 0, l = matches.Count; i < l; i++)
                {
                    lstDataRows.Add(matches[i].Value);
                }

                return lstDataRows[0];
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Dynamicall add rows of gridview to the html template.
        /// </summary>
        /// <param name="lstDataRows">list of datarows</param>
        /// <param name="strHtmlFinalRow">Final string after replacing with form datagridview</param>
        /// <param name="strHtmlRow">html string which contains datagridview row</param>
        public void ReplaceTableWithGridValues(List<string> lstDataRows, ref string strHtmlFinalRow, ref string strHtmlRow, DataGridView dataGridView)
        {
            if (lstDataRows != null && lstDataRows.Count == 1)
            {                
                if (dataGridView != null && dataGridView.RowCount > 0)
                {

                    string strTemp = "";
                    strHtmlRow = lstDataRows[0];
                    int datarowCount = dataGridView.RowCount;
                    DataGridViewRowCollection dataGridViewRowCollection = dataGridView.Rows;

                    for (int i = 0; i < datarowCount; i++)
                    {
                        strTemp = lstDataRows[0];

                        if (i != 0)
                        {
                            strTemp = strTemp.Replace("DataRows", "DataRows" + i + 1);
                        }

                        string str = Convert.ToString(dataGridViewRowCollection[i].Cells[9].Value);
                        if (!string.IsNullOrEmpty(str))
                        {
                            str = str.Replace("\n", "<br />");
                        }

                        strTemp = strTemp.Replace("Address", str);
                        strTemp = strTemp.Replace("ESI Location", Convert.ToString(dataGridViewRowCollection[i].Cells[0].Value));
                        strTemp = strTemp.Replace("Location", str);

                        
                        strTemp = strTemp.Replace("Action", Convert.ToString(dataGridViewRowCollection[i].Cells[1].Value));
                        strTemp = strTemp.Replace("Energize", Convert.ToString(dataGridViewRowCollection[i].Cells[2].Value));
                        strTemp = strTemp.Replace("Quantity", Convert.ToString(dataGridViewRowCollection[i].Cells[3].Value));
                        strTemp = strTemp.Replace("Wattage", Convert.ToString(dataGridViewRowCollection[i].Cells[4].Value));
                        strTemp = strTemp.Replace("Lamp Type", Convert.ToString(dataGridViewRowCollection[i].Cells[5].Value));
                        strTemp = strTemp.Replace("Rate Schedule", Convert.ToString(dataGridViewRowCollection[i].Cells[6].Value));
                        strTemp = strTemp.Replace("Luminaire Style", Convert.ToString(dataGridViewRowCollection[i].Cells[7].Value));

                        str = Convert.ToString(dataGridViewRowCollection[i].Cells[8].Value);
                        if(!string.IsNullOrEmpty(str))
                        {
                            str = str.Replace("\n", "<br />");
                        }
                        strTemp = strTemp.Replace("Identifying Type", str);

                        strTemp = strTemp.Replace("Light Source", Convert.ToString(dataGridViewRowCollection[i].Cells[5].Value));


                        if (string.IsNullOrEmpty(strHtmlFinalRow))
                            strHtmlFinalRow = strTemp;
                        else
                        {
                            strHtmlFinalRow = strHtmlFinalRow + strTemp;
                        }
                    }
                }
            }
        }

    }
}
