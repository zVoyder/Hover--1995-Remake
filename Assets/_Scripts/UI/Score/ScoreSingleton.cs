using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// ScoreSingleton class implementing the singleton design pattern
/// to maintain a single instance of the score throughout multiple scenes
/// </summary>
public class ScoreSingleton : MonoBehaviour
{
    public static ScoreSingleton instance; //static instance of ScoreSingleton 
    public SO_Score so_Score; // Reference to the scriptable object for storing score

    TMP_Text _scoreText; // Reference to the text component for displaying the score


    private void Awake()
    {
        // Check if an instance of ScoreSingleton already exists
        if (instance == null)
        {
            // If not, set this object as the instance and subscribe to the SceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
            instance = this;
            // Create an instance of the scriptable object for storing the score
            so_Score = ScriptableObject.CreateInstance<SO_Score>();
            // Prevent this object from being destroyed when changing scenes
            DontDestroyOnLoad(this);
        }
        else
        {
            // If an instance already exists, destroy this object
            Destroy(gameObject);
        }
    }


    //Event that is called every time a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Check if an object with the "ScoreUI" tag exists in the scene
        if (Extension.Finder.TryFindGameObjectWithTag("ScoreUI", out GameObject scoreUI))
        {
            // Get the Text component from the object and store it in _scoreText
            _scoreText = scoreUI.GetComponent<TextMeshProUGUI>();
            // Update the text with the current score
            UpdateText();
        }
    }

    /// <summary>
    /// Add to the score
    /// </summary>
    /// <param name="toAdd">The amount to add to the score</param>
    public void AddScore(int toAdd)
    {
        // Add to the score stored in the scriptable object and multiply it by the multiplier
        so_Score.score += (int)((float)toAdd * so_Score.multiplier);
        // Update the text with the new score
        UpdateText();
    }

    public void RemoveScore(int toRemove)
    {
        int totalToRemove = (int)((float)toRemove * so_Score.multiplier);

        if (so_Score.score - totalToRemove < 0)
        {
            so_Score.score = 0;
        }
        else
        {
            so_Score.score -= totalToRemove;
        }

        // Update the text with the new score
        UpdateText();
    }

    /// <summary>
    /// Update the text component with the current score
    /// </summary>
    private void UpdateText()
    {
        _scoreText.text = so_Score.score.ToString();
    }

}
