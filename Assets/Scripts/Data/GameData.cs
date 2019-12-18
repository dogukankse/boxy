using Managers;

public class GameData
{
    private static GameData instance;

    public static GameData Instance()
    {
        return instance ?? (instance = SaveManager.IsSaveExists() ? SaveManager.Load() : new GameData());
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
    public int score;

    #endregion

    #region Booster

    public int magnetBoosterCount = 10;
    public int slowBoosterCount = 10;
    public int bombBoosterCount = 10;

    #endregion

    #region Settings

    public float bornDelay = .5f;
    public float speed = 2f;
    public float pointRatio = .8f;
    public float playerSpeed = 1f;

    #endregion
}