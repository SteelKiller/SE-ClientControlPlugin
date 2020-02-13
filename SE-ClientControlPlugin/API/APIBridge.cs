using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using SE_ClientControlPlugin.API.WebResponse;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;

namespace SE_ClientControlPlugin.API
{
    class APIBridge
    {

        List<ICommandsBase> commands;
        public ICommandsBase Command { get; private set; }

        private Thread listenerThread;
        public bool isRunning;
        private bool commandReady;
        public bool CommandReady { get { bool tmpVar = commandReady; commandReady = false; return tmpVar; } }
        public int Port { get; }
        public string SecurityKey { get; }

        public APIBridge(int port)
        {
            Port = port;
            commands = new List<ICommandsBase>()
            {
                new MoveGridTo()
            };
            SecurityKey = "V7ry55j2i3WTaLBYxDuFtg==";
            Command = null;
            commandReady = false;
            isRunning = true;
            listenerThread = new Thread(PluginProcessing);
            listenerThread.Start();
        }

        private string GenerateSecurityKey()
        {
            return "V7ry55j2i3WTaLBYxDuFtg==";
        }


        private string[] GetArgumentsFromLink(string link)
        {
            string[] firstArgs = link.Split('&');
            return firstArgs.Select(arg => { return arg.Split('=')[1]; }).ToArray();
        }
        private string[] ReceiveData(Socket client)
        {
            string ReceivedData = "";
            byte[] bytesReceived = new byte[1024];
            int bytes = 0;

            Regex LinkPattern = new Regex(@"GET\s\/customapi\/(\w*)\?(.*)\sHTTP");
            Regex AuthorizationPattern = new Regex(@"Authorization:\s(\d*):(.*)\r\n");
            Regex DatePattern = new Regex(@"Date:\s(.*)\r\n");
            Thread.Sleep(10);
            if (client.Available == 0)
                return null;

            do
            {
                bytes = client.Receive(bytesReceived, bytesReceived.Length, 0);
                ReceivedData += Encoding.ASCII.GetString(bytesReceived, 0, bytes);
            }
            while (client.Available > 0);

            string AuthorizationNonce = AuthorizationPattern.Match(ReceivedData).Groups[1].Value;
            string AuthorizationHash = AuthorizationPattern.Match(ReceivedData).Groups[2].Value;
            string AuthorizationDate = DatePattern.Match(ReceivedData).Groups[1].Value;
            string CommandName = LinkPattern.Match(ReceivedData).Groups[1].Value;
            string CommandArgs = HttpUtility.UrlDecode(LinkPattern.Match(ReceivedData).Groups[2].Value);
            string checkMessage = $"/customapi/{CommandName}?{CommandArgs}\r\n{AuthorizationNonce}\r\n{AuthorizationDate}\r\n";

            if (!Authorization(AuthorizationHash, checkMessage))
                return null;

            return new string[] { CommandName }.Concat(GetArgumentsFromLink(CommandArgs)).ToArray();

        }

        private bool Authorization(string hash, string msg)
        {
            byte[] messageBuffer = Encoding.UTF8.GetBytes(msg);

            byte[] key = Convert.FromBase64String(SecurityKey);
            byte[] computedHash;
            using (HMACSHA1 hmac = new HMACSHA1(key))
            {
                computedHash = hmac.ComputeHash(messageBuffer);
            }

            if (!Convert.ToBase64String(computedHash).Equals(hash))
                return false;
            return true;

        }

        private bool AnswerWaiting()
        {
            DateTime beginTimeWaiting = DateTime.Now;
            commandReady=true;

            while (Command.getCommandResult() == EnumCommands.WAITING &&
                (beginTimeWaiting - DateTime.Now).TotalSeconds <= 2)
            {
                Thread.Sleep(50);
            }
            return Command.getCommandResult() == EnumCommands.SUCCES;
        }

        private bool SendCommandArgs(string[] args)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].ProcessingInThread(args))
                {
                    Command = commands[i];
                    return true;
                }
            }
            return false;

        }

        private void PluginProcessing()
        {
            IPAddress IA = IPAddress.Parse("127.0.0.1");
            IPEndPoint IEP = new IPEndPoint(IA, Port);
            //Socket listener = new Socket(IEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            TcpListener listener = new TcpListener(IA,Port);
            Socket client;
            listener.Start();
            while (isRunning)
            {
                if (!listener.Pending())
                {
                    Thread.Sleep(50);
                    continue;
                }

                client = listener.AcceptSocket();

                string[] args;                
                if ((args = ReceiveData(client)) == null)
                {
                    SendClientResponse(client, new Forbidden());
                    continue;
                }
                
                if (!SendCommandArgs(args))
                {                    
                    SendClientResponse(client, new NotFound());
                    continue;
                }

                if (!AnswerWaiting())
                {
                    SendClientResponse(client, new RequestTimeout());
                    continue;
                }

                SendClientResponse(client, Command.Response());
                //Thread.Sleep(100);
            }


        }

        private void SendClientResponse(Socket client, IResponseBase response)
        {
            client.Send(Encoding.ASCII.GetBytes(response.GetResponseString()));           
            client.Close();
            client.Dispose();          
        }
    }
}
