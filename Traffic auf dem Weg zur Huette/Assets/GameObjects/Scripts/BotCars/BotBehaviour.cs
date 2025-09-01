using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotBehaviour : MonoBehaviour
{
    public LayerMask mask;


    public float topSpeed;
    [SerializeField] private AudioClip[] clip;

    private Transform _transform;
    private float acc = 0;
    private bool brakebool = false;

    private bool crashed;
    private Rigidbody rb;
    private AudioSource source;

    private void Awake()
    {
        _transform = transform;
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        topSpeed = Random.Range(17f, 26.0f);
        source = GetComponent<AudioSource>();

        var temp2 = clip.Length - 0.1f;
        var temp = Random.Range(0f, temp2);
        source.clip = clip[(int)temp];

        StartCoroutine(checkFront());
        CarController.OnPlayerTeleport += HandleTeleport;
    }


    private void Update()
    {
        if (!crashed) transform.Translate(Vector3.forward * (topSpeed * Time.deltaTime));

        if (transform.position.z < -650f)
        {
            transform.position = new Vector3(_transform.position.x, _transform.position.y,
                800 - (-650f - transform.position.z));
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void OnDestroy()
    {
        CarController.OnPlayerTeleport -= HandleTeleport;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment") || collision.gameObject.CompareTag("Player"))
            this.collision();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Environment")) collision();
    }


    private void HandleTeleport(float distance)
    {
        topSpeed += Random.Range(3f, 5f);

        transform.position =
            new Vector3(_transform.position.x, _transform.position.y, transform.position.z - distance);
        transform.rotation = _transform.rotation;
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
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            if (Physics.Raycast(new Vector3(transform.position.x, 1.5f, transform.position.z), Vector3.forward,
                    out var hitInfo, 35f, mask))
            {
                var bB = hitInfo.transform.gameObject.GetComponent<BotBehaviour>();
                if (bB.crashed)
                {
                    rb.constraints = RigidbodyConstraints.None;
                    crashed = true;
                    StartCoroutine(kill());
                    yield return new WaitForSeconds(6f);
                }
                else
                {
                    var temp = bB.getTopSpeed();
                    bB.setTopSpeed(topSpeed);
                    topSpeed = temp;
                }
            }
        }
    }


    public void collision()
    {
        rb.constraints = RigidbodyConstraints.None;
        source.Play();
        crashed = true;
        rb.AddExplosionForce(3f, transform.position, 3f);

        StartCoroutine(kill());
    }

    private IEnumerator kill()
    {
        yield return new WaitForSeconds(5f);
        transform.position = new Vector3(_transform.position.x, _transform.position.y, 421);
        transform.rotation = _transform.rotation;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionX;
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        crashed = false;
    }
}