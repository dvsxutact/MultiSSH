using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace MultiSsh
{
    internal class Program
    {
        private static readonly Dictionary<string, SshClient> _activeClients = new Dictionary<string, SshClient>();
        private static bool _keepRunning = true;

        public static void Main(string[] args)
        {
            do
            {
                string input = Console.ReadLine();

                if (input == null)
                    continue;

                ParseInput(input.ToLower());
/*
                switch (input.ToLower())
                {
                    case "connect":
                        ParseInput(input.ToLower());
                        break;

                    case "disconnect":
                        ParseInput(input.ToLower());
                        break;

                    case "status":
                        ParseInput(input.ToLower());
                        break;

                    case "help":
                        HelpCmds.GetHelp();
                        break;

                    default:
                        Console.WriteLine("Sorry, i did not understand your request. try using the help command.");
                        break;
                }
                */
            } while (_keepRunning);

            HandleQuit();
        }

        private static void ParseInput(string input)
        {
            if (input == null)
                return;

            string[] sptInput = input.Split(' ');
            if (sptInput.Length >= 1)
            {
                // 0 should always be the command we're running, 1 should be the first parameter.
                switch (sptInput[0])
                {
                    case "connect":
                    {
                        if (sptInput.Length < 5)
                            HelpCmds.GetHelp("connect");

                        string host = sptInput[1];
                        string username = sptInput[2];
                        string password = sptInput[3];
                        string name = sptInput[4];

                        // TODO: find a way to handle output from the server.
                        string error = "";
                        CreateSshConnection(host, username, password, out error, name);

                        if (!string.IsNullOrEmpty(error))
                        {
                            Console.WriteLine("Connect error: {0}", error);
                        }
                    }
                        break;

                    // execute <options> <command> <params>
                    case "execute":
                    {
                    }
                        break;

                    case "exec":
                    {
                    }
                        break;

                    case "disconnect":
                        break;

                    case "status":
                        GetStatus();
                        break;

                    case "help":
                        HelpCmds.GetHelp();
                        break;

                    case "!quit":
                        _keepRunning = false;
                        break;

                    default:
                        Console.WriteLine("Sorry, i did not understand your request. try using the help command.");
                        break;
                }
            }
        }

        // TODO: Add methods for using other auth methods.
        public static bool CreateSshConnection(string host, string username, string password, out string error,
            string name = "")
        {
            bool toReturn = false;
            error = "";

            try
            {
                // Setup our auth method (I.E set username and password)
                AuthenticationMethod auth = new PasswordAuthenticationMethod("username", "password");

                // Create our connection information
                ConnectionInfo connInfo = new ConnectionInfo("host", "username", auth);

                // now connect to the server.
                SshClient client = new SshClient(connInfo);
                client.Connect();

                if (string.IsNullOrEmpty(name))
                    _activeClients.Add("", client);
                else
                    _activeClients.Add(name, client);

                Console.WriteLine("Connected to: {0}", host);

                toReturn = true;
            }
            catch (SshConnectionException sce)
            {
                // TODO: Handle this correctly.
                Console.WriteLine(sce.ToString());
            }
            catch (SshException se)
            {
                // TODO: Handle this correctly.
                Console.WriteLine(se.ToString());
            }
            catch (SocketException se)
            {
                if (se.Message == "No such host is known")
                {
                    Console.WriteLine("Could not connect to host. Please check the hostname.");
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle this correctly.
                Console.WriteLine(ex.ToString());
            }
            finally
            {
            }

            return (toReturn);
        }

        private static void GetStatus()
        {
            Console.WriteLine("Checking status of connections.");

            if (_activeClients.Count >= 1)
            {
                Console.WriteLine("");
                Console.WriteLine("Name        IPAddress        ConnectionStatus");
                foreach (var kvp in _activeClients)
                {
                    string statusString = "";
                    if (kvp.Value.IsConnected)
                        statusString = "Connected";
                    else
                        statusString = "Disconnected";

                    Console.WriteLine("{0}        {1}        {2}", kvp.Key, kvp.Value.ConnectionInfo.Host,
                        statusString);
                }
            }
            else
            {
                Console.WriteLine("We're checking into it, please give us just a moment.");
                Thread.Sleep(800);
                Console.WriteLine("Still thinking.. hang in there!");
                Thread.Sleep(500);
                Console.WriteLine("Ok, we're sure now.. you dont have any open connections.");
            }

            Console.WriteLine("Connected to {0} clients. type status details for more information.");
        }

        private static void HandleQuit()
        {
            foreach (var keyValuePair in _activeClients)
            {
                keyValuePair.Value.Disconnect();
                keyValuePair.Value.Dispose();
            }
        }
    }
}