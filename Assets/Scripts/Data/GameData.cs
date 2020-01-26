using System;
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
        //return instance ?? (instance = new GameData());
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

    public int magnetBoosterCount = 5;
    public int slowBoosterCount = 5;
    public int bombBoosterCount = 5;

    #endregion

    #region Settings

    [NonSerialized] public float bornDelay = 1f;
    [NonSerialized] public float objectSpeed = 6f;
    [NonSerialized] public float pointBornRatio = .7f;
    [NonSerialized] public float playerSpeed = 5f;
    public string language = "English";

    #endregion

    #region Color

    private float r = ColorData.InitialColor().r;

    private float g = ColorData.InitialColor().g;

    private float b = ColorData.InitialColor().b;

    private float a = ColorData.InitialColor().a;

    [NonSerialized] private Color color = Color.clear;

    public Color Color
    {
        get
        {
            if (color == Color.clear)
            {
                color = new Color(r, g, b, a);
            }

            return color;
        }
        set
        {
            r = value.r;
            g = value.g;
            b = value.b;
            a = value.a;
            color = value;
        }
    }

    #endregion

    public string username;


    public override string ToString()
    {
        return
            $"GameState: {gameState}\nHS: {highScore} LS: {lastScore}\nMagnetCount: {magnetBoosterCount}" +
            $"SlowCount: {slowBoosterCount} BombCount: {bombBoosterCount}\n Color: {color} r:{r} g:{g} b: {b} a:{a}" +
            $"Username: {username} Language: {language}";
    }
}