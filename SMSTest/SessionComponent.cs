using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SMSTest.API;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;

namespace SMSTest
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public partial class SMSTestSession : MySessionComponentBase
    {
        public static SMSTestSession Instance;
        public TouchUiKit API { get; private set; }
        public SMSTestTSS TSS;
        public PBInterface PBInterface { get; private set; }

        public override void LoadData()
        {
            if (MyAPIGateway.Utilities.IsDedicated)
                return;

            Instance = this;
            API = new TouchUiKit();
            API.Load();

            PBInterface = new PBInterface("SMSTest");
            SetupAPI();
        }

        protected override void UnloadData()
        {
            API?.Unload();
            Instance = null;
        }
    }
}
