using System;
using System.Collections.Generic;
using VRageMath;
using SMSTest.API;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using Sandbox.ModAPI;

namespace SMSTest
{
    public class SMSTestApp : TouchApp
    {
        readonly string version = "v0.0.0";
        public Label TestPBLable = new Label("Waiting to recieve value");

        public SMSTestApp(IMyCubeBlock block, IMyTextSurface surface) : base(block, surface)
        {
            DefaultBg = true;

            var windowBar = new WindowBar($"SMS Test - [{version}]");
            windowBar.BgColor = Color.DarkCyan;
            windowBar.Label.TextColor = Color.White;
            windowBar.Anchor = ViewAnchor.SpaceBetween;

            var window = new View(ViewDirection.Column);
            window.Border = new Vector4(3);
            window.Padding = new Vector4(9);

            Dictionary<int, View> tabsElements = new Dictionary<int, View>
            {
                { 0, new View(ViewDirection.Column) },
                { 1, new View(ViewDirection.Column) },
            };

            int oldTab = 0;

            var tabs = new Switch(new string[] { "Tab 1", "Tab 2" }, 0, (int newTab) =>
            {
                tabsElements[oldTab].Enabled = false;
                tabsElements[newTab].Enabled = true;
                oldTab = newTab;
            });
            tabs.Border = new Vector4(1);

            // Tab 1
            var progressbarLable = new Label("Test Progress Bar");
            var progressbar = new ProgressBar(0, 100);

            var sliderLable = new Label("Test Slider");
            var slider = new Slider(0, 100, (float v) =>
            {
                progressbar.Value = v;
            });

            tabsElements[0].AddChild(sliderLable);
            tabsElements[0].AddChild(slider);
            tabsElements[0].AddChild(progressbarLable);
            tabsElements[0].AddChild(progressbar);
            tabsElements[0].AddChild(TestPBLable);

            // Tab 2
            var testLable = new Label("Test Lable");
            var textLable = new Label("Block Name");
            var textArea = new TextField();
            textArea.Text = "TestLight";
            textArea.OnSubmit = (string text) =>
            {
                TestPBLable.Text = text;
            };
            var buttonLable = new Label("TestLight");
            var button = new Button("Toggle light", () =>
            {
                if (textArea.Text == "")
                    return;
                var tempBlock = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(block.CubeGrid).GetBlockWithName(textArea.Text) as IMyFunctionalBlock;
                if (tempBlock != null)
                    tempBlock.Enabled ^= true;
            });
            tabsElements[1].AddChild(testLable);
            tabsElements[1].AddChild(textLable);
            tabsElements[1].AddChild(textArea);
            tabsElements[1].AddChild(buttonLable);
            tabsElements[1].AddChild(button);
            

            // Assemble Page
            windowBar.AddChild(tabs);
            AddChild(windowBar);
            foreach (var tab in tabsElements)
            {
                tab.Value.Enabled = false;
                window.AddChild(tab.Value);
            }
            tabsElements[0].Enabled = true;
            AddChild(window);
        }
    }
}
