using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private AreaName area;

    [Header("¿¬°áµÈ Æ÷Å»")]
    [SerializeField] private Teleporter connectedPortal;

    private Transform teleportPos;

    private void Start()
    {
        teleportPos = transform.GetChild(0);
        area = connectedPortal.GetComponentInParent<AreaName>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform teleportPos = connectedPortal.teleportPos;
            GameObject player = other.gameObject;

            StartCoroutine(TeleportCoroutine(player, teleportPos));
        }
    }

    private IEnumerator TeleportCoroutine(GameObject player, Transform teleportTrans)
    {
        yield return new WaitForSeconds(2f);

        CharacterController cc = player.GetComponent<CharacterController>();

        cc.enabled = false;
        player.transform.position = teleportTrans.position;
        player.transform.rotation = teleportTrans.rotation;
        cc.enabled = true;

        var popUp = PopupManager.Instance.InstantPopUp(area.popUpText);
        var panel = popUp.GetComponentInChildren<AlramPanel>();
        panel.SetAlramText($"{area.areaName}");
    }
}
