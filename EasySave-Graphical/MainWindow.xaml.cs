using Easysave_Core.Service.SaveService;
using System.Windows;
using Easysave_Core.Service.StatusService.Languages;
using EasySave_Graphical.ViewModel;
using System.IO;
using Easysave_Core.Service;
using System.Threading;
using System.Diagnostics;

using Easysave_Core.Business;
using Easysave_Core.Service.StatusService;

namespace EasySave_Graphical
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MyParam parameters;
        public ItemToTranslate itemToTranslate;
        public ContextSave context;
        public MainWindow()
        {
            //Get the current Process for this application
            Process EasySaveProcess = Process.GetCurrentProcess();
            //Collect all the process
            Process[] AllProcess = Process.GetProcesses();
            //Compare if the process is already running using the ID
            foreach (Process Processus in AllProcess)
                //Compare ID of process
                if (EasySaveProcess.Id != Processus.Id)
                    //Compare Name of process
                    if (EasySaveProcess.ProcessName == Processus.ProcessName)
                    {
                        MessageBox.Show("The Process is already running");
                        this.Close();
                    }
            
            InitializeComponent();
            //Thread mysocket = new Thread(new SocketServer(ServeurSocket));
            SocketServer socketserver = new SocketServer();
            Thread mysocket = new Thread(() => socketserver.ServeurSocket(0,0,null,null));
            mysocket.Start();

            
            parameters = ParamManager.Read();
            itemToTranslate = ServiceTranslation.GetData(parameters.Lang);
            itemToTranslate.lang = parameters.Lang;
            InitializeComponent();
            InitData();
            

        }

        public void InitData()
        {
            BtnSave.Content = itemToTranslate.Saves;
            BtnSettings.Content = itemToTranslate.Settings;
            BtnQuit.Content = itemToTranslate.Quit;
        }

        private void SaveView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new SaveViewModel();
        }

        private void ParametersView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new ParametersViewModel();

        }

        private void SaveStatus_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new StatusVieModel();
        }

        private void Btn_Quit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    
}
