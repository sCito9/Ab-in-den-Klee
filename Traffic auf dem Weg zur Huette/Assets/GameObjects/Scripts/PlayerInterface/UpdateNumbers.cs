using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateNumbers : MonoBehaviour
{
    private byte speed = 0;
    private CarController carController;
   
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        carController = FindObjectOfType<CarController>();
        
        
    }

    private void FixedUpdate()
    {
       
        
        speed = (byte)carController.getSpeed();
        text.text = speed + " km/h";
    }


   
}
