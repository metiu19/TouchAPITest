using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSTest;

namespace SMSTest.API
{
    public class SMSTestPBAPI
    {
        public SMSTestPBAPI(Sandbox.ModAPI.Ingame.IMyProgrammableBlock pb)
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
