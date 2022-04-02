using Easysave_Core.Business;
using Easysave_Core.Service.LogService;
using Easysave_Core.Service.StatusService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Easysave_Core.Service.SaveService
{
    public class DifferentialSave : SaveStrategy
    {
        public Saves saves = new Saves();
        MyParam parameters = ParamManager.Read();
        ManualResetEvent _pauseOrStopEvent;
        public int encryptiontime;
        private Mutex mutex = new Mutex();
        public object lockObj = new object();
        public bool myBool = true;

        public event PropertyChangedEventHandler PropertyChanged;
        public DifferentialSave(Profiles profiles)
        {
            saves.SaveName = profiles.name;
            saves.SourceRepository = profiles.src;
            saves.DestinationRepository = profiles.dst;
            saves.SaveType = "Differential Save";
            saves.DifferentialSave = this;
            _pauseOrStopEvent = new ManualResetEvent(true);
        }

        public void GetPercent(Saves saves)
        {
            saves.NbFilesSourceRepository = Directory.GetFiles(saves.SourceRepository, "*.*", SearchOption.AllDirectories).Length;
            saves.NbFilesDestinationRepository = Directory.GetFiles(saves.DestinationRepository, "*.*", SearchOption.AllDirectories).Length;
        }

        // Differential with strategy
        public override void SaveInterface(DirectoryInfo source, DirectoryInfo target)
        {
            string mySource = source.FullName;
            List<string> priorityFiles = CheckPriorityFile(mySource, parameters.Prioritary_Ext);
            int count = 0;
            string[] allDirSource = Directory.GetDirectories(mySource, "*", SearchOption.AllDirectories);

            foreach (string pfile in priorityFiles)
            {

                string fileSrcToDest = pfile.Replace(source.FullName, target.FullName);
                string destFile = System.IO.Path.Combine(target.FullName, fileSrcToDest);
                bool dirExist = Directory.Exists(Path.GetDirectoryName(destFile));
                if (!dirExist)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                }
                if (File.Exists(destFile))
                {
                    var info = new FileInfo(destFile);

                    var sourceInfo = new FileInfo(pfile);
                    if (info.LastWriteTime < sourceInfo.LastWriteTime)
                    {
                        lock (lockObj)
                        {

                            System.IO.File.Copy(pfile, destFile, true);
                            encryptiontime = encryptiontime + XOR(target.FullName);
                            _pauseOrStopEvent.WaitOne(Timeout.Infinite);
                        }
                        GetPercent(saves);
                    }
                }
                else
                {
                    lock (lockObj)
                    {
                        System.IO.File.Copy(pfile, destFile, true);
                        encryptiontime = encryptiontime + XOR(target.FullName);
                        _pauseOrStopEvent.WaitOne(Timeout.Infinite);
                    }
                    GetPercent(saves);
                }
            }

            DateTime localDate = DateTime.Now;
            string now = localDate.ToString("yyyy-MM-dd-HH-mm-ss");





            System.IO.Directory.CreateDirectory(target.FullName);
            if (System.IO.Directory.Exists(source.FullName))
            {
                string[] files = System.IO.Directory.GetFiles(source.FullName);
                string[] destfiles = System.IO.Directory.GetFiles(target.FullName);

                foreach (DirectoryInfo sourceSubdirectory in source.GetDirectories())
                {
                    DirectoryInfo targetSubdirectory = target.CreateSubdirectory(sourceSubdirectory.Name);

                    SaveInterface(sourceSubdirectory, targetSubdirectory);

                }
                foreach (string s in files)
                {
                    if (destfiles.Length != 0)
                    {
                        foreach (string d in destfiles)
                        {



                            string fileName = System.IO.Path.GetFileName(s);

                            string fileEndPath = fileName.Substring(fileName.Length - 4, 4);
                            string fileBeginPath = fileName.Substring(0, fileName.Length - 4);

                            string destFileWithDate = fileBeginPath + now + fileEndPath;


                            string destFileName = System.IO.Path.GetFileName(d);

                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
                            FileInfo fileInfos = new FileInfo(s);

                            System.IO.FileInfo destFileInfo = new System.IO.FileInfo(destFileName);

                            System.IO.FileInfo fileInfoPath = new System.IO.FileInfo(System.IO.Path.Combine(source.FullName, fileInfo.Name));
                            System.IO.FileInfo destFileInfoPath = new System.IO.FileInfo(System.IO.Path.Combine(target.FullName, destFileInfo.Name));

                            System.IO.FileInfo destWithSrcFileName = new System.IO.FileInfo(System.IO.Path.Combine(target.FullName, fileInfo.Name));

                            System.IO.FileInfo destFileModifPath = new System.IO.FileInfo(System.IO.Path.Combine(@$"{target.FullName}\old", destFileWithDate));


                            if (fileInfo.Name.Equals(destFileInfo.Name))
                            {

                                if (fileInfoPath.LastWriteTime > destFileInfoPath.LastWriteTime)
                                {

                                    if (priorityFiles.Contains(fileInfo.Name))
                                    {
                                        continue;
                                    }
                                    CheckDirectory(target);
                                    System.IO.File.Copy(destFileInfoPath.FullName, destFileModifPath.FullName, false);
                                    System.IO.File.Delete(destFileInfoPath.FullName);
                                    fileInfoPath.LastWriteTime = destFileInfoPath.LastWriteTime;
                                    if (fileInfos.Length < parameters.Max_Size_File)
                                    {
                                        System.IO.File.Copy(s, destWithSrcFileName.FullName, true);
                                    }
                                    else if (fileInfos.Length > parameters.Max_Size_File)
                                    {
                                        mutex.WaitOne();
                                        System.IO.File.Copy(s, destWithSrcFileName.FullName, true);
                                        mutex.ReleaseMutex();
                                        _pauseOrStopEvent.WaitOne(Timeout.Infinite);
                                        GetPercent(saves);

                                        encryptiontime = encryptiontime + XOR(target.FullName);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Not equals");

                                }

                            }

                            else if (destWithSrcFileName.Exists)
                            {

                            }
                            else if (!destFileInfo.Exists)
                            {
                                if (priorityFiles.Contains(fileInfo.Name))
                                {
                                    continue;
                                }
                            }
                        }


                    }
                    else if (destfiles.Length == 0)
                    {

                        foreach (string file in files)
                        {
                            if (priorityFiles.Contains(s))
                            {
                                continue;
                            }
                            string fileSrcPathToDestPath = file.Replace(source.FullName, target.FullName);

                            string fileBeginPath = fileSrcPathToDestPath.Substring(0, fileSrcPathToDestPath.Length - 4);
                            string fileEndPath = fileSrcPathToDestPath.Substring(fileSrcPathToDestPath.Length - 4, 4);

                            string destFileTest = System.IO.Path.Combine(target.FullName, fileSrcPathToDestPath);

                            System.IO.File.Copy(file, destFileTest, true);

                        }
                        //encryptiontime = encryptiontime + XOR(target.FullName);
                    }


                }


            }
            LogsManager logmanager = new LogsManager();
            logmanager.GenerateLog(saves, encryptiontime);
        }

        public void CheckDirectory(DirectoryInfo target)
        {
            var dirName = @$"{target.FullName}\old";
            Directory.CreateDirectory(dirName);
        }

        public List<string> CheckPriorityFile(string src, string extension)
        {
            List<string> PriorityFile = new List<string>();
            string[] files = Directory.GetFiles(src, $"*.{extension}", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                PriorityFile.Add(file);
            }
            return PriorityFile;
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

