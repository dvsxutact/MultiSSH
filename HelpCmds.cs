using System;

namespace MultiSsh
{
    public class HelpCmds
    {
        public static void GetHelp()
        {
            Console.WriteLine("Thanks for asking for help. you can type help <command> to get more information about each command");
            Console.WriteLine("");
            Console.WriteLine("Command        Description");
            Console.WriteLine("connect        Connect to an SSH server.");
            Console.WriteLine("disconnect    Disconnects you from the ssh server.");
            Console.WriteLine("status        Displays the status of your connections");
            Console.WriteLine("quit          use !quit to exit the software.");
            
        }
        
        public static void GetHelp(string command)
        {
            Console.WriteLine("Thanks for asking for help. you can type help <command> to get more information about each command");
            Console.WriteLine("");
            Console.WriteLine("Command        Description");
            Console.WriteLine("connect        Connect to an SSH server.");
            Console.WriteLine("disconnect    Disconnects you from the ssh server.");
            Console.WriteLine("status        Displays the status of your connections");
            Console.WriteLine("quit          use !quit to exit the software.");
            
        }
    }
}