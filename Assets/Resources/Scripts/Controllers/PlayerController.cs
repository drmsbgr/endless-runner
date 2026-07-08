using RatRush.Entities;
using RatRush.Enums;
using RatRush.Managers;
using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace RatRush.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider capsuleCol;
        [SerializeField] private Animator ratAnimator;
        [SerializeField] private PlayerAudioController playerAudioController;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [Header("Player Settings")]
        [SerializeField] private float horizontalRange = 3f;
        [SerializeField] private float swaySpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float gravity;
        [SerializeField] private float stickGroundVel;
        [SerializeField] private float standHeight = 1.563419f;
        [SerializeField] private float crouchHeight = 1.563419f / 2f;
        [Header("Ground Check Settings")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayerMask;
        [Header("Visual Settings")]
        [SerializeField] private Vector3 eatingRotation;
        [SerializeField] private GameObject cheeseInHand;
        private Rigidbody rb;
        private bool hasJumped;
        private bool isGrounded;
        private Lane targetLane;
        private bool isCrouch;
        private float curCrouchWeight;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            targetLane = Lane.Middle;
        }

        public void OnMoveLeft(CallbackContext context)
        {
            if (GameManager.instance.gameStatus != GameStatus.Running)
                return;
            if (context.performed && targetLane != Lane.Left)
                targetLane--;
        }

        public void OnMoveRight(CallbackContext context)
        {
            if (GameManager.instance.gameStatus != GameStatus.Running)
                return;
            if (context.performed && targetLane != Lane.Right)
                targetLane++;
        }

        public void OnCrouch(CallbackContext context)
        {
            if (GameManager.instance.gameStatus != GameStatus.Running)
                return;

            if (context.started)
            {
                isCrouch = true;
                capsuleCol.height = crouchHeight;
                capsuleCol.center = crouchHeight / 2f * Vector3.up;
            }
            else if (context.canceled)
            {
                isCrouch = false;
                capsuleCol.height = standHeight;
                capsuleCol.center = standHeight / 2f * Vector3.up;
            }
        }

        public void OnJump(CallbackContext context)
        {
            if (GameManager.instance.gameStatus != GameStatus.Running)
                return;
            if (context.performed && isGrounded && !hasJumped)
                StartJump();
        }

        private void StartJump()
        {
            hasJumped = true;
        }

        void OnEnable()
        {
            GameEvents.OnGameStart += OnGameStart;
        }

        void OnDisable()
        {
            GameEvents.OnGameStart -= OnGameStart;
        }

        private void OnGameStart()
        {
            speedFactor = initialSpeedFactor;
            cheeseInHand.SetActive(false);
            //rigidbody içeren bir kopya rastgele yöne fırlatılabilir.
            ratAnimator.Play("Locomotion");
        }

        private float speedFactor = 1f;
        [SerializeField] private float initialSpeedFactor = 1f;
        [SerializeField] private float speedFactorRate = 0.01f;
        [SerializeField] private float maxSpeedFactor = 2f;

        void Update()
        {
            if (GameManager.instance.gameStatus == GameStatus.Running)
            {
                ratAnimator.transform.localRotation = Quaternion.Slerp(ratAnimator.transform.localRotation, Quaternion.Euler(Vector3.zero), 10f * Time.deltaTime);
                if (speedFactor < maxSpeedFactor)
                {
                    speedFactor += speedFactorRate * Time.deltaTime;
                    if (speedFactor > maxSpeedFactor)
                        speedFactor = maxSpeedFactor;
                    ratAnimator.SetFloat("speedFactor", speedFactor);
                }
            }

            var targetC = isCrouch ? 1f : 0f;
            curCrouchWeight = Mathf.Lerp(curCrouchWeight, targetC, 10f * Time.deltaTime);
            ratAnimator.SetFloat("Crouch", curCrouchWeight);
        }

        void FixedUpdate()
        {
            isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayerMask, QueryTriggerInteraction.Ignore);
            ratAnimator.SetBool("isGrounded", isGrounded);

            var newVelocity = rb.linearVelocity;

            float targetX = 0f;
            switch (targetLane)
            {
                case Lane.Left:
                    targetX = -horizontalRange;
                    break;
                case Lane.Middle:
                    targetX = 0f;
                    break;
                case Lane.Right:
                    targetX = horizontalRange;
                    break;
            }

            var distanceX = targetX - rb.position.x;

            if (Mathf.Abs(distanceX) < 0.05f)
            {
                newVelocity.x = 0f;
                rb.position = new Vector3(targetX, rb.position.y, rb.position.z);
            }
            else
                newVelocity.x = distanceX * swaySpeed;


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
            {
                if (isCrouch)
                    newVelocity.y -= gravity * 5f * Time.fixedDeltaTime;
                else
                    newVelocity.y -= gravity * Time.fixedDeltaTime;
            }

            rb.linearVelocity = newVelocity;
        }

        void OnDrawGizmos()
        {
            if (groundCheckPoint != null)
            {
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + Vector3.back);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + lastContactDir);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Collectible>(out var collectible))
            {
                GameManager.instance.ActivateParticle(other.transform.position);
                collectible.Collect();
                playerAudioController.Collect();
                GameEvents.OnPlayerCollect?.Invoke(collectible);
            }
        }

        private Vector3 lastContactDir;

        public void Crush()
        {
            ratAnimator.Play("Crush");
        }

        void OnCollisionEnter(Collision collision)
        {
            if (GameManager.instance.gameStatus == GameStatus.Over)
                return;
            if (collision.collider.TryGetComponent<Obstacle>(out var obstacle))
            {
                var dir = transform.position - collision.collider.transform.position;
                dir.y = 0f;
                lastContactDir = dir;

                var angle = Vector3.Angle(Vector3.back, dir);

                impulseSource.GenerateImpulse(2f);

                if (angle <= 30f)
                {
                    //tek yer
                    GameManager.instance.GameOver();
                    Crush();

                    if (obstacle.obstacleType == ObstacleType.Human)
                    {
                        ratAnimator.transform.localScale = new(.8f, .1f, .8f);
                    }
                    else
                    {
                        ratAnimator.transform.localScale = new(.8f, .1f, .8f);
                        ratAnimator.transform.SetLocalPositionAndRotation(Vector3.up, Quaternion.Euler(-90f, 0f, 0f));
                    }

                    playerAudioController.Crush(obstacle.obstacleType);

                    UIManager.instance.deathCause.StringReference = obstacle.GetDeathStringReference();
                    GameEvents.OnPlayerImpact?.Invoke(true);
                }
                else
                {
                    playerAudioController.Damage();
                    GameEvents.OnPlayerImpact?.Invoke(false);
                }
            }
        }
    }
}
