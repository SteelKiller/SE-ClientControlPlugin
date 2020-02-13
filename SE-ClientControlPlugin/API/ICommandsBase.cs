using SE_ClientControlPlugin.API.WebResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_ClientControlPlugin.API
{
    interface ICommandsBase
    {
        string CommandName { get; }
        bool ProcessingInThread(string[] args);
        void ExecuteCommand();
        EnumCommands getCommandResult();
        IResponseBase Response();
    }
}
