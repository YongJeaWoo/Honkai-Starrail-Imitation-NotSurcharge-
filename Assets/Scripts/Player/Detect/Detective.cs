using UnityEngine;
using UnityEngine.UI;

public class Detective : MonoBehaviour
{
    [Header("Å½Áö ¹üÀ§")]
    [SerializeField] private float detectRadius;

    private Image infoImage;

    private LookTarget lookTarget;
    private SetPlayerUISystem uiSystem;

    private bool isDetecting;

    private void Start()
    {
        GetComponents();
    }

    private void Update()
    {
        DetectObject();
    }

    private void GetComponents()
    {
        uiSystem = FindAnyObjectByType<SetPlayerUISystem>();
        lookTarget = GetComponent<LookTarget>();

        infoImage = uiSystem.GetInfoImage();
    }

    private void DetectObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);
        isDetecting = false;

        foreach (var col in colliders)
        {
            if (col == null) continue;

            if (col.TryGetComponent<IDetect>(out var interactive))
            {
                isDetecting = true;
                infoImage.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    lookTarget.TargetToRotation(col);
                    interactive.DetectiveBehaviour();
                }
                break;
            }

            if (!isDetecting)
            {
                infoImage.gameObject.SetActive(false);
            }
        }
    }

    public Image SetInfoImage(Image infoImage) => this.infoImage = infoImage;
    public Image GetInfoImage() => infoImage;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
