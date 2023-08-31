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
        PlayerPrefs.DeleteAll();

        instance = this;
        //SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
    }

    // References
    public Player player;
    //public List<Container> chests;
    //public FloatingTextManager floatingTextManager;
    //public CharacterMenu playerMenu;
}
