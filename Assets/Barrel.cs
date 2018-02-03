using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour {

    public float explosionPower = 40;
    public float explosionRadius = 5f;

    public float maxTimer = 2;
    private float timer = 0;

    bool triggered = false;
    public bool destroyedAfterExplosion = true;
    public bool respawnAfterExplosion = true;

    float launchSpeed = 10f;

    float respawnBoxRadius = 10f;
    Vector3 respawnAnchorPoint = new Vector3(-1, 5, 7);

    AudioSource explosionSound;

    Rigidbody body;

    public bool enableRandomMovement = false;
    Vector3 randomDirection;
    float movePower = 1f;
    float maxSpeed = 5;

    public GameObject explosion;


    // Use this for initialization
    void Start()
    {
        timer = maxTimer;
        body = this.GetComponent<Rigidbody>() as Rigidbody;

        randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        randomDirection.Normalize();

    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                ApplyExplosionForce();
                ManageRearming();
            }
        }

        if (this.transform.position.y < -10f)
        {
            Destroy(this.gameObject);
        }

        if(enableRandomMovement)
        {
            if (body.velocity.magnitude < maxSpeed)
            {
                body.AddForce(randomDirection * movePower);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!triggered)
        {
            CarModel otherCar = collision.collider.GetComponent<CarModel>() as CarModel;

            if (otherCar != null)
            {
                triggered = true;
                Vector3 direction = this.transform.position - otherCar.transform.position;
                direction.y = 0;
                direction.Normalize();
                direction.y = 0.8f;

                body.velocity = direction * launchSpeed;
            }
        }
        else
        {
            ApplyExplosionForce();
            ManageRearming();
        }
    }

    private void ApplyExplosionForce()
    {
        Instantiate(this.explosion, this.transform.position, Quaternion.identity);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, this.explosionRadius);
        foreach (Collider hit in colliders)
        {
            print("Applying explosion");
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb != this.body)
            {
                // rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);
                Vector3 mineToBody = rb.transform.position - this.transform.position;
                float distToMine = mineToBody.magnitude;
                Vector3 m2bNorm = mineToBody.normalized;
                m2bNorm.y = 0.5f;

                rb.velocity = m2bNorm * this.explosionPower;
            }

        }
    }

    private void ManageRearming()
    {
        if (respawnAfterExplosion)
        {
            RespawnBarrel();
        }
        else if (destroyedAfterExplosion)
        {
            Destroy(this.gameObject);
        }
        else
        {
            triggered = false;
            timer = maxTimer;
        }
    }

    void RespawnBarrel()
    {
        triggered = false;
        timer = maxTimer;

        Vector3 randomVector = new Vector3(Random.Range(-respawnBoxRadius, respawnBoxRadius), 0.0f, Random.Range(-respawnBoxRadius, respawnBoxRadius));

        Rigidbody body = this.transform.GetComponent<Rigidbody>() as Rigidbody;
        body.velocity = new Vector3(0f, 0f, 0f);
        body.angularVelocity = new Vector3(0f, 0f, 0f);

        body.position = respawnAnchorPoint + randomVector;
        body.rotation = Quaternion.EulerAngles(0f, 0f, 0f);


    }
}
