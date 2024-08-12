using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleUI : MonoBehaviour
{
    // 파티클 시스템
    public ParticleSystem cycleParticle;
    // 조준 UI 스프라이트
    public Image aimingUI; 

    private ParticleSystem.Particle[] particles;

    void Start()
    {
        if (cycleParticle == null || aimingUI == null)
        {
            Debug.LogError("파티클 시스템 또는 조준 UI가 설정되지 않았습니다.");
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

        // 현재 활성화된 파티클을 가져옵니다.
        int particleCount = cycleParticle.GetParticles(particles);

        // 파티클이 하나라도 있다면 첫 번째 파티클의 크기를 가져옵니다.
        if (particleCount > 0)
        {
            // 첫 번째 파티클의 크기
            float size = particles[0].GetCurrentSize(cycleParticle);

            // 파티클의 크기를 조정
            float adjustedSize = size / 13f;

            // 조준 UI의 크기를 조정
            aimingUI.rectTransform.localScale = new Vector3(adjustedSize, adjustedSize, 1);
        }
    }
}
