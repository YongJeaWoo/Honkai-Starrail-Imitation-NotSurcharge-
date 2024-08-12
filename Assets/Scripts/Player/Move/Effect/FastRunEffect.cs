using System.Collections;
using UnityEngine;

public class FastRunEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem onceRunEffectPrefab;
    [SerializeField] private TrailRenderer[] runTrailRender;

    private Coroutine runTrailCoroutine;

    private void Awake()
    {
        RunTrailRenderOff();
    }

    public void PlayFastRunEffect()
    {
        onceRunEffectPrefab.Play();
    }

    private void RunTrailRenderOff()
    {
        if (runTrailCoroutine != null)
        {
            for (int i = 0; i < runTrailRender.Length; i++)
            {
                runTrailRender[i].enabled = false;
            }
        }
    }

    public void ShowTrailRenderEvent()
    {
        if (runTrailCoroutine != null) StopCoroutine(runTrailCoroutine);

        int trailRenderCountToShow = Random.Range(2, 4); 

        for (int i = 0; i < trailRenderCountToShow; i++)
        {
            int randomIndex = Random.Range(0, runTrailRender.Length);

            runTrailRender[randomIndex].enabled = true;
            runTrailCoroutine = StartCoroutine(DisableTrail(runTrailRender[randomIndex]));
        }
    }

    public void HideTrailRenderEvent()
    {
        for (int i = 0; i < runTrailRender.Length; i++)
        {
            runTrailRender[i].enabled = false;

            if (runTrailCoroutine != null)
            {
                StopCoroutine(runTrailCoroutine);
            }
        }
    }

    private IEnumerator DisableTrail(TrailRenderer trailRenderer)
    {
        float duration = 0.5f;
        yield return new WaitForSeconds(duration);
        trailRenderer.enabled = false;
    }
}
