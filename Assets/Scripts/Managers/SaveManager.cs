using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Managers
{
    public static class SaveManager
    {
        private static string path = $"{Application.persistentDataPath}/save.json";

        public static void Save()
        {
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
                string json = JsonUtility.ToJson(GameData.Instance());
                byte[] info = new UTF8Encoding(true).GetBytes(json);
                fs.Write(info, 0, info.Length);
                Debug.Log("Saved");
            }
        }

        public static bool IsSaveExists()
        {
            return File.Exists(path);
        }

        public static GameData Load()
        {
            using (FileStream fs = File.OpenRead(path))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string saveData = sr.ReadToEnd();
                    GameData gameData = JsonUtility.FromJson<GameData>(saveData);
                    Debug.Log("Loaded");
                    return gameData;
                }
            }
        }
    }
}