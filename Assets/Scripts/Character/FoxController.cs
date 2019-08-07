using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{

    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;
    private float shakeElapsedTime = 0f;

    public float speed = 0.1f;
    public float rotateSpeed = 5.0f;
    public int distanceValue = 5;
    public float alpha = 100f;
    public ParticleSystem explosion;
    public GameObject foxSprite, target;
    private bool randomFly = true;

    Vector3 newPosition;

    void Start()
    {
        PositionChange();
        Color tmp = foxSprite.GetComponent<SpriteRenderer>().color;
        tmp.a = alpha;
        foxSprite.GetComponent<SpriteRenderer>().color = tmp;
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {

        ShakeCamera();

        float distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance < distanceValue)
        {
            randomFly = false;
            LookAt2D(target.gameObject.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, Time.deltaTime * speed * 10f);
        }

        if (randomFly)
        {
            if (Vector2.Distance(transform.position, newPosition) < 1)
                PositionChange();

            transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * speed);

            LookAt2D(newPosition);
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Explosion"))
        {
            Exploit();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        shakeElapsedTime = shakeDuration;
        Exploit();
    }

    void PositionChange()
    {
        newPosition = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
    }

    void LookAt2D(Vector3 lookAtPosition)
    {
        float distanceX = lookAtPosition.x - transform.position.x;
        float distanceY = lookAtPosition.y - transform.position.y;
        float angle = Mathf.Atan2(distanceX, distanceY) * Mathf.Rad2Deg;

        Quaternion endRotation = Quaternion.AngleAxis(angle, Vector3.back);
        transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, Time.deltaTime * rotateSpeed);
    }

    private void ShakeCamera()
    {
        // If the Cinemachine componet is not set, avoid update
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (shakeElapsedTime > 0)
            {
                Debug.Log("ey");
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
                // Update Shake Timer
                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0f;
            }
        }
    }

    void Exploit()
    {
        explosion.Play();
        foxSprite.SetActive(false);
        Destroy(gameObject, 2);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
