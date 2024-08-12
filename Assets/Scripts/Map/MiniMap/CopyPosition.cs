using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private bool x, y, z;

    private Transform target;    
    
    private void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        if (!target) return;

        transform.position = new Vector3((x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }
    public Transform SetTarget(Transform _target) => target = _target;
}
