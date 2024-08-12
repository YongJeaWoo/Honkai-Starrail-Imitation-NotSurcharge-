using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleUI : MonoBehaviour
{
    // ��ƼŬ �ý���
    public ParticleSystem cycleParticle;
    // ���� UI ��������Ʈ
    public Image aimingUI; 

    private ParticleSystem.Particle[] particles;

    void Start()
    {
        if (cycleParticle == null || aimingUI == null)
        {
            Debug.LogError("��ƼŬ �ý��� �Ǵ� ���� UI�� �������� �ʾҽ��ϴ�.");
            return;
        }
        particles = new ParticleSystem.Particle[cycleParticle.main.maxParticles];
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        // ���� Ȱ��ȭ�� ��ƼŬ�� �����ɴϴ�.
        int particleCount = cycleParticle.GetParticles(particles);

        // ��ƼŬ�� �ϳ��� �ִٸ� ù ��° ��ƼŬ�� ũ�⸦ �����ɴϴ�.
        if (particleCount > 0)
        {
            // ù ��° ��ƼŬ�� ũ��
            float size = particles[0].GetCurrentSize(cycleParticle);

            // ��ƼŬ�� ũ�⸦ ����
            float adjustedSize = size / 13f;

            // ���� UI�� ũ�⸦ ����
            aimingUI.rectTransform.localScale = new Vector3(adjustedSize, adjustedSize, 1);
        }
    }
}
