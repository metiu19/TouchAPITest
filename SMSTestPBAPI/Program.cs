using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        SMSTestPBAPI PBAPI;

        public Program()
        {
            PBAPI = new SMSTestPBAPI(Me);

            if (PBAPI == null)
                throw new Exception("API could not be created!");
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (argument == "")
                return;

            Echo(argument);
            PBAPI.ChangeValue(argument);
        }

        public class SMSTestPBAPI
        {
            public SMSTestPBAPI(IMyProgrammableBlock pb)
            {
                var delegates = pb.GetProperty("SMSTest")?.As<IReadOnlyDictionary<string, Delegate>>()?.GetValue(pb);

                if (delegates == null)
                    return;

                AssignMethod(out changeValue, delegates["changeValue"]);
            }

            private void AssignMethod<T>(out T field, object method) => field = (T)method;

            public void ChangeValue(string value) => changeValue?.Invoke(value);
            Action<string> changeValue;
        }
    }
}
