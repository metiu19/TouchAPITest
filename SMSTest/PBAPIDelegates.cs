using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Utils;

namespace SMSTest
{
    public partial class SMSTestSession
    {
        void SetupAPI()
        {
            PBInterface.AddMethod("changeValue", new Action<string>(ChangeValue));
        }

        void ChangeValue(string value)
        {
            MyAPIGateway.Utilities.ShowNotification($"Current: {TSS.App.TestPBLable.Text}\nNew: {value}", 4000);
            TSS.App.TestPBLable.Text = value;
        }
    }
}
