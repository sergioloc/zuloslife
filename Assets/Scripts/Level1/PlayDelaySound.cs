using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDelaySound : MonoBehaviour
{
    private AudioSource audioSource;
    public float delay = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayDelayed(delay);
    }
}
