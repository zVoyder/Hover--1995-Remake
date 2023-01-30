using System.Collections;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    public float fadeDuration = 1f;
    public Color fadeColor;

    private float alpha = 0f;

    private void OnEnable()
    {
        DoFadeOut();
    }

    public void DoFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void DoFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float startAlpha = 1f;
        float progress = 0f;

        while (progress < 1f)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / fadeDuration);
            alpha = Mathf.Lerp(startAlpha, 0f, progress);
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float startAlpha = 0f;
        float progress = 0f;

        while (progress < 1f)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / fadeDuration);
            alpha = Mathf.Lerp(startAlpha, 1f, progress);
            yield return null;
        }
    }

    private void OnGUI()
    {
        GUI.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
    }
}