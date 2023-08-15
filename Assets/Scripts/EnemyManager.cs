using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public int deadCount = 0;
    public int winCondition = 3;

    public void EnemyIsDead()
    {
        deadCount += 1;
        print("Se han matado " + deadCount + "/" + winCondition + " enemigos!");
    }

    void Update()
    {
        if (deadCount >= winCondition)
        {
            print("Se han matado " + deadCount + "/" + winCondition + " enemigos!");
            SceneManager.LoadScene("Win");
        }
    }
}
