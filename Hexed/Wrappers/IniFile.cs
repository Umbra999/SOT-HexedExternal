using System.Text;
using System.Text.Json;

namespace Hexed.Wrappers
{
    internal class IniFile
    {
        private Dictionary<string, Dictionary<string, string>> data;
        private string filePath;

        public IniFile(string path)
        {
            filePath = path;
            data = ReadIniFile();
        }

        private Dictionary<string, Dictionary<string, string>> ReadIniFile()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
            }

            return new Dictionary<string, Dictionary<string, string>>();
        }

        private void SaveIniFile()
        {
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, json, Encoding.Unicode);
        }

        public bool HasKey(string section, string name)
        {
            if (data.TryGetValue(section, out var sectionData))
            {
                return sectionData.ContainsKey(name);
            }

            return false;
        }

        public string GetString(string section, string name, string defaultValue = "", bool autoSave = false)
        {
            if (data.TryGetValue(section, out var sectionData) && sectionData.TryGetValue(name, out var value))
            {
                return value;
            }

            if (autoSave)
            {
                SetString(section, name, defaultValue);
                if (autoSave) SaveIniFile();
            }

            return defaultValue;
        }

        public void SetString(string section, string name, string value)
        {
            if (!data.TryGetValue(section, out var sectionData))
            {
                sectionData = new Dictionary<string, string>();
                data[section] = sectionData;
            }

            sectionData[name] = value;

            SaveIniFile();
        }

        public int GetInt(string section, string name, int defaultValue = 0, bool autoSave = false)
        {
            if (int.TryParse(GetString(section, name), out int value))
            {
                return value;
            }

            if (autoSave)
            {
                SetInt(section, name, defaultValue);
                SaveIniFile();
            }

            return defaultValue;
        }

        public void SetInt(string section, string name, int value)
        {
            SetString(section, name, value.ToString());
        }

        public float GetFloat(string section, string name, float defaultValue = 0f, bool autoSave = false)
        {
            if (float.TryParse(GetString(section, name), out float value))
            {
                return value;
            }

            if (autoSave)
            {
                SetFloat(section, name, defaultValue);
                SaveIniFile();
            }

            return defaultValue;
        }

        public void SetFloat(string section, string name, float value)
        {
            SetString(section, name, value.ToString());
        }

        public bool GetBool(string section, string name, bool defaultValue = false, bool autoSave = false)
        {
            string stringValue = GetString(section, name);
            if (!string.IsNullOrEmpty(stringValue) && bool.TryParse(stringValue, out bool value))
            {
                return value;
            }

            if (autoSave)
            {
                SetBool(section, name, defaultValue);
                SaveIniFile();
            }

            return defaultValue;
        }

        public void SetBool(string section, string name, bool value)
        {
            SetString(section, name, value.ToString());
        }
    }
}
