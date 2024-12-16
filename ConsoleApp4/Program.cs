// Програма складається з двох проектів: ConsoleApp і LabLibrary
// LabLibrary - бібліотека класів
// ConsoleApp - консольний додаток, який викликає бібліотеку

// LabLibrary.csproj
// Бібліотека класів, яка підтримує практичні завдання

// Основний консольний додаток
using System;
using System.IO;
using LabLibrary;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0].ToLower() == "help")
            {
                PrintHelp();
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "version":
                    PrintVersion();
                    break;

                case "run":
                    HandleRunCommand(args);
                    break;

                case "set-path":
                    HandleSetPathCommand(args);
                    break;

                default:
                    Console.WriteLine("Unknown command. Use 'help' for usage information.");
                    break;
            }
        }

        static void PrintVersion()
        {
            Console.WriteLine("Author: Your Name");
            Console.WriteLine("Version: 1.0.0");
        }

        static void HandleRunCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please specify a lab to run: lab1, lab2, or lab3");
                return;
            }

            string lab = args[1].ToLower();
            string inputFile = null;
            string outputFile = null;

            for (int i = 2; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-i":
                    case "--input":
                        inputFile = args[++i];
                        break;
                    case "-o":
                    case "--output":
                        outputFile = args[++i];
                        break;
                }
            }

            inputFile = ResolveFilePath(inputFile, "input.txt");
            outputFile = ResolveFilePath(outputFile, "output.txt");

            if (inputFile == null || !File.Exists(inputFile))
            {
                Console.WriteLine("Error: Input file not found.");
                return;
            }

            switch (lab)
            {
                case "lab1":
                    LabRunner.RunLab1(inputFile, outputFile);
                    break;
                case "lab2":
                    LabRunner.RunLab2(inputFile, outputFile);
                    break;
                case "lab3":
                    LabRunner.RunLab3(inputFile, outputFile);
                    break;
                default:
                    Console.WriteLine("Unknown lab specified. Use lab1, lab2, or lab3.");
                    break;
            }
        }

        static void HandleSetPathCommand(string[] args)
        {
            string path = null;

            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-p":
                    case "--path":
                        path = args[++i];
                        break;
                }
            }

            if (path == null || !Directory.Exists(path))
            {
                Console.WriteLine("Error: Invalid path specified.");
                return;
            }

            Environment.SetEnvironmentVariable("LAB_PATH", path);
            Console.WriteLine($"LAB_PATH set to {path}");
        }

        static string ResolveFilePath(string filePath, string defaultFileName)
        {
            if (!string.IsNullOrEmpty(filePath)) return filePath;

            string labPath = Environment.GetEnvironmentVariable("LAB_PATH");
            if (!string.IsNullOrEmpty(labPath))
            {
                filePath = Path.Combine(labPath, defaultFileName);
                if (File.Exists(filePath)) return filePath;
            }

            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            filePath = Path.Combine(homePath, defaultFileName);
            return File.Exists(filePath) ? filePath : null;
        }

        static void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  version                Show program version and author");
            Console.WriteLine("  run <lab> [-i file] [-o file]  Run specified lab with optional input/output files");
            Console.WriteLine("  set-path -p path       Set path for LAB_PATH environment variable");
        }
    }
}
