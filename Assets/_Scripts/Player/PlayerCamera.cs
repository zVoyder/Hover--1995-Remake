using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float smoothness = 0.2f;


    void Update()
    {
        transform.SetPositionAndRotation(
            Vector3.Lerp(transform.position, player.position, smoothness * Time.deltaTime),
            Quaternion.Lerp(transform.rotation, player.rotation, smoothness * Time.deltaTime)
            );
    }

}
