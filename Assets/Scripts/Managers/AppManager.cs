using System;
using UnityEngine;

namespace Managers
{
    public class AppManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject shopMenu;
        [SerializeField] private GameObject gameScreen;

        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameManager gameManager;

        private GameObject game;

        private void Awake()
        {
            LanguageManager.Instance().SetLanguage("English");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //uiManager.CreateGameWindow = CreateGame;
            uiManager.SetTexts();
            uiManager.CreateGameWindow = CreateGame;
        }


        private GameObject CreateGame(GameObject canvas)
        {
            game = Instantiate(gameScreen, canvas.transform, false);
            StartCoroutine(AdsManager.Instance().ShowBannerWhenReady());
            gameManager = game.GetComponentInChildren<GameManager>();
            
            uiManager.SetScoreText(gameManager.GetScoreText());
            gameManager.player.UpdateScore = uiManager.SetScore;
            gameManager.UpdeteUI = uiManager.UpdateUI;
            return game;
        }

        private void OnDestroy()
        {
            SaveManager.Save(GameData.Instance());
            print("appmanager on destroy");
        }
    }
}