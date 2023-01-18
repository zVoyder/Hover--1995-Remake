using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extension;


/// <summary>
/// This class simulate a minimap using a camera component and Rendered Texture
/// </summary>
public class MinimapCamera : MonoBehaviour
{
    public Color backgroundColor = Color.black; // color of the minimap background

    public GameObject pointer; // pointer object that the minimap camera will follow

    public RenderTexture renderMinimapTexture; // render texture that the minimap camera will render to
    [Range(5, 20)] public float cameraViewSize = 10f; // size of the minimap's view
    [Range(50, 100)] public float cameraHeight = 50f; // height of the minimap camera

    private Transform _toFollow; // the transform of the object to follow
    private LayerMask _visibleLayers = Extension.Constants.Layers.MINIMAP; // the layers that the minimap camera should render
    private Camera _camera; // the minimap camera
    private Image _backGroundImage; // the image component of the minimap

    private void Start()
    {

        if (Finder.TryFindGameObjectWithTag(Extension.Constants.Tags.MINIMAP, out GameObject gm) // try to find the minimap object
            && gm.TryGetComponent<Image>(out Image img)) // try to get the image component from the minimap object
        {
            _backGroundImage = img;
            _backGroundImage.color = backgroundColor; // set the color of the minimap's background
        }
        else
        {
            Debug.LogError("The MINIMAP GameObject was not found.");
        }

        if (Finder.TryFindGameObjectWithTag(Extension.Constants.Tags.PLAYER, out GameObject pl))
        {
            GameObject p = GameObject.Instantiate(pointer, pl.transform) as GameObject; // instantiate the pointer object as a child of the player
            p.transform.localPosition = new Vector3(0, 1f, 0);

            _toFollow = p.transform; // set the object to follow to the pointer object's transform
        }
        else
        {
            Debug.LogError("The PLAYER GameObject was not found.");
        }

        _camera = gameObject.AddComponent<Camera>() as Camera; // add a camera component to this game object
        _camera.clearFlags = CameraClearFlags.SolidColor;
        _camera.backgroundColor = backgroundColor;
        _camera.targetTexture = renderMinimapTexture;
        _camera.useOcclusionCulling = false;
        _camera.allowHDR = false;
        _camera.allowMSAA = false;
        _camera.allowDynamicResolution = false;
        _camera.cullingMask = _visibleLayers; // set the camera's culling mask to the visible layers

        _camera.transform.position = new Vector3(_toFollow.transform.position.x, cameraHeight, _toFollow.transform.position.z); // set the camera's initial position to be above the pointer object


        Matrix4x4 ortho = Matrix4x4.Ortho(-cameraViewSize, cameraViewSize, -cameraViewSize, cameraViewSize, 0.3f, cameraHeight); // create an orthographic projection matrix
        _camera.farClipPlane = cameraHeight;
        _camera.projectionMatrix = ortho; // set the camera's projection matrix to the orthographic matrix
    }

    private void Update()
    {
        // update the minimap camera's position to match the pointer object's position
        transform.position = new Vector3(_toFollow.position.x, transform.position.y, _toFollow.position.z); ;

        // update the minimap camera's rotation to match the pointer object's rotation
        transform.rotation = Quaternion.Euler(90, _toFollow.eulerAngles.y, 0f);
    }
}
