using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace KnightDuel
{
    public class RewardCoinImg : MonoBehaviour
    {
        [SerializeField] RectTransform rect;

        public void AnimateCoin(float[] angles, Vector2 radiusRange, float duration, Ease ease, float delay, RectTransform endPos)
        {
            gameObject.SetActive(true);
            
            float angle = angles[Random.Range(0, angles.Length)];

            float radius = Random.Range(radiusRange.x, radiusRange.y);


            float xPos = radius * Mathf.Cos(Mathf.Deg2Rad * angle) * Random.Range(-1, 2);


            float yPos = radius * Mathf.Sin(Mathf.Deg2Rad * angle) * Random.Range(-1, 2);


            rect.DOLocalMove(new Vector2(xPos, yPos), 0.25f).SetEase(Ease.OutBack).OnComplete
                (() =>
                {
                    GameManager.instance.PlaySFX(7, 1.5f, subIndex:1);
                    rect.DOMove(endPos.position, duration)
                    .SetEase(ease)
                    .SetDelay(delay)
                    .OnComplete(() =>
                    {
                        GameManager.instance.PlaySFX(7, 1.5f);
                        endPos.DOShakeScale(.1f, strength: 0.5f);
                        endPos.DOScale(1f, .2f);
                        rect.gameObject.SetActive(false);
                    }
                );
                }
                );

            
        }
    }
}