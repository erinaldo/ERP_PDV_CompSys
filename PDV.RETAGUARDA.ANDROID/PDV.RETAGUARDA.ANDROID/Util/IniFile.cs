﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PDV.RETAGUARDA.WEB.Util
{
    public class IniFile
    {
        private Dictionary<string, Dictionary<string, string>> _iniFileContent;
        private readonly Regex _sectionRegex = new Regex(@"(?<=\[)(?<SectionName>[^\]]+)(?=\])");
        private readonly Regex _keyValueRegex = new Regex(@"(?<Key>[^=]+)=(?<Value>.+)");

        public IniFile()
        {
        }

        ~IniFile()
        {
            if (this._iniFileContent != null)
                this._iniFileContent.Clear();
            this._iniFileContent = null;
        }

        public IniFile(string filename)
        {
            _iniFileContent = new Dictionary<string, Dictionary<string, string>>();
            if (filename != null)
                Load(filename);
        }

        /// <summary>
        /// Get a specific value from the .ini file
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <returns>The value of the given key in the given section, or NULL if not found</returns>
        public string GetValue(string sectionName, string key, string defaultValue = null)
        {
            if (_iniFileContent.ContainsKey(sectionName) && _iniFileContent[sectionName].ContainsKey(key))
                return _iniFileContent[sectionName][key];
            else
                return defaultValue;
        }

        /// <summary>
        /// Set a specific value in a section
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(string sectionName, string key, string value)
        {
            if (!_iniFileContent.ContainsKey(sectionName)) _iniFileContent[sectionName] = new Dictionary<string, string>();
            _iniFileContent[sectionName][key] = value;
        }

        /// <summary>
        /// Get all the Values for a section
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns>A Dictionary with all the Key/Values for that section (maybe empty but never null)</returns>
        public Dictionary<string, string> GetSection(string sectionName)
        {
            if (_iniFileContent.ContainsKey(sectionName))
                return new Dictionary<string, string>(_iniFileContent[sectionName]);
            else
                return new Dictionary<string, string>();
        }

        /// <summary>
        /// Set an entire sections values
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="sectionValues"></param>
        public void SetSection(string sectionName, IDictionary<string, string> sectionValues)
        {
            if (sectionValues == null) return;
            _iniFileContent[sectionName] = new Dictionary<string, string>(sectionValues);
        }


        /// <summary>
        /// Load an .INI File
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool Load(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    var content = File.ReadAllLines(filename);
                    _iniFileContent = new Dictionary<string, Dictionary<string, string>>();
                    string currentSectionName = string.Empty;
                    foreach (var line in content)
                    {
                        Match m = _sectionRegex.Match(line);
                        if (m.Success)
                        {
                            currentSectionName = m.Groups["SectionName"].Value;
                        }
                        else
                        {
                            m = _keyValueRegex.Match(line);
                            if (m.Success)
                            {
                                string key = m.Groups["Key"].Value;
                                string value = m.Groups["Value"].Value;

                                Dictionary<string, string> kvpList;
                                if (_iniFileContent.ContainsKey(currentSectionName))
                                {
                                    kvpList = _iniFileContent[currentSectionName];
                                }
                                else
                                {
                                    kvpList = new Dictionary<string, string>();
                                }
                                kvpList[key] = value;
                                _iniFileContent[currentSectionName] = kvpList;
                            }
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}