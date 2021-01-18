///
/// Did you ever wish that one field you needed was public instead of private?
/// I made this script just for those cases. 
/// After Building this project, add the exe and the mono.cecil file to your '<modname>Resource' folder
/// LLBMM will automaticly run the exe and make the changes for the end user.
/// 
/// The exe has to be named ASMRewriter.exe to automaticly run with LLBMM
///

using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace ASMRewriter
{
    class Program
    {
        private static string ASMPath = "";
        private static string ASMName = @"\Assembly-CSharp.dll";
        const string MOD_NAME = "QuickRematch";

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var finalASMPath = args[0].Replace("%20", " ");
                Directory.SetCurrentDirectory(finalASMPath);
            } 

            Console.WriteLine("Reading assembly...");
            AssemblyDefinition modDef = AssemblyDefinition.ReadAssembly(Directory.GetCurrentDirectory() + $"\\{MOD_NAME}.dll", new ReaderParameters { ReadWrite = true });
            try
            {
                using (AssemblyDefinition aDef = AssemblyDefinition.ReadAssembly(Directory.GetCurrentDirectory() + ASMName, new ReaderParameters { ReadWrite = true }))
                {

                    //This is where the magic happens: asm.SetClassPublic, asm.SetMethodPublic and asm.SetFieldPublic
                    //asm.InjectCallToMethod(aDef, "LLHandlers.BallHandler", "CreateBall", 0, modDef, "MethodReplace", "Audiohook", InjectFlags.None);
                    //asm.SetFieldPublic(aDef, "GameplayEntities.VisualEntity", "visualTable");
                    //asm.SetMethodPublic(aDef, "World", "SaveState", ChangeType.Public);
                    asm.SetFieldPublic(aDef, "LLScreen.UIScreen", "currentScreens");
                    asm.SetFieldPublic(aDef, "LLScreen.ScreenPlayers", "characterButtons");

                    try { aDef.Write(); } catch (Exception ex) { Console.WriteLine(ex); }
                    aDef.Dispose();
                    //Console.ReadLine();
                }
            }catch{
                Console.WriteLine("Could not open assembly, press Enter to exit...");
                //Console.ReadLine();
            }

            modDef.Dispose();
        }
    }

    public enum ChangeType
    {
        Public = 0,
        Virtual = 1,
        Abstract = 2
    }


}
