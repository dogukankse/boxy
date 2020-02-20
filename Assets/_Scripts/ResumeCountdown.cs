using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResumeCountdown : MonoBehaviour
{
    [SerializeField] private Text number;

    void Start()
    {
        number.DOFade(0f, 1f).OnComplete(() =>
        {
            number.text = "2";
            number.DOFade(1f, 0f).OnComplete(() =>
            {
                number.DOFade(0f, 1f).OnComplete(() =>
                {
                    number.text = "1";
                    number.DOFade(1f, 0f).OnComplete(() =>
                    {
                        number.DOFade(0f, 1f).OnComplete(() =>
                        {
                            number.DOFade(1f, 0f).OnComplete(() => { Destroy(gameObject); });
                        });
                    });
                });
            });
        });

        /*GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 180), 3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
        });*/
    }
}