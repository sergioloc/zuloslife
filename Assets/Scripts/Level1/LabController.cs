﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabController : MonoBehaviour
{
    public float speed = 0.1f;
    public int range = 5;
    public GameObject target, deathParticle;
    private Vector2 newPosition;
    private Animator animator;
    private float distanceX, distanceY, angle;

    void Start()
    {
        PositionChange();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (target != null){
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < range)
            {
                animator.SetBool("View", true);
                transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, Time.deltaTime * speed * 10f);
            }
            else
            {
                RandomFly();
            }
        } 
        else
        {
            RandomFly();
        }
  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag("Explosion"))
        {
            Exploit();
        }*/
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Die");
        }
    }

    private void RandomFly(){
        animator.SetBool("View", false);
        if (Vector2.Distance(transform.position, newPosition) < 1)
            PositionChange();
        transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * speed);
    }

    private void PositionChange()
    {
        newPosition = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
    }

    public void Die()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}