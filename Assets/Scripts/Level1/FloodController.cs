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

    [Space(10)]
    public UnityEvent OnSwitchLight;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            door.SetActive(true);
            StartCoroutine(StartLevel());
        }
        else if (collision.gameObject.CompareTag("PlayerDeath") && activated)
        {
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
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f);
        CinemachineController.instance.ModifyZoom(6f);
        animator.SetTrigger("Stop");
        activated = false;
        door.SetActive(false);
        OnSwitchLight.Invoke();
    }
}
