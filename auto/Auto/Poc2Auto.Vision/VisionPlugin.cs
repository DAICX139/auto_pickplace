using AlcUtility;
using Poc2Auto.Common;

namespace Poc2Auto.Vision
{
    class VisionPlugin : PluginBase
    {
        public VisionPlugin() : base(ModuleTypes.Vision.ToString())
        {
        }

        public override bool Load()
        {
            ExpectedModuleIds = null;

            GetMessageHandler(MessageNames.CMD_BottomCamera).DataReceived += UpLookCamera;
            GetMessageHandler(MessageNames.CMD_RightTopCamera).DataReceived += DownLookCamera;

            //UpdateModuleStatus(true);

            return base.Load();
        }

        private void DownLookCamera(MessageHandler arg1, ReceivedData arg2)
        {
            throw new System.NotImplementedException();
        }

        private void UpLookCamera(MessageHandler arg1, ReceivedData arg2)
        {
            throw new System.NotImplementedException();
        }
    }
}
