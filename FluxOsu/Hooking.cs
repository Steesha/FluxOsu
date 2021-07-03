using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace FluxOsu
{
    class Hooking
    {
        const string PATCH_ID = "today.flux.osu";
        public static void getAllAssembly()
        {
            Tool.Log("-------class finder-------");
            Assembly assembly = Assembly.LoadFrom("osu!.exe");
            foreach (Type type in assembly.GetTypes())
            {
                Tool.Log(type.FullName);
                MethodInfo[] methods = type.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    if (method.IsStatic && method.IsPublic)
                    {
                        Tool.Log("\t" + method.Name);
                    }
                }
            }
            Tool.Log("-------class finder-------");
        }
        static MethodInfo GetMethod(string name) => typeof(Hooking).GetMethod(name, BindingFlags.Static | BindingFlags.Public);
        public static void PatchTest()
        {
            Assembly assembly = Assembly.LoadFrom("osu!.exe");
            Harmony harmony = new Harmony(PATCH_ID);
            MethodInfo mOriginal = AccessTools.Method(assembly.GetType("#=qGYPmMoxFLRTUB5dZ371RvCq_XlHIZWwMJ$TQQBfwnVY="), "#=ztaXMwBY=");
            if (mOriginal == null)
            {
                Tool.Log("get function1 failed.");
                return;
            }
            Tool.Log("get function1 succeed.");

            harmony.Patch(mOriginal, new HarmonyMethod(GetMethod("pre")));
        }

        public static void pre()
        {
            Tool.Log("PREVIEW");
        }
    }
}
