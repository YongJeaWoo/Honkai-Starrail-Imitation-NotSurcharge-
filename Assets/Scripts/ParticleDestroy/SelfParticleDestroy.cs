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
            Debug.LogWarning($"��ƼŬ �ý����� �������� ����");
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
