using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBrakeForce;
    private bool isBreaking;
    private bool coll = false;
    [SerializeField]
    private float currentSpeed;
    private Rigidbody rb;
    private float acc = 0;
    private bool brakebool = false;
    private float steer = 0;
    [SerializeField]private AudioSource source;

    public Canvas canvas;
    public Canvas newInterface;

    [SerializeField]
    private float motorForce;
    [SerializeField]
    private float brakeForce;
    [SerializeField]
    private float maxSteerAngle;
    [SerializeField] private float topSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public AudioClip[] clip;

    public List<WheelCollider> Front_Wheels;
    public List<WheelCollider> Back_Wheels;

    public List<Transform> Front_Wheels_Transform;
    public List<Transform> Back_Wheels_Transform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        float temp = UnityEngine.Random.Range(0,clip.Length-0.01f);
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
            HandleSteering();
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

        if (transform.position.y < -10)
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

    public void stopSteering()
    {
        steer = 0;
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

    public void steerLeft()
    {
        steer = -1;
    }

    public void steerRight()
    {
        steer = 1;
    }



    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        foreach (WheelCollider wheel in Front_Wheels)
        {
            wheel.steerAngle = currentSteerAngle;
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

    private void GetInput()
    {
        GetVerticalInput();
        GetHorizontalInput();

    }
    public void GetHorizontalInput()
    {
        horizontalInput = steer;
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
        if (!coll)
        {
            source.Play();
            rb.mass = 500;
            Debug.Log("methode");
            coll = true;
            Front_Wheels[0].brakeTorque = 1000;

            rb.AddExplosionForce(10f, transform.position, 7f);
            Destroy(canvas);
            Instantiate(newInterface);
        }
    }

      


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collider");
        if (collision.gameObject.CompareTag("Environment"))
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

            float dist = transform.position.z - cameraTransform.position.z;

            transform.position = new Vector3(transform.position.x, transform.position.y, -640);

            cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, -640 - dist);
        }
    }

}

