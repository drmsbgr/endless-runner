using System;
using RatRush.Enums;
using RatRush.Managers;
using UnityEngine;

namespace RatRush.Entities
{
    public class Cat : MonoBehaviour
    {
        private Vector3 targetPos;
        private Quaternion targetRot;
        private CatStatus catStatus;
        [SerializeField] private Animator catAnimator;
        [SerializeField] private float farZ = -5f;
        [SerializeField] private float nearZ = -2.5f;
        [SerializeField] private float nearestZ = -1f;

        private float timer;

        void OnEnable()
        {
            GameEvents.OnPlayerImpact += OnPlayerImpact;
            GameEvents.OnGameOver += OnGameOver;
        }

        void OnDisable()
        {
            GameEvents.OnPlayerImpact -= OnPlayerImpact;
            GameEvents.OnGameOver -= OnGameOver;
        }

        private void OnGameOver()
        {
            catAnimator.SetBool("Run", false);
        }

        private void OnPlayerImpact(bool fullDamage)
        {
            if (!fullDamage)
            {
                if (catStatus == CatStatus.Nearest)
                {
                    GameManager.instance.GameOver();
                    UIManager.instance.deathCause.StringReference = new("UI_TABLE", "UI_FOOD_FOR_CAT");
                }
                else
                {
                    catStatus++;
                    timer = 3.5f;
                }
            }
        }

        void Update()
        {
            if (timer > 0f && catStatus != CatStatus.Far && GameManager.instance.gameStatus == GameStatus.Running)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    catStatus--;
                    timer = 3.5f;
                }
            }

            float z = farZ;
            switch (catStatus)
            {
                case CatStatus.Far:
                    break;
                case CatStatus.Near:
                    z = nearZ;
                    break;
                case CatStatus.Nearest:
                    z = nearestZ;
                    break;
            }

            targetPos = new(GameManager.instance.player.transform.position.x, transform.position.y, z);
            var dir = GameManager.instance.player.transform.position - transform.position;
            dir.y = 0f;
            targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);

            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, targetPos, 7f * Time.deltaTime), Quaternion.Slerp(transform.rotation, targetRot, 7f * Time.deltaTime));
        }
    }
}
