using UnityEngine;

public class DelayedSelfDestruction : MonoBehaviour
{
    public float timeAlive = 5f;

    void Awake()
    {
        Invoke("SelfDestruct", timeAlive);
    }

    void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
