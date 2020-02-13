using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch.Commands;
using Torch.Commands.Permissions;
using Torch.Mod;
using Torch.Mod.Messages;
using VRage.Game.ModAPI;
using VRageMath;

namespace SE_ClientControlPlugin.Commands
{
    [Category("SE_CCP")]
    public class Admin : CommandModule
    {
        [Command("moveGridTo", "move current grid to GPS coordinates")]
        [Permission(MyPromoteLevel.Owner)]
        public void MoveGridTo(string gridId, string gps)
        {

            if (Context.Player != null)
            {
                ulong id = Context.Player.SteamUserId;
                Context.Respond($"Ваш грид пожалуйста is {gridId} тама {gps} по прозьбе {id}(еще в разработке)");
            }
            else
            {
                Context.Respond($"Ваш грид пожалуйста is {gridId} тама {gps}(еще в разработке)");
            }
        }

        [Command("generateSecuritykey", "Generate new SecurityKey")]
        [Permission(MyPromoteLevel.Owner)]
        public void GenerateSecurityKey()
        {

            Context.Respond($"Ключ сгенерирован(еще в разработке)");

        }

        //!SE_CCP checkPlayers
        [Command("checkPlayers", "show all players")]
        [Permission(MyPromoteLevel.Owner)]
        public void CheckPlayers(long id)
        {
            string message = "";

            MyCubeGrid grid = (MyCubeGrid)MyEntities.GetEntityById(id);
            MatrixD m = grid.WorldMatrix;

            message += $"{m.Translation.X} {m.Translation.Y} {m.Translation.Z}";
            

            Context.Respond($"{message}");

        }
    }
}
