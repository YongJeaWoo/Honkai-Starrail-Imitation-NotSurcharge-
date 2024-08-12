using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerSystem : MonoBehaviour
{
    [Header("캐릭터 변경 키")]
    [SerializeField] private KeyCode[] changeKeys;

    private GameEventSystem eventSystem;
    private Transform playerSetPos;
    private GameObject player;
    private GameObject[] fieldPlayers;
    private GameObject miniMapCam;
    private CinemachineFreeLook freeCam;
    private CameraInit initCam;
    private SetPlayerUISystem uiSystem;
    private PanelActivate panelActivate;
    private List<int> partyIndex;
    private bool isInit = false;

    private int activePlayerIndex = -1;

    public GameObject[] FieldPlayers { get => fieldPlayers; set => fieldPlayers = value; }

    public List<int> PartyIndex { get => partyIndex; set => partyIndex = value; }

    private void Start()
    {
        InitValues();
    }

    private void Update()
    {
        CharacterChange();
    }

    private void InitValues()
    {
        eventSystem = PlayerDataManager.Instance.GetEventSystem();
        uiSystem = GetComponent<SetPlayerUISystem>();
        initCam = gameObject.AddComponent<CameraInit>();
        freeCam = FindObjectOfType<CinemachineFreeLook>();
        activePlayerIndex = PlayerDataManager.Instance.GetActivePlayerIndex();

        GameObject pos = new GameObject($"Player Collection");
        pos.transform.parent = transform;
        playerSetPos = pos.transform;
        panelActivate = PopupManager.Instance.GetPanelActivation();

        InitializePlayers();
    }

    private void InitializePlayers()
    {
        CharacterStateSOJ[] playerStates = PlayerDataManager.Instance.GetPlayerSOJData();
        FieldPlayers = new GameObject[playerStates.Length];

        for (int i = 0; i < playerStates.Length; i++)
        {
            if (playerStates[i].PlayerPrefab != null)
            {
                GameObject newPlayer = Instantiate(playerStates[i].PlayerPrefab);

                newPlayer.SetActive(false);
                newPlayer.transform.SetParent(playerSetPos);
                FieldPlayers[i] = newPlayer;
            }
        }

        if (eventSystem.GetFirstEvent())
        {
            player = FieldPlayers[activePlayerIndex].gameObject;
            player.SetActive(true);
            InitUISet();
        }

        SetInitPlayer();
    }

    private void InitUISet()
    {
        uiSystem.SetInfoUIPlayer(player);
        uiSystem.SetSkillAttackUIPlayer(player);
        uiSystem.SetSkillButton(player);
    }

    private void TargetSet(GameObject target)
    {
        miniMapCam = GameObject.Find("Map Camera").gameObject;
        var pos = miniMapCam.gameObject.GetComponent<CopyPosition>();
        pos.SetTarget(target.transform);
    }

    private void SetInitPlayer()
    {
        activePlayerIndex = PlayerDataManager.Instance.GetActivePlayerIndex();

        if (activePlayerIndex < 0 || activePlayerIndex >= FieldPlayers.Length)
        {
            activePlayerIndex = 0;
        }

        player = FieldPlayers[activePlayerIndex];

        Vector3 playerPos = PlayerDataManager.Instance.GetPlayerPosition();
        Quaternion playerRot = PlayerDataManager.Instance.GetPlayerRotation();

        if (playerPos != null && playerRot != null)
        {
            player.transform.position = playerPos;
            player.transform.rotation = playerRot;
        }

        Change(activePlayerIndex);
        SetCamera();
    }

    private void SetCamera()
    {
        initCam.InitCameraSet(freeCam, player);
    }

    private void CharacterChange()
    {
        if (panelActivate.IsAnyPanelActive()) return;

        for (int i = 0; i < changeKeys.Length; i++)
        {
            if (i < PartyIndex.Count && Input.GetKeyDown(changeKeys[i]))
            {
                Change(PartyIndex[i]);
            }
        }
    }

    public void Change(int index)
    {
        if (index < 0 || index >= FieldPlayers.Length) return;

        if (isInit && index == activePlayerIndex && activePlayerIndex != -1) return;

        isInit = true;

        Vector3 currentPosition = player.transform.position;
        Quaternion currentRotation = player.transform.rotation;

        if (activePlayerIndex != -1)
        {
            FieldPlayers[activePlayerIndex].SetActive(false);
        }

        FieldPlayers[index].transform.position = currentPosition;
        FieldPlayers[index].transform.rotation = currentRotation;

        FieldPlayers[index].SetActive(true);

        player = FieldPlayers[index];

        InitUISet();
        TargetSet(player);
        SetCamera();

        activePlayerIndex = index;
        PlayerDataManager.Instance.SetActivePlayerIndex(activePlayerIndex);
    }

    public GameObject GetPlayer() => player;
}
