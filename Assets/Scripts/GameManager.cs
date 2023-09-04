using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        PlayerPrefs.DeleteAll();
        //SceneManager.sceneLoaded += LoadState;

        Application.targetFrameRate = 60;
        StaticGlobals.Fullscreen = Screen.fullScreen;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // References
    public Player player;
    public MainMenu mainMenu;
    public ActionBars playerUI;
    //public List<Container> chests;
    //public FloatingTextManager floatingTextManager;
    //public CharacterMenu playerMenu;

    public void Start()
    {
        mainMenu.gameObject.SetActive(false);
    }
    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { PauseGame(); }
    }

    public void Fullscreen(bool state)
    {
        if(state)
        {
            Screen.SetResolution(640, 480, FullScreenMode.ExclusiveFullScreen);
        }
        else Screen.SetResolution(640, 480, FullScreenMode.Windowed, new RefreshRate() { numerator = 60, denominator = 1 });
    }
    public void QuitGame() { Application.Quit(); }
    //public void LoadLevel(string menu) { SceneManager.LoadScene(menu); }

    public void PauseGame()
    {
        if (!mainMenu.gameObject.activeSelf)
        {
            mainMenu.gameObject.SetActive(true);

            Time.timeScale = 1.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            mainMenu.gameObject.SetActive(false);

            Time.timeScale = 1.0f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        player.mouseSensitivity = StaticGlobals.MouseSensitivity;
        player.GetComponent<HP>().godMode = StaticGlobals.GodMode;
    }

    public void RefreshUI()
    {
        if (player.GetActionCooldown() > 0)
        {
            playerUI.action1Timer.fillAmount += (Time.deltaTime / player.GetActionCooldown());
            playerUI.action2Timer.fillAmount += (Time.deltaTime / player.GetActionCooldown());
        }
        else
        {
            playerUI.action1Timer.fillAmount = 0;
            playerUI.action2Timer.fillAmount = 0;
        }
    }

}
