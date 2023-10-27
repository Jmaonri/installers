using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program{
    static void Main(){
        try
        {
            // Set console text color.
            Console.ForegroundColor = ConsoleColor.Blue;

            // Introduction.
            Console.WriteLine("Welcome to the momo cobblemon installer.\n" +
                            "This installer will create a new profile on your Minecraft installer and download all the required mods.\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[0] No/Exit\n[1] Yes/Continue");

            // Exit on 0/No.
            if(Console.ReadLine() == "0"){
                Environment.Exit(0);
            }

            // Clear console.
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;

            // Get the user's %APPDATA% directory.
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // If .minecraft directory is not found, print error and exit installer.
            if (!Directory.Exists(Path.Combine(appDataPath, ".minecraft"))){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(".minecraft folder not found.\n" + 
                                "This installer only supports installations on the C drive.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to exit..");
                Console.ReadLine();
                Environment.Exit(0);
            }

            // If launcher_profiles.json does not exists, print error and exit installer.
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "launcher_profiles.json"))){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("launcher profiles not found.\n" + 
                                "Have you run your minecraft launcher after installing it?");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to exit..");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("launcher_profiles json found");

            // Create Custom Installations folder in .minecraft if it does not exist.
            if (!Directory.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations"))){
                Directory.CreateDirectory(Path.Combine(appDataPath, ".minecraft", "custom installations"));
                Console.WriteLine("Created custom installations folder");
            }

            // Create cobblemon folder and subfolders.
            if (!Directory.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1"))){
                Directory.CreateDirectory(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1"));
                Console.WriteLine("Created cobblemon installation folder");
            }
            if (!Directory.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "resourcepacks"))){
                Directory.CreateDirectory(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "resourcepacks"));
                Console.WriteLine("Created resourcepacks folder");
            }
            if (!Directory.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "shaderpacks"))){
                Directory.CreateDirectory(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "shaderpacks"));
                Console.WriteLine("Created shaderpacks folder");
            }
            if (!Directory.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods"))){
                Directory.CreateDirectory(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods"));
                Console.WriteLine("Created mods folder");
            }
            
            // Copy options file if it exists and there is already one not created in the new installation.
            if (File.Exists(Path.Combine(appDataPath, ".minecraft", "options.txt")) && !File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "options.txt"))){
                File.Copy(Path.Combine(appDataPath, ".minecraft", "options.txt"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "options.txt"));
                Console.WriteLine("Copied options.txt file to new installation");
            }

            Thread.Sleep(1000);

            // Get the current directory where the .exe is run.
            string currentDirectory = Environment.CurrentDirectory;

            // Install Fabric.
            Directory.CreateDirectory(Path.Combine(appDataPath, ".minecraft", "versions", "fabric-loader-0.14.24-1.20.1"));
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "versions", "fabric-loader-0.14.24-1.20.1", "fabric-loader-0.14.24-1.20.1.jar"))){
                File.Copy(Path.Combine(currentDirectory, "files", "fabric-loader-0.14.24-1.20.1.jar"), Path.Combine(appDataPath, ".minecraft", "versions", "fabric-loader-0.14.24-1.20.1", "fabric-loader-0.14.24-1.20.1.jar"));
                Console.WriteLine("Fabric jar installed successfully");
            }
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "versions", "fabric-loader-0.14.24-1.20.1", "fabric-loader-0.14.24-1.20.1.json"))){
                File.Copy(Path.Combine(currentDirectory, "files", "fabric-loader-0.14.24-1.20.1.json"), Path.Combine(appDataPath, ".minecraft", "versions", "fabric-loader-0.14.24-1.20.1", "fabric-loader-0.14.24-1.20.1.json"));
                Console.WriteLine("Fabric json installed successfully");
            }

            // Wait 1 second.
            Thread.Sleep(1000);

            // Install mods.
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "sodium.jar"))){
                File.Copy(Path.Combine(currentDirectory, "files", "sodium.jar"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "sodium.jar"));
            }
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "modmenu.jar"))){
                File.Copy(Path.Combine(currentDirectory, "files", "modmenu.jar"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "modmenu.jar"));
            }
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "logical_zoom.jar"))){
                File.Copy(Path.Combine(currentDirectory, "files", "logical_zoom.jar"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "logical_zoom.jar"));
            }
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "fabric-api.jar"))){
                File.Copy(Path.Combine(currentDirectory, "files", "fabric-api.jar"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "fabric-api.jar"));
            }
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "Cobblemon.jar"))){
                File.Copy(Path.Combine(currentDirectory, "files", "Cobblemon.jar"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "Cobblemon.jar"));
            }
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "iris-mc.jar"))){
                File.Copy(Path.Combine(currentDirectory, "files", "iris-mc.jar"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "iris-mc.jar"));
            }
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "cullleaves.jar"))){
                File.Copy(Path.Combine(currentDirectory, "files", "cullleaves.jar"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "cullleaves.jar"));
            }
            if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "jei.jar"))){
                File.Copy(Path.Combine(currentDirectory, "files", "jei.jar"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "mods", "jei.jar"));
            }


            Console.WriteLine("Mods installed");

            Thread.Sleep(1000);

            Console.Clear();
            Console.WriteLine("Would you like to install that one resourcepack and shader?\n" + 
                              "(If you don't know what im talking about you should probably choose no)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[0] No\n[1] Yes");

            Console.ForegroundColor = ConsoleColor.Blue;
            
            // User picked yes, install shaderpack and resourcepack
            if(Console.ReadLine() == "1"){
                Console.Clear();
                if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "resourcepacks", "the_CraftTM.zip"))){
                    File.Copy(Path.Combine(currentDirectory, "files", "the_CraftTM.zip"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "resourcepacks", "the_CraftTM.zip"));
                }
                if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "resourcepacks", "Better-Leaves.zip"))){
                    File.Copy(Path.Combine(currentDirectory, "files", "Better-Leaves.zip"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "resourcepacks", "Better-Leaves.zip"));
                }
                if (!File.Exists(Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "shaderpacks", "ComplementaryUnbound.zip"))){
                    File.Copy(Path.Combine(currentDirectory, "files", "ComplementaryUnbound.zip"), Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1", "shaderpacks", "ComplementaryUnbound.zip"));
                }

                Console.WriteLine("Resourcepack and Shaders installed");
            }

            // Cobblemon Logo in base64.
            var cobblemonLogo = "data:image/png;base64,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";

            string cobblemonInstallationDirectory = Path.Combine(appDataPath, ".minecraft", "custom installations", "cobblemon 1.20.1");

            // Edit launcher_profiles.json
            Console.WriteLine("Editing launcher_profiles json");
            string jsonFilePath = Path.Combine(appDataPath, ".minecraft", "launcher_profiles.json");
            string json = File.ReadAllText(jsonFilePath);
            JObject data = JObject.Parse(json);
            JObject newProfile = new JObject
            {
                { "created", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
                { "gameDir", cobblemonInstallationDirectory},
                { "icon", cobblemonLogo },
                { "javaArgs", "-Xmx6G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M" },
                { "lastUsed", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
                { "lastVersionId", "fabric-loader-0.14.24-1.20.1" },
                { "name", "Cobblemon 1.20.1" },
                { "type", "custom" }
            };
            data["profiles"]["Cobblemon"] = newProfile;
            string updatedJson = data.ToString(Formatting.Indented);
            File.WriteAllText(jsonFilePath, updatedJson);

            Thread.Sleep(1000);
            Console.WriteLine("launcher profiles json succesfully edited");
            Thread.Sleep(1000);

            Console.Clear();
            Console.WriteLine("Launcher has finished. Press any key to exit..");
            Console.ReadLine();
        }
        catch (Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + exception.Message);
            Console.ReadLine();
        }
    }
}


