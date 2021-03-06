﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabController : MonoBehaviour
{
    public float speed = 0.1f;
    public int range = 5;
    public GameObject deathParticle;
    private GameObject target;
    private Vector2 newPosition;
    private Animator animator;
    private float distanceX, distanceY, angle;
    private bool firstTime;
    public AudioSource dieAudio;

    void Start()
    {
        firstTime = true;
        target = GameObject.Find("Player");
        newPosition = new Vector2(transform.position.x, transform.position.y - 5f);
        animator = GetComponent<Animator>();
    }

    void Update()
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
        if (collision.gameObject.CompareTag("Player"))
        {
            dieAudio.Play();
            animator.SetTrigger("Die");
        }
    }

    private void RandomFly(){
        animator.SetBool("View", false);
        if (Vector2.Distance(transform.position, newPosition) < 1)
            ChangePosition();
        if (firstTime)
            transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * speed * 10);
        else
            transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * speed);
        firstTime = false;
    }

    private void ChangePosition()
    {
        newPosition = new Vector2(Random.Range(transform.position.x-5f, transform.position.x+5f),
                                  Random.Range(transform.position.y-5f, transform.position.y+5f));
    }

    public void Die()
    {
        dieAudio.Play();
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
