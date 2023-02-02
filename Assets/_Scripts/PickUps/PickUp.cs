using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        SPRING,
        CLOAK,
        WALL,
        GREENLIGHT,
        REDLIGHT
    }

    public bool random;
    public PickUpType pickUp;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory pli))
        {
            if (random)
            {
                pickUp = (PickUpType)Random.Range(0, 4);
            }

            switch (pickUp)
            {

                case PickUpType.SPRING:
                    pli.IncreaseSpringCounter();
                break;

                case PickUpType.CLOAK:
                    pli.IncreaseCloakCounter();
                    break;

                case PickUpType.WALL:
                    pli.IncreaseWallCounter();
                    break;

                case PickUpType.GREENLIGHT:
                    pli.SpeedBuff();
                break;

                case PickUpType.REDLIGHT:
                    pli.SpeedNerf();
                break;
            
            }
        }
    }

}
