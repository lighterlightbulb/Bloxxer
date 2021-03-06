﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Json;

namespace Bloxxer.Utils
{
    public class JsonManager
    {
        private static readonly string PrerequisitesPath = Directory.GetCurrentDirectory() + @"\resources\prerequisites.json";

        private static void CheckValidJson()
        {
            bool valid = true;

            try
            {
                var obj = JsonValue.Parse(File.ReadAllText(PrerequisitesPath));
            }
            catch
            {
                valid = false;
            }

            if (!valid)
            {
                ReplaceAllPreReqs();
            }
        }

        public static void ReplaceAllPreReqs()
        {
            if (!File.Exists(PrerequisitesPath))
            {
                File.Create(PrerequisitesPath);
            }
            
            var prerequisites = new JObject {
                new JProperty("configuration", new JObject {
                    new JProperty("version", Properties.Resources.Version)
                }),
                new JProperty("preferences", new JObject {
                    new JProperty("execution", new JObject {
                        new JProperty("show", false),
                        new JProperty("method", 0),
                        new JProperty("injectOnExecution", false)
                    }),
                    new JProperty("theme", 0),
                    new JProperty("bloxxerOnTop", false),
                    new JProperty("robloxOnTop", true),
                    new JProperty("recentlyUsed")
                })
            };

            SaveJson(prerequisites.ToString());
        }

        public static void GetPreferences()
        {
            JObject json = GetJson();
            JToken preferences = json["preferences"];

            GlobalVars.RobloxOnTop             = Convert.ToBoolean(preferences["robloxOnTop"]);
            GlobalVars.BloxxerOnTop            = Convert.ToBoolean(preferences["bloxxerOnTop"]);
            GlobalVars.Theme                   = Convert.ToInt32(preferences["theme"]);
            GlobalVars.ExecutionMessage        = Convert.ToBoolean(preferences["execution"]["show"]);
            GlobalVars.ExecutionMessageMethod  = Convert.ToInt32  (preferences["execution"]["method"]);
            GlobalVars.InjectOnExecution       = Convert.ToBoolean(preferences["execution"]["injectOnExecution"]);
        }

        public static void SaveJson(string json)
        {
            File.WriteAllText(PrerequisitesPath, json);
        }

        public static JObject GetJson()
        {
            CheckValidJson();

            return JObject.Parse(File.ReadAllText(PrerequisitesPath));
        }

        public static IList<string> GetRecentlyUsed()
        {
            CheckValidJson();

            JObject parsedJson = JObject.Parse(File.ReadAllText(PrerequisitesPath));
            return parsedJson["preferences"]["recentlyUsed"].Select(s => (string)s).ToList();
        }
    }
}
