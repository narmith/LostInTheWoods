using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 10f;

    void Update()
    {
        this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
}
