using RatRush.Managers;
using UnityEngine;

namespace RatRush
{
    public class Cat : MonoBehaviour
    {
        private Vector3 targetPos;
        private Quaternion targetRot;

        void Update()
        {
            targetPos = new(GameManager.instance.player.transform.position.x, transform.position.y, transform.position.z);
            var dir = GameManager.instance.player.transform.position - transform.position;
            dir.y = 0f;
            targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);

            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, targetPos, 7f * Time.deltaTime), Quaternion.Slerp(transform.rotation, targetRot, 7f * Time.deltaTime));
        }
    }
}
