using UnityEngine;

namespace TFBG
{
    public class Level : Singleton<Level>
    {
        [SerializeField]
        public PlayerController playerTemplate;

        private PlayerController player;

        public float GameplayDeltaTime => player.IsAlive ? Time.deltaTime : 0.0f;

        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            player = Instantiate(playerTemplate);
        }
    }
}