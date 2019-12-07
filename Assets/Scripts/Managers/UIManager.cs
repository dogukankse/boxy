using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject tempSettings;
        [SerializeField] private GameObject gameScreen;

        [SerializeField] private Text highscoreText;
        [SerializeField] private Text lastScoreText;
        [SerializeField] private Text rankingText;

        private GameObject currScreen;


        [SerializeField] private Text highscore;
        [SerializeField] private Text lastScore;
        [SerializeField] private Text magnetCount;
        [SerializeField] private Text slowCount;
        [SerializeField] private Text bombCount;

        private GameObject game;


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
                {
                    Destroy(game);
                }
                else if (currScreen == mainMenu)
                {
                    Application.Quit();
                }
                else
                {
                    currScreen.SetActive(false);
                }

                mainMenu.SetActive(true);
            }
        }

        public void PlayButtonListener()
        {
            SaveManager.Save(GameData.Instance());
            mainMenu.SetActive(false);
            currScreen = gameScreen;
            game = Instantiate(gameScreen, canvas.transform, false);
            game.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }

        public void SetTexts()
        {
            LanguageManager manager = LanguageManager.Instance();
            highscoreText.text = manager.getString(StringData.highscore);
            lastScoreText.text = manager.getString(StringData.lastScore);
            rankingText.text = manager.getString(StringData.ranking);
        }

        void UpdateUI()
        {
            highscore.text = "" + GameData.Instance().highScore;
            lastScore.text = "" + GameData.Instance().lastScore;
            magnetCount.text = "" + GameData.Instance().magnetBoosterCount;
            slowCount.text = "" + GameData.Instance().slowBoosterCount;
            bombCount.text = "" + GameData.Instance().bombBoosterCount;
        }

        void TempSettings()
        {
            settingsMenu.SetActive(false);
            tempSettings.SetActive(true);
        }
    }
}