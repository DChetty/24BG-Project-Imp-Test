using UnityEditor;
using UnityEngine;

namespace TFBG
{
    public class ObjectSpawner : MonoBehaviour
    {
        [Tooltip("Object to spawn instances of.")]
        [SerializeField]
        private GameObject template;
        [Tooltip("Range to randomly select a spawn delay from.")]
        [SerializeField]
        private Vector2 spawnDelayRangeSec = new(2.5f, 10.0f);
        [Tooltip("Radius to randomly select a spawn location from.")]
        [SerializeField]
        private float spawnRadius = 0.1f;

        private float spawnTimer;

        private void Start()
        {
            spawnTimer = Random.Range(spawnDelayRangeSec.x, spawnDelayRangeSec.y);
        }

        private void Update()
        {
            spawnTimer -= Level.Instance.GameplayDeltaTime;
            if (spawnTimer <= 0.0f)
            {
                SpawnObject();
                spawnTimer = Random.Range(spawnDelayRangeSec.x, spawnDelayRangeSec.y);
            }
        }

        private void SpawnObject()
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector3 position = transform.position;
            position.x += offset.x;
            position.z += offset.y;
            Instantiate(template, position, Quaternion.identity);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, transform.up, spawnRadius);
        }
#endif
    }
}