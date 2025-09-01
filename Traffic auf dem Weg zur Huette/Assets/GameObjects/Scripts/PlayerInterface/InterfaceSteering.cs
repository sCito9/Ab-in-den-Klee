using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSteering : MonoBehaviour
{
    private CarController controller;

    private void Start()
    {
        controller = FindObjectOfType<CarController>();
    }

    public void SteerRight()
    {
        controller.steerRight();
    }

    public void SteerLeft()
    {

        controller.steerLeft();
    }

    public void stopSteering()
    {
        controller.stopSteering();
    }


    public void accelerate()
    {
        controller.accelerate();
    }
    public void decelerate()
    {
        controller.decelerate();
    }

    public void brake()
    {
        controller.brake();
    }

    public void stopBrake()
    {
        controller.stopBrake();
    }
}
