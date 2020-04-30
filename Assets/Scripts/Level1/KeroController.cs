using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeroController : MonoBehaviour
{

    [Header("Targets in radius")]
    public List<Transform> targets;
    public int targetIndex;
    public Transform player;
    public float speed = 0;
    private bool moving = false;
    private Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
    }

    void FixedUpdate(){
        if (moving){
            StartCoroutine(Dash());
            float distance = targets[NearestTarget()].position.x - player.position.x;
            if (Mathf.Abs(distance) < 2)
                moving = false;
            else if (Mathf.Abs(distance) < 4)
                StartCoroutine(Uppercut());
            
        }
    }

    public void MoveToTarget()
    {
        moving = true;
    }

    private IEnumerator Dash(){
        player.position = Vector2.Lerp(player.position, targets[NearestTarget()].position, speed * Time.deltaTime);
        animator.SetBool("Dash", true);
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("Dash", false);
    }

    private IEnumerator Uppercut(){
        animator.SetBool("Uppercut", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Uppercut", false);
    }

    private int NearestTarget()
    {
        float[] distances = new float[targets.Count];

        for (int i = 0; i < targets.Count; i++)
        {
            distances[i] = (Mathf.Abs(targets[i].position.x - player.position.x));
        }

        float minDistance = Mathf.Min(distances);
        int index = 0;

        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }
        return index;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Guard"))
        {
            if (!targets.Contains(collision.transform))
                targets.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Guard"))
        {
            if (targets.Contains(collision.transform))
                targets.Remove(collision.transform);
        }
    }

}
