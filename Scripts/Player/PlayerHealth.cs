using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth; // Max hälsa för spelaren
    public int currentHealth; // Aktuell hälsa
    public Animator damageViewAnimation; // Referens till spelarens Animator

    private LevelLoader levelLoader;

    private void Awake()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Minska aktuell hälsa
        Debug.Log("Player took damage! Current health: " + currentHealth);

        if (damageViewAnimation != null && currentHealth == 1)
        {
            damageViewAnimation.SetTrigger("GotHit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Reload game scene
    private void Die()
    {
        damageViewAnimation.SetTrigger("Dead");
        Debug.Log("Player has died.");

        Invoke(nameof(DisablePlayer), 1f);
        levelLoader.LoadNextChosenLevel("Death_scene"); // Outro level   
    }

    private void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
}
