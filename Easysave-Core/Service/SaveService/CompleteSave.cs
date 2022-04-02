
using Easysave_Core.Business;
using Easysave_Core.Service.LogService;
using Easysave_Core.Service.StatusService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Easysave_Core.Service.SaveService
{


    public class CompleteSave : SaveStrategy
    {
        //créer un objet static qui contient toutes les infos dont j'ai besoins

        // Complete with strategy
        MyParam parameters = ParamManager.Read();
        public object lockObj = new object();
        private Mutex mutex = new Mutex();
        public Saves saves = new Saves();
        public ManualResetEvent _pauseOrStopEvent;

        public event PropertyChangedEventHandler PropertyChanged;
        public CompleteSave(Profiles profiles)
        {
            saves.SaveName = profiles.name;
            saves.SourceRepository = profiles.src;
            saves.DestinationRepository = profiles.dst;
            saves.SaveType = "Complete Save";
            saves.CompleteSave = this;
            _pauseOrStopEvent = new ManualResetEvent(true);
        }

        public void GetPercent(Saves saves)
        {
            saves.NbFilesSourceRepository = Directory.GetFiles(saves.SourceRepository, "*.*", SearchOption.AllDirectories).Length;
            saves.NbFilesDestinationRepository = Directory.GetFiles(saves.DestinationRepository, "*.*", SearchOption.AllDirectories).Length;   
        }

        public override void SaveInterface(DirectoryInfo source, DirectoryInfo target)
        {
            saves.status = false;
            Console.WriteLine(saves.SaveName);

            DateTime localDate = DateTime.Now;
            string now = localDate.ToString("yyyy-MM-dd-HH-mm-ss");

            string[] allDirSource = Directory.GetDirectories(source.FullName, "*", SearchOption.AllDirectories);

            foreach (string subDir in allDirSource)
            {
                Directory.CreateDirectory(subDir.Replace(source.FullName, target.FullName));
            }

            string[] files = Directory.GetFiles(source.FullName, "*.*", SearchOption.AllDirectories);

            List<string> priorityFiles = CheckPriorityFile(source.FullName, parameters.Prioritary_Ext);

            foreach (string file in priorityFiles)
            {
                lock (lockObj)
                {
                    string fileSrcPathToDestPath = file.Replace(source.FullName, target.FullName);
                    string fileBeginPath = fileSrcPathToDestPath.Substring(0, fileSrcPathToDestPath.Length - 4);
                    string fileEndPath = fileSrcPathToDestPath.Substring(fileSrcPathToDestPath.Length - 4, 4);
                    string fileWithDate = fileBeginPath + now + fileEndPath;
                    File.Copy(file, fileSrcPathToDestPath, true);
                    File.Move(fileSrcPathToDestPath, fileWithDate);
                    GetPercent(saves);
                }

            }

            foreach (string file in files)
            {
                if (priorityFiles.Contains(file))
                {
                    continue;
                }                
                _pauseOrStopEvent.WaitOne(Timeout.Infinite);
                string fileSrcPathToDestPath = file.Replace(source.FullName, target.FullName);
                string fileBeginPath = fileSrcPathToDestPath.Substring(0, fileSrcPathToDestPath.Length - 4);
                string fileEndPath = fileSrcPathToDestPath.Substring(fileSrcPathToDestPath.Length - 4, 4);
                string fileWithDate = fileBeginPath + now + fileEndPath;

                FileInfo fileInfo = new FileInfo(file);


                if (fileInfo.Length < parameters.Max_Size_File)
                {
                    File.Copy(file, fileSrcPathToDestPath, true);
                    System.IO.File.Move(fileSrcPathToDestPath, fileWithDate);
                }

                else if (fileInfo.Length > parameters.Max_Size_File)

                {
                    mutex.WaitOne();
                    File.Copy(file, fileSrcPathToDestPath, true);
                    System.IO.File.Move(fileSrcPathToDestPath, fileWithDate);
                    mutex.ReleaseMutex();

                }
                GetPercent(saves);
            }
            
            int encryptiontime = XOR(target.FullName); ;
            LogsManager logmanager = new LogsManager();
            logmanager.GenerateLog(saves, encryptiontime);

            saves.status = true;
        }

        public void Pause()
        {
            _pauseOrStopEvent.Reset();
        }
        public void Resume()
        {
            _pauseOrStopEvent.Set();
        }
        public void Stop()
        {
            Thread.CurrentThread.Join();
        }

        public void CheckDirectory(string[] targetPath)
        {
            foreach (string dst in targetPath)
            {
                var dirName = $@"{dst}";
                DirectoryInfo di = Directory.CreateDirectory(dirName);
            }

        }
        
        public List<string> CheckPriorityFile(string src, string extension)
        {
            List<string> PriorityFile = new List<string>();
            string[] files = Directory.GetFiles(src, $"*.{extension}", SearchOption.AllDirectories);
            foreach ( string file in files)
            {
                PriorityFile.Add(file);
            }
            return PriorityFile;
        }
        
        public static int XOR(string directory)
        {
            var process = new Process();
            process.StartInfo.FileName = $@"..\..\..\..\CryptoSave\CryptoSoft.exe";
            process.StartInfo.Arguments = $"{directory}";
            process.StartInfo.Verb = "RUN";
            process.Start();
            process.WaitForExit();
            
            return process.ExitCode;
        }
    }
}
