using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ActivateGameObject : Action
{
    public GameObject gameObject;

    public override void SetAction()
    {
        gameObject.SetActive(true);
    }
}
