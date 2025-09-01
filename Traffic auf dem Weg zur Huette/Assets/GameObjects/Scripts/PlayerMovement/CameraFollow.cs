using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeed;
    private CarController carController;
    [SerializeField] private float speed;


    private void Start()
    {
        carController = FindObjectOfType<CarController>();
        speed = carController.getSpeed();
    }
    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
        speed = carController.getSpeed();
        if (speed >= 71)
        {

            translateSpeed = 10;
        }

    }

    private void HandleTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 3, transform.position.z);
    }

    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
