﻿using System;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SMSTest
{
    [MyTextSurfaceScript("SMSTest", "SMS Test")]
    public partial class SMSTestTSS : MyTSSCommon
    {
        public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

        readonly IMyCubeBlock block;
        readonly IMyTerminalBlock terminalBlock;
        readonly IMyTextSurface surface;

        public SMSTestApp App { get; private set; }

        bool init = false;
        int ticks = 0;

        public SMSTestTSS(IMyTextSurface surface, IMyCubeBlock block, Vector2 size) : base(surface, block, size)
        {
            this.block = block;
            this.terminalBlock = block as IMyTerminalBlock;
            this.surface = surface;

            this.surface.ScriptBackgroundColor = Color.Black;
            this.surface.ScriptForegroundColor = Color.SteelBlue;

            SMSTestSession.Instance.TSS = this;
        }

        public void Init()
        {
            if (!SMSTestSession.Instance.API.IsReady)
                return;

            if (init)
                return;
            init = true;

            App = new SMSTestApp(block, surface);
            App.Theme.Scale = Math.Min(Math.Max(Math.Min(this.Surface.SurfaceSize.X, this.Surface.SurfaceSize.Y) / 512, 0.4f), 2);
            App.Cursor.Scale = App.Theme.Scale;

            terminalBlock.OnMarkForClose += BlockMarkedForClose;
        }

        void BlockMarkedForClose(IMyEntity entity)
        {
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();

            App?.ForceDispose();
            terminalBlock.OnMarkForClose -= BlockMarkedForClose;
        }

        public override void Run()
        {
            try
            {
                if (!init && ticks++ < (6 * 2))
                    return;

                Init();

                if (App == null)
                    return;

                base.Run();

                using (var frame = m_surface.DrawFrame())
                {
                    App.ForceUpdate();
                    frame.AddRange(App.GetSprites());
                }
            }
            catch (Exception e)
            {
                MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

                if (MyAPIGateway.Session?.Player != null)
                    MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} ]", 5000, MyFontEnum.Red);
            }
        }
    }
}
