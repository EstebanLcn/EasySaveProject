using Easysave_Core.Service.StatusService;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace EasySave_Graphical
{

    /// <summary>
    /// Logique d'interaction pour ParamWindow.xaml
    /// </summary>
    public partial class ParamWindow : Window
    {
        public string path = ".\\reject_sw.txt";

        public ParamWindow()
        {
            InitializeComponent();
            this.CBO_Language.SelectedIndex = 0;
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = sr.ReadToEnd();
                    current_process.Content = line;
                    sr.Close();
                    sr.Dispose();
                };

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            string textBoxParam = process_name_txtbx.Text;
            current_process.Content = textBoxParam;
            MyProcess myProcess = new MyProcess(textBoxParam, ProcessManager.Check(textBoxParam));
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(myProcess.Name);
                sw.Close();
            };
        }

    }
}
