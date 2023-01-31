using System.Collections;
using UnityEngine;


/// <summary>
/// Script for creating a screen fade effect.
/// </summary>
public class CameraFade : MonoBehaviour
{
    public enum StartFade // Enum for choosing the start fade type.
    {
        FADEIN,
        FADEOUT,
        FADEIN_FADEOUT,
        FADEOUT_FADEIN,
        NONE
    }

    [Tooltip("Fade on Start condition")]public StartFade fadeStart = StartFade.NONE; // starting fade
    [Tooltip("Fade duration time in seconds"), Range(1, 100), SerializeField]private float _fadeDuration = 1f; // fade time in seconds duration
    [Tooltip("Color of the fade")]public Color fadeColor;

    private float alpha = 0f; // Current alpha value of the fade effect.

    private void Start()
    {
        switch (fadeStart)
        {
            case StartFade.FADEIN:

                DoFadeIn(_fadeDuration);

                break;

            case StartFade.FADEOUT:

                DoFadeOut(_fadeDuration);

                break;

            case StartFade.FADEIN_FADEOUT:
                DoFadeInOut(_fadeDuration);

                break;


            case StartFade.FADEOUT_FADEIN:

                DoFadeOutIn(_fadeDuration);

                break;

            default:
                break;
        }
    }


    /// <summary>
    /// Start fade out effect.
    /// </summary>
    public void DoFadeOut(float time)
    {
        StartCoroutine(FadeOut(time));
    }

    /// <summary>
    /// Start fade in effect.
    /// </summary>
    public void DoFadeIn(float time)
    {
        StartCoroutine(FadeIn(time));
    }

    /// <summary>
    /// Start fade out followed by fade in.
    /// </summary>
    public void DoFadeOutIn(float time)
    {
        StartCoroutine(FadeOutIn(time));
    }


    /// <summary>
    /// Start fade in followed by fade out.
    /// </summary>
    public void DoFadeInOut(float time)
    {
        StartCoroutine(FadeInOut(time));
    }


    /// <summary>
    /// Coroutine for fading in followed by fading out.
    /// </summary>
    private IEnumerator FadeInOut(float time)
    {
        yield return FadeIn(time);
        yield return FadeOut(time);
    }


    /// <summary>
    /// Coroutine for fading out followed by fading in.
    /// </summary>
    private IEnumerator FadeOutIn(float time)
    {
        yield return FadeOut(time);
        yield return FadeIn(time);
    }

    /// <summary>
    /// Coroutine for fading out.
    /// </summary>
    private IEnumerator FadeOut(float time)
    {
        float startAlpha = 1f; // Alpha is 1 so the FadeOut start with a DrawTexture already fully visible.
        float progress = 0f;

        // Continuously update the alpha value while the progress is less than 1.
        while (progress < 1f)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / time); // clamping the progress value to the range of 0 to 1, the fading effect is always controlled and never exceeds the desired fadeDuration.
            alpha = Mathf.Lerp(startAlpha, 0f, progress);
            yield return null;
        }
    }

    /// <summary>
    /// Coroutine for fading in.
    /// </summary>
    private IEnumerator FadeIn(float time)
    {
        float startAlpha = 0f; // Alpha is 0 because the FadeIn start with a DrawTexture invisible.
        float progress = 0f;

        while (progress < 1f)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / time);
            alpha = Mathf.Lerp(startAlpha, 1f, progress);
            yield return null;
        }
    }

    /// <summary>
    /// OnGUI event for drawing the texture.
    /// </summary>
    private void OnGUI()
    {
        GUI.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture); // Draw Texture with the size of the screen
    }
}