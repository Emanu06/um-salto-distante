using UnityEngine;

namespace Platformer
{
    public class PowerUP : MonoBehaviour
    {
        public float duration = 5f;
        public float speedMultiplier = 2f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.StartCoroutine(player.SpeedBoost(duration, speedMultiplier));
                    Destroy(gameObject);
                }
            }
        }
    }
}
