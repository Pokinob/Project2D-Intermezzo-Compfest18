using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldSceneChange : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
