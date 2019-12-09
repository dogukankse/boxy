using System;
using Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Sprite gameBg;
        [SerializeField] private Sprite bg;

        [SerializeField] private Image background;
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject tempSettings;
        [SerializeField] private GameObject shopMenu;
        [SerializeField] private GameObject gameScreen;

        [SerializeField] private Text highscoreText;
        [SerializeField] private Text lastScoreText;
        [SerializeField] private Text rankingText;

        [SerializeField] private Text scoreText;

        [SerializeField] private Text highscore;
        [SerializeField] private Text lastScore;
        [SerializeField] private Text magnetCount;
        [SerializeField] private Text slowCount;
        [SerializeField] private Text bombCount;


        private GameObject game;
        private GameObject currScreen;

        public Func<GameObject, GameObject> CreateGameWindow;

        private void Start()
        {
            UpdateUI();
            currScreen = mainMenu;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (currScreen == gameScreen)
                    Destroy(game);
                else if (currScreen == mainMenu)
                    Application.Quit();
                else
                    currScreen.SetActive(false);
                
                currScreen = mainMenu;
                mainMenu.SetActive(true);
            }
        }

        public void PlayButtonListener()
        {
            mainMenu.SetActive(false);
            currScreen = gameScreen;
            game = CreateGameWindow(canvas);
            game.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            background.sprite = gameBg;
        }


        public void SettingsButtonListener()
        {
            currScreen.SetActive(false);
            currScreen = settingsMenu;
            settingsMenu.SetActive(true);
        }

        public void ShopButtonListener()
        {
            currScreen.SetActive(false);
            currScreen = shopMenu;
            shopMenu.SetActive(true);
        }

        public void UpdateUI()
        {
            print("updateUI");

            highscore.text = "" + GameData.Instance().highScore;
            lastScore.text = "" + GameData.Instance().lastScore;
            magnetCount.text = "" + GameData.Instance().magnetBoosterCount;
            slowCount.text = "" + GameData.Instance().slowBoosterCount;
            bombCount.text = "" + GameData.Instance().bombBoosterCount;
        }


        public void SetTexts()
        {
            LanguageManager manager = LanguageManager.Instance();
            highscoreText.text = manager.getString(StringData.highscore);
            lastScoreText.text = manager.getString(StringData.lastScore);
            rankingText.text = manager.getString(StringData.ranking);
        }

        public void SetScoreText(Text scoreText)
        {
            this.scoreText = scoreText;
        }


        public void SetScore(int score)
        {
            scoreText.text = "" + score;
        }
    }
}