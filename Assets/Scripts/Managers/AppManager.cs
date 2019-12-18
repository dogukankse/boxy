using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Managers
{
    public enum State
    {
        START_PLAYING,
        PLAYING,
        PAUSE,
        EXIT_PLAYING,
        MAIN_MENU
    }

    public class AppManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameManager gameManager;

        private State gameState;
        private GameObject game;

        private void Awake()
        {
            LanguageManager.Instance().SetLanguage("English");
            GameData.Instance().gameState = State.MAIN_MENU;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            uiManager.SetTexts();
            uiManager.CreateGameWindow = CreateGame;
            uiManager.DestroyGame = DestroyGame;
        }

        private void CreateGame(GameObject canvas)
        {
            Debug.Log("CreateGame girdi");
            game = Instantiate(gameScreen, canvas.transform, false);
            StartCoroutine(AdsManager.Instance().ShowBannerWhenReady());
            gameManager = game.GetComponentInChildren<GameManager>();
            GameData.Instance().gameState = State.PLAYING;
            uiManager.SetScoreText(gameManager.GetScoreText());
            gameManager.playerCollider.UpdateScore = uiManager.SetScore;
            gameManager.playerCollider.GameOver = uiManager.GameOver;
            gameManager.UpdeteUI = uiManager.UpdateUI;
            uiManager.game = game;
            uiManager.explotion = game.GetComponentInChildren<UIParticleSystem>();
        }

        private void DestroyGame()
        {
            Destroy(game);
        }

        private void OnDestroy()
        {
            SaveManager.Save(GameData.Instance());
            print("appmanager on destroy");
        }
    }
}