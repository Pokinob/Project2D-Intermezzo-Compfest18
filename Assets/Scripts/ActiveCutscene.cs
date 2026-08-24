using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ActiveCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private bool triggered = false;
    private bool fadePlay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            timelineManager.GetInstance().currentTimeline = playableDirector;
            if(fadePlay)
            {
                DialogueManager.GetInstance().fadeInScene.Play();
            }
            StartCoroutine(play());
            triggered = true;
        }
    }

    private IEnumerator play()
    {
        yield return new WaitUntil(() => DialogueManager.GetInstance().fadeInScene.state != PlayState.Playing);
        timelineManager.GetInstance().playTimeline();
    }

}
