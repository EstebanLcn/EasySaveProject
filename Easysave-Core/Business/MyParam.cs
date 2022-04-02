using Easysave_Core.Service.StatusService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easysave_Core.Business
{
    public  class MyParam
    {
        public string Lang { get; set; }
        public string Reject_sw { get; set; }
        public string Prioritary_Ext { get; set; }
        public long Max_Size_File { get; set; }
    }
}
