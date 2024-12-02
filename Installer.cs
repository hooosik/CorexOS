using System;
using System.IO;
using Sys = Cosmos.System;

namespace CorexOS
{
    public static class Installer
    {
        public static void RunInstaller(ref string currentDirectory)
        {
            DrawBanner();

            Console.WriteLine("Welcome to the CorexOS Installer!");
            Console.WriteLine();

            DrawSeparator();
            Console.WriteLine("Starting system cleanup...");
            DrawSeparator();

            // Clean up the root directory
            CleanRootDirectory();

            Console.WriteLine("System cleanup complete. Setting up CorexOS structure...");
            DrawSeparator();

            // Create main directories
            string corexPath = Path.Combine(currentDirectory, "CorexOS");
            string userPath = Path.Combine(currentDirectory, "User");
            string programmsPath = Path.Combine(currentDirectory, "Programms");

            Directory.CreateDirectory(corexPath);
            Directory.CreateDirectory(userPath);
            Directory.CreateDirectory(programmsPath);

            // Populate directories
            CreateUserDirectories(userPath);
            CreateSystemFiles(corexPath, programmsPath);

            DrawSeparator();
            Console.WriteLine("Installation complete! The system will now reboot...");
            DrawBanner();

            Sys.Power.Reboot(); // Trigger system reboot
        }

        private static void CleanRootDirectory()
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(@"0:\"))
                {
                    if (dir.EndsWith("CorexOS") || dir.EndsWith("User") || dir.EndsWith("Programms"))
                        continue;

                    Directory.Delete(dir, true);
                }

                foreach (var file in Directory.GetFiles(@"0:\"))
                {
                    File.Delete(file);
                }

                Console.WriteLine("All unnecessary files and folders have been deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during cleanup: {ex.Message}");
            }
        }

        private static void CreateUserDirectories(string userPath)
        {
            try
            {
                string[] userDirs = { "Documents", "Downloads", "Pictures", "Videos", "Music" };

                foreach (var dir in userDirs)
                {
                    Directory.CreateDirectory(Path.Combine(userPath, dir));
                }

                Console.WriteLine("User directories created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user directories: {ex.Message}");
            }
        }

        private static void CreateSystemFiles(string corexPath, string programmsPath)
        {
            try
            {
                DrawSection("Creating System Files");

                // Create kernel/system file
                string kernelFilePath = Path.Combine(corexPath, "CorexOS.bin");
                File.WriteAllText(kernelFilePath, "This is the CorexOS kernel file.");

                // Create config file
                string configFilePath = Path.Combine(corexPath, "config.txt");
                File.WriteAllText(configFilePath, "CorexOS Default Configuration");

                // Ask user for username and password
                Console.WriteLine();
                DrawSeparator();
                Console.WriteLine("Create your user account:");
                DrawSeparator();

                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = ReadPassword();

                // Save credentials
                string credentialsFilePath = Path.Combine(corexPath, "credentials.txt");
                File.WriteAllText(credentialsFilePath, $"{username}:{password}");
                Console.WriteLine("User credentials saved successfully.");

                // Create a placeholder application
                string appFilePath = Path.Combine(programmsPath, "ExampleApp.txt");
                File.WriteAllText(appFilePath, "This is an example application placeholder.");

                Console.WriteLine("System files created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating system files: {ex.Message}");
            }
        }

        private static string ReadPassword()
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

        private static void DrawBanner()
        {
            Console.WriteLine("====================================");
            Console.WriteLine("          CorexOS Installer         ");
            Console.WriteLine("====================================");
            Console.WriteLine(@"
  /$$$$$$                                                                   
 /$$__  $$                                                                  
| $$  \__/  /$$$$$$   /$$$$$$   /$$$$$$  /$$   /$$                          
| $$       /$$__  $$ /$$__  $$ /$$__  $$|  $$ /$$/                          
| $$      | $$  \ $$| $$  \__/| $$$$$$$$ \  $$$$/                           
| $$    $$| $$  | $$| $$      | $$_____/  >$$  $$                           
|  $$$$$$/|  $$$$$$/| $$      |  $$$$$$$ /$$/\  $$                          
 \______/  \______/ |__/       \_______/|__/  \__/                          
                                                                            
                                                                            
                                                                            
 /$$$$$$                       /$$               /$$ /$$                    
|_  $$_/                      | $$              | $$| $$                    
  | $$   /$$$$$$$   /$$$$$$$ /$$$$$$    /$$$$$$ | $$| $$  /$$$$$$   /$$$$$$ 
  | $$  | $$__  $$ /$$_____/|_  $$_/   |____  $$| $$| $$ /$$__  $$ /$$__  $$
  | $$  | $$  \ $$|  $$$$$$   | $$      /$$$$$$$| $$| $$| $$$$$$$$| $$  \__/
  | $$  | $$  | $$ \____  $$  | $$ /$$ /$$__  $$| $$| $$| $$_____/| $$      
 /$$$$$$| $$  | $$ /$$$$$$$/  |  $$$$/|  $$$$$$$| $$| $$|  $$$$$$$| $$      
|______/|__/  |__/|_______/    \___/   \_______/|__/|__/ \_______/|__/      
                                                                            
                                                                            
                                                                            
            ");
            Console.WriteLine("====================================");
        }

        private static void DrawSeparator()
        {
            Console.WriteLine("------------------------------------");
        }

        private static void DrawSection(string sectionTitle)
        {
            DrawSeparator();
            Console.WriteLine($":: {sectionTitle} ::");
            DrawSeparator();
        }
    }
}
