using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject impactEffect;
    public int damage = 2;
    public float Speed = 30f;
    public Rigidbody rg;
    public AudioSource audioFx;

    void Update()
    {
        this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        audioFx.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!audioFx.isPlaying) { audioFx.Play(); }

        if (other.gameObject.TryGetComponent(out HP targetHP))
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            targetHP.TakeDamage(damage);
        }

        this.Speed = 0.5f;
        rg.isKinematic = false;
    }
}
