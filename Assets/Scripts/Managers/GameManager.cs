using System;
using System.Collections.Generic;
using DG.Tweening;
using EasyMobile;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private RectTransform player;
        [SerializeField] private GameObject point;
        [SerializeField] private GameObject obstacle;
        [SerializeField] public PlayerCollider playerCollider;
        [SerializeField] private Text scoreText;

        [SerializeField] private GameObject magnetBooster;
        [SerializeField] private GameObject slowBooster;
        [SerializeField] private GameObject bombBooster;

        private bool isBoosterValid = true;
        private bool isSpeedValid = true;
        private bool isRatioValid = true;


        public Button magnet;
        public Button slow;
        public Button bomb;

        public UnityAction UpdeteUI;

        private float bornLeftX;
        private float bornRightX;
        private float bornY;
        private float startPosX;

        private int callID;
        private int cubeID;
        public List<GameObject> currentCubes;

        private float gameWidth;
        private float gameHeight;

        #region UnityLifeCycle

        private void Awake()
        {
            print($"width: {Screen.width} height: {Screen.height}");
            print(
                $"rect.size width: {canvas.GetComponent<RectTransform>().rect.size.x} rect.size height: {canvas.GetComponent<RectTransform>().rect.size.y}");
            print($"{canvas.GetComponent<RectTransform>().sizeDelta}");

            currentCubes = new List<GameObject>();
            //gameWidth = canvas.GetComponent<RectTransform>().rect.size.x;
            //gameHeight = canvas.GetComponent<RectTransform>().rect.size.y;
            gameWidth = Screen.width;
            gameHeight = Screen.height;


            bornLeftX = (gameWidth / -2f);
            bornRightX = (gameWidth / 2f);
            bornY = 2f * gameHeight / 10f; // + (gameHeight / 10f);
            bornY = gameHeight + (gameHeight / 50f); // + (gameHeight / 10f);
        }

        private void Start()
        {
            Sequence tweenSequence = DOTween.Sequence();
            callID = 1;
            tweenSequence.PrependCallback(CreateCube).PrependInterval(GameData.Instance().bornDelay)
                .SetLoops(-1).SetId(callID);
        }

        private void Update()
        {
            if (playerCollider.Score == 0) return;

            if (playerCollider.Score % 5 == 0 && isSpeedValid)
            {
                print("update speed");
                GameData.Instance().objectSpeed += 0.2f;
                GameData.Instance().playerSpeed += 0.2f;
                isSpeedValid = false;
            }
            else if (playerCollider.Score % 5 != 0)
                isSpeedValid = true;

            if (playerCollider.Score % 10 == 0 && isRatioValid)
            {
                print("Update ratio");
                isRatioValid = false;
                //TODO: puan ve engel sayıları güncellenecek
            }
            else if (playerCollider.Score % 10 != 0)
                isRatioValid = true;
        }

        private void OnDestroy()
        {
            print("gameManager on destroy");
            DOTween.Kill(callID);
            GameData.Instance().lastScore = playerCollider.Score;
            if (GameServices.IsInitialized())
                GameServices.ReportScore(GameData.Instance().lastScore, EM_GameServicesConstants.Leaderboard_Rakings);
            if (GameData.Instance().lastScore > GameData.Instance().highScore)
                GameData.Instance().highScore = GameData.Instance().lastScore;
            UpdeteUI();
        }

        #endregion

        #region Powerups

        public void Magnet()
        {
            DOTween.Kill(11);
            foreach (GameObject cube in currentCubes)
            {
                if (cube != null)
                    DOTween.To(() => cube.GetComponent<RectTransform>().anchoredPosition3D,
                        (x) => { cube.GetComponent<RectTransform>().DOMove(playerCollider.transform.position, 1f); },
                        player.transform.position, 1f).OnComplete(RemoveNulls);
            }
        }

        public void Slow()
        {
            GameData.Instance().objectSpeed *= 2f;
            float start = 0;
            DOTween.To(() => start, (x) => start = x, 3f, 3f)
                .OnComplete(() => { GameData.Instance().objectSpeed /= 2f; });
        }

        public void Bomb()
        {
            foreach (GameObject cube in currentCubes)
            {
                Destroy(cube);
            }

            RemoveNulls();
        }

        #endregion

        void CreateCube()
        {
            GameObject cube = null;
            startPosX = Random.Range(bornLeftX - gameWidth / 10f, bornRightX + gameWidth / 10f);
            float r = Random.Range(0f, 1f);


            if (playerCollider.Score % 30 == 0 && playerCollider.Score != 0 && isBoosterValid)
            {
                print("create booster");
                if (r < 0.33f)
                {
                    //magnetbooster
                    cube = Instantiate(magnetBooster, canvas.transform, false);
                }
                else if (r >= 0.33f && r < 0.66f)
                {
                    //slowbooster
                    cube = Instantiate(slowBooster, canvas.transform, false);
                }
                else
                {
                    //bombbooster
                    cube = Instantiate(bombBooster, canvas.transform, false);
                }

                cube.GetComponent<Image>().color = GameData.Instance().Color;
                currentCubes.Add(cube);
                cubeID = 11;

                isBoosterValid = false;
            }
            else
            {
                if (GameData.Instance().pointBornRatio <= r)
                {
                    cube = Instantiate(obstacle, canvas.transform, false);
                    cubeID = 10;
                }
                else
                {
                    cube = Instantiate(point, canvas.transform, false);
                    cube.GetComponent<Image>().color = GameData.Instance().Color;
                    cubeID = 11;
                    currentCubes.Add(cube);
                }
            }

            if (playerCollider.Score % 30 != 0)
                isRatioValid = true;

            RectTransform rectTransform = cube.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(startPosX, bornY);

            float curve = Random.Range((gameWidth * 14f) / 100f, gameWidth - (gameWidth * 14f) / 100f);
            rectTransform.DOPath(
                    new Vector3[]
                    {
                        new Vector2(curve, gameHeight * 23f / 100f),
                        new Vector2(curve, -gameHeight / 10f), //-100f),
                    },
                    GameData.Instance().objectSpeed, PathType.CatmullRom,
                    PathMode.Ignore, 5, Color.red)
                .SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    rectTransform.DORotate(new Vector3(0, 0, 360), GameData.Instance().objectSpeed,
                        RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear).SetRelative(true);
                })
                .OnComplete(() =>
                {
                    Destroy(cube);
                    RemoveNulls();
                }).SetId(cubeID);
        }

        private void RemoveNulls()
        {
            currentCubes.RemoveAll(item => item == null);
        }

        public Text GetScoreText()
        {
            return scoreText;
        }

        private void RandomBoosterItem()
        {
            //todo: rasgele booster canı artıracak item düşecek
        }
    }
}