using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Data.SqlTypes;

/// <summary>
/// This class holds all the methods needed to create directories, move
/// files, and create a minecraft launcher profile.
/// </summary>
public class Installer
{
    private const string DOT_MINECRAFT_FOLDER_NAME = ".minecraft";
    private readonly string DOT_MINECRAFT_FOLDER;


    private const string LAUNCHER_PROFILES_JSON = "launcher_profiles.json";
    private const string VERSIONS_FOLDER = "versions";
    private const string FABRIC_FOLDER = "fabric-loader-0.14.24-1.20.1";
    private const string CUSTOM_INSTALLATIONS_FOLDER = "custom installations";
    private const string PREVIOUS_CUSTOM_INSTALLATION_FOLDER = "cobblemon 1.20.1";
    private const string CUSTOM_INSTALLATION_FOLDER = "The Bros v1";
    private const string RESOURCEPACK_FOLDER = "resourcepacks";
    private const string SHADERPACK_FOLDER = "shaderpacks";
    private const string MODS_FOLDER = "mods";
    private const string CONFIG_FOLDER = "config";
    private const string OPTIONS_FILE = "options.txt";
    private const string JOURNEYMAP_FOLDER = "journeymap";
    private const string JOURNEYMAP_VERSION = "5.9";
    
    private readonly string CURRENT_DIRECTORY;
    private readonly string APP_DATA_DIR;
    
    private const string FILES_FOLDER = "files";
    private const string FABRIC_JAR = "fabric-loader-0.14.24-1.20.1.jar";
    private const string FABRIC_JSON = "fabric-loader-0.14.24-1.20.1.json";


    public Installer()
    {
        APP_DATA_DIR = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        CURRENT_DIRECTORY = Environment.CurrentDirectory;
        DOT_MINECRAFT_FOLDER = Path.Combine(APP_DATA_DIR, DOT_MINECRAFT_FOLDER_NAME);
    }

