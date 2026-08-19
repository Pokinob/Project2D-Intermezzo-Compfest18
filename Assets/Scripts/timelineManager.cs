using UnityEngine;
using UnityEngine.Playables;

public class timelineManager : MonoBehaviour
{
    public static timelineManager instance;
    public PlayableDirector currentTimeline;

    public static timelineManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void playTimeline()
    {
        currentTimeline.Play();
    }

}
