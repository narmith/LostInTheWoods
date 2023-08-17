using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public int killCount = 0;
    public int winCondition = 3;

    public void EnemyIsDead()
    {
        killCount += 1;
    }

    void Update()
    {
        if (killCount >= winCondition)
        {
            //print("You have killed " + deadCount + "/" + winCondition + " enemies!");
            SceneManager.LoadScene("Win");
        }
    }
}
