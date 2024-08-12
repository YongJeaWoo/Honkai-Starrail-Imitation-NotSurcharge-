using UnityEngine;

public class LookTarget : MonoBehaviour
{
    [Header("바라보는 회전 속도")]
    [SerializeField] private float rotSpeed;

    private bool isRotating = false;
    private Quaternion targetRotation;
    private Transform parentObj;
    private CharacterMovement movement;

    private void Start()
    {
        SetParentObject();
    }

    private void Update()
    {
        PerformRotation();
    }

    private void SetParentObject()
    {
        parentObj = transform.parent.parent;
        movement = GetComponentInParent<CharacterMovement>();
    }

    public void TargetToRotation(Collider _target)
    {
        if (_target != null)
        {
            Vector3 direction = (_target.transform.position - parentObj.position).normalized;
            direction.y = 0f;

            if (targetRotation == Quaternion.LookRotation(direction)) return;            

            targetRotation = Quaternion.LookRotation(direction);
            isRotating = true;
            movement.enabled = false;

            Vector3.Cross(parentObj.forward, direction);
        }
    }

    private void PerformRotation()
    {
        if (isRotating)
        {
            parentObj.rotation = Quaternion.RotateTowards(parentObj.rotation, targetRotation, rotSpeed * Time.deltaTime);

            if (Quaternion.Angle(parentObj.rotation, targetRotation) < 0.1f)
            {
                parentObj.rotation = targetRotation;
                isRotating = false;
                movement.enabled = true;
            }
        }
    }
}
