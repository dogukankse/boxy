using Data;
using Managers;
using UnityEngine;

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

    public State gameState;

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

    public float bornDelay = .5f;
    public float objectSpeed = 2f;
    public float pointBornRatio = .8f;
    public float playerSpeed = 10f;

    #endregion

    public Color color = ColorData.InitialColor();
    public float mainCanvasHeight;
}