    public void Initialize(){
        Console.ForegroundColor = ConsoleColor.Blue;

        string prompt = "Welcome to the installer.\n" +
                        "This installer will create a new profile on your Minecraft installer and install all the required mods.\n";
        if(!PromptInstallation(prompt)){
            Environment.Exit(0);
        }

        // If .minecraft directory is not found, print error and exit installer.
        if (!Directory.Exists(Path.Combine(DOT_MINECRAFT_FOLDER))){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(".minecraft folder not found.\n" + 
                              "This installer only supports installations on the C drive.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to exit..");
            Console.ReadLine();
            Environment.Exit(0);
        }

        // If launcher_profiles.json does not exists, print error and exit installer.
        if (!File.Exists(Path.Combine(DOT_MINECRAFT_FOLDER, LAUNCHER_PROFILES_JSON))){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("launcher profiles not found.\n" + 
                                "Have you run your minecraft launcher after installing it?");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to exit..");
            Console.ReadLine();
            Environment.Exit(0);
        }

        // Create Custom Installations folder and its subfolders in .minecraft if they do not exist.
        CreateFolder(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER); // custom installations
        CreateFolder(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER), CUSTOM_INSTALLATION_FOLDER); // The Bros v1
        CreateFolder(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER),  RESOURCEPACK_FOLDER); // resourcepacks
        CreateFolder(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER), SHADERPACK_FOLDER); // shaderpacks
        CreateFolder(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER), MODS_FOLDER); // mods
        CreateFolder(Path.Combine(DOT_MINECRAFT_FOLDER, VERSIONS_FOLDER), FABRIC_FOLDER); // fabric
        CreateFolder(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER), CONFIG_FOLDER); // config
        CreateFolder(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER), JOURNEYMAP_FOLDER); // journeymap
        CreateFolder(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, JOURNEYMAP_FOLDER), CONFIG_FOLDER); // journeymap config
        CreateFolder(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, JOURNEYMAP_FOLDER, CONFIG_FOLDER), JOURNEYMAP_VERSION); // journeymap version

        // Start moving files.
        CopyOptionsFile();
        CopyJourneymapFiles();
        CopyFile(Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, "fabric", FABRIC_JAR), Path.Combine(DOT_MINECRAFT_FOLDER, VERSIONS_FOLDER, FABRIC_FOLDER, FABRIC_JAR)); // im tired of making const's pa pa.
        CopyFile(Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, "fabric", FABRIC_JSON), Path.Combine(DOT_MINECRAFT_FOLDER, VERSIONS_FOLDER, FABRIC_FOLDER, FABRIC_JSON)); // Fabric .json
        CopyFile(Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, "ComplementaryUnbound.zip"), Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, SHADERPACK_FOLDER, "ComplementaryUnbound.zip")); // complementary unbound shaderpack.
        CopyShaderConfig();
        CopyFile(Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, "the_CraftTM.zip"), Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, RESOURCEPACK_FOLDER, "The_CraftTM.zip")); // Resourcepack
        CopyFile(Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, "Better Leaves.zip"), Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, RESOURCEPACK_FOLDER, "Better Leaves.zip")); // Resourcepack TWO!!!
        CopyFile(Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, "servers.dat"), Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, "servers.dat")); // Servers.dat

        // Install all mods.
        string modsSource = Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, MODS_FOLDER);
        string[] mods = Directory.GetFiles(modsSource);
        foreach(string mod in mods){
            string modName = Path.GetFileName(mod);
            string modDestination = Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, MODS_FOLDER, modName);

            CopyFile(mod, modDestination);
        }

        CopyFile(Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, "iris.properties"), Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, CONFIG_FOLDER, "iris.properties"));

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Installed all mods");

        EditLauncherProfiles();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.WriteLine("Installer has finished. Press any key to exit..");
        Console.ReadKey();
    }

    /// <summary>
    /// Prompts the user with a choice they can respond to with yes or no.
    /// </summary>
    /// <param name="prompt">The prompt that will be printed to the user.</param>
    /// <returns>True or False depending on user choice.</returns>
    private static bool PromptInstallation(string prompt){
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(prompt);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[0] No\n[1] Yes/Continue");

        if(Console.ReadLine() == "0"){
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Creates a folder. Warns if folder already exists.
    /// </summary>
    /// <param name="folderPath">The path in which the folder will be created.</param>
    /// <param name="folderName">The name which will be given to the newly created folder.</param>
    private static void CreateFolder(string folderPath, string folderName){
        if(Directory.Exists(Path.Combine(folderPath, folderName))){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{folderName} already exists. Skipping.");
            return;
        }

        Directory.CreateDirectory(Path.Combine(folderPath, folderName));
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{folderName} folder created.");
    }

    /// <summary>
    /// Copies a file. Warns if file already exists in destination.
    /// </summary>
    /// <param name="fileOrigin">The file to be copied. This is a path.</param>
    /// <param name="fileDestination">The destination of the copied file. This is a path</param>
    private static void CopyFile(string fileOrigin, string fileDestination){
        if(File.Exists(fileDestination)){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{fileDestination} already exists. Skipping.");
            return;
        }

        File.Copy(fileOrigin, fileDestination);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{fileDestination} installed.");
    }

    /// <summary>
    /// Copies the 'options.txt' file from either an older custom installation 
    /// or the root of the .minecraft folder to the new custom installation.
    /// Warns if file already exists at destination.
    /// </summary>
    private void CopyOptionsFile(){
        if(!File.Exists(Path.Combine(DOT_MINECRAFT_FOLDER, OPTIONS_FILE))){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("options.txt does not exist in .minecraft folder. An options file will not be copied to the new installation");
            return;
        }

        if (File.Exists(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, OPTIONS_FILE))){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("options.txt already exists. Skipping.");
            return;
        }

        // Priorotize previous 'custom installation'. For this time, cobblemon.
        if (File.Exists(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, PREVIOUS_CUSTOM_INSTALLATION_FOLDER, OPTIONS_FILE))){
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Retrieving options.txt file from cobblemon folder");
            CopyFile(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, PREVIOUS_CUSTOM_INSTALLATION_FOLDER, OPTIONS_FILE), Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, OPTIONS_FILE));
            return;
        }

        CopyFile(Path.Combine(DOT_MINECRAFT_FOLDER, OPTIONS_FILE), Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, OPTIONS_FILE));
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Copied options.txt file to new installation");
    }

    /// <summary>
    /// Copies the complementary unbound shader config file from an older custom installation.
    /// Warns if file already exists.
    /// </summary>
    private void CopyShaderConfig(){
        if (!File.Exists(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, PREVIOUS_CUSTOM_INSTALLATION_FOLDER, SHADERPACK_FOLDER, "ComplementaryUnbound.zip.txt"))){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("No previous shader config file found");
            return;
        }

        if(File.Exists(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, SHADERPACK_FOLDER, "ComplementaryUnbound.zip.txt"))){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Shader config file already exists. Skipping.");
            return;
        }

        File.Copy(Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, PREVIOUS_CUSTOM_INSTALLATION_FOLDER, SHADERPACK_FOLDER, "ComplementaryUnbound.zip.txt"), Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, SHADERPACK_FOLDER, "ComplementaryUnbound.zip.txt"));
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Copied shaderpack config file to new installation");
    }

    /// <summary>
    /// Edits the 'launcher_profiles.json' and creates a new profile key containing
    /// the directory of the new installation, a custom logo, and java args.
    /// </summary>
    private void EditLauncherProfiles(){
        string profileLogo = File.ReadAllText(Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, "image.txt"));

        string installationDirectory = Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER);
        string json = File.ReadAllText(Path.Combine(DOT_MINECRAFT_FOLDER, LAUNCHER_PROFILES_JSON));

        JObject jsonData = JObject.Parse(json);
        JObject newProfile = new JObject
        {
            { "created", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
            { "gameDir", installationDirectory},
            { "icon", profileLogo },
            { "javaArgs", "-Xmx6G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M" },
            { "lastUsed", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
            { "lastVersionId", FABRIC_FOLDER }, // Folder has same name as version id so we can just use it.
            { "name", "The Bros v1" },
            { "type", "custom" }
        };

        #pragma warning disable CS8602 // Dereference of a possibly null reference.
        jsonData["profiles"]["The Bros v1"] = newProfile;
        #pragma warning restore CS8602 // Dereference of a possibly null reference.

        string updatedJson = jsonData.ToString(Formatting.Indented);
        File.WriteAllText(Path.Combine(DOT_MINECRAFT_FOLDER, LAUNCHER_PROFILES_JSON), updatedJson);

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("launcher_profiles.json edited successfully");
    }

    /// <summary>
    /// Runs a foreach loop that copies all the files from the current dir
    /// journeymap folder to the custom installation's journeymap folder.
    /// </summary>
    private void CopyJourneymapFiles(){
        string filesSource = Path.Combine(CURRENT_DIRECTORY, FILES_FOLDER, JOURNEYMAP_FOLDER);
        string[] files = Directory.GetFiles(filesSource);
        foreach(string file in files){
            string fileName = Path.GetFileName(file);
            string fileDestination = Path.Combine(DOT_MINECRAFT_FOLDER, CUSTOM_INSTALLATIONS_FOLDER, CUSTOM_INSTALLATION_FOLDER, JOURNEYMAP_FOLDER, CONFIG_FOLDER, JOURNEYMAP_VERSION, fileName);

            CopyFile(file, fileDestination);
        }
    }
}