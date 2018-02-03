using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

    public float explosionPower = 40;
    public float explosionRadius = 5f;

    public float maxTimer = 2;
    private float timer = 0;

    bool triggered = false;
    public bool destroyedAfterExplosion = false;

    private MeshRenderer lightRenderer;
    float lastBlinkToggleTimer = 0;
    float initialBlinkRate = 0.5f;

    AudioSource explosionSound;

    public GameObject explosion;

    // Use this for initialization
    void Start () {
        timer = maxTimer;

        lightRenderer = this.transform.GetChild(0).GetComponent<MeshRenderer>() as MeshRenderer;
    }
	
	// Update is called once per frame
	void Update () {
		if (triggered)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                ApplyExplosionForce();
                ManageRearming();
            }
        }

        ManageFlashingLight();
	}

    private void OnTriggerEnter(Collider collider)
    {
        triggered = true;
        lastBlinkToggleTimer = 0;
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

            if (rb != null)
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
        if (destroyedAfterExplosion)
        {
            Destroy(this.gameObject);
        }
        else
        {
            triggered = false;
            timer = maxTimer;
        }
    }

    private void ManageFlashingLight()
    {
        if (!triggered)
        {
            lightRenderer.enabled = true;
        }
        else
        {
            lastBlinkToggleTimer += Time.deltaTime;
            float percentLeft = timer / maxTimer;
            if (percentLeft < 0.5)
            {
                percentLeft += 0.1f;
            }

            //Blink off and on at an increasing frequency;
            if (lastBlinkToggleTimer > percentLeft * initialBlinkRate)
            {
                lightRenderer.enabled = !lightRenderer.enabled;
                lastBlinkToggleTimer = 0f;
            }

        }
    }
}
