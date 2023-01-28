using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad;

    private void OnEnable()
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

}
