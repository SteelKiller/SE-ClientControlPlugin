using Sandbox.Game.Entities;
using SE_ClientControlPlugin.API.WebResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VRageMath;

namespace SE_ClientControlPlugin.API
{
    class MoveGridTo : ICommandsBase
    {
        Vector3D newPosition;
        long EntityId;
        bool clearOwner;
        MyCubeGrid targetGrid;
        private EnumCommands CommandStatus = EnumCommands.WAITING;

        public string CommandName { get => "moveGridTo"; }

        private bool SearchTargetGrid(long EntityId, bool clearOwner, Vector3D newPosition)
        {
            this.EntityId = EntityId;
            this.clearOwner = clearOwner;
            this.newPosition = newPosition;
            var grids = MyEntities.GetEntities().OfType<MyCubeGrid>().ToList();


            for (int i = 0; i < grids.Count; i++)
            {
                if (grids[i].EntityId == EntityId)
                {
                    targetGrid = grids[i];
                    return true;
                }
            }
            return false;
        }

        public void ExecuteCommand()
        {
            if (targetGrid == null)
            {
                CommandStatus = EnumCommands.ERROR;
                return;
            }

            
            MatrixD m = targetGrid.WorldMatrix;
            m.Translation = newPosition;
            targetGrid.PositionComp.SetWorldMatrix(m, forceUpdate: true, updateChildren: true, updateLocal: true, forceUpdateAllChildren: true, ignoreAssert: true);
            //targetGrid.Teleport(m);
            //newPosition.X += 0.01;
            //targetGrid.PositionComp.SetPosition(newPosition);
            CommandStatus = EnumCommands.SUCCES;
        }

        public EnumCommands getCommandResult()
        {
            return CommandStatus;
        }

        private Vector3D GetVector3DFromGPS(string GPS)
        {
            string[] s_gps = GPS.Replace(".", ",").Split(':');
            return new Vector3D(Convert.ToDouble(s_gps[2]), Convert.ToDouble(s_gps[3]), Convert.ToDouble(s_gps[4]));
        }

        /*private bool checkArguments(string[] args)
        {
            try 
            { 

            }
            catch(FormatException e)
            {
                return false;
            }
            return true;
        }*/

        public bool ProcessingInThread(string[] args)
        {           
            if (!CommandName.Equals(args[0]))
                return false; 

            
            if (!SearchTargetGrid(Convert.ToInt64(args[1]), false, GetVector3DFromGPS(args[3])))
            {
                return false;
            }
            return true;
        }

        public IResponseBase Response()
        {
            Succes response = new Succes();
            response.StatusCode = StatusCode.OK;
            response.SetContent("hk");
            return response;

        }
    }
}
