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
        REDLIGHT,
        SHIELD,
        BREAKOUT
    }

    public bool random;
    public PickUpType pickUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory pli))
        {
            if(random)
                pickUp = (PickUpType)Random.Range(0, System.Enum.GetValues(typeof(PickUpType)).Length - 1);

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
                    if (!pli.IsShielded)
                        pli.SpeedNerf();
                break;

                case PickUpType.SHIELD:
                    pli.Shield();
                    break;

                case PickUpType.BREAKOUT:
                    if (!pli.IsShielded)
                        pli.Breakout();
                    break;
            }
        }
    }
}
