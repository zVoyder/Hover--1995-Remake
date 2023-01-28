using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Extension.Data;

[RequireComponent(typeof(MeshRenderer))]
public class EmissionFlicker : MonoBehaviour
{
    Material mat;
    public Color color;
    public IntRange intensity;
    public float time;


    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        float t = 0;
        while (true)
        {
            float emiss = Mathf.Lerp(intensity.min, intensity.max, Mathf.PingPong(t / time, 1));
            mat.SetColor("_EmissionColor", color * emiss);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

}