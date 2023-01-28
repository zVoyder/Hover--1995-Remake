using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : Action
{
    public float time = 10;
    public string sceneToLoad;

    override public void SetAction()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
