using System;
using System.IO;

namespace AddFileToProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length != 2)
            //{
            //    ShowUsage();
            //    File.WriteAllLines("C:\\Programs\\Sandbox\\A\\A.Tests\\log2.log", new string[] { $"There are {args.Length} arguments {args[0]}" });
            //    return;
            //}

            if ((args.Length != 0) && (args[0].Contains("?")))
            {
                ShowUsage();
                return;
            }

            string[] prefixs = new string[] { "--PathToProjectFile", "--NewFilePath=" };
            CmdArgExtractor cae = new CmdArgExtractor(prefixs, "--", "=");

            var aaa = args[0].Split(' ');

            if (!cae.ValidArgsPrefixes(aaa))
            {
                ShowUsage();
                return;
            }

            string[] myArgs = cae.GetArgValues(args[0].Split(' '));
            Console.WriteLine("---- GetArgValues output ----");

            foreach (string arg in myArgs)
            {
                Console.WriteLine("{0}", arg);
            }

            string[,] my2dArr = cae.GetArgsTwoDimArray(args);
            Console.WriteLine("---- GetArgsTwoDimArray output ----");
            for (int i = 0; i < my2dArr.GetLength(1); i++)
            {
                Console.WriteLine("Arg qualifier: {0}\tValue: {1}", my2dArr[0,i], my2dArr[1,i]);
            }
            
            var projectManager = new ProjectManager();
            projectManager.AddFileToProject(myArgs[0], myArgs[1]);
        }

        private static void ShowUsage()
        {
            Console.WriteLine("AddFileToProject --PathToProjectFile=PathToProjectFile --NewFilePath=RelitivePathToNewFile");
        }
    }
}
