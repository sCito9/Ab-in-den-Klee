using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    [SerializeField] private float currentSpeed;

    [SerializeField] private AudioSource source;

    public Canvas canvas;
    public Canvas newInterface;

    [SerializeField] private float motorForce;

    [SerializeField] private float brakeForce;

    [SerializeField] private float maxSteerAngle;

    [SerializeField] private float topSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public AudioClip[] clip;

    public List<WheelCollider> Front_Wheels;
    public List<WheelCollider> Back_Wheels;

    public List<Transform> Front_Wheels_Transform;
    public List<Transform> Back_Wheels_Transform;
    private float acc;
    private bool brakebool;
    private bool coll;
    private float currentBrakeForce;
    private float currentSteerAngle;

    private float horizontalInput;
    private bool isBreaking;
    private Rigidbody rb;

    private Score score;
    private float steer;
    private float verticalInput;


    private void Start()
    {
        score = FindObjectOfType<Score>();
        rb = GetComponent<Rigidbody>();
        var temp = Random.Range(0, clip.Length - 0.01f);
        source.clip = clip[(int)temp];
        OnPlayerTeleport += HandleTeleport;
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
            foreach (var wheel in Back_Wheels) wheel.motorTorque = 0;
            foreach (var wheel in Front_Wheels) wheel.steerAngle = -45;
            if (grounded()) rb.AddExplosionForce(10f, transform.position, 7f);
        }

        if (transform.position.y < -10) collision();


        UpdateWheels();
        UpdateSpeed();
    }

    private void OnDestroy()
    {
        OnPlayerTeleport -= HandleTeleport;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment")) this.collision();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Environment")) collision();

        if (other.gameObject.CompareTag("Loop")) OnPlayerTeleport?.Invoke(1061);
    }

    public static event Action<float> OnPlayerTeleport;

    private void HandleTeleport(float distance)
    {
        minSpeed += 5;
        score.addScore();
        var dist = transform.position.z - cameraTransform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, -640);
        cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, -640 - dist);
    }

    public float getSpeed()
    {
        return currentSpeed;
    }

    private bool grounded()
    {
        foreach (var wheel in Back_Wheels)
            if (wheel.isGrounded == false)
                return false;

        foreach (var wheel in Front_Wheels)
            if (wheel.isGrounded == false)
                return false;

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
        foreach (var wheel in Back_Wheels)
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

        foreach (var wheel in Front_Wheels)
            if (currentSpeed < minSpeed)
                wheel.brakeTorque = 0;
            else
                wheel.brakeTorque = currentBrakeForce;

        if (brakebool)
            currentBrakeForce = brakeForce;
        else
            currentBrakeForce = 0;
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
        foreach (var wheel in Front_Wheels) wheel.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        for (var i = 0; i < Front_Wheels.Count; i++) UpdateSingleWheel(Front_Wheels[i], Front_Wheels_Transform[i]);

        for (var i = 0; i < Back_Wheels.Count; i++) UpdateSingleWheel(Back_Wheels[i], Back_Wheels_Transform[i]);
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
            coll = true;
            Front_Wheels[0].brakeTorque = 1000;

            rb.AddExplosionForce(10f, transform.position, 3f);
            canvas.gameObject.SetActive(false);
            newInterface.gameObject.SetActive(true);
        }
    }
}