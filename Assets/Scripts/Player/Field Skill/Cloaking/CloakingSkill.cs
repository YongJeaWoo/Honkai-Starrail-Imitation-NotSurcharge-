using System.Collections;
using UnityEngine;

public class CloakingSkill : FieldSkill
{
    [Header("마테리얼 부모 오브젝트")]
    [SerializeField] private GameObject meshParent;
    [Header("클로킹 시간")]
    [SerializeField] private float cloakingTimer;
    [Header("투명도 수치")]
    [SerializeField] private float transparency;

    private bool isCloaking = false;

    private Renderer[] renderers;
    private Material[] originMats;

    private void Start()
    {
        InitMaterial();
    }

    private void InitMaterial()
    {
        renderers = meshParent.GetComponentsInChildren<Renderer>();
        originMats = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originMats[i] = renderers[i].sharedMaterial;
        }
    }

    public override void UseFieldSkill()
    {
        if (isCloaking) return;

        isCloaking = true;

        foreach (Renderer render in renderers)
        {
            Material mat = render.material;
            mat.shader = Shader.Find("Custom/Cloaking");

            mat.SetFloat("_Transparency", transparency);
        }

        StartCoroutine(CloakingCoroutine(cloakingTimer));
    }

    private IEnumerator CloakingCoroutine(float duration)
    {
        float timer = duration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        isCloaking = false;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterial = originMats[i];
        }
    }

    public bool GetIsCloaking() => isCloaking;
}
