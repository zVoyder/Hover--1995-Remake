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

    public RectTransform minimapUI;
    public Vector2Int sizeRenderTexture;
    [Range(1, 5)] public float luminosity = 1f; // light intensity of the light attached to this gameobject
    [Range(5, 50)] public float cameraViewSize = 10f; // size of the minimap's view
    [Range(5, 100)] public float cameraHeight = 50f; // height of the minimap camera

    private Transform _toFollow; // the transform of the object to follow
    private LayerMask _visibleLayers = Constants.Layers.ALLMINIMAP; // the layers that the minimap camera should render

    private Light _minimapLight;

    private void Start()
    {
        Image background = minimapUI.GetComponent<Image>();
        background.color = backgroundColor; // set the color of the minimap's background

        if (Finder.TryFindGameObjectWithTag(Constants.Tags.PLAYER, out GameObject pl))
        {
            GameObject p = GameObject.Instantiate(pointer, pl.transform) as GameObject; // instantiate the pointer object as a child of the player
            p.transform.localPosition = new Vector3(0, 1f, 0);

            _toFollow = p.transform; // set the object to follow to the pointer object's transform
        }
        else
        {
            Debug.LogError("The PLAYER GameObject was not found.");
        }

        _minimapLight = gameObject.AddComponent<Light>() as Light;

        foreach (Camera cam in pl.GetComponentsInChildren<Camera>()) // Attach the ignore light component foreach camera of the player
        {
            cam.cullingMask = ~Constants.Layers.MINIMAP;
            cam.gameObject.AddComponent<IgnoreLight>().LIghtIgnored = _minimapLight; // add a the IgnoreLight component and set its 
        }

        _minimapLight.type = LightType.Directional;
        _minimapLight.color = Color.white;
        _minimapLight.intensity = luminosity;
        _minimapLight.shadows = LightShadows.None;
        _minimapLight.cullingMask = _visibleLayers;

        Camera mpCamera = gameObject.AddComponent<Camera>() as Camera; // add a camera component to this game object
        mpCamera.clearFlags = CameraClearFlags.SolidColor;
        mpCamera.backgroundColor = backgroundColor;
        mpCamera.targetTexture = 
            new RenderTexture(sizeRenderTexture.x, sizeRenderTexture.y, 8, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);//Creating a new render texture with the size
        mpCamera.targetTexture.antiAliasing = 1;
        mpCamera.targetTexture.Create(); //constructor to create it

        minimapUI.GetComponentInChildren<RawImage>().texture = mpCamera.targetTexture; // setting the rendertexture in the rawimage of the minimapUI
        mpCamera.useOcclusionCulling = false;
        mpCamera.allowHDR = false;
        mpCamera.allowMSAA = false;
        mpCamera.allowDynamicResolution = false;
        mpCamera.cullingMask = _visibleLayers; // set the camera's culling mask to the visible layers

        mpCamera.transform.position = new Vector3(_toFollow.transform.position.x, cameraHeight, _toFollow.transform.position.z); // set the camera's initial position to be above the pointer object


        Matrix4x4 ortho = Matrix4x4.Ortho(-cameraViewSize, cameraViewSize, -cameraViewSize, cameraViewSize, 0.3f, cameraHeight); // create an orthographic projection matrix
        mpCamera.farClipPlane = cameraHeight;
        mpCamera.projectionMatrix = ortho; // set the camera's projection matrix to the orthographic matrix
    }

    private void Update()
    {
        // update the minimap camera's position to match the pointer object's position
        transform.position = new Vector3(_toFollow.position.x, transform.position.y, _toFollow.position.z); ;

        // update the minimap camera's rotation to match the pointer object's rotation
        transform.rotation = Quaternion.Euler(90, _toFollow.eulerAngles.y, 0f);

        
    }


}
