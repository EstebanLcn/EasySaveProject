using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Easysave_Core.Business
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Profiles
    {
        [JsonProperty]
        public string name { get; set; }
        [JsonProperty]
        public string src { get; set; }
        [JsonProperty]
        public string dst { get; set; }

        public bool status;

        public int Progress;



        public override string ToString()
        {
            return $"Nom : {name}\n" +
                $"Source Directory : {src}\n" +
                $"Save Directory : {dst}";
        }

    }
}
