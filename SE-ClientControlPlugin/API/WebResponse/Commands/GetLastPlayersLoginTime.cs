using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SE_ClientControlPlugin.API.WebResponse.Commands
{
    class GetLastPlayersLoginTime : ICommandsBase
    {
        public string CommandName => "GetLastPlayersLoginTime";

        PlayersInformationFormat[] playerInformation;

        public void ExecuteCommand()
        {
            return;
        }

        public EnumCommands getCommandResult()
        {
            return EnumCommands.SUCCES;
        }

        public bool ProcessingInThread(string[] args)
        {

            if (!CommandName.Equals(args[0]))
                return false;         

            var players = MySession.Static.Players.GetAllIdentities().ToList();

            playerInformation = new PlayersInformationFormat[players.Count];

            for (int i = 0; i < players.Count; i++)
            {               
                if (players[i] != null)
                {
                    playerInformation[i] = new PlayersInformationFormat(){  Name=players[i].DisplayName,
                                                                            LastLoginTime=players[i].LastLoginTime, 
                                                                            UserId =players[i].IdentityId} ;
                }
                                
            }
            return true;
        }

        public IResponseBase Response()
        {
            Succes response = new Succes();
            response.StatusCode = StatusCode.OK;
            response.SetContent(new JavaScriptSerializer().Serialize(playerInformation));
            return response;
        }
    }
}
