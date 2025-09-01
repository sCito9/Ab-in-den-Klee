using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BotBehaviour : MonoBehaviour
{

    
    private const string VERTICAL = "Vertical";
    public LayerMask mask;
    
    private float verticalInput;
    
    private float currentBrakeForce;
    private bool isBreaking;
    private bool coll = false;
    [SerializeField]
    private float currentSpeed;
    private Rigidbody rb;
    private float acc = 0;
    private bool brakebool = false;
    private SpawnBots spawnBots;
    private Score score;
    private AudioSource source;
    
    

    [SerializeField]
    private float motorForce;
    [SerializeField]
    private float brakeForce;
    
    public float topSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private AudioClip[] clip;
    
    

    public List<WheelCollider> Front_Wheels;
    public List<WheelCollider> Back_Wheels;

    public List<Transform> Front_Wheels_Transform;
    public List<Transform> Back_Wheels_Transform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        minSpeed = UnityEngine.Random.Range(40.0f, 59.0f);
        topSpeed = UnityEngine.Random.Range(60.0f, 120.0f);
        spawnBots = FindObjectOfType<SpawnBots>();
        score = FindObjectOfType<Score>();
        source = GetComponent<AudioSource>();

        float temp2 = clip.Length - 0.1f;

        float temp = UnityEngine.Random.Range(0f, temp2);
        source.clip = clip[(int)temp];
    }

    public float getSpeed()
    {
        return currentSpeed;
    }


    private void FixedUpdate()
    {
        GetInput();

        if (coll == false)
        {
            HandleMotor();
            if (transform.position.x < -10.5)
            {
              //  transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
              //  transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            
        }
        else
        {
            foreach (WheelCollider wheel in Back_Wheels)
            {
                wheel.motorTorque = 0;
            }
            foreach (WheelCollider wheel in Front_Wheels)
            {
                wheel.steerAngle = -45;
            }
            if (grounded())
            {
                rb.AddExplosionForce(10f, transform.position, 7f);
            }
        }

        if(transform.position.y < -10)
        {
            collision();
        }



        UpdateWheels();
        UpdateSpeed();
    }

    private bool grounded()
    {
        foreach (WheelCollider wheel in Back_Wheels)
        {
            if (wheel.isGrounded == false)
            {
                return false;
            }
        }
        foreach (WheelCollider wheel in Front_Wheels)
        {
            if (wheel.isGrounded == false)
            {
                return false;
            }
        }
        return true;

    }

    private void UpdateSpeed()
    {
        currentSpeed = rb.velocity.magnitude * 3.6f;
    }

    public void accelerate()
    {
        acc = 1;
    }

    public void brake()
    {
        brakebool = true;
    }
    public void stopBrake()
    {
        brakebool = false;

    }

    

    private void HandleMotor()
    {
        foreach (WheelCollider wheel in Back_Wheels)
        {
            if (currentSpeed < minSpeed)
            {
                wheel.motorTorque = motorForce;
                wheel.brakeTorque = 0;
            }
            else if (currentSpeed <= topSpeed)
            {
                wheel.motorTorque = verticalInput * motorForce;
                wheel.brakeTorque = currentBrakeForce;
            }
            else
            {
                wheel.motorTorque = 0;
            }
        }
        foreach (WheelCollider wheel in Front_Wheels)
        {
            if (currentSpeed < minSpeed)
            {
                wheel.brakeTorque = 0;
            }
            else
            {
                wheel.brakeTorque = currentBrakeForce;
            }
        }

        if (brakebool)
        {

            currentBrakeForce = brakeForce;
        }
        else
        {
            currentBrakeForce = 0;
        }
    }

    

    private void UpdateWheels()
    {
        for (int i = 0; i < Front_Wheels.Count; i++)
        {
            UpdateSingleWheel(Front_Wheels[i], Front_Wheels_Transform[i]);
        }

        for (int i = 0; i < Back_Wheels.Count; i++)
        {
            UpdateSingleWheel(Back_Wheels[i], Back_Wheels_Transform[i]);
        }
    }
    private void UpdateSingleWheel(WheelCollider wheel, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheel.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    public float getTopSpeed()
    {
        return topSpeed;
    }

    public void setTopSpeed(float speed)
    {
        topSpeed = speed;
    }

    private void GetInput()
    {
        
        GetVerticalInput();

        StartCoroutine(checkFront());
        
        

    }
   
    private IEnumerator checkFront()
    {
        yield return new WaitForSeconds(0.7f);
        if (Physics.Raycast(new Vector3(transform.position.x, 1.5f, transform.position.z), Vector3.forward, out RaycastHit hitInfo, 35f, mask))
        {
            Debug.Log("Auto!");
            brakebool = true;
            BotBehaviour bB = hitInfo.transform.gameObject.GetComponent<BotBehaviour>();
            float temp = bB.getTopSpeed();
            bB.setTopSpeed(topSpeed);
            topSpeed = temp;
            decelerate();

        }
        else
        {
            accelerate();
            brakebool = false;
        }

    }

    public void GetVerticalInput()
    {
        verticalInput = acc;
        /* verticalInput = Input.GetAxis("Vertical");
         if (verticalInput < 0)
         {
             verticalInput = 0;
         }*/
    }

    public void decelerate()
    {
        acc = 0;
    }

    public void collision()
    {
        source.Play();
        rb.mass = 500;
        Debug.Log("methode");
        coll = true;
        Front_Wheels[0].brakeTorque = 1000;

        rb.AddExplosionForce(10f, transform.position, 7f);

        

        StartCoroutine(kill());
    }

    private IEnumerator kill (){

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collider");
        if (collision.gameObject.CompareTag("Environment")||collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collidertag");
            this.collision();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if (other.gameObject.CompareTag("Environment"))
        {
            Debug.Log("triggertag");
            this.collision();
        }
        if (other.gameObject.CompareTag("Loop"))
        {

            transform.position = new Vector3(transform.position.x, transform.position.y, -640);
            score.addScore();
  
        }else if (other.gameObject.CompareTag("LoopCounter"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 421);
            score.addScore();
        }
    }


}
