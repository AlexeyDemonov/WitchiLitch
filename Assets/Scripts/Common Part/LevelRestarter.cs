using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestarter : MonoBehaviour
{
    public FallChecker FallChecker;

    // Start is called before the first frame update
    void Start()
    {
        FallChecker.Fallen += RestartLevel;
    }

    // This function is called when the MonoBehaviour will be destroyed
    void OnDestroy()
    {
        FallChecker.Fallen -= RestartLevel;
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}