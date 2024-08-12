using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Collider col;

    [SerializeField] protected ParticleSystem projectilePS;
    [SerializeField] protected GameObject hitPrefab;
    [SerializeField] protected GameObject flash;
    [SerializeField] protected ParticleSystem hitPS;
    [SerializeField] protected GameObject[] detached;
    [SerializeField] protected Light lightSource;
    [SerializeField] protected float hitOffset = 0;
    [SerializeField] protected bool notDestroy = false;

    protected void CollisionActive(Collision collision)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        if (lightSource != null)
            lightSource.enabled = false;
        col.enabled = false;
        projectilePS.Stop();
        projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        if (hitPrefab != null)
        {
            hitPrefab.transform.rotation = rot;
            hitPrefab.transform.position = pos;
            hitPrefab.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0);
            hitPS.Play();
        }

        foreach (var detachedPrefab in detached)
        {
            if (detachedPrefab != null)
            {
                ParticleSystem detachedPS = detachedPrefab.GetComponent<ParticleSystem>();
                detachedPS.Stop();
            }
        }
        if (notDestroy)
            StartCoroutine(DisableTimer(hitPS.main.duration));
        else
        {
            if (hitPS != null)
            {
                Destroy(gameObject, hitPS.main.duration);
            }
            else
                Destroy(gameObject, 1);
        }
    }

    protected IEnumerator DisableTimer(float time)
    {
        yield return new WaitForSeconds(time);

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        yield break;
    }
}
