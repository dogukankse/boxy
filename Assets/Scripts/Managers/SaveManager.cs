using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using EasyMobile;
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

        public static SavedGame OpenCloudData()
        {
            SavedGame save = null;
            GameServices.SavedGames.OpenWithAutomaticConflictResolution("save", (SavedGame savedGame, string error) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    Debug.Log("Saved game loaded.");
                    save = savedGame;
                }
                else
                {
                    Debug.LogError("Save load error " + error);
                }
            });
            return null;
        }

        public static void CloudSave(SavedGame save)
        {
            if (save.IsOpen)
            {
                byte[] data = GameData.Instance().SerializeGameData();
                GameServices.SavedGames.WriteSavedGameData(
                    save, data, (SavedGame newSavedGame, string error) =>
                    {
                        if (string.IsNullOrEmpty(error))
                            Debug.Log("Saved game data has been written successfully!");
                        else
                            Debug.LogError("Writing saved game data failed with error: " + error);
                    });
            }
            else
            {
                // The saved game is not open. You can optionally open it here and repeat the process.
                Debug.LogError("You must open the saved game before writing to it.");
            }
        }

        public static void CloudLoad(SavedGame save)
        {
            if (save.IsOpen)
            {
                GameServices.SavedGames.ReadSavedGameData(save,
                    (SavedGame loadedSavedGame, byte[] data, string error) =>
                    {
                        if (string.IsNullOrEmpty(error))
                        {
                            Debug.Log("Saved game data has been retrieved successfully");
                            if (data.Length > 0)
                            {
                                //load adata
                                Debug.Log("DATA: " + data);
                                GameData localData;
                                using (MemoryStream stream = new MemoryStream(data))
                                {
                                    IFormatter formatter = new BinaryFormatter();
                                    localData = (GameData) formatter.Deserialize(stream);
                                    Debug.Log(localData + "");
                                }

                                GameData.Instance().bombBoosterCount = localData.bombBoosterCount;
                                GameData.Instance().slowBoosterCount = localData.slowBoosterCount;
                                GameData.Instance().magnetBoosterCount = localData.magnetBoosterCount;
                                GameData.Instance().color = localData.color;
                                GameData.Instance().language = localData.language;
                            }
                            else
                                Debug.LogError("The saved game has no data!");
                        }
                        else
                            Debug.LogError("Reading saved game data failed: " + error);
                    });
            }
            else
                Debug.LogError("You must open the saved game before reading it");
        }
    }
}