using UnityEngine;

public class SelfParticleDestroy : MonoBehaviour
{
    private ParticleSystem myParticle;

    private void Start()
    {
        CheckParticle();
    }

    private void CheckParticle()
    {
        myParticle = GetComponent<ParticleSystem>();

        if (myParticle == null)
        {
            Debug.LogWarning($"파티클 시스템이 존재하지 않음");
            return;
        }
    }

    private void Update()
    {
        DestroySelf();
    }

    private void DestroySelf()
    {
        if (myParticle && !myParticle.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
