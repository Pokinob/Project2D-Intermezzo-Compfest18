using Ink.UnityIntegration;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class introManager : MonoBehaviour
{
    [SerializeField] private GameObject wasdPanel;
    [SerializeField] private GameObject shiftPanel;
    [SerializeField] private TextMeshProUGUI guideText;
    [SerializeField] private InkFile inkTutorial;
    private bool isShiftGetPressed = false;
    private bool isWASDGetPressed = false;
    public static introManager instance;

    public static introManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of introManager found! Destroying duplicate.");
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if (isWASDGetPressed)
        {
            if (InputManager.GetInstance().GetMoveDirection() != Vector2.zero)
            {
                isWASDGetPressed = false;
                StartCoroutine(changeToShift());
            }
        }else if (isShiftGetPressed)
        {
            if (PlayerOverworld.GetInstance().isSprint && InputManager.GetInstance().GetMoveDirection() != Vector2.zero)
            {
                isShiftGetPressed = false;
                StartCoroutine(finishTutorial());
            }
        }

    }

    private IEnumerator changeToShift()
    {
        yield return new WaitForSeconds(2f);
        shiftPanel.SetActive(true);
        guideText.text = "Sprint";
        isShiftGetPressed = true;
    }

    private IEnumerator finishTutorial()
    {
        yield return new WaitForSeconds(2f);
        shiftPanel.SetActive(false);
        wasdPanel.SetActive(false);
        Destroy(gameObject);
    }

    public void onWASDPressed()
    {
        isWASDGetPressed = true;
        wasdPanel.SetActive(true);
        guideText.text = "Move";
    }

    public void startTutorial()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkTutorial);
    }


}
