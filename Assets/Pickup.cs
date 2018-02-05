using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    SPEEDY,
    MEATY
}

public class Pickup : MonoBehaviour
{
    public PowerupType powerupType = PowerupType.MEATY;
    // Use this for initialization
    void Start()
    {
        powerupType = (PowerupType)Random.Range(0, System.Enum.GetValues(typeof(PowerupType)).Length);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        CarModel otherCar = collider.GetComponent<CarModel>() as CarModel;

        if (otherCar != null)
        {
            otherCar.ApplyPowerup(this.powerupType);
            Destroy(this.gameObject);
        }

    }
}
