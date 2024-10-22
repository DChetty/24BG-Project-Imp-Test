using UnityEngine;
using UnityEngine.InputSystem;

namespace TFBG
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float movementSpeed = 5f;

        [SerializeField]
        private float startingHealth = 10f;

        [SerializeField]
        private float immuneTimeSec = 2f;

        private Vector2 moveInput;
        private float immuneTimer;

        /// <summary>
        /// Gets whether the player is alive.
        /// </summary>
        public bool IsAlive => Health > 0f;

        /// <summary>
        /// Gets the player's current health.
        /// </summary>
        public float Health { get; private set; }

        /// <summary>
        /// Gets the player's current score.
        /// </summary>
        public int Score { get; private set; }

        private bool IsImmune => immuneTimer > 0f;

        private void Awake()
        {
            Health == startingHealth;
        }

        private void Update()
        {
            Vector3 movement = new Vector3(moveInput.x, 0.0f, moveInput.y);

            transform.position += movement * (Level.Instance.GameplayDeltaTime * movementSpeed);
            Vector3 direction = transform.position + movement;
            Debug.Log($"Player new position {transform.position} and direction {direction}");
            
            transform.LookAt(direction);

            if (IsImmune)
            {
                immuneTimer -= Level.Instance.GameplayDeltaTime;
                if (!IsImmune)
                {
                    Renderer renderer = GetComponentInChildren<Renderer>();
                    renderer.material.color = Color.white;
                }
            }
        }

        /// <summary>
        /// Update movement vector from an <see cref="InputAction"/>.
        /// </summary>
        /// <param name="context">Information about what triggered the action.</param>
        public void Move(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                moveInput = Vector2.zero;
            }
            else
            {
                moveInput = context.ReadValue<Vector2>();
            }
        }

        /// <summary>
        /// Deal damage to the player. If the player's health reaches 0 or below as a result of this damage, they die.
        /// If the player is currently immune to damage (was recently damaged) this method will have no effect.
        /// </summary>
        /// <param name="damageToDeal">The amount of damage to deal to the player.</param>
        /// <returns><see langword="true"/> if damage was dealt to the player, <see langword="false"/> otherwise.</returns>
        public bool Damage(float damageToDeal)
        {
            if (IsImmune)
            {
                return false;
            }

            Health -= damageToDeal;
            if (Health <= 0.0f)
            {
                Die();
                return true;
            }

            immuneTimer = immuneTimeSec;
            Renderer renderer = GetComponentInChildren<Renderer>();
            renderer.material.color = Color.red;
            Handheld.Vibrate();

            return true;
        }

        /// <summary>
        /// Award score to the player.
        /// </summary>
        /// <param name="scoreToAward">The amount of score to award to the player.</param>
        public void AwardScore(int scoreToAward)
        {
            Score += scoreToAward;
        }

        private void Die()
        {
            Handheld.Vibrate();
            Debug.Log($"Player died with a score of {Score}.");
        }
    }
}