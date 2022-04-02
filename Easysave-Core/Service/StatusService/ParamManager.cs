using Easysave_Core.Business;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Easysave_Core.Service.StatusService
{
    public static class ParamManager
    {
        static string Path = @"..\\..\\..\\MyParam.json";
        public static MyParam Read()
        {
                MyParam myParam = JsonSerializer.Deserialize<MyParam>(File.ReadAllText(Path));
                return myParam;
        }

        public static void Update(string lang, string reject, string priority, long maxSize)
        {

            var options = new JsonSerializerOptions { WriteIndented = true, };
            MyParam myParam = Read();

                if (lang != null)
                {
                    myParam.Lang = lang;
                }
                if(reject != null)
                {
                    myParam.Reject_sw = reject;
                }
                if(priority != null)
                {
                    myParam.Prioritary_Ext = priority;
                }
                if (maxSize != 0)
                {
                    myParam.Max_Size_File = maxSize;
                }
            string json = JsonSerializer.Serialize(myParam, options);
            File.WriteAllText(Path, json);

        }
    }
}
