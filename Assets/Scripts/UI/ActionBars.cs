using UnityEngine;
using UnityEngine.UI;

public class ActionBars : MonoBehaviour
{
    public Image action1Icon;
    public Image action1Timer;
    public Image action2Icon;
    public Image action2Timer;

    public void Start()
    {
        if (GameManager.instance.playerUI != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            GameManager.instance.playerUI = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //public float action1Percentage;
}
