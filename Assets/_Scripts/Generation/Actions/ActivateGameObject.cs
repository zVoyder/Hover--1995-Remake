using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ActivateGameObject : Action
{
    public GameObject gameObjectToEnable;

    public override void SetAction()
    {
        gameObjectToEnable.SetActive(true);
    }
}
