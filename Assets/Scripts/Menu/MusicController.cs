using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
     private void Awake()
     {
         DontDestroyOnLoad(transform.gameObject);
     }
 
     public void PlayMusic()
     {
         audioSource.Play();
     }
 
     public void StopMusic()
     {
         audioSource.Stop();
     }
}
