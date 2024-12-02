using System;
using System.IO;

namespace CorexOS.Shell
{
    public static class ScriptRunner
    {
        public static void ExecuteScript(string scriptPath, ref string currentDirectory)
        {
            // Check if the file exists
            if (!File.Exists(scriptPath))
            {
                Console.WriteLine($"Script file not found: {scriptPath}");
                return;
            }

            try
            {
                // Read all lines from the script file
                string[] scriptLines = File.ReadAllLines(scriptPath);

                Console.WriteLine($"Executing script: {scriptPath}");

                foreach (string line in scriptLines)
                {
                    string command = line.Trim();

                    // Skip empty lines and comments
                    if (string.IsNullOrWhiteSpace(command) || command.StartsWith("#"))
                        continue;

                    // Execute each line as a command
                    string[] commandParts = command.Split(' ');
                    CommandList.ExecuteCommand(commandParts, ref currentDirectory);
                }

                Console.WriteLine("Script execution completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing script: {ex.Message}");
            }
        }
    }
}
