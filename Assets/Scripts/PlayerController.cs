using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private GameObject player;
    private float dir = 1;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        player.GetComponent<Image>().color = GameData.Instance().color;
    }

    private void Update()
    {
        if (slider.value == 0f || slider.value == 1f)
            dir *= -1;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                dir *= -1;
        }
#if UNITY_EDITOR
        //pc controller for test
        if (Input.GetMouseButtonDown(0))
        {
            dir *= -1;
        }
#endif
        if (GameData.Instance().gameState == State.PLAYING)
            slider.value += .1f * GameData.Instance().playerSpeed * dir * Time.deltaTime;
    }
}