using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_ClientControlPlugin.API.WebResponse
{
    class Response
    {
        private Dictionary<int, string> statusCode;
        private Dictionary<string, string> headers;
        private string Content;
        public int ContentLength { get; private set; }
        public StatusCode StatusCode { set; get; }        

        public Response()
        {
            StatusCode = StatusCode.OK;
            headers = new Dictionary<string, string>();
            statusCode = new Dictionary<int, string>() { {200, "OK" }, { 403, "Forbidden" }, 
                                                         { 408, "Request Timeout" }, { 404, "Not Found" }, };
            Content = "";
        }

        public void SetContent(string content)
        {
            ContentLength = content.Length;
            Content = content;
        }

        public void AddHeader(string key,string value)
        {
            headers.Add(key, value);
        }

        public string GetResponseText()
        {
            string response = $"HTTP/1.1 {(int)StatusCode} {statusCode[(int)StatusCode]}\r\n" +
                                "Server: MyServer / 0.0.1(Win32)\r\n" +
                                $"Date: {DateTime.UtcNow.ToString("r", CultureInfo.InvariantCulture)}\r\n" +
                                "Connection: keep-alive\r\n" +
                                "Content-Type: text/javascript\r\n";
            if (Content.Length > 0)
            {
                response += $"Content-Length: {Content.Length}\r\n\r\n";
                response += Content;
                return response;
            }

            response += $"Content-Length: 7\r\n\r\n";
            response += "content";
            return response;

        }
    }
}
