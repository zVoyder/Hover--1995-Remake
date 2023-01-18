using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extension.Methods;

[RequireComponent(typeof(RectTransform))]
public class MomentumLine : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float thick = 5f, maxHeight = 100f;


    private RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(thick, 0f);
    }
    
    
    void Update()
    {
        float curr = Mathematics.Percent(rigidBody.velocity.magnitude * 10, maxHeight);
        if (curr > maxHeight)
            curr = maxHeight;


        _rectTransform.sizeDelta = new Vector2(thick, curr);

        //float r = ;
        _rectTransform.rotation = Quaternion.Euler(rigidBody.velocity * rigidBody.velocity.magnitude);

        Debug.Log(curr);
    }
}
