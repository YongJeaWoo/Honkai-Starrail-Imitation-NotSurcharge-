using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [Header("캐릭터 프리팹")]
    [SerializeField] private CharacterStateSOJ[] playerSOJ;

    private GameEventSystem eventSystem;

    private List<int> battlePlayerList;
    private int activePlayerIndex;

    private Vector3 playerPos = new Vector3(0,0,0);
    private Quaternion playerRot = new Quaternion(0,0,0,0);

    protected override void DoAwake()
    {
        EventMethod();
    }

    private void EventMethod()
    {
        GameObject eventObj = new GameObject($"Event Object");
        eventObj.transform.parent = transform;
        eventSystem = eventObj.AddComponent<GameEventSystem>();
    }

    public List<int> SetPlayerBattler(List<int> lists) => battlePlayerList = new List<int>(lists);
    public List<int> GetPlayerBattler()
    {
        if (battlePlayerList == null || battlePlayerList.Count == 0)
        {
            return new List<int> { 0, 1, 2, 3 };
        }

        return new List<int>(battlePlayerList);
    }

    public CharacterStateSOJ[] GetPlayerSOJData() => playerSOJ;

    public GameEventSystem GetEventSystem() => eventSystem;

    public int GetActivePlayerIndex() => activePlayerIndex;
    public void SetActivePlayerIndex(int index) => activePlayerIndex = index;

    public Vector3 GetPlayerPosition() => playerPos;
    public Quaternion GetPlayerRotation() => playerRot;

    public void SetPlayerTransform(Vector3 pos, Quaternion rot)
    {
        playerPos = pos;
        playerRot = rot;
    }
}
