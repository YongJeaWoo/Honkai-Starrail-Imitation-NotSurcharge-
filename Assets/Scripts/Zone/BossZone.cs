public class BossZone : NormalEnemyZone
{
    private readonly string clearPanelName = $"Game Clear";
    private bool isClear = false;

    public override void CallRespawn()
    {
        base.CallRespawn();

        if (isClear) return;

        var popUp = PopupManager.Instance.InstantPopUp(clearPanelName);
        var clear = popUp.GetComponent<GameClear>();

        if (clear.onceClear)
        {
            isClear = true;
        }
    }
}
