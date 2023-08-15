using UnityEngine;

public class DelayedSelfDestruction : MonoBehaviour
{
    public float movSpeed = 0f;
    public float timeAlive = 5f;

    void Awake()
    {
        Invoke("SelfDestruct", timeAlive);
    }

    void LateUpdate()
    {
        this.transform.Translate(Vector3.forward * movSpeed * Time.deltaTime);
    }

    void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
