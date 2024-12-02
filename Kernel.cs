using System;
using System.IO;
using Cosmos.System.FileSystem;
using Sys = Cosmos.System;

namespace CorexOS
{
    public class Kernel : Sys.Kernel
    {
        private string currentDirectory = @"0:\"; // Default current directory
        private CosmosVFS vfs;

        protected override void BeforeRun()
        {
            Console.WriteLine("Initializing CorexOS...");

            // Register the Virtual File System (VFS)
            try
            {
                vfs = new CosmosVFS();
                Cosmos.System.FileSystem.VFS.VFSManager.RegisterVFS(vfs);
                Console.WriteLine("Virtual File System initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Virtual File System: {ex.Message}");
                return;
            }

            // Check for system files and run the installer if necessary
            if (!File.Exists(@"0:\CorexOS\config.txt"))
            {
                Console.WriteLine("System files missing. Running installer...");
                Installer.RunInstaller(ref currentDirectory);
            }

            // Perform user login
            PerformLogin();
            Console.WriteLine("Login successful. CorexOS is ready.");
        }

        protected override void Run()
        {
            Console.WriteLine("CorexOS is now running. Type 'help' for a list of commands.");

            while (true)
            {
                try
                {
                    // Handle terminal input
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{currentDirectory}:> ");
                    Console.ResetColor();

                    string input = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        string[] commandParts = input.Split(' ');
                        CommandList.ExecuteCommand(commandParts, ref currentDirectory);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in main loop: {ex.Message}");
                }
            }
        }

        private void PerformLogin()
        {
            string credentialsPath = @"0:\CorexOS\credentials.txt";

            if (!File.Exists(credentialsPath))
            {
                Console.WriteLine("No user credentials found. Please reinstall CorexOS.");
                return;
            }

            string[] credentials = File.ReadAllText(credentialsPath).Split(':');
            string savedUsername = credentials[0];
            string savedPassword = credentials[1];

            while (true)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();

                Console.Write("Password: ");
                string password = ReadPassword();

                if (username == savedUsername && password == savedPassword)
                {
                    break;
                }

                Console.WriteLine("Invalid username or password. Please try again.");
            }
        }

        private string ReadPassword()
        {
            System.Text.StringBuilder password = new System.Text.StringBuilder();
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return password.ToString();
        }
    }
}
