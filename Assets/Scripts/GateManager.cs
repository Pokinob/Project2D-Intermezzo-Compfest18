using UnityEngine;

public class GateManager : MonoBehaviour
{
    int gateLevel = -1;

    [SerializeField] Animator animator;
    private bool alreadyOpen = false;
    public static GateManager Instance;
    public static GateManager GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(gateLevel <= 0)
        {
            animator.SetBool("Open", false);
            alreadyOpen = false;
            return;
        }
        else if(!alreadyOpen)
        {
            alreadyOpen = true;
            animator.SetBool("Open", true);
            return;
        }
    }

    public void SetGateLevel(int level)
    {
        gateLevel = level;
    }
}
