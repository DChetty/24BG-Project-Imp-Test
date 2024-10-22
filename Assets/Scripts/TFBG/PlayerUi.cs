using TMPro;
using UnityEngine;

namespace TFBG
{
    public class PlayerUi : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreLabel;
        [SerializeField]
        private TMP_Text healthLabel;

        private PlayerController Player => FindObjectOfType<PlayerController>();

        private void Update()
        {
            Debug.Log($"Player Score: {Player.Score} and Health: {Player.Health}");
            scoreLabel.text = $"Score: {Player.Score}";
            healthLabel.text = $"Health: {Player.Health}";
        }
    }
}
