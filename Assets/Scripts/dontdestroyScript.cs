using UnityEngine;

public class dontdestroyScript : MonoBehaviour
{
    public static dontdestroyScript instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
