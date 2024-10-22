using UnityEngine;

namespace TFBG
{
    public class MeleeEnemyController : MonoBehaviour
    {
        [Tooltip("Range to randomly select movement speed from.")]
        [SerializeField]
        private Vector2 movementSpeedRange = new(3f, 6f);

        [Tooltip("The maximum distance at which to start targeting the player.")]
        [SerializeField]
        private float maxTargetPlayerDistance = 5f;

        [Tooltip("Amount of time after attacking player that enemy will be immobile and harmless.")]
        [SerializeField]
        private float stunTimeSec = 1f;

        [Tooltip("Number of times enemy will damage the player before the enemy is destroyed.")]
        [SerializeField]
        private int attacksBeforeDestruction = 3;

        [Tooltip("Damage the enemy will deal to the player when attacking.")]
        [SerializeField]
        private float damageToDeal = 1f;

        private float stunTimer;
        private float attacksRemaining;
        private Vector3 movement = Vector3.zero;
        private float movementSpeed;

        private PlayerController Player => FindObjectOfType<PlayerController>();
        private bool IsStunned => stunTimer > 0f;

        private void Awake()
        {
            stunTimer = stunTimeSec;
            attacksRemaining = attacksBeforeDestruction;
            movementSpeed = Random.Range(movementSpeedRange.x, movementSpeedRange.y);

            UpdateMovementTowardsPlayer();
        }

        private void Update()
        {
            if (IsStunned)
            {
                stunTimer -= Level.Instance.GameplayDeltaTime;
                if (!IsStunned)
                {
                    Renderer renderer = GetComponentInChildren<Renderer>();
                    renderer.material.color = Color.white;
                }
                return;
            }

            if ((Player.transform.position - transform.position).magnitude <= maxTargetPlayerDistance)
            {
                UpdateMovementTowardsPlayer();
            }

            transform.position += movement;
        }

        private void UpdateMovementTowardsPlayer()
        {
            Vector3 movementDirection = (Player.transform.position - transform.position).normalized;
            movement = movementDirection * (Level.Instance.GameplayDeltaTime * movementSpeed);

            transform.LookAt(Player.transform.position);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsStunned)
            {
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Destroy(gameObject);
                return;
            }

            if (other.gameObject == Player.gameObject)
            {
                CollideWithPlayer();
            }
        }

        private void CollideWithPlayer()
        {
            if (!Player.Damage(damageToDeal))
            {
                return;
            }

            stunTimer = stunTimeSec;
            Renderer renderer = GetComponentInChildren<Renderer>();
            renderer.material.color = Color.black;

            attacksRemaining--;
            if (attacksRemaining <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}