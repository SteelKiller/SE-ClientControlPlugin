using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_ClientControlPlugin.API.WebResponse.Commands
{
    class PlayersInformationFormat
    {
        public string Name { get; set; }
        public long UserId { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}
