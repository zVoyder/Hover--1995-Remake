using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Scene Menu");
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Scene Level1");
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Scene Level2");
            SceneManager.LoadScene(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Scene Level3");
            SceneManager.LoadScene(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Scene Credits");
            SceneManager.LoadScene(4);
        }

    }
}
