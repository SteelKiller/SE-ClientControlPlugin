using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_ClientControlPlugin.API.WebResponse
{
    class NotFound : IResponseBase
    {
        public string GetResponseString()
        {
            return "HTTP/1.1 404 Not Found\r\n" +
                    "Server: Apache / 2.2.14(Win32)\r\n" +
                    $"Date: {DateTime.UtcNow.ToString("r", CultureInfo.InvariantCulture)}\r\n" +
                    "Connection: keep-alive\r\n" +
                    "Content-Type: text/javascript\r\n" +
                    "Content-Length: 9\r\n" +
                    "\r\n" +
                    "Not Found";
        }
    }
}
