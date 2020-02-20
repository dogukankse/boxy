using System.Collections;
using System.Xml;
using UnityEngine;

namespace _Scripts.Managers
{
    public class LanguageManager
    {
        private Hashtable strings;
        private readonly TextAsset textAsset = (TextAsset) Resources.Load("strings", typeof(TextAsset));
        private static LanguageManager instance;

        public static LanguageManager Instance()
        {
            return instance ?? (instance = new LanguageManager());
        }


        public void SetLanguage(string language)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(textAsset.text);

            strings = new Hashtable();
            if (xml.DocumentElement != null)
            {
                XmlElement element = xml.DocumentElement[language];
                Debug.Log(language);
                if (element != null)
                {
                    IEnumerator elemEnum = element.GetEnumerator();
                    while (elemEnum.MoveNext())
                    {
                        XmlElement item = (XmlElement) elemEnum.Current;
                        if (item != null) strings.Add(item.GetAttribute("name"), item.InnerText);
                    }
                }
                else
                {
                    Debug.LogError("Language doesnt exist" + language);
                }
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