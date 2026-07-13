using System.Linq;
using RatRush.Enums;
using RatRush.Managers;
using UnityEngine;

namespace RatRush.Entities
{
    public class Human : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private readonly Collider[] nearObstacles = new Collider[5];
        [SerializeField] private LayerMask obstacleMask;
        private static readonly float jumpCooldown = .8f;
        private float jumpTimer;

        void Update()
        {
            if (jumpTimer > 0f)
            {
                jumpTimer -= Time.deltaTime;
                if (jumpTimer <= 0f)
                    jumpTimer = 0f;
            }

            bool shouldRun = GameManager.instance.gameStatus == GameStatus.Running;
            var count = Physics.OverlapSphereNonAlloc(transform.position, 1f, nearObstacles, obstacleMask, QueryTriggerInteraction.Ignore);

            if (jumpTimer <= 0f && count > 0)
            {
                var nearest = nearObstacles.Take(count).OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault(x => x.GetComponent<Obstacle>().obstacleType != ObstacleType.Human);
                if (nearest != null)
                {
                    if (nearest.GetComponent<Obstacle>().obstacleType is ObstacleType.TrashcanFall or ObstacleType.Normal || (nearest.GetComponent<Obstacle>().obstacleType == ObstacleType.Bench && nearest.transform.parent.GetComponent<Bench>().HasNoHuman()))
                    {
                        animator.SetTrigger("jump");
                        jumpTimer = jumpCooldown;
                    }
                    else
                        shouldRun = false;
                }
            }

            animator.SetBool("run", shouldRun);
        }
    }
}
