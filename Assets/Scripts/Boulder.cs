using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool isPushing = false;
    private bool CanMove(Vector2 targetPos)
    {
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.2f, obstacleLayer);

        return hit == null;
    }

    public void Push(Vector2 pushDir)
    {
        if (isPushing)
        {
            return;
        }
        Vector2 target = (Vector2)transform.position + pushDir;
        if (CanMove(target))
        {
            StartCoroutine(Move(target));
        }
    }

    private IEnumerator Move(Vector2 targetPos)
    {
        isPushing = true;
        InputManager.GetInstance().FreezeInput();
        while (Vector2.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime);

            yield return null;
        }
        transform.position = targetPos;
        isPushing = false;
        InputManager.GetInstance().UnfreezeInput();
    }
}
