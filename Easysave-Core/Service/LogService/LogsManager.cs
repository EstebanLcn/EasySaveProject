using Easysave_Core.Business;
using Easysave_Core.Service.SaveService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Easysave_Core.Service.LogService
{
    public class LogsManager
    {
        string LogFileDirectoryPath = @".\log\";
        string format = "dd-MM-yyyy HH.mm.ss";
        string formatDay = "dd-MM-yyyy";
        string LogFileExtension = ".json";

        public void GenerateLog(Saves saves, int encryptiontime)
        {
            CheckDirectory();
            WriteInLog(saves, encryptiontime);
        }

        public void CheckDirectory()
        {
            var dirName = $@".\log\";
            DirectoryInfo di = Directory.CreateDirectory(dirName);
        }

        public void WriteInLog(Saves saves, int encryptiontime)
        {
            //NEXT
            //Write infos in logfile
            //
            
            string LogDate = DateTime.Now.ToString(formatDay);
            string LogDateformat = DateTime.Now.ToString(format);

            string FileInfos ="";
            if (encryptiontime < 0)
            {
                string reporterror = "Encryption Error";
                FileInfos = $"{LogDateformat} {saves.SaveName} : {saves.SaveType} from {saves.SourceRepository} to {saves.DestinationRepository} encryptiontime : {reporterror}";
            }
            else if (encryptiontime == 0)
            {
                string reportnoencryption = "No Encryption";
                FileInfos = $"{LogDateformat} {saves.SaveName} : {saves.SaveType} from {saves.SourceRepository} to {saves.DestinationRepository} encryptiontime : {reportnoencryption}";
            }
            else if (encryptiontime > 0)
            {
                FileInfos = $"{LogDateformat} {saves.SaveName} : {saves.SaveType} from {saves.SourceRepository} to {saves.DestinationRepository} encryptiontime : {encryptiontime}ms";
            }
            string fileName = LogFileDirectoryPath + LogDate + LogFileExtension;

            try
            {
                // File already exist ? delete    
                if (File.Exists(fileName))
                {
                    Console.WriteLine("Opening File");


                    // Append Text 
                    using (StreamWriter sw = File.AppendText(fileName))
                    {

                        sw.Write(FileInfos + "\n");

                        Console.WriteLine($"Writing {LogDate}");
                        Console.ReadLine();
                    }

                }
                else
                {
                    Console.WriteLine("Creating New File");

                    CreateLogFile(fileName, saves, FileInfos);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void CreateLogFile(string fileName, Saves saves, string FileInfos)
        {

            // Creating New file   
            using (FileStream fileStr = File.Create(fileName))
            {
                // Add SaveInfos 
                Byte[] text = new UTF8Encoding(true).GetBytes((FileInfos + "\n").ToString().ToCharArray());
                fileStr.Write(text, 0, text.Length);
            }
            //Reading the file
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }

        public static void ProcessStart(string processName,string filename ,string directory = null)
        {
            directory = @".\log\"+filename+ ".json";

            Process process = new Process();
            process.StartInfo.FileName = processName;
            if (!string.IsNullOrEmpty(directory))
                process.StartInfo.Arguments = directory;
            // need to open the correct processus notepad +path
            process.Start();

            Console.WriteLine($"Process {process.ProcessName} - ID {process.Id} - STARTED");

            Thread.Sleep(2000);

            if (process.HasExited)
                Console.WriteLine($"Process {process.ProcessName} - ID {process.Id} - CLOSED");
            else
            {
                Console.WriteLine($"Process {process.ProcessName} - ID {process.Id} - WAITING");
                process.WaitForExit();
                Console.WriteLine($"Process {process.ProcessName} - ID {process.Id} - CLOSED");
                process.Close();
            }

        }

        public void ShowLastLogFile()
        {
            string LogDate = DateTime.Now.ToString(formatDay);
            if (!Directory.Exists(LogFileDirectoryPath)) {
                Directory.CreateDirectory(LogFileDirectoryPath);
                string fileName = LogFileDirectoryPath + LogDate + LogFileExtension;
                CreateLogFile(LogDate, null, null);
            }
            string[] AllLogFilespath = Directory.GetFiles(LogFileDirectoryPath);
            //List that will get all the name of files
            List<string> AllLogFiles = new List<string>();

            DateTime LastDate = DateTime.Today.AddDays(-100);
            string LastSaveDate = LastDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            Console.WriteLine("Valeur String" + LastSaveDate);

            //Just keep the name of the file without path and extension
            foreach (string allfiles in AllLogFilespath)
            {
                //Console.WriteLine(Path.GetFileNameWithoutExtension(allfiles));
                AllLogFiles.Add(Path.GetFileNameWithoutExtension(allfiles));
            }

            //Compare each date to find the last time a logFile has been generated
            foreach (string allfiles in AllLogFiles)
            {

                if (DateTime.Compare(DateTime.ParseExact(allfiles, formatDay, CultureInfo.InvariantCulture), DateTime.ParseExact(LastSaveDate, formatDay, CultureInfo.InvariantCulture)) >= 0)
                {
                    LastSaveDate = DateTime.ParseExact(allfiles, formatDay, CultureInfo.InvariantCulture).ToString(formatDay, CultureInfo.InvariantCulture);
                    Console.WriteLine("dernière date " + LastSaveDate);
                }
                else
                {
                    Console.WriteLine(DateTime.ParseExact(allfiles, formatDay, CultureInfo.InvariantCulture) + "n'est pas reconnue");
                }
            }
            //Print the last save date

            Console.WriteLine("Last Log File Generated : " + LastSaveDate);

            var Content = new System.Text.StringBuilder();

            ProcessStart("notepad.exe",LastSaveDate);

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(LogFileDirectoryPath + LastSaveDate + LogFileExtension);
            //Content var will get all the SavesInfos since the last LogGeneration

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of LastLogGenerated = ");
            foreach (string line in lines)
            {

                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
                Content.AppendLine(line);

            }
        }
    }
}
