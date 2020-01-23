using UnityEngine;
using UnityEngine.UI;

public class ShareTemp : MonoBehaviour
{
    [SerializeField] private Text username;
    [SerializeField] private Text score;
    void Start()
    {
        SetUsername();
        SetScore();
        
        ScreenCapture.CaptureScreenshot("highscore");
    }


    private void SetUsername()
    {
        username.text = GameData.Instance().username;
    }

    private void SetScore()
    {
        score.text = GameData.Instance().lastScore.ToString();
    }
}