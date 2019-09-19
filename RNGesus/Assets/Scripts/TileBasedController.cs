/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBasedController : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private RigidBody2D rb2D;
    private float inverseMoveTime;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<RigidBody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RayCastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            
        }
    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqrRemaningDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemaningDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D, end, inverseMoveTime * inverseMoveTime.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemaningDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void OnCantMove <T> (T component)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/
