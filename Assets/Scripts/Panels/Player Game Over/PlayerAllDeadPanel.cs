using System.Collections;
using UnityEngine;

public class PlayerAllDeadPanel : MonoBehaviour
{
    protected virtual void Start()
    {
        StartCoroutine(InputKeyCoroutine());
    }

    protected virtual IEnumerator InputKeyCoroutine()
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

    protected bool IsAnyKeyDown()
    {
        return Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
    }
}
