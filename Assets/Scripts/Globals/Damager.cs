using UnityEngine;

public class Damager : MonoBehaviour
{
    public int damage = 10;
    public GameObject impactEffect;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Health otherHealth))
        {
            otherHealth.TakeDamage(damage);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Environment")
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}


