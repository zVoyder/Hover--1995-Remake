using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public Vector3 offsetRising;
    public BoxCollider colliderStairs;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<RBPlayerMovement>(out RBPlayerMovement player))
        {
            if (player.transform.position.y - transform.position.y < 1.412f)
            {
                player.transform.position += offsetRising + new Vector3(0, 0, colliderStairs.bounds.size.z);
            }
        }
    }
}
