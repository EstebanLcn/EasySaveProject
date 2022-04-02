using Deported_Client.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Deported_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Saves> savesList = new List<Saves>();
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;
        public MainWindow()
        {

            InitializeComponent();
            msg("Client Started");
            clientSocket.Connect("127.0.0.1", 1302);
            label1.Width = 400;
            label1.Content = "Client Socket Program - Server Connected ...";
            



        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                __ProjectSaveListView_.ItemsSource = null;
                serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes(" Message $");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                byte[] inStream = new byte[4096];
                serverStream.Read(inStream, 0, inStream.Length);
                savesList = Desserialize(inStream);
                __ProjectSaveListView_.ItemsSource = savesList;
            }
            catch
            {

            }
            
        }

        public static List<Saves> Desserialize(byte[] data)
        {
            List<Saves> result = new List<Saves>();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    while (reader != null)
                    {
                        try
                        {
                            string name = reader.ReadString();
                            string sourceRepository = reader.ReadString();
                            string destinationRepository = reader.ReadString();
                            int nbFilesSourceRepository = reader.ReadInt32();
                            int nbFilesDestinationRepository = reader.ReadInt32();
                            if (name == "")
                                break;
                            result.Add(new Saves() 
                            {
                                SaveName = name,
                                SourceRepository = sourceRepository,
                                DestinationRepository = destinationRepository,
                                NbFilesSourceRepository = nbFilesSourceRepository,
                                NbFilesDestinationRepository = nbFilesDestinationRepository
                            });
                        }
                        catch
                        {
                            break;
                        }

                    }

                }
            }
            return result;
        }

        public void msg(string mesg)
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + mesg;
        }

        private void Resume(object sender, RoutedEventArgs e)
        {

        }

        private void Pause(object sender, RoutedEventArgs e)
        {

        }

        private void __ProjectSaveListView__SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
