//----------------------------------------------------------------------------+
//        Class: SendReceiveXMLMessage
//  Description: This class holds necessary properties and methods to make request to Web service URL and gets the response.
//                                                                  
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 25/02/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: SendReceiveXMLMessage.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 25/02/18   Time: 18:00  Desc : Created
// User: hkonda     Date: 22/03/18   Time: 18:00  Desc : Added constructor
// User: hkonda     Date: 23/03/18   Time: 18:00  Desc : Added constructor parameter description and made content type as optional parameter
//----------------------------------------------------------------------------+

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace GTechnology.Oncor.CustomAPI
{
    public class SendReceiveXMLMessage
    {
        public string URL { get; set; }
        public HttpVerbs? Method { get; set; }
        public string ContentType { get; set; }
        public string RequestXMLBody { get; set; }
        public string ResponseXML { get; private set; }

        public SendReceiveXMLMessage()
        {

        }

        /// <summary>
        /// Constructor to initialize the parameters
        /// </summary>
        /// <param name="p_URL">Endpoint address URL</param>
        /// <param name="p_ContentType">Content type of the request. ex - "text/xml" or "application/soap+xml" etc</param>
        /// <param name="p_RequestXMLBody">Request xmml in string format</param>
        /// <param name="p_Method">HTTP verbs. ex - HttpVerbs.Post  </param>
        public SendReceiveXMLMessage(string p_URL, string p_RequestXMLBody, HttpVerbs p_Method, string p_ContentType = "text/xml")
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
                request.Method = Method.ToString();
                request.ContentLength = 0;
                request.ContentType = ContentType;

                if (!string.IsNullOrEmpty(RequestXMLBody) && Method == HttpVerbs.Post)
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
                if (Method == null)
                {
                    Method = HttpVerbs.Get;
                }
                if (string.IsNullOrEmpty(ContentType))
                {
                    ContentType = "text/xml";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
