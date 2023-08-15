using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public bool godMode = false;
    public int maxHealth = 10;
    public int currentHealth = 10;
    public HealthBar healthBar;
    public AudioSource audioLowHealth;
    public AudioSource audioTakeDmg;
    public AudioSource audioGetHeal;

    EnemyManager enemyManager;

    private int _probability;
    public GameObject dropWhenDead;

    void Start()
    {
        currentHealth = maxHealth;
        if(healthBar != null)
        {
            healthBar.setMaxHealth(maxHealth);
        }

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
                //SendMessage("EnemyIsDead");
            }
            else if (this.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("GameOver");
            }

            if(dropWhenDead!=null)
            {
                _probability = Random.Range(1, 10);

                if (_probability < 3)
                {
                    print("Dropped an item!");
                    Instantiate(dropWhenDead, transform.position, transform.rotation);
                }
            }

            Destroy(this.gameObject);
        }

        // SOUND
        if (this.gameObject.CompareTag("Player"))
        {
            if (audioLowHealth == null)
                { print(">> " + gameObject.name + " missing sound: LowHealth."); }
            else if (currentHealth <= maxHealth / 4 && !audioLowHealth.isPlaying)
                {
                    audioLowHealth.Play();
                }
                else if (currentHealth > 30 && audioLowHealth.isPlaying)
                {
                    audioLowHealth.Stop();
                }
        }
    }
    public void TakeDamage(int damage)
    {
        if (!godMode)
        {
            currentHealth -= damage;

            // UI
            if (healthBar != null)
            {
                healthBar.setHealth(currentHealth);
            }
            //print("Entity " + gameObject.name + " " + currentHealth + "/" + maxHealth + ".");
        }

        // SOUND
        if (audioTakeDmg == null)
            { print(">> " + gameObject.name + " missing sound: TakeDmg."); }
        else if (currentHealth > 0)
            {
                audioTakeDmg.Play();
            }
    }

    public void GetHealed(int heal)
    {
        currentHealth += heal;

        // UI
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (healthBar != null)
        {
            healthBar.setHealth(currentHealth);
        }

        // SOUND
        if (audioGetHeal != null)
        {
            if (currentHealth > 0)
            {
                audioGetHeal.Play();
            }
        }
    }
}
