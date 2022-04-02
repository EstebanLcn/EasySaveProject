using Easysave_Core.Business;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Easysave_Core.Service.StatusService
{
    public static class ProfileManager
    {
        public static List<Profiles> Import()
        {
            using (var streamReader = new StreamReader(@"..\..\..\..\profiles.json"))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<List<Profiles>>(jsonReader);

                }
            }   
        }

        public static void Export(string name, string src, string dst)
        {
            List<Profiles> profilesList = new List<Profiles>();
            Profiles profil = new Profiles
            {
                name = name,
                src = src,
                dst = dst
            };
            profilesList = Import();
            profilesList.Add(profil);
           
            string JsonString = JsonConvert.SerializeObject(profilesList, Formatting.Indented);

            using (var streamWriter = new StreamWriter(@"..\..\..\..\profiles.json"))
            {
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    var serializer = new JsonSerializer();
                    jsonWriter.Formatting = Formatting.Indented;
                    serializer.Serialize(jsonWriter, JsonConvert.DeserializeObject(JsonString));
                }
            }
        }
    }
}
