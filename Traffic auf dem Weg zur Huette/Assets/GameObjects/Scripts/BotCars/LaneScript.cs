using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneScript : MonoBehaviour
{
    private bool entered = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Environment"))
        {
            entered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Environment"))
        {
            entered = false;
        }
    }

    public bool getEntered()
    {
        return entered;
    }



}
