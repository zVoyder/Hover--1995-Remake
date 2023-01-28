using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ActivateGameObject : Action
{
    public GameObject uIToShowOnLoose;

    public override void SetAction()
    {
        uIToShowOnLoose.SetActive(true);
    }
}
