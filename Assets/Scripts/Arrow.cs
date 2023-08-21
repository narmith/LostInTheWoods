using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject impactEffect;
    public int impactDmg = 10;
    public float forwardSpeed = 30f;
    private Rigidbody rg;
    public AudioSource audioFx;

    private void Start()
    {
        rg = this.gameObject.GetComponent<Rigidbody>();
        audioFx = this.gameObject.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (forwardSpeed > 0 && rg)
        {
            forwardSpeed -= 1f * Time.deltaTime;
            rg.AddRelativeForce(Vector3.forward * forwardSpeed);
        } else forwardSpeed = 0;
    }

    public void OnCollisionEnter(Collision otherColl)
    {
        forwardSpeed = 0;

        if (audioFx && !audioFx.isPlaying) { audioFx.Play(); }

        if (otherColl.gameObject.CompareTag("Enemy"))
        {
            if (otherColl.gameObject.TryGetComponent(out HP targetHP))
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
                targetHP.DamageHP(impactDmg);
            }
        }
    }
}
