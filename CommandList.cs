using System;
using System.IO;
using Sys = Cosmos.System;

namespace CorexOS
{
    public static class CommandList
    {
        public static void ExecuteCommand(string[] commandParts, ref string currentDirectory)
        {
            if (commandParts == null || commandParts.Length == 0)
                return;

            switch (commandParts[0].ToLower())
            {
                case "shutdown":
                    try { Sys.Power.Shutdown(); } catch (Exception e) { Console.WriteLine("Error during shutdown: " + e.Message); }
                    break;

                case "help":
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("hello - Simple OS command");
                    Console.WriteLine("about - Know about OS");
                    Console.WriteLine("ls - List files in the current directory");
                    Console.WriteLine("mkdir [dirname] - Create a new directory");
                    Console.WriteLine("touch [filename] - Create a new empty file");
                    Console.WriteLine("rm [filename] - Remove a file");
                    Console.WriteLine("cd [dirname] - Change directory");
                    Console.WriteLine("tree - Display directory structure");
                    Console.WriteLine("cp [source] [destination] - Copy file");
                    Console.WriteLine("cat [filename] - Display file content");
                    Console.WriteLine("echo [text] > [filename] - Write text to file");
                    Console.WriteLine("reboot - Reboot the system");
                    Console.WriteLine("clear - Clear the console");
                    Console.WriteLine("reinstall - Reinstall CorexOS (Secret Command)");
                    Console.WriteLine("run [filename].sh - Run Shell Commands");
                    break;

                case "about":
                    Console.WriteLine("CorexOS - COSMOS OS based project");
                    Console.WriteLine("Version: 1.0");
                    break;

                case "reboot":
                    try { Sys.Power.Reboot(); } catch (Exception e) { Console.WriteLine("Error during reboot: " + e.Message); }
                    break;

                case "hello":
                    Console.WriteLine("Hello User! Thanks for using CorexOS!");
                    break;

                case "ls":
                    ListFiles(currentDirectory);
                    break;

                case "mkdir":
                    if (commandParts.Length > 1)
                    {
                        CreateDirectory(commandParts[1], currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Usage: mkdir [dirname]");
                    }
                    break;

                case "touch":
                    if (commandParts.Length > 1)
                    {
                        CreateFile(commandParts[1], currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Usage: touch [filename]");
                    }
                    break;

                case "rm":
                    if (commandParts.Length > 1)
                    {
                        RemoveFile(commandParts[1], currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Usage: rm [filename]");
                    }
                    break;

                case "cd":
                    if (commandParts.Length > 1)
                    {
                        ChangeDirectory(commandParts[1], ref currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Usage: cd [dirname]");
                    }
                    break;

                case "tree":
                    DisplayTree(currentDirectory, "");
                    break;

                case "cp":
                    if (commandParts.Length > 2)
                    {
                        CopyFile(commandParts[1], commandParts[2], currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Usage: cp [source] [destination]");
                    }
                    break;

                case "cat":
                    if (commandParts.Length > 1)
                    {
                        DisplayFileContent(commandParts[1], currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Usage: cat [filename]");
                    }
                    break;

                case "echo":
                    if (commandParts.Length > 2 && commandParts[1] == ">")
                    {
                        WriteToFile(commandParts[2], string.Join(" ", commandParts, 3, commandParts.Length - 3), currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Usage: echo [text] > [filename]");
                    }
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "reinstall": // Secret command to reinstall CorexOS
                    Console.WriteLine("Starting CorexOS reinstallation...");
                    Installer.RunInstaller(ref currentDirectory); // Re-run the installer
                    break;
                case "run":
                    if (commandParts.Length > 1)
                    {
                        string scriptPath = Path.Combine(currentDirectory, commandParts[1]);
                        Shell.ScriptRunner.ExecuteScript(scriptPath, ref currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Usage: run [script.sh]");
                    }
                    break;

                default:
                    Console.WriteLine("No such command");
                    break;
            }
        }

        // Existing methods for directory and file handling remain unchanged.
        private static void ListFiles(string currentDirectory)
        {
            try
            {
                string[] files = Directory.GetFiles(currentDirectory);
                string[] directories = Directory.GetDirectories(currentDirectory);

                foreach (var dir in directories)
                {
                    Console.WriteLine("<DIR> " + Path.GetFileName(dir));
                }
                foreach (var file in files)
                {
                    Console.WriteLine(Path.GetFileName(file));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error listing files: " + ex.Message);
            }
        }

        private static void CreateDirectory(string dirName, string currentDirectory)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dirName))
                {
                    Console.WriteLine("Directory name cannot be null or empty.");
                    return;
                }
                Directory.CreateDirectory(Path.Combine(currentDirectory, dirName));
                Console.WriteLine("Directory created: " + dirName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating directory: " + ex.Message);
            }
        }

        private static void CreateFile(string fileName, string currentDirectory)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Console.WriteLine("File name cannot be null or empty.");
                    return;
                }
                string filePath = Path.Combine(currentDirectory, fileName);
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                    Console.WriteLine("File created: " + fileName);
                }
                else
                {
                    Console.WriteLine("File already exists: " + fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating file: " + ex.Message);
            }
        }

        private static void RemoveFile(string fileName, string currentDirectory)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Console.WriteLine("File name cannot be null or empty.");
                    return;
                }
                string filePath = Path.Combine(currentDirectory, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine("File removed: " + fileName);
                }
                else
                {
                    Console.WriteLine("File not found: " + fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error removing file: " + ex.Message);
            }
        }

        private static void ChangeDirectory(string dirName, ref string currentDirectory)
        {
            try
            {
                if (dirName == "/")
                {
                    currentDirectory = @"0:\";
                    Console.WriteLine("Changed directory to root: " + currentDirectory);
                }
                else if (dirName == "..")
                {
                    if (currentDirectory != @"0:\")
                    {
                        currentDirectory = Directory.GetParent(currentDirectory)?.FullName ?? currentDirectory;
                        Console.WriteLine("Changed directory to: " + currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Already at root directory.");
                    }
                }
                else
                {
                    string newPath = Path.Combine(currentDirectory, dirName);
                    if (Directory.Exists(newPath))
                    {
                        currentDirectory = newPath;
                        Console.WriteLine("Changed directory to: " + currentDirectory);
                    }
                    else
                    {
                        Console.WriteLine("Directory not found: " + dirName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error changing directory: " + ex.Message);
            }
        }

        private static void DisplayTree(string path, string indent)
        {
            try
            {
                Console.WriteLine(indent + Path.GetFileName(path));
                string[] directories = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                foreach (var dir in directories)
                {
                    DisplayTree(dir, indent + "  ");
                }
                foreach (var file in files)
                {
                    Console.WriteLine(indent + "  " + Path.GetFileName(file));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error displaying tree: " + ex.Message);
            }
        }

        private static void CopyFile(string source, string destination, string currentDirectory)
        {
            try
            {
                string sourcePath = Path.Combine(currentDirectory, source);
                string destinationPath = Path.Combine(currentDirectory, destination);
                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, destinationPath, true);
                    Console.WriteLine("File copied to: " + destination);
                }
                else
                {
                    Console.WriteLine("Source file not found: " + source);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error copying file: " + ex.Message);
            }
        }

        private static void DisplayFileContent(string fileName, string currentDirectory)
        {
            try
            {
                string filePath = Path.Combine(currentDirectory, fileName);
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    Console.WriteLine(content);
                }
                else
                {
                    Console.WriteLine("File not found: " + fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error displaying file content: " + ex.Message);
            }
        }

        private static void WriteToFile(string fileName, string content, string currentDirectory)
        {
            try
            {
                string filePath = Path.Combine(currentDirectory, fileName);
                File.WriteAllText(filePath, content);
                Console.WriteLine("Content written to: " + fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to file: " + ex.Message);
            }
        }
    }
}
