using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_ClientControlPlugin.API.WebResponse
{
    class Succes : Response, IResponseBase
    {
        public string GetResponseString()
        {
            return GetResponseText();

        }
    }
}
