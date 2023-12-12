// Hi Ryan.

class Program
{
    static void Main(){
        try{
            Installer installer = new();
            installer.Initialize();
        } catch(Exception exception) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception);
            Console.ReadKey();
        }
    }
}



