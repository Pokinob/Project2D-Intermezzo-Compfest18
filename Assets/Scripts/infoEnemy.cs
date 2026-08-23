using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class infoEnemy : MonoBehaviour
{
    public Slider enemyHp;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyHPpoint;
    public TextMeshProUGUI enemyStatus;
    public GameObject statusPanel;
    public bool isStun = false;

    private void Update()
    {
        if (isStun)
        {
            enemyStatus.text = "Stun";
            statusPanel.SetActive(true);
        }
        else
        {
            statusPanel.SetActive(false);
        }
    }
}
