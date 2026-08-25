using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ActiveCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private bool triggered = false;
    public bool fadePlay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            timelineManager.GetInstance().currentTimeline = playableDirector;
            if(fadePlay)
            {
                DialogueManager.GetInstance().fadeInScene.Play();
                DialogueManager.GetInstance().fadeInScene.playableGraph.GetRootPlayable(0).SetSpeed(1.5f);
            }
            StartCoroutine(play());
            triggered = true;
        }
    }

    private IEnumerator play()
    {
        DialogueManager.GetInstance().canContinue = false;
        yield return new WaitUntil(() => DialogueManager.GetInstance().fadeInScene.state != PlayState.Playing);
        timelineManager.GetInstance().playTimeline();
    }

}
