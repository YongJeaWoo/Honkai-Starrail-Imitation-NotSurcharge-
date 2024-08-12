using UnityEngine;

public class AutoDestroyObject : MonoBehaviour
{
    private float destroyTime = .8f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
