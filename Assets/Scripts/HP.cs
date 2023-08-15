using UnityEngine;
using UnityEngine.SceneManagement;

public class HP : MonoBehaviour
{
    public bool godMode = false;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public HealthBar healthBar;
    EnemyManager enemyManager;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.setMaxHealth(maxHealth);
        }

        //check EnemyManager existe y si tiene su componente
        GameObject manager = GameObject.Find("EnemyManager");
        if (manager == null) { print("No existe el GameObject: EnemyManager."); }
        else
        {
            if (manager.GetComponent<EnemyManager>() == null) { print("No existe el componente: EnemyManager."); }
            else { enemyManager = manager.GetComponent<EnemyManager>(); }
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            if (this.gameObject.CompareTag("Enemy"))
            {
                enemyManager.EnemyIsDead();
            }
            else if (this.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("GameOver");
            }

            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!godMode)
        {
            currentHealth -= damage;
        }
    }
}
