using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace NicoUtilities
{
    public class DialogueCSVLoader : Singleton<DialogueCSVLoader>
    {
        #region Serialized Fields
        [SerializeField] private TextAsset dialogueCSV = null;
        [SerializeField] private Language defaultLanguage = Language.EN;
        #endregion
        #region Private Fields
        private Dictionary<int, Dictionary<string, string>> dialogueMap = new();
        private List<string> availableLanguages = new();
        private string selectedLanguage = "";
        #endregion

        #region Unity Life Cycle
        protected override void Awake()
        {
            base.Awake();
            LoadDialogueData();
        }
        #endregion

        #region Private Methods
        private void LoadDialogueData()
        {
            dialogueMap.Clear();
            availableLanguages.Clear();
            if (dialogueCSV == null) { return; }
            using (StringReader reader = new StringReader(dialogueCSV.text))
            {
                string headerLine = reader.ReadLine();
                if (string.IsNullOrEmpty(headerLine)) return;

                var headers = ParseCsvLine(headerLine);
                int idIndex = headers.IndexOf("ID");

                Dictionary<string, int> languageColumnIndices = new();
                for (int i = 0; i < headers.Count; i++)
                {
                    if (headers[i].StartsWith("Text_"))
                    {
                        string lang = headers[i].Substring(5).ToUpper();
                        languageColumnIndices[lang] = i;
                        availableLanguages.Add(lang);
                    }
                }

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var fields = ParseCsvLine(line);
                    if (!int.TryParse(fields[idIndex], out int id)) continue;

                    if (!dialogueMap.ContainsKey(id))
                        dialogueMap[id] = new Dictionary<string, string>();

                    foreach (var langPair in languageColumnIndices)
                    {
                        string lang = langPair.Key;
                        int index = langPair.Value;

                        if (index < fields.Count)
                            dialogueMap[id][lang] = fields[index];
                    }
                }
            }
        }
        private List<string> ParseCsvLine(string line)
        {
            List<string> result = new();
            StringBuilder current = new();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                        continue;
                    }
                    inQuotes = !inQuotes;
                    continue;
                }
                if (c == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                    continue;
                }
                current.Append(c);
            }

            result.Add(current.ToString());
            return result;
        }
        #endregion

        #region Public Methods
        public void SetLanguage(Language language) { selectedLanguage = language.ToString(); }
        public string GetDialogueText(int id)
        {
            selectedLanguage = string.IsNullOrEmpty(selectedLanguage) ? defaultLanguage.ToString() : selectedLanguage;
            if (dialogueMap.TryGetValue(id, out var langDict))
            {
                if (langDict.TryGetValue(selectedLanguage, out var text)) { return text; }
                if (langDict.TryGetValue(defaultLanguage.ToString().ToUpper(), out var fallbackText)) { return fallbackText; }
            }
            return $"[Missing Dialogue for ID {id} - {selectedLanguage}]";
        }
        #endregion
    }
    public enum Language
    {
        EN, ES,
    }
}