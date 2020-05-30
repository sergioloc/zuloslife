using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
     private void Awake()
     {
         DontDestroyOnLoad(transform.gameObject);
         audioSource = GetComponent<AudioSource>();
     }

     private void Start(){
         PlayMusic();
     }
 
     public void PlayMusic()
     {
         if (!GameObject.Find("MusicMenu").GetComponent<AudioSource>().isPlaying)
            audioSource.Play();
     }
 
     public void StopMusic()
     {
         audioSource.Stop();
     }
}
