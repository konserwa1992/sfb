using System;
using System.Runtime.InteropServices;

namespace WpfLibrary1
{
    public class Class1
    {
        public static int IMain(string args)
        {
            AllocConsole(); //Open a console window from the same process.  
            Console.WriteLine("<_C# Loaded_>"); //Proof its working.

            AttackTarget(0x210, 0x00000951);



            return 0;
        }

        [DllImport("kernel32")]
        static extern bool AllocConsole();

        [DllImport("ClrBootstrap.dll")]
        static extern int AttackTarget(uint skill, uint monsterIndex);


    }
}
