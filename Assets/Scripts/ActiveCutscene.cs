using UnityEngine;
using UnityEngine.Playables;

public class ActiveCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;

    private static ActiveCutscene instance;

    public static ActiveCutscene GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    //private bool triggered = false;
    //
    //private void OnTriggerEnter2D(Collider2D collision)  if using trigger
    //{
    //    if (collision.CompareTag("Player") && !triggered)
    //    {
    //        playableDirector.Play();
    //        triggered = true;
    //    }
    //}

    public void startCutscene()
    {
        playableDirector.Play();
    }
}
