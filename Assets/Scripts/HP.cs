using UnityEngine;
using UnityEngine.SceneManagement;

public class HP : MonoBehaviour
{
    public bool godMode = false;
    private int maxHealth = 100;
    public int currentHealth = 100;
    HealthBar healthBar;
    EnemyManager enemyManager;

    void Start()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            healthBar = FindObjectOfType<HealthBar>();
            if (!healthBar) { print("Error: HealthBar does not exist!"); }
            else healthBar.setMaxHealth(maxHealth);
        }

        enemyManager = FindObjectOfType<EnemyManager>();
        if (!enemyManager) { print("Error: EnemyManager does not exist!"); }
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
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }

        if (healthBar)
        {
            healthBar.setHealth(currentHealth);
        }
    }

    public bool GodMode() // Ask if this has GodMode enabled.
    {
        return godMode;
    }
    public void GodMode(bool state) // Set GodMode ON or OFF.
    {
        godMode=state;
    }
    public int Health()
    {
        return currentHealth;
    }
    public void Health(int newHealth) // Change the current health to a new value.
    {
        currentHealth = newHealth;
    }
    public int MaxHealth()
    {
        return maxHealth;
    }
    public void MaxHealth(int newMaxHealth) // Change the max health to a new value.
    {
        maxHealth = newMaxHealth;
    }
    public int HealHP(int HealingQty) // Heal the current health by value.
    {
        if (HealingQty > 0) // Someone is actually healing you, right? 
        {
            if (currentHealth > 0) // You are not already dead, are you?
            {
                currentHealth += HealingQty;
                if(currentHealth > maxHealth) // Over-healing check
                {
                    int finalHealQty = (currentHealth - maxHealth - HealingQty);
                    currentHealth = maxHealth;
                    return finalHealQty;
                }
                else return HealingQty;
            }
        }
        return 0;
    }
    public int DamageHP(int DmgQty)
    {
        if (!godMode && DmgQty > 0 && currentHealth > 0)
        {
            int finalDmgQty = currentHealth - DmgQty;
            if (finalDmgQty <= 0)
            {
                finalDmgQty = currentHealth;
                currentHealth = 0;
                return finalDmgQty;
            }
            else
            {
                currentHealth -= DmgQty;
                return DmgQty;
            }
        }
        return 0;
    }
}
