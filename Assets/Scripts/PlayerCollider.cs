using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollider : MonoBehaviour
{
    public Action<int> UpdateScore;

    public int Score { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Point"))
        {
            Score++;
            print(Score);
            Destroy(other.gameObject);
            UpdateScore(Score);
        }
        else
        {
            print("game over");
        }
    }
}