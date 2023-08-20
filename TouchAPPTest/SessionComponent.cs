using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using TouchAPPTest.API;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum;

namespace TouchAPPTest
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class TestSessionComponent : MySessionComponentBase
    {
        public static TestSessionComponent Instance;
        public TouchUiKit API { get; private set; }

        public override void LoadData()
        {
            if (MyAPIGateway.Utilities.IsDedicated)
                return;

            Instance = this;
            API = new TouchUiKit();
            API.Load();
        }

        protected override void UnloadData()
        {
            API?.Unload();
            Instance = null;
        }
    }
}
