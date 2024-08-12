using UnityEngine;

public class AreaName : MonoBehaviour
{
    public string popUpText = $"Area Info Panel";

    [Header("구역 이름")]
    [SerializeField] public string areaName;

    private UITextController textController;
    private GameEventSystem eventSystem;

    [SerializeField] private AudioClip clip;

    private bool hasTriggered = false;

    private void Start()
    {
        FindObject();
    }

    private void FindObject()
    {
        textController = GetComponent<UITextController>();
        eventSystem = PlayerDataManager.Instance.GetEventSystem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!eventSystem.GetFirstEvent() || hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (clip != null)
                AudioManager.instance.BgmPlay(clip);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == null) return;

        if (other.CompareTag("Player"))
        {
            textController.SetAreaText(areaName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasTriggered = false;
        }
    }
}
