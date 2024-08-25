using System.Collections;
using UnityEngine;

public class GameClear : PlayerAllDeadPanel
{
    public bool onceClear = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator InputKeyCoroutine()
    {
        onceClear = true;

        yield return new WaitForSeconds(2.5f);

        while (true)
        {
            if (IsAnyKeyDown())
            {
                PopupManager.Instance.RemovePopUp(gameObject.name);
                yield break;
            }

            yield return null;
        }
    }
}
