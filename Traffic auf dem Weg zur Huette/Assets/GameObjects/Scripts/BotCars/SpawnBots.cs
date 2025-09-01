using System.Collections;
using UnityEngine;

public class SpawnBots : MonoBehaviour
{
    public GameObject[] cars;
    private int enterMutex;

    private void Start()
    {
        CarController.OnPlayerTeleport += HandleTeleport;
    }

    private void OnDestroy()
    {
        CarController.OnPlayerTeleport -= HandleTeleport;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("AICars")) enterMutex++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("AICars")) enterMutex--;
        ;
    }


    private void HandleTeleport(float distance)
    {
        StartCoroutine(spawn(1));
        StartCoroutine(spawn(2));
        StartCoroutine(spawn(3));
        StartCoroutine(spawn(4));
    }

    public IEnumerator spawn(int lane)
    {
        yield return new WaitUntil(() => enterMutex == 0);
        switch (lane)
        {
            case 1:
                Instantiate(cars[Random.Range(0, 4)], new Vector3(-18.4f, 0.22f, gameObject.transform.position.z),
                    Quaternion.AngleAxis(180f, Vector3.up));
                break;
            case 2:
                Instantiate(cars[Random.Range(0, 4)], new Vector3(-13.20901f, 0.22f, gameObject.transform.position.z),
                    Quaternion.AngleAxis(180f, Vector3.up));
                break;
            case 3:
                Instantiate(cars[Random.Range(0, 4)], new Vector3(-7.527071f, 0.22f, gameObject.transform.position.z),
                    Quaternion.AngleAxis(180, Vector3.up));
                break;
            case 4:
                Instantiate(cars[Random.Range(0, 4)], new Vector3(-2.415442f, 0.22f, gameObject.transform.position.z),
                    Quaternion.AngleAxis(180, Vector3.up));
                break;
        }
    }
}