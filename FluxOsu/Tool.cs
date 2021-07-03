using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FluxOsu
{
    public class Tool
    {
        #region WinAPI
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeLibrary(IntPtr hModule);
        #endregion

        public delegate bool GetClPath_(out IntPtr path, out int len);
        public static GetClPath_ GetClPath;

        public static string log = "";
        public static int dllhandle = 0;

        public static void Log(string txt)
        {
            log += txt + "\n";
        }

        public static int GetModuleHandle()
        {
            if (dllhandle != 0) return dllhandle;

            foreach (ProcessModule pm in Process.GetCurrentProcess().Modules)
            {
                if (pm.ModuleName == "ncl.dll")
                    return dllhandle = (int)pm.BaseAddress;
            }
            return 0;
        }

        public static bool init()
        {
            if (GetModuleHandle() == 0) return false;
            GetClPath = (GetClPath_)toDelegate("GetClPath", typeof(GetClPath_));
            if (GetClPath == null) return false;
            return true;
        }

        //将要执行的函数转换为委托
        public static Delegate toDelegate(string APIName, Type t)
        {
            if (dllhandle == 0) return null;
            IntPtr api = GetProcAddress((IntPtr)dllhandle, APIName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }
    }
}
