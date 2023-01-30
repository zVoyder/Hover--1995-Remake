using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple script for changing scene On Trigger Enter
/// </summary>
[RequireComponent(typeof(Collider))]
public class ChangeSceneOnTrigger : MonoBehaviour
{
    public string triggerTag = Constants.Tags.PLAYER;
    public string sceneToLoad;
    public float timeToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            StartCoroutine(LoadScene());
        }
    }


    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(timeToLoad);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
