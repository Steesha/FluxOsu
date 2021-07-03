using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace FluxOsu
{
    class Program
    {
        const string PATCH_ID = "today.flux.osu";
        static MethodInfo GetMethod(string name) => typeof(Program).GetMethod(name, BindingFlags.Static | BindingFlags.Public);

        static void Main(string[] args)
        {
            //Init harmony class.
            Harmony harmony = new Harmony(PATCH_ID);
            MethodInfo mOriginal = AccessTools.Method(typeof(ExampleClass), "function1");
            if (mOriginal == null)
            {
                Console.WriteLine("get function1 failed.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("get function1 succeed.");

            //Patch method.
            MethodInfo metpatched = null;
            try
            {
                metpatched = harmony.Patch(mOriginal, null, null, new HarmonyMethod(GetMethod(nameof(function1_transpiler))), null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("[Flux][ex]" + "[" + ex.ParamName + "]" + ex.Message);
                Console.WriteLine("hook function1 failed.");
                Console.ReadLine();
                return;
            }


            if (metpatched == null)
            {
                Console.WriteLine("hook function1 failed.");
                Console.ReadLine();
                return;
            }
            ExampleClass.function1("<USER PARAMETER>");
            harmony.Unpatch(mOriginal, HarmonyPatchType.Transpiler, PATCH_ID);
            Console.ReadLine();
        }

        //patch ILcode.
        public static IEnumerable<CodeInstruction> function1_transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Console.WriteLine(nameof(function1_transpiler));
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldstr)
                {
                    codes[i].operand = (object)"<NEW STR>";
                }
            }
            return codes.AsEnumerable();
        }
    }

    class ExampleClass
    {
        public static void function1(string test)
        {
            Console.WriteLine("<RAW STR>" + test);
        }
    }
}
