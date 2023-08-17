using UnityEngine;

public class CalculateAggro : MonoBehaviour
{
    ZombieMovement zombie;

    private void Start()
    {
        zombie = GetComponentInParent<ZombieMovement>();
    }

    public void OnTriggerEnter(Collider otherColl)
    {
        if (otherColl.CompareTag("Player"))
        {
            zombie.AggroTarget(otherColl.gameObject);
            //print(this.transform.parent.name + " detected " + otherColl.name + ".");
        }
    }

    public void OnTriggerExit(Collider otherColl)
    {
        if (otherColl.CompareTag("Player"))
        {
            zombie.AggroTarget(zombie.gameObject);
        }
    }
}
