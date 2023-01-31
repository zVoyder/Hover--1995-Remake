using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for the camera portal effect
/// </summary>
[RequireComponent(typeof(Camera))]
public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera; // The camera I want to apply this effect
    public Transform door;
    public Transform portal;

    public Material cameraMat;

    private Camera _camera;


    void Start()
    {
        _camera = GetComponent<Camera>();

        if (_camera.targetTexture != null)
        {
            _camera.targetTexture.Release();
        }

        _camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMat.mainTexture = _camera.targetTexture;
    }

    private void LateUpdate()
    {
        Vector3 playerOffsetFromPortal = playerCamera.position - portal.position; // Calculating the offset position of the player
        transform.position = door.position + playerOffsetFromPortal;

        float angle = Quaternion.Angle(door.rotation, portal.rotation); // Calculating the angle offset

        Quaternion rotationDifference = Quaternion.AngleAxis(angle, Vector3.up); // Rotation Offset Difference
        Vector3 camDirection = rotationDifference * playerCamera.forward; // Multiply it by the playerCamera forward so the rotation is based off where the player is looking
        transform.rotation = Quaternion.LookRotation(camDirection, Vector3.up); // Set the new rotation
    
    }
}
