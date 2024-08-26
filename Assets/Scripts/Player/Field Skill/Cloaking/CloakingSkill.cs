using System.Collections;
using UnityEngine;

public class CloakingSkill : FieldSkill
{
    [Header("���׸��� �θ� ������Ʈ")]
    [SerializeField] private GameObject meshParent;
    [Header("Ŭ��ŷ ���̴�")]
    [SerializeField] private Shader cloakingShader;
    [Header("Ŭ��ŷ �ð�")]
    [SerializeField] private float cloakingTimer;
    [Header("���� ��ġ")]
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
        PlaySoundEvent();
        isCloaking = true;

        foreach (Renderer render in renderers)
        {
            Material mat = new Material(cloakingShader);
            mat.CopyPropertiesFromMaterial(render.sharedMaterial);

            mat.SetFloat("_Transparency", transparency);
            render.material = mat;
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
