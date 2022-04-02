using Easysave_Core.Business;
using Easysave_Core.Service.SaveService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Easysave_Core.Service
{
    public class SocketServer 
    {

        public static byte[] Serialize(List<Saves> people)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    foreach (Saves saves in people)
                    {
                        writer.Write(saves.SaveName);
                        writer.Write(saves.SourceRepository);
                        writer.Write(saves.DestinationRepository);
                        writer.Write(saves.NbFilesSourceRepository);
                        writer.Write(saves.NbFilesDestinationRepository);
                    }
                }
                return m.ToArray();
            }
        }

        public void ServeurSocket(int nbfilesrc, int nbfiledst, string srcrepo, string dstrepo)
        {
            TcpListener serverSocket = new TcpListener(1302);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();


                clientSocket = serverSocket.AcceptTcpClient();
                handleClinet client = new handleClinet();
                client.startClient(clientSocket, Convert.ToString(counter));

        }
        public class handleClinet
        {
            TcpClient clientSocket;
            string clNo;
            public void startClient(TcpClient inClientSocket, string clineNo)
            {
                this.clientSocket = inClientSocket;
                this.clNo = clineNo;
                Thread ctThread = new Thread(doChat);
                ctThread.Start();
            }

            private void doChat()
            {
                bool statesocket = true;
                int requestCount = 0;
                byte[] bytesFrom = new byte[4096];
                string dataFromClient = null;
                Byte[] sendBytes = null;
                byte[] serverResponse = null;
                string rCount = null;
                requestCount = 0;

                //Read from CLient
                requestCount = requestCount + 1;
                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                Console.WriteLine(" >> " + "From client-" + clNo + dataFromClient);

                //Write to Client
                rCount = Convert.ToString(requestCount);
                List<Saves> saves = SavesListRunning.getThreadsList();

                sendBytes = Serialize(saves);

                //sendBytes = Encoding.ASCII.GetBytes("bonjour");
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();
                //Console.WriteLine(" >> " + serverResponse);
            }
        }
    }
}
