//----------------------------------------------------------------------------+
//        Class: SendReceiveXMLMessage
//  Description: This class holds necessary properties and method to make request to Web service URL and gets the response.
//                                                                  
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 16/04/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: SendReceiveXMLMessage.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 16/04/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using System;
using System.IO;
using System.Net;
using System.Text;

namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// Class that contains properties and method to make request to Web service URL and gets the response.
    /// </summary>
    public class SendReceiveXMLMessage
    {
        /// <summary>
        /// Endpoint address URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Type of method GET or POST.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Content type of the request. ex - "text/xml" or "application/soap+xml" etc
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Request xml in string format
        /// </summary>
        public string RequestXMLBody { get; set; }

        /// <summary>
        /// Response xml in string format
        /// </summary>
        public string ResponseXML { get; private set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public SendReceiveXMLMessage()
        {

        }

        /// <summary>
        /// Constructor to initialize the parameters
        /// </summary>
        /// <param name="p_URL">Endpoint address URL</param>
        /// <param name="p_ContentType">Content type of the request. ex - "text/xml" or "application/soap+xml" etc</param>
        /// <param name="p_RequestXMLBody">Request xml in string format</param>
        /// <param name="p_Method">Type of method GET or POST.</param>
        public SendReceiveXMLMessage(string p_URL, string p_RequestXMLBody, string p_Method, string p_ContentType = "text/xml")
        {
            URL = p_URL;
            ContentType = p_ContentType;
            RequestXMLBody = p_RequestXMLBody;
            Method = p_Method;
        }

        /// <summary>
        /// Method to send request to a End Point Address and stores the response in a property
        /// </summary>
        public void SendMsgToEF()
        {
            try
            {
                SetDefaults();
                WebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = Method;
                request.ContentLength = 0;
                request.ContentType = ContentType;

                if (!string.IsNullOrEmpty(RequestXMLBody) && Method.ToUpper() == "POST")
                {
                    var encoding = new UTF8Encoding();
                    var bytes = Encoding.ASCII.GetBytes(RequestXMLBody);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseValue = string.Empty;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new WebException();
                    }

                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                ResponseXML = reader.ReadToEnd();
                            }
                    }
                }
            }
            catch (WebException)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set default values of request parameters
        /// </summary>
        private void SetDefaults()
        {
            try
            {
                if (string.IsNullOrEmpty(Method))
                {
                    Method = "GET";
                }
                if (string.IsNullOrEmpty(ContentType))
                {
                    ContentType = "text/xml; encoding='utf-8'";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
