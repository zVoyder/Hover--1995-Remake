using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegatePlatform : MonoBehaviour
{
    PlayerInventory pi;

    void Start()
    {
        pi = GetComponent<PlayerInventory>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<RBPlayerMovement>(out RBPlayerMovement player))
        {
            pi.m_flagsCounter -= 1;
        }
    }
}
