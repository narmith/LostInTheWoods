using UnityEngine;

public class SimpleLookAt : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        if (target) { this.transform.LookAt(target.transform); }
    }
}
