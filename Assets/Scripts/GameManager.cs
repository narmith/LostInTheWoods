using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    // References
    public Player player;
    //public List<Container> chests;
    //public FloatingTextManager floatingTextManager;
    //public CharacterMenu playerMenu;
}
