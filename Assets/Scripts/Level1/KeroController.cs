using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public class KeroController : MonoBehaviour
{

    [Header("Player")]
    public GameObject player;

    [Header("Values")]
    private float speed = 0;
    public float minDistance = 2.5f;
    public float minVelocity = 7;

    [Header("Sounds")]
    private AudioSource audioSource;
    public AudioClip thunderSound;
    public AudioClip swordSound;


    [Header("Targets in area")]
    public List<Transform> targets;
    
    private bool isMovingAttack = false;
    private string currentAttack;
    private Animator animator;
    private Rigidbody2D rb2d;

    void Start(){
        GetRemoteConfigValues();
        animator = GetComponent<Animator>();
        rb2d = player.GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void GetRemoteConfigValues(){
        speed = ConfigManager.appConfig.GetInt("keroSpeed");
        if (speed == 0) speed = 7;
    }

    void Update(){
        if (isMovingAttack){
            int indexTarget = NearestTarget();
            if (indexTarget != -1)
                player.transform.position = Vector2.Lerp(player.transform.position, targets[indexTarget].position, speed * Time.deltaTime);
            
            float nearest = Mathf.Abs(DistanceToNearest());
            if (nearest < minDistance || nearest == 999f){
                isMovingAttack = false;
                animator.SetBool(currentAttack, false);
            }
        }
    }

    public void Attack(){
        //Animation
        if (Mathf.Abs(rb2d.velocity.x) > minVelocity){
            currentAttack = "Dash";
            audioSource.PlayOneShot(swordSound);
        }
        else
            currentAttack = "Smash";

        //Movement
        if (targets.Count > 0){
            MoveToTarget();
        }
        else {
            StartCoroutine(AttackToNothing());
        }
        
    }

    private void MoveToTarget()
    {
        if (Mathf.Abs(DistanceToNearest()) > minDistance){
            isMovingAttack = true;
            animator.SetBool(currentAttack, true);
        }
        else {
            StartCoroutine(CloseSmash());
        }
    }

    private float DistanceToNearest(){
        if (targets.Count > 0)
            return targets[NearestTarget()].position.x - player.transform.position.x;
        return 999f;
    }

    private IEnumerator CloseSmash()
    {
        animator.SetBool("Smash", true);
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("Smash", false);
    }

    private IEnumerator AttackToNothing()
    {
        animator.SetBool(currentAttack, true);
        yield return new WaitForSeconds(0.25f);
        animator.SetBool(currentAttack, false);
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

    public void ShakeScreen(){
        CinemachineController.instance.Shake(0f);
    }

    public void PlayThunderSound(){
        audioSource.PlayOneShot(thunderSound);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!targets.Contains(collision.transform))
                targets.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (targets.Contains(collision.transform)){
                targets.Remove(collision.transform);
            }
        }
    }

}
