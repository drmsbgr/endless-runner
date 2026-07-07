using RatRush.Enums;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace RatRush.Controllers
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float horizontalRange = 3f;
        [SerializeField] private float swaySpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float gravity;
        [SerializeField] private float stickGroundVel;
        [Header("Ground Check Settings")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayerMask;
        private Rigidbody rb;
        private bool hasJumped;
        private bool isGrounded;
        private Lane targetLane;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            targetLane = Lane.Middle;
        }

        public void OnMoveLeft(CallbackContext context)
        {
            if (context.performed && targetLane != Lane.Left)
                targetLane--;
        }

        public void OnMoveRight(CallbackContext context)
        {
            if (context.performed && targetLane != Lane.Right)
                targetLane++;
        }

        public void OnJump(CallbackContext context)
        {
            if (context.performed && isGrounded && !hasJumped)
                StartJump();
        }

        private void StartJump()
        {
            hasJumped = true;
        }

        void FixedUpdate()
        {
            isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayerMask, QueryTriggerInteraction.Ignore);

            var newVelocity = rb.linearVelocity;

            float targetX = 0f;
            switch (targetLane)
            {
                case Lane.Left: targetX = -horizontalRange; break;
                case Lane.Middle: targetX = 0f; break;
                case Lane.Right: targetX = horizontalRange; break;
            }

            newVelocity.x = (targetX - rb.position.x) * swaySpeed;

            if (isGrounded)
            {
                if (hasJumped)
                {
                    hasJumped = false;
                    newVelocity.y = Mathf.Sqrt(2f * gravity * jumpHeight);
                }
                else
                    newVelocity.y = -stickGroundVel;
            }
            else
                newVelocity.y -= gravity * Time.fixedDeltaTime;

            rb.linearVelocity = newVelocity;
        }

        void OnDrawGizmos()
        {
            if (groundCheckPoint != null)
            {
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
            }
        }
    }
}
