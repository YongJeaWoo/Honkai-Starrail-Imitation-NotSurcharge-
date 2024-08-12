using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(SelfDes), 2f);
    }

    private void SelfDes()
    {
        Destroy(gameObject);
    }
}
