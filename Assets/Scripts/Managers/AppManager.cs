using System;
using DG.Tweening;
using EasyMobile;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public enum State
    {
        PLAYING,
        PAUSE,
        EXIT_PLAYING,
        MAIN_MENU,
        SETTINGS,
        SHOP,
        RANKINGS
    }

    public class AppManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private int videoAdLimit;

        private State gameState;
        private GameObject game;
        private SavedGame save;

        private int _gameCount;


        #region UnityLifeCycle

        private void Awake()
        {
            //easymobile init
            if (!RuntimeManager.IsInitialized())
                RuntimeManager.Init();

            //for google play services
            if (!GameServices.IsInitialized())
                GameServices.Init();

            //default or last language
            LanguageManager.Instance().SetLanguage(GameData.Instance().language);

            //dotween settings
            DOTween.logBehaviour = LogBehaviour.Verbose;

            _gameCount = 0;

            GameData.Instance().gameState = State.MAIN_MENU;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            uiManager.SetTexts();
            uiManager.CreateGameWindow = CreateGame;
            uiManager.DestroyGame = DestroyGame;
            AdsManager.Instance().OnRewardedVideoFinished = uiManager.UpdateUI;
        }

        private void Update()
        {
            if (GameData.Instance().gameState == State.MAIN_MENU)
            {
                if (_gameCount == videoAdLimit)
                {
                    AdsManager.Instance().ShowVideo();
                    _gameCount = 0;
                }
            }
        }

        private void OnEnable()
        {
            GameServices.UserLoginSucceeded += OnUserLoginSucceeded;
            GameServices.UserLoginFailed += OnUserLoginFailed;
        }

        private void OnDisable()
        {
            GameServices.UserLoginSucceeded -= OnUserLoginSucceeded;
            GameServices.UserLoginFailed -= OnUserLoginFailed;
        }

        private void OnDestroy()
        {
            SaveManager.Save();
        }

        #endregion


        #region Events

        void OnUserLoginSucceeded()
        {
            Debug.Log("User logged in successfully.");
            uiManager.UpdateUI();
        }

        void OnUserLoginFailed()
        {
            Debug.Log("User login failed.");
            uiManager.UpdateUI();
        }

        #endregion

        private void CreateGame(GameObject frame)
        {
            print("before createGame: " + _gameCount);
            _gameCount += 1;
            Debug.Log("CreateGame girdi");
            game = Instantiate(gameScreen, frame.transform, false);
            StartCoroutine(AdsManager.Instance().ShowBannerWhenReady());
            gameManager = game.GetComponentInChildren<GameManager>();
            uiManager.SetScoreText(gameManager.GetScoreText());
            gameManager.playerCollider.UpdateScore = uiManager.SetScore;
            gameManager.playerCollider.GameOver = uiManager.GameOver;


            gameManager.UpdeteUI += uiManager.UpdateUI;
            gameManager.UpdeteUI += uiManager.UpdateGameUI(gameManager.magnet.GetComponentInChildren<Text>(),
                gameManager.slow.GetComponentInChildren<Text>(), gameManager.bomb.GetComponentInChildren<Text>());

            uiManager.Magnet = gameManager.Magnet;
            uiManager.Slow = gameManager.Slow;
            uiManager.Bomb = gameManager.Bomb;

            gameManager.magnet.onClick.AddListener(uiManager.MagnetPower);
            gameManager.slow.onClick.AddListener(uiManager.SlowPower);
            gameManager.bomb.onClick.AddListener(uiManager.BombPower);


            uiManager.game = game;
            uiManager.explotion = game.GetComponentInChildren<ParticleSystem>();

            print("after createGame: " + _gameCount);
        }

        private void DestroyGame()
        {
            Destroy(game);
        }
    }
}