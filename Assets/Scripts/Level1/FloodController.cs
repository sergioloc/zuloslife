using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class FloodController : MonoBehaviour
{
    public GameObject door;
    private bool activated = false;
    private Animator animator;
    public AudioSource alarmSound;
    private AudioSource floodSound;

    [Space(10)]
    public UnityEvent OnSwitchLight;

    void Start()
    {
        animator = GetComponent<Animator>();
        floodSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            door.SetActive(true);
            StartCoroutine(StartLevel());
        }
    }

    void Update(){
        if (!LevelOneValues.isPlayerAlive && activated){
            activated = false;
            StartCoroutine(RestartLevel());
        }
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(3f);
        activated = true;
        animator.SetTrigger("Start");
        CinemachineController.instance.ModifyZoom(10f);
        OnSwitchLight.Invoke();
        StartCoroutine(PlayAlarm());
        floodSound.Play();
    }

    IEnumerator PlayAlarm()
    {  
        while (activated){
            alarmSound.Play();
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f);
        CinemachineController.instance.ModifyZoom(6f);
        animator.SetTrigger("Stop");
        door.SetActive(false);
        OnSwitchLight.Invoke();
        floodSound.Stop();
        alarmSound.Stop();
    }
}
