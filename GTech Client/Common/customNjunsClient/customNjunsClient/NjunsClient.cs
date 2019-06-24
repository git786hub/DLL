using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace GTechnology.Oncor.CustomAPI
{
    public class NjunsClient
    {
        public string EndPoint { get; set; }
        public HttpVerbs Method { get; set; }
        public string ContentType { get; set; }
        public string RequestXml { get; set; }

        public NjunsClient()
        {
            EndPoint = "";
            Method = HttpVerbs.Get;
            ContentType = "text/xml";
            RequestXml = "";
        }
        public NjunsClient(string endpoint)
        {
            EndPoint = endpoint;
            Method = HttpVerbs.Get;
            ContentType = "text/xml";
            RequestXml = "";
        }
        public NjunsClient(string endpoint, HttpVerbs method)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            RequestXml = "";
        }

        public NjunsClient(string endpoint, HttpVerbs method, string requestXml)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            RequestXml = requestXml;
        }


        public string GetXMLReponse()
        {
            return GetXMLReponse("");
        }

        public string GetXMLReponse(string parameters)
        {
            var request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;

            if (!string.IsNullOrEmpty(RequestXml) && Method == HttpVerbs.Post)
            {
                var encoding = new UTF8Encoding();
                //var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(Message);
                var bytes = Encoding.ASCII.GetBytes(RequestXml);
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
                    var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                // grab the response
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                }

                return responseValue;
            }
        }

    }
}
