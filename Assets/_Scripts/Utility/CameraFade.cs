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

                DoFadeIn();

                break;

            case StartFade.FADEOUT:

                DoFadeOut();

                break;

            case StartFade.FADEIN_FADEOUT:
                DoFadeInOut();

                break;


            case StartFade.FADEOUT_FADEIN:

                DoFadeOutIn();

                break;

            default:
                break;
        }
    }


    /// <summary>
    /// Start fade out effect.
    /// </summary>
    public void DoFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// Start fade in effect.
    /// </summary>
    public void DoFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Start fade out followed by fade in.
    /// </summary>
    public void DoFadeOutIn()
    {
        StartCoroutine(FadeOutIn());
    }


    /// <summary>
    /// Start fade in followed by fade out.
    /// </summary>
    public void DoFadeInOut()
    {
        StartCoroutine(FadeInOut());
    }


    /// <summary>
    /// Coroutine for fading in followed by fading out.
    /// </summary>
    private IEnumerator FadeInOut()
    {
        yield return FadeIn();
        yield return FadeOut();
    }


    /// <summary>
    /// Coroutine for fading out followed by fading in.
    /// </summary>
    private IEnumerator FadeOutIn()
    {
        yield return FadeOut();
        yield return FadeIn();
    }

    /// <summary>
    /// Coroutine for fading out.
    /// </summary>
    private IEnumerator FadeOut()
    {
        float startAlpha = 1f; // Alpha is 1 so the FadeOut start with a DrawTexture already fully visible.
        float progress = 0f;

        // Continuously update the alpha value while the progress is less than 1.
        while (progress < 1f)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / _fadeDuration); // clamping the progress value to the range of 0 to 1, the fading effect is always controlled and never exceeds the desired fadeDuration.
            alpha = Mathf.Lerp(startAlpha, 0f, progress);
            yield return null;
        }
    }

    /// <summary>
    /// Coroutine for fading in.
    /// </summary>
    private IEnumerator FadeIn()
    {
        float startAlpha = 0f; // Alpha is 0 because the FadeIn start with a DrawTexture invisible.
        float progress = 0f;

        while (progress < 1f)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / _fadeDuration);
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