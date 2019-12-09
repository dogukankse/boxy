using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameObject player;
    private float dir = 1;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        player.GetComponent<Image>().color = ColorData.orange;
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

        //pc controller for test
        if (Input.GetMouseButtonDown(0))
        {
            dir *= -1;
        }

        slider.value += .1f * speed * dir * Time.smoothDeltaTime;
    }
}