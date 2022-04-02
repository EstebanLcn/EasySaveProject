using Easysave_Core.Business;
using Easysave_Core.Service.StatusService;
using Easysave_Core.Service.StatusService.Languages;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace EasySave_Graphical.Views
{

    /// <summary>
    /// Logique d'interaction pour ParametersView.xaml
    /// </summary>
    public partial class ParametersView : UserControl
    {
        public MyParam parameters;
        public ItemToTranslate itemToTranslate;
        public ParametersView()
        {
            InitializeComponent();
            this.CBO_Language.SelectedIndex = 0;
            parameters = ParamManager.Read();
            itemToTranslate = ServiceTranslation.GetData(parameters.Lang);
            itemToTranslate.lang = parameters.Lang;
            InitData();
        }

        public void InitData()
        {
            BtnExtensionFile.Content = itemToTranslate.Apply;
            BtnApplyFileSize.Content = itemToTranslate.Apply;
            currentProcess.Text = parameters.Reject_sw;
            currentSizeFile.Text = parameters.Max_Size_File.ToString();
            currentExtensionFile.Text = parameters.Prioritary_Ext;
            _Parameters.Text = itemToTranslate._Parameters;
            _CurrentProcess.Text = itemToTranslate._Current;
            _ValidProcessName.Text = itemToTranslate._ValidProcess;
            _Language.Text = itemToTranslate._Language;
            _CurrentLanguage.Text = itemToTranslate._Current;
            _SelectLanguage.Text = itemToTranslate._SelectLanguage;
            CurrentLanguage.Text = itemToTranslate.Language;
            BtnApplyChanges.Content = itemToTranslate.Apply;
            BtnApplyProcess.Content = itemToTranslate.Apply;
        }

        private void Btn_Change_Process(object sender, RoutedEventArgs e)
        {
            string textBoxParam = txtbxChangeProcess.Text;
            currentProcess.Text = textBoxParam;
            MyProcess myProcess = new MyProcess(textBoxParam, ProcessManager.Check_Process(textBoxParam));
            ParamManager.Update(null, textBoxParam, null, 0);
        }

        private void Change_Language_Clicked(object sender, RoutedEventArgs e)
        {
            string value = CBO_Language.Text;
            itemToTranslate.ChangeLang(value);
            itemToTranslate = ServiceTranslation.GetData(value);
            ParamManager.Update(value, null, null, 0);
            CurrentLanguage.Text = itemToTranslate.Language;
            InitData();
            MessageBox.Show(itemToTranslate.MSG_Restart);
        }

        private void SizeFile_Clicked(object sender, RoutedEventArgs e)
        {
            long value = Convert.ToInt32(txtbxSizeFile.Text);
            currentSizeFile.Text = value.ToString();
            ParamManager.Update(null, null, null, value);
        }

        private void ExtensionFile_Clicked(object sender, RoutedEventArgs e)
        {
            string value = txtbxExtensionFile.Text;
            currentExtensionFile.Text = value;
            ParamManager.Update(null, null, value, 0);
        }
    }
}
