using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public string tagName = Constants.Tags.ENEMY;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(tag))
        {
            Destroy(gameObject);
        }
    }
}
