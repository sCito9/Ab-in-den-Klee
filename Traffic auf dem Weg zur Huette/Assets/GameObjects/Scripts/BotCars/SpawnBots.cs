using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

enum Lane
{
    leftCounter, rightCounter, left, right
}

public class SpawnBots : MonoBehaviour
{
    public GameObject[] cars;
    private Lane lane = Lane.leftCounter;
    public LaneScript[] coll;
    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawn();
            spawn();
            spawn();
        }
    }

    public void spawn()
    {
        int number = Random.Range(0, cars.Length-1);
        int temp = 0;

        switch (lane)
        {
            case Lane.leftCounter:
                while (temp == 0)
                {
                    if (!coll[0].getEntered())
                    {

                        Instantiate(cars[number], new Vector3(-13.4f, 0.14f, coll[0].transform.position.z), Quaternion.AngleAxis(180f, Vector3.up));
                        temp = 1;
                    }
                }
                lane = Lane.rightCounter;
                break ;
            case Lane.rightCounter:
                while (temp == 0)
                {
                    if (!coll[1].getEntered())
                    {
                        Instantiate(cars[number], new Vector3(-18.5f, 0.14f, coll[1].transform.position.z), Quaternion.AngleAxis(180f, Vector3.up));
                        temp = 1;
                    }
                }
                lane = Lane.left;
                break ;
            case Lane.left:
                while (temp == 0)
                {
                    if (!coll[2].getEntered())
                    {
                        Instantiate(cars[number], new Vector3(-7.5f, 0.14f, coll[2].transform.position.z), Quaternion.identity);
                        temp = 1;
                    }
                }
                lane = Lane.right;
                break ;
            case Lane.right:
                while (temp == 0)
                {
                    if (!coll[3].getEntered())
                    {
                        Instantiate(cars[number], new Vector3(-2.5f, 0.14f, coll[3].transform.position.z), Quaternion.identity);
                        temp = 1;
                    }
                }
                lane = Lane.leftCounter;
                break ;  
        }

        temp = 0;

        
    }


}
