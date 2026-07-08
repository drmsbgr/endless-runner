using RatRush.Enums;
using RatRush.Managers;
using UnityEngine;

namespace RatRush.Entities
{
    public class Human : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        void Update()
        {
            animator.SetBool("run", GameManager.instance.gameStatus == GameStatus.Running);
        }
    }
}
