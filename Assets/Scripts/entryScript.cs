using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class entryScript : MonoBehaviour
{
    [SerializeField] private int entryNumber;
    private float duration = 0.2f;
    [SerializeField] private bool changeScene;
    [SerializeField] private bool isPuzzle;
    private Vector2 walkDir;
    [SerializeField] private Vector2 walkDirFix;
    [SerializeField] private PlayableDirector fadeInScene;
    [SerializeField] private PlayableDirector fadeOutScene;
    private GameObject entryPoint;
    [SerializeField] private GameObject entryFix;

    private void Awake()
    {
        duration = 0.2f;
        walkDir = walkDirFix;
        entryPoint = entryFix;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !PlayerOverworld.GetInstance().isFreeze)
        {
            PlayerOverworld.GetInstance().isFreeze = true;
            entryPoint = entryFix;
            walkDir = walkDirFix;
            fadeInScene.Play();
            PlayerOverworld.GetInstance().forceMove(walkDir, duration);
            if (changeScene)
            {
                //changescene;
            }
            else
            {

                if (isPuzzle)
                {
                    if (entryNumber != 0)
                    {
                        entryPoint = P3Manager.GetInstance().checkEntry(entryNumber);
                        walkDir = Vector2.down;
                        if (entryPoint == null)
                        {
                            entryPoint = entryFix;
                            walkDir = walkDirFix;
                        }
                    }
                    else
                    {
                        entryPoint = P3Manager.GetInstance().resetPuzzle();
                        walkDir = Vector2.down;
                        if (entryPoint == null)
                        {
                            entryPoint = entryFix;
                            walkDir = walkDirFix;
                        }
                    }
                    StartCoroutine(delayEntry());
                }
                else
                {
                    StartCoroutine(delayEntry());
                }

            }
        }
    }

    IEnumerator delayEntry()
    {
        yield return new WaitForSeconds(0.2f);
        PlayerOverworld.GetInstance().transform.position = entryPoint.transform.position;
        yield return new WaitForSeconds(0.2f);
        PlayerOverworld.GetInstance().forceMove(walkDir, duration);
        fadeOutScene.Play();
        yield return new WaitForSeconds(0.2f);
        PlayerOverworld.GetInstance().isFreeze = false;
    }


}
