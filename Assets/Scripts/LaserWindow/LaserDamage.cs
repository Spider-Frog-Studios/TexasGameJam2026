using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public int damageAmount = 10;

    // Triggered the exact frame the player touches the laser's collider area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered laser trigger! Dealing damage: " + damageAmount);
            // Replace with your health script logic if needed:
            // other.GetComponent<PlayerHealth>()?.TakeDamage(damageAmount);
        }
    }

    // Triggered every frame the player stays inside the laser's collider area
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is staying inside laser trigger! Dealing continuous damage.");
        }
    }
}
