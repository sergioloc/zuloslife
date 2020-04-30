using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeroController : MonoBehaviour
{

    [Header("Player")]
    public GameObject player;

    [Header("Values")]
    public float speed = 0;
    public float minDistance = 0;
    public float minVelocity = 0;

    [Header("Targets in area")]
    public List<Transform> targets;
    
    private bool isMoving = false;
    private string currentAttack;
    private Animator animator;
    private Rigidbody2D rb2d;

    void Start(){
        animator = GetComponent<Animator>();
        rb2d = player.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        if (isMoving){
            player.transform.position = Vector2.Lerp(player.transform.position, targets[NearestTarget()].position, speed * Time.deltaTime);
            if (Mathf.Abs(DistanceToNearest()) < minDistance){
                isMoving = false;
                animator.SetBool(currentAttack, false);
            } 
        }
    }

    public void Attack(){
        if (Mathf.Abs(rb2d.velocity.x) > minVelocity)
            currentAttack = "Dash";
        else
            currentAttack = "Smash";
        MoveToTarget();
    }

    public void StopAttack(){
        
    }


    private void MoveToTarget()
    {
        if (NearestTarget() != -1 && Mathf.Abs(DistanceToNearest()) > minDistance){
            isMoving = true;
            animator.SetBool(currentAttack, true);
        }
    }

    private float DistanceToNearest(){
        return targets[NearestTarget()].position.x - player.transform.position.x;
    }

    private int NearestTarget()
    {
        float[] distances = new float[targets.Count];

        for (int i = 0; i < targets.Count; i++)
        {
            distances[i] = (Mathf.Abs(targets[i].position.x - player.transform.position.x));
        }

        float minDistance = Mathf.Min(distances);
        int index = -1;

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
