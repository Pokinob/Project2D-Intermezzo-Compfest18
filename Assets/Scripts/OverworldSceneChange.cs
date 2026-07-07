using UnityEngine;
using UnityEngine.SceneManagement;

// https://umistudioblog.com/en/%E3%80%90unity-beginners-guide%E3%80%913-ways-to-share-data-across-scenes-without-losing-it%E3%80%90complete-guide-for-beginners%E3%80%91/
public static class SceneTransferData
{
    public static string transferDirection;
}

public class OverworldSceneChange : MonoBehaviour
{
    public string sceneName;
    public string transferDirection;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            SceneTransferData.transferDirection = transferDirection;
            SceneManager.LoadScene(sceneName);
        }
    }

}
