using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void PlayGame() { SceneManager.LoadScene("World"); }
    public void Fullscreen() { StaticGlobals.Fullscreen = !StaticGlobals.Fullscreen; Screen.fullScreen = StaticGlobals.Fullscreen; }
    public void QuitGame() { Application.Quit(); }

}
