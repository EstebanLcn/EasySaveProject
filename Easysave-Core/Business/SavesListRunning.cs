using Easysave_Core.Service.SaveService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Easysave_Core.Business
{
    public static class SavesListRunning
    {
        public static List<Saves> saves = new List<Saves>();

        public static List<Saves> getThreadsList()
        {
            return saves;
        }

        public static void addThreadsList(Saves save)
        {
            saves.Add(save);
        }

        public static void removeThreadsList(Saves save)
        {
            saves.Remove(save);
        }
    }
}
