using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace SpireVenture.Utilities
{
    public static class ClientOptions
    {
        public static int ResolutionHeight { get; private set; }
        public static int ResolutionWidth { get; private set; }
        public static bool Fullscreen { get; private set; }
        static Dictionary<string, string> optionsDict;
        static private bool changed = false;

        public static void Initialize()
        {
            // load from flat file
            String documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            String clientPath = Path.Combine(documentsPath, "SingleSpire");
            String optionsFilePath = Path.Combine(clientPath, "options.txt");

            optionsDict = new Dictionary<string, string>();

            if (!Directory.Exists(clientPath))
                Directory.CreateDirectory(clientPath);

            if (File.Exists(optionsFilePath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(optionsFilePath))
                    {
                        String line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] pair = line.Split(':');
                            optionsDict.Add(pair[0], pair[1]);
                        }
                    }
                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }

                foreach (string key in optionsDict.Keys)
                {
                    switch (key)
                    {
                        case "resolutionH":
                            ResolutionHeight = Convert.ToInt32(optionsDict["resolutionH"]);
                            break;
                        case "resolutionW":
                            ResolutionWidth = Convert.ToInt32(optionsDict["resolutionW"]);
                            break;
                        case "fullscreen":
                            Fullscreen = Convert.ToBoolean(optionsDict["fullscreen"]);
                            break;
                    }
                }
            }
            else
            {
                CreateDefaultOptions();
            }
        }

        private static void CreateDefaultOptions()
        {
            SetResolution(600, 800);
            SetFullscreen(false);
            Save();
        }

        public static void Save()
        {
            if (changed)
            {
                String documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                String clientPath = Path.Combine(documentsPath, "SpireVenture");
                String optionsFilePath = Path.Combine(clientPath, "options.txt");

                if (!Directory.Exists(clientPath))
                    Directory.CreateDirectory(clientPath);

                File.Delete(optionsFilePath); // delete if there, doesn't throw error if not

                StringBuilder sb = new StringBuilder();
                foreach (string key in optionsDict.Keys)
                {
                    sb.AppendLine(key + ":" + optionsDict[key]);
                }

                using (StreamWriter outfile = new StreamWriter(optionsFilePath))
                    outfile.Write(sb.ToString());

                changed = false;
            }
        }

        public static void SetResolution(int H, int W)
        {
            changed = true;
            ResolutionHeight = H;
            ResolutionWidth = W;
            if (optionsDict.ContainsKey("resolutionH"))
                optionsDict["resolutionH"] = Convert.ToString(H);
            else
                optionsDict.Add("resolutionH", Convert.ToString(H));
            if (optionsDict.ContainsKey("resolutionW"))
                optionsDict["resolutionW"] = Convert.ToString(W);
            else
                optionsDict.Add("resolutionW", Convert.ToString(W));
        }

        public static void SetFullscreen(bool full)
        {
            changed = true;
            Fullscreen = full;
            if (optionsDict.ContainsKey("fullscreen"))
                optionsDict["fullscreen"] = Convert.ToString(full);
            else
                optionsDict.Add("fullscreen", Convert.ToString(full));
        }
    }
}
