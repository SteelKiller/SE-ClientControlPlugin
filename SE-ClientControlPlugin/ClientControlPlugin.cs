using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch.API;
using Torch.Session;
using Torch.API.Plugins;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Torch;
using Torch.API.Managers;
using Torch.Managers.PatchManager;
using Torch.API.Session;
using Torch.Views;
using VRage.Game;
using VRage.Game.ModAPI;
using Torch.Mod;
using Torch.Mod.Messages;
using NLog;
using SE_ClientControlPlugin.API;
using VRageMath;

namespace SE_ClientControlPlugin
{
    public class ClientControlPlugin : TorchPluginBase
    {
   
        private Logger _log = LogManager.GetCurrentClassLogger();
        
        public Guid Id => Guid.Parse("11fca5c4-01b6-4fc3-a215-602e2325be2b");

        public string Version => "0.1";

        public string Name => "SE_ClientControlPlugin";

        public PluginState State => PluginState.Enabled;

        private APIBridge bridge;
        public override void Dispose()
        {
            bridge.isRunning = false;
        }

        public override void Init(ITorchBase torchBase)
        {
            bridge = new APIBridge(8081);
        }

        public override void Update()
        {     
            if (bridge.CommandReady)
            {                
                bridge.Command.ExecuteCommand();              
            }
        }



    }
}
