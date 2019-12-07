using System.Collections;
using System.Xml;
using UnityEngine;

namespace Managers
{
    public class LanguageManager
    {
        private  Hashtable strings;
        private static readonly string path = $"{Application.dataPath}/Language/strings.xml";
        private static LanguageManager instance;

        public static LanguageManager Instance()
        {
            return instance ?? (instance = new LanguageManager());
        }


        public void SetLanguage(string language)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            strings = new Hashtable();
            XmlElement element = xml.DocumentElement[language];
            if (element != null)
            {
                IEnumerator elemEnum = element.GetEnumerator();
                while (elemEnum.MoveNext())
                {
                    XmlElement item = (XmlElement) elemEnum.Current;
                    strings.Add(item.GetAttribute("name"), item.InnerText);
                }
            }
            else
            {
                Debug.LogError("Language doesnt exist" + language);
            }
        }

        public string getString(string name)
        {
            if (!strings.ContainsKey(name))
            {
                Debug.LogError("The specified string does not exist: " + name);
                return "";
            }

            return strings[name].ToString();
        }
    }
}