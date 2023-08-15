using UnityEngine;

public class SimpleLookAt : MonoBehaviour
{
    public GameObject target;

    void LateUpdate()
    {
        if (target) { this.transform.LookAt(target.transform); }
        else GameObject.FindGameObjectWithTag("Player");
    }
}
