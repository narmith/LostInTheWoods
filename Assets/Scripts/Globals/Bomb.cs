using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage = 30;
    public float bombDistance = 500f;
    public bool goBoom = false;
    public GameObject explosionEffect;
    public GameObject lightEffect;

    Renderer objectColor;
    float timedCounter = 0;

    void Explode()
    {
        goBoom = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (goBoom == true)
        {
            if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Destructible"))
            {
                if (other.gameObject.TryGetComponent(out Health otherHealth))
                {
                    otherHealth.TakeDamage(damage);
                }
            }

            Instantiate(lightEffect, transform.position, transform.rotation);
            Instantiate(explosionEffect, transform.position, transform.rotation);

            Destroy(this.gameObject);
        }
    }

    void Awake()
    {
        objectColor = this.gameObject.GetComponent<Renderer>();
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bombDistance);
        Invoke("Explode", 3);
    }

    void Colorer()
    {
        if (objectColor.material.color != Color.yellow)
        {
            objectColor.material.SetColor("_Color", Color.yellow);
        }
        else objectColor.material.SetColor("_Color", Color.red);

        timedCounter = 1f;
    }

    void Update()
    {
        /* ALTERNATE COLORS */
        if(gameObject!=null)
        {
            if (timedCounter <= 0)
            { Colorer(); }
            else
            { timedCounter -= 3f * Time.deltaTime; } //color change frecuency
        }
        /* --- */
    }
}
