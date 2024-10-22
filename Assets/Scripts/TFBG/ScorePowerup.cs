using UnityEngine;

namespace TFBG
{
    public class ScorePowerup : MonoBehaviour
    {
        [Tooltip("Score the powerup will award to the player when collected.")]
        [SerializeField]
        private int scoreToAward = 1;

        private PlayerController Player => FindObjectOfType<PlayerController>();

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject != Player.gameObject)
            {
                return;
            }

            Player.AwardScore(scoreToAward);

            Destroy(gameObject);
        }
    }
}