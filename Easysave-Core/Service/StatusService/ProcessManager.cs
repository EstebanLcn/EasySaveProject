using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Easysave_Core.Service.StatusService
{
    public class ProcessManager
    {

        public static bool Check_Process(string process)
        {
            Process[] processes = Process.GetProcessesByName(process);
            if (processes.Length != 0)
                return false;

            return true;


        }

    }

    public class MyProcess
    {
        public string Name { get; set; }
        public bool Running { get; set; }



        public MyProcess(string process, bool state)
        {
            Name = process;
            Running = state;
        }


    }
}

