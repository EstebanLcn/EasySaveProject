using Easysave_Core.Business;
using Easysave_Core.Service.LogService;
using Easysave_Core.Service.SaveService;
using Easysave_Core.Service.StatusService;
using Easysave_Core.Service.StatusService.Languages;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace EasySave_Graphical.Views
{
    /// <summary>
    /// Logique d'interaction pour SaveView.xaml
    /// </summary>
    /// 
    
    public partial class SaveView : UserControl 
    {
        public SaveStatus saveStatus;
        public List<Saves> threadsList;
        ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = new ManualResetEvent(true);
        public CompleteSave completeSave;
        public DifferentialSave differentialSave;
        public MyParam parameters;
        public ContextSave context;
        public ItemToTranslate itemToTranslate;

        public SaveView()
        {
            parameters = ParamManager.Read();
            InitializeComponent();
            threadsList = SavesListRunning.getThreadsList();
            dataGridProcess.ItemsSource = ProfileManager.Import();
            itemToTranslate = ServiceTranslation.GetData(parameters.Lang);
            itemToTranslate.lang = parameters.Lang;
            InitData();
            
        }

        public void InitData()
        {
            txtblkProfil.Text = itemToTranslate.ProfileName;
            BtnCreateProfil.Content = itemToTranslate.AddProfile;
            BtnCompleteSave.Content = itemToTranslate.CompleteSave;
            BtnDifferentialSave.Content = itemToTranslate.DifferentialSave;
            BtnLogs.Content = itemToTranslate.Logs;
        }
        private void dataGridProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Profile_Grid(object sender, AddingNewItemEventArgs e)
        {

        }

        private void _SaveProcessProgressBar__ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Btn_Add_Profile_Click(object sender, RoutedEventArgs e)
        {
            ProfileManager.Export(txtbxProfil.Text, txtbxSource.Text, txtbxDestination.Text);
            dataGridProcess.ItemsSource = ProfileManager.Import();
        }

        private void Btn_Complete_Save(object sender, RoutedEventArgs e)
        {
            var profilesSelected = dataGridProcess.SelectedItems;
            bool mavar = ProcessManager.Check_Process(parameters.Reject_sw);
            
                foreach (Profiles profile in profilesSelected)
                {
                if (mavar)
                {
                    context = new ContextSave();
                    DirectoryInfo source = new DirectoryInfo(profile.src);
                    DirectoryInfo target = new DirectoryInfo(profile.dst);
                    
                    new Thread(() =>
                    {
                        //ThreadGrid.ItemsSource = threadsList;
                        //ajoute le trhead a la liste
                        //recharge la grid
                        context.SetStrategy(completeSave = new CompleteSave(profile));
                        SavesListRunning.addThreadsList(completeSave.saves);
                        context.ContextInterface(source, target);
                        SavesListRunning.removeThreadsList(completeSave.saves);
                        
                    }).Start();
                }
                else
                {
                    MessageBox.Show(itemToTranslate.MSG_ProcessBlocking);

                    Pause();

                }
            }
           
        }
        public void Pause()
        {
            var profilesSelected = SavesListRunning.getThreadsList();
            foreach (Saves Profiles in profilesSelected)
            {
                if (Profiles.CompleteSave == null)
                {
                    Profiles.DifferentialSave.Pause();
                }
                else
                {
                    Profiles.CompleteSave.Pause();
                }

            }
        }

        private void Btn_Differential_Save(object sender, RoutedEventArgs e)
        {
            var profilesSelected = dataGridProcess.SelectedItems;
            bool mavar = ProcessManager.Check_Process(parameters.Reject_sw);
            if (mavar)
            {
                foreach (Profiles profile in profilesSelected)
                {
                
                    context = new ContextSave();
                    DirectoryInfo source = new DirectoryInfo(profile.src);
                    DirectoryInfo target = new DirectoryInfo(profile.dst);
                    new Thread(() =>
                    {
                        //ThreadGrid.ItemsSource = threadsList;
                        //ajoute le trhead a la liste
                        //recharge la grid
                        context.SetStrategy(differentialSave = new DifferentialSave(profile));
                        SavesListRunning.addThreadsList(differentialSave.saves);
                        context.ContextInterface(source, target);
                        SavesListRunning.removeThreadsList(differentialSave.saves);

                    }).Start();

                }
                
            }
            else
            {
                MessageBox.Show(itemToTranslate.MSG_ProcessBlocking);

                Pause();

            }
        }

        private void Btn_Logs(object sender, RoutedEventArgs e)
        {
            LogsManager logmanager = new LogsManager();
            logmanager.ShowLastLogFile();
        }
    }
}
