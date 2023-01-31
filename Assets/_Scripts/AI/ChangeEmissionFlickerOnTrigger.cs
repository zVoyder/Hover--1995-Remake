using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmissionFlickerOnTrigger : MonoBehaviour
{
    public Color newColor = Color.white;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<EmissionFlicker>(out EmissionFlicker ef))
        {
            ef.color = newColor;
        }
    }
}
