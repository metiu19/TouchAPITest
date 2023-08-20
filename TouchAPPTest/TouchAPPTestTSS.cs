using System;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace TouchAPPTest
{
    [MyTextSurfaceScript("TouchAppTest", "Touch APP Test")]
    public class TouchAPPTestTSS : MyTSSCommon
    {
        public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

        readonly IMyCubeBlock block;
        readonly IMyTerminalBlock terminalBlock;
        readonly IMyTextSurface surface;

        SampleApp app;

        bool init = false;
        int ticks = 0;

        public TouchAPPTestTSS(IMyTextSurface surface, IMyCubeBlock block, Vector2 size) : base(surface, block, size)
        {
            this.block = block;
            this.terminalBlock = (IMyTerminalBlock)block;
            this.surface = surface;

            this.surface.ScriptBackgroundColor = Color.Black;
            this.surface.ScriptForegroundColor = Color.SteelBlue;
        }

        public void Init()
        {
            if (!TestSessionComponent.Instance.API.IsReady)
                return;

            if (init)
                return;
            init = true;

            app = new SampleApp(block, surface);
            app.Theme.Scale = Math.Min(Math.Max(Math.Min(this.Surface.SurfaceSize.X, this.Surface.SurfaceSize.Y) / 512, 0.4f), 2);
            app.Cursor.Scale = app.Theme.Scale;

            // Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessage($"Init: {_app}", "TouchSampleTSS");

            terminalBlock.OnMarkForClose += BlockMarkedForClose;
        }

        public override void Dispose()
        {
            base.Dispose();

            app?.ForceDispose();
            terminalBlock.OnMarkForClose -= BlockMarkedForClose;
        }

        void BlockMarkedForClose(IMyEntity ent)
        {
            Dispose();
        }

        public override void Run()
        {
            try
            {
                if (!init && ticks++ < (6 * 2)) // 2 secs
                    return;

                Init();

                if (app == null)
                    return;

                base.Run();

                using (var frame = m_surface.DrawFrame())
                {
                    app.ForceUpdate();
                    frame.AddRange(app.GetSprites());
                }
            }
            catch (Exception e)
            {
                app = null;
                MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

                if (MyAPIGateway.Session?.Player != null)
                    MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} ]", 5000, MyFontEnum.Red);
            }
        }
    }
}
