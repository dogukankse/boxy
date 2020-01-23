using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Data;
using Managers;
using UnityEngine;

[Serializable]
public class GameData
{
    private static GameData instance;

    public static GameData Instance()
    {
        return instance ?? (instance = SaveManager.IsSaveExists() ? SaveManager.Load() : new GameData());
    }

    public static void SetGameData(GameData gameData)
    {
        instance = gameData;
    }

    private GameData()
    {
    }

    #region GameState

    [NonSerialized] public State gameState;

    #endregion

    #region Scores

    public int highScore;
    public int lastScore;

    #endregion

    #region Booster

    public int magnetBoosterCount = 10;
    public int slowBoosterCount = 10;
    public int bombBoosterCount = 10;

    #endregion

    #region Settings

    [NonSerialized] public float bornDelay = .5f;
    [NonSerialized] public float objectSpeed = 2f;
    [NonSerialized] public float pointBornRatio = .8f;
    [NonSerialized] public float playerSpeed = 10f;

    #endregion

    public Color color = ColorData.InitialColor();
    public string username;
    public string language = "English";

    public byte[] SerializeGameData()
    {
        byte[] data;
        using (MemoryStream stream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            data = stream.ToArray();
        }

        return data;
    }

    public override string ToString()
    {
        return
            $"GameState: {gameState}\nHS: {highScore} LS: {lastScore}\nMagnetCount: {magnetBoosterCount}" +
            $"SlowCount: {slowBoosterCount} BombCount: {bombBoosterCount}\n Color: {color} Username: {username} Language: {language}";
    }
}