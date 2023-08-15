using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject doorL;
    public GameObject doorR;
    Vector3 startingLPos;
    Vector3 startingRPos;
    public Vector3 dist = new Vector3(1, 0, 0);
    public string triggerTag="Player";
    public bool isBlocked=false;

    private void Start()
    {
        if (doorL.gameObject != null)
        {
            startingLPos = doorL.transform.position;
        }
        if (doorR.gameObject != null)
        {
            startingRPos = doorR.transform.position;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (doorL != null && doorR != null)
        {
            if (other.CompareTag(triggerTag) && isBlocked == false)
            {
                doorL.transform.Translate(dist * -1);
                doorR.transform.Translate(dist);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (doorL.transform.position != startingLPos)
        {
            doorL.transform.position = startingLPos;
        }
        if (doorR.transform.position != startingRPos)
        {
            doorR.transform.position = startingRPos;
        }
    }
}
