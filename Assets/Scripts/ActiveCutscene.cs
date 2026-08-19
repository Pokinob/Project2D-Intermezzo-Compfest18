using UnityEngine;
using UnityEngine.Playables;

public class ActiveCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            timelineManager.GetInstance().currentTimeline = playableDirector;
            timelineManager.GetInstance().playTimeline();
            triggered = true;
        }
    }

}
