using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_ClientControlPlugin.API.WebResponse
{
    enum StatusCode
    {
        OK=200,
        Forbidden=403,
        RequestTimeout=408,
        NotFound=404
    }
}
