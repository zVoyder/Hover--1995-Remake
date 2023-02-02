using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFor : MonoBehaviour
{
    public string triggerTag = Constants.Tags.PLAYER;
    [Range(1, 60), Tooltip("How long has to be disabled")]public float time = 2f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            gameObject.SetActive(false);
            Invoke("EnableObject", time);
        }
    }

    private void EnableObject()
    {
        gameObject.SetActive(true);
    }
}
