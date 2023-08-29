using UnityEngine;

public class CalculateAggro : MonoBehaviour
{
    Ninja ninja;

    private void Start()
    {
        ninja = GetComponentInParent<Ninja>();
    }

    public void OnTriggerEnter(Collider otherColl)
    {
        if (otherColl.CompareTag("Player"))
        {
            ninja.AggroTarget(otherColl.gameObject);
        }
    }

    public void OnTriggerExit(Collider otherColl)
    {
        if (otherColl.CompareTag("Player"))
        {
            ninja.AggroTarget(ninja.gameObject);
        }
    }
}
