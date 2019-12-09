using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

enum States
{
    START_PLAYING,
    PLAYING,
    PAUSE,
    EXIT_PLAYING
}

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private RectTransform bornLeft;
        [SerializeField] private RectTransform bornRight;
        [SerializeField] private RectTransform die;

        [SerializeField] private GameObject canvas;

        [SerializeField] private GameObject point;
        [SerializeField] private GameObject obstacle;
        [SerializeField] public PlayerCollider player;
        [SerializeField] private Text scoreText;

        public UnityAction UpdeteUI;


        private float bornLeftX;
        private float bornRightX;
        private float bornY;
        private float dieY;

        private int callID;

        private void Awake()
        {
            bornLeftX = bornLeft.anchoredPosition.x;
            bornRightX = bornRight.anchoredPosition.x;
            bornY = bornLeft.anchoredPosition.y;
            dieY = die.anchoredPosition.y;
            SendGameManager();
        }

        private GameManager SendGameManager()
        {
            return this;
        }

        private void Start()
        {
            callID = LeanTween.delayedCall(GameData.Instance().bornDelay, CreateCube).setRepeat(-1).id;
        }


        void CreateCube()
        {
            GameObject cube;
            float startPosX = Random.Range(bornLeftX, bornRightX);
            //float toX = Random.Range(dieLeftX, dieRightX);
            if (Random.Range(0f, 1f) <= GameData.Instance().pointRatio)
                cube = Instantiate(obstacle, canvas.transform, true);
            else
            {
                cube = Instantiate(point, canvas.transform, true);
                cube.GetComponent<Image>().color = ColorData.orange;
            }


            RectTransform rectTransform = cube.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(startPosX, bornY, 0);

            LeanTween.move(rectTransform, new Vector3(startPosX, dieY, 0), GameData.Instance().speed)
                .setOnComplete(() => { Destroy(cube); });
            //LeanTween.move(cube.GetComponent<RectTransform>(), new Vector3(toX, dieY, 0), 1f).setOnComplete(() => Destroy(cube));
        }

        private void OnDestroy()
        {
            print("gameManager on destroy");
            LeanTween.cancel(callID);
            GameData.Instance().lastScore = player.Score;
            if (GameData.Instance().lastScore > GameData.Instance().highScore)
                GameData.Instance().highScore = GameData.Instance().lastScore;
            UpdeteUI();
        }

        public Text GetScoreText()
        {
            return scoreText;
        }
    }
}