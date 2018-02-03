using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    SPEEDY,
    SUPERRAM,
    WIDE
}

public class Pickup : MonoBehaviour
{
    PowerupType powerupType = PowerupType.SUPERRAM;
    // Use this for initialization
    void Start()
    {

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
        }

    }
}
