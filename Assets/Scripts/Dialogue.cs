using UnityEngine;
using Ink.Runtime;

public class Dialogue : MonoBehaviour
{
    public TextAsset inkAsset;
    Story story;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        story = new Story(inkAsset.text);
    }

    void Start()
    {
        while (story.canContinue)
        {
            Debug.Log(story.Continue());
        }
    }
}
