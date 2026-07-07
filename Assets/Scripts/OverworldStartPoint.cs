using UnityEngine;

public class OverworldStartPoint : MonoBehaviour
{
    public string transferDirection;

    void Start()
    {
        if (SceneTransferData.transferDirection == transferDirection)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.localPosition = transform.localPosition;
        }
    }
}
