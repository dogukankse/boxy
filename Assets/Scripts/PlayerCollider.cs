using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollider : MonoBehaviour
{
    public Action<int> UpdateScore;
    public Action GameOver;

    public int Score { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Point"))
        {
            Score++;
            Destroy(other.gameObject);
            UpdateScore(Score);
        }
        else if (other.CompareTag("Obstacle"))
        {
            print("game over");
            GameOver();
        }
        else if (other.CompareTag("MagnetBooster"))
        {
            GameData.Instance().magnetBoosterCount += 1;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("SlowBooster"))
        {
            GameData.Instance().slowBoosterCount += 1;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BombBooster"))
        {
            GameData.Instance().bombBoosterCount += 1;
            Destroy(other.gameObject);
        }
    }
}