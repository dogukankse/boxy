using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Managers
{
    public static class SaveManager
    {
        private static string path = $"{Application.persistentDataPath}/{SystemInfo.deviceUniqueIdentifier}.bxy";

        public static void Save()
        {
            try
            {
                using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, GameData.Instance());
                    Debug.Log("Saved");
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"Error when saving: {exception.Message}");
            }


            /*using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
                string json = JsonUtility.ToJson(GameData.Instance());
                byte[] info = new UTF8Encoding(true).GetBytes(json);
                fs.Write(info, 0, info.Length);
                Debug.Log("Saved");
            }*/
        }

        public static bool IsSaveExists()
        {
            return File.Exists(path);
        }

        public static GameData Load()
        {
            Debug.Log("Loading");
            try
            {
                GameData savedData;
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    savedData = (GameData) formatter.Deserialize(fileStream);
                    Debug.Log(savedData.ToString());
                    savedData.bornDelay = 1f;
                    savedData.objectSpeed = 6f;
                    savedData.pointBornRatio = .7f;
                    savedData.playerSpeed = 5f;
                    return savedData;
                }
            }
            catch (Exception e)

            {
                throw new Exception($"Error when loading: {e.Message}");
            }


/*using (FileStream fs = File.OpenRead(path))
{
    using (StreamReader sr = new StreamReader(fs))
    {
        string saveData = sr.ReadToEnd();
        GameData gameData = JsonUtility.FromJson<GameData>(saveData);
        Debug.Log("Loaded");
        return gameData;
    }
}*/
        }
    }
}