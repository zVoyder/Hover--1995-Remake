using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float timeRecover;
    private RBPlayerMovement player;
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<RBPlayerMovement>(out player)) //when the player collides set the variable true
        {
            StartCoroutine(ReEnableIn(timeRecover, player.CanMove));
        }
        Debug.Log("tigger");
    }
    private IEnumerator ReEnableIn(float time, bool b)
    {
        player.CanMove = false;
        player.m_rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(time);
        player.CanMove = true;
    }
}

