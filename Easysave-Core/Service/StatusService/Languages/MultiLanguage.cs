using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Easysave_Core.Service.StatusService.Languages
{
    public class MultiLanguage
    {
    }
    public class ItemToTranslate
    {
        /// <summary>
        /// Main Windows
        /// </summary>
        public string lang { get; set; } = "en";
        public string Language { get; set; }
        public string CompleteSave { get; set; }
        public string DifferentialSave { get; set; }
        public string ProfileName { get; set; }
        public string AddProfile { get; set; }
        public string Logs { get; set; }
        public string _Parameters { get; set; }
        public string _Current { get; set; }
        public string _ValidProcess { get; set; }
        public string _Language { get; set; }
        public string _SelectLanguage { get; set; }
        public string Apply { get; set; }
        public string Saves { get; set; }
        public string Settings { get; set; }
        public string Quit { get; set; }
        public string MSG_Restart { get; set; }
        public string MSG_ProcessBlocking { get; set; }
        public string Resume { get; set; }
        public string Pause { get; set; }

        public void ChangeLang(string lang)
        {
            this.lang = lang;
            ServiceTranslation.GetData(lang);
            Console.WriteLine(this.lang);
        }
    }
    public static class ServiceTranslation
    {
        public static ItemToTranslate item = new ItemToTranslate();


        public static ItemToTranslate GetData(string lang)
        {

            ItemToTranslate languageJson = JsonSerializer.Deserialize<ItemToTranslate>(File.ReadAllText(@$"..\..\..\Language\{lang}.json"));
            item.Language = languageJson.Language;
            item.CompleteSave = languageJson.CompleteSave;
            item.DifferentialSave = languageJson.DifferentialSave;
            item.ProfileName = languageJson.ProfileName;
            item.AddProfile = languageJson.AddProfile;
            item.Logs = languageJson.Logs;
            item._Parameters = languageJson._Parameters;
            item._Current = languageJson._Current;
            item._ValidProcess = languageJson._ValidProcess;
            item._Language = languageJson._Language;
            item.Apply = languageJson.Apply;
            item._SelectLanguage = languageJson._SelectLanguage;
            item.Saves = languageJson.Saves;
            item.Settings = languageJson.Settings;
            item.Quit = languageJson.Quit;
            item.MSG_Restart = languageJson.MSG_Restart;
            item.MSG_ProcessBlocking = languageJson.MSG_ProcessBlocking;
            item.Resume = languageJson.Resume;
            item.Pause = languageJson.Pause;
            return languageJson;

        }

        
    }
}
