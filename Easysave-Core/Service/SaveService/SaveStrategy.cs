using Easysave_Core.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Easysave_Core.Service.SaveService
{
   public abstract class SaveStrategy
    {
        public abstract void SaveInterface(DirectoryInfo source, DirectoryInfo target);    
    }
}
