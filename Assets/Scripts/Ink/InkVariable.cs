using Ink.Runtime;
using UnityEngine;

public class InkVariable : MonoBehaviour
{

    public Story globalVariableStory { get; private set; }
    public static InkVariable Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one InkVariable in the scene.");
        }
        Instance = this;
        globalVariableStory = null;
    }
}
