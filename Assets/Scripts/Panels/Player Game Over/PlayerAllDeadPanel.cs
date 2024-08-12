using System.Collections;
using UnityEngine;

public class PlayerAllDeadPanel : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(InputKeyCoroutine());
    }

    private IEnumerator InputKeyCoroutine()
    {
        yield return new WaitForSeconds(2.5f);

        while (true)
        {
            if (IsAnyKeyDown())
            {
                BattleEntryManager.Instance.BattleEndCoroutine();
                PopupManager.Instance.RemovePopUp(gameObject.name);
                yield break;
            }

            yield return null;
        }
    }

    private bool IsAnyKeyDown()
    {
        return Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
    }
}
