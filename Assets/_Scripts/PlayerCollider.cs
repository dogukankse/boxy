using UnityEngine;
using UnityEngine.Events;

namespace _Scripts
{
    public class PlayerCollider : MonoBehaviour
    {
        public UnityAction<int> UpdateScore;
        public UnityAction GameOver;
        public UnityAction<GameObject> DestroyOther;
        public UnityAction UpdateUI;

        public int Score { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Point"))
            {
                Score++;
                DestroyOther(other.gameObject);
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
                DestroyOther(other.gameObject);
                UpdateUI();
            }
            else if (other.CompareTag("SlowBooster"))
            {
                GameData.Instance().slowBoosterCount += 1;
                DestroyOther(other.gameObject);
                UpdateUI();

            }
            else if (other.CompareTag("BombBooster"))
            {
                GameData.Instance().bombBoosterCount += 1;
                DestroyOther(other.gameObject);
                UpdateUI();
            }
        }
    }
}