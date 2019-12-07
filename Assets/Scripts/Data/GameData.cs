using Managers;

public class GameData
{
    private static GameData instance;

    public static GameData Instance()
    {
        if (instance == null)
        {
            instance = SaveManager.IsSaveExists() ? SaveManager.Load() : new GameData();
        }

        return instance;
    }

    private GameData()
    {
    }


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

    #endregion
}