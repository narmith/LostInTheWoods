using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        if (GameManager.instance.mainMenu != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            GameManager.instance.mainMenu = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool Fullscreen() { return StaticGlobals.Fullscreen; }
    public void Fullscreen(bool newState) { StaticGlobals.Fullscreen = newState; }
    public bool GodMode() { return StaticGlobals.GodMode; }
    public void GodMode(bool newState) { StaticGlobals.GodMode = newState; }
    public float Sensitivity() { return StaticGlobals.MouseSensitivity; }
    public void Sensitivity(float newSensitivity) { StaticGlobals.MouseSensitivity = newSensitivity; }

}
