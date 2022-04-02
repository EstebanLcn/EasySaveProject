using Easysave_Core.Business;
using Easysave_Core.Service.SaveService;
using Easysave_Core.Service.StatusService;
using Easysave_Core.Service.StatusService.Languages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave_Graphical.Views
{
    /// <summary>
    /// Logique d'interaction pour SaveStatus.xaml
    /// </summary>
    public partial class SaveStatus : UserControl
    {
        public MyParam parameters;
        public ItemToTranslate itemToTranslate;
        public List<Saves> saves;
        public SaveStatus()
        {
            InitializeComponent();
            parameters = ParamManager.Read();
            itemToTranslate = ServiceTranslation.GetData(parameters.Lang);
            itemToTranslate.lang = parameters.Lang;
            __ProjectSaveListView_.ItemsSource = SavesListRunning.getThreadsList();
            BtnLogs_Copy1.Content = itemToTranslate.Resume;
            BtnLogs_Copy.Content = itemToTranslate.Pause;

        }

        public void Pause(object sender, RoutedEventArgs e)
        {
            var profilesSelected = __ProjectSaveListView_.SelectedItems;
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
        


        private void Resume(object sender, RoutedEventArgs e)
        {
            var profilesSelected = __ProjectSaveListView_.SelectedItems;
            foreach (Saves Profiles in profilesSelected)
            {
                if (Profiles.CompleteSave == null)
                {
                    Profiles.DifferentialSave.Resume();
                }
                else
                {
                    Profiles.CompleteSave.Resume();
                }

            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            var profilesSelected = __ProjectSaveListView_.SelectedItems;
            foreach (Saves Profiles in profilesSelected)
            {
                if (Profiles.CompleteSave == null)
                {
                    Profiles.DifferentialSave.Stop();
                }
                else
                {
                    Profiles.CompleteSave.Stop();
                }

            }
        }

        private void Reload(object sender, RoutedEventArgs e)
        {
           

            
        }

        private void ___SaveProcessProgressBar__ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
