using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        private float bornLeftX;
        private float bornRightX;
        private float bornY;
        private float dieY;


        [SerializeField] private GameObject UIManager;
        private int score;
        private int callID;

        private void Awake()
        {
            bornLeftX = bornLeft.anchoredPosition.x;
            bornRightX = bornRight.anchoredPosition.x;
            bornY = bornLeft.anchoredPosition.y;
            dieY = die.anchoredPosition.y;
        }

        private void Start()
        {
            callID = LeanTween.delayedCall(GameData.Instance().bornDelay, CreateCube).setRepeat(-1).id;
        }

        void UpdateScore()
        {
        }


        void CreateCube()
        {
            GameObject cube;
            float startPosX = Random.Range(bornLeftX, bornRightX);
            //float toX = Random.Range(dieLeftX, dieRightX);
            if (Random.Range(0f, 1f) <= GameData.Instance().pointRatio)
                cube = Instantiate(obstacle, canvas.transform, true);
            else
                cube = Instantiate(point, canvas.transform, true);

            RectTransform rectTransform = cube.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(startPosX, bornY, 0);

            LeanTween.move(rectTransform, new Vector3(startPosX, dieY, 0), GameData.Instance().speed)
                .setOnComplete(() => { Destroy(cube); });
            //LeanTween.move(cube.GetComponent<RectTransform>(), new Vector3(toX, dieY, 0), 1f).setOnComplete(() => Destroy(cube));
        }

        private void OnDestroy()
        {
            LeanTween.cancel(callID);
        }
    }
}