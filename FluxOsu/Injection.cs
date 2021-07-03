using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FluxOsu
{
    public interface IClass
    {
        void init();
        string getlog();
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class Class : IClass
    {
        public void init()
        {
            if (!Tool.init()) return;
            Tool.Log("[SYS]Init Success!");
            Tool.Log("[SYS]DllHandle:" + Tool.dllhandle);
            //Hooking.getAllAssembly();
            Hooking.PatchTest();
        }

        public string getlog()
        {
            string temp = Tool.log;
            Tool.log = string.Empty;
            return temp;
        }
    }
}
