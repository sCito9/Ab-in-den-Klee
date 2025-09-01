using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BotBehaviour : MonoBehaviour
{

    

    public LayerMask mask;
    

    
    [SerializeField]
    private float currentSpeed;
    private Rigidbody rb;
    private float acc = 0;
    private bool brakebool = false;
    private Score score;
    private AudioSource source;
    
    private bool crashed = false;

    private Transform _transform;
    
    
    
    public float topSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private AudioClip[] clip;

    private void Awake()
    {
        _transform = transform;
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        minSpeed = UnityEngine.Random.Range(40.0f, 59.0f);
        topSpeed = UnityEngine.Random.Range(60.0f, 120.0f);
        score = FindObjectOfType<Score>();
        source = GetComponent<AudioSource>();

        float temp2 = clip.Length - 0.1f;
        float temp = UnityEngine.Random.Range(0f, temp2);
        source.clip = clip[(int)temp];
    }



    private void UpdateSpeed()
    {
        currentSpeed = rb.velocity.magnitude * 3.6f;
    }

    public void accelerate()
    {
        acc = 1;
    }


    private void FixedUpdate()
    {
        if (!crashed)
        {
            rb.AddForce(transform.forward * (topSpeed * Time.deltaTime));
        }
    }


    public float getTopSpeed()
    {
        return topSpeed;
    }

    public void setTopSpeed(float speed)
    {
        topSpeed = speed;
    }
    
   
    private IEnumerator checkFront()
    {
        yield return new WaitForSeconds(0.7f);
        if (Physics.Raycast(new Vector3(transform.position.x, 1.5f, transform.position.z), Vector3.forward, out RaycastHit hitInfo, 35f, mask))
        {
            BotBehaviour bB = hitInfo.transform.gameObject.GetComponent<BotBehaviour>();
            float temp = bB.getTopSpeed();
            bB.setTopSpeed(topSpeed);
            topSpeed = temp;
            

        }
        

    }
    
    

    public void collision()
    {
        source.Play();
        rb.mass = 500;
        crashed = true;
        rb.AddExplosionForce(10f, transform.position, 7f);
        
        StartCoroutine(kill());
    }

    private IEnumerator kill (){

        yield return new WaitForSeconds(5f);
        rb.mass = 1600;
        transform.position = new Vector3(_transform.position.x, _transform.position.y, 421);
        transform.rotation = _transform.rotation;
        crashed = false;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment")||collision.gameObject.CompareTag("Player"))
        {
            this.collision();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if (other.gameObject.CompareTag("Environment"))
        {
            this.collision();
        }
        if (other.gameObject.CompareTag("Loop"))
        {

            transform.position = new Vector3(_transform.position.x, _transform.position.y, -640);
            transform.rotation = _transform.rotation;
            score.addScore();
  
        }else if (other.gameObject.CompareTag("LoopCounter"))
        {
            transform.position = new Vector3(_transform.position.x, _transform.position.y, 421);
            transform.rotation = _transform.rotation;
            score.addScore();
        }
    }


}
