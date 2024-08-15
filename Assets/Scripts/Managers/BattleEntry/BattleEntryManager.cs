using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleEntryManager : Singleton<BattleEntryManager>
{
    private string nextScene;
    [Header("공격 성공 텍스트")]
    [SerializeField] private string attackSuccessText;
    [Header("공격 실패 텍스트")]
    [SerializeField] private string attackFailText;

    [Header("배틀 진입 시작 타이머")]
    [SerializeField] private TextMeshProUGUI battleText;
    [Header("배틀 진입 시작 타이머")]
    [SerializeField] private float waitTime;
    private WaitForSeconds waitTimer;

    [Header("배틀 진입 사진 캡처")]
    [SerializeField] private Image captureImage;

    [Space(5f)]
    [Header("배틀 진입 패널")]
    [SerializeField] private Image fadePanel;
    [Header("페이드아웃 패널 속도")]
    [SerializeField] private float fadePanelSpeed;

    [Space(2f)]
    [Header("배틀 진입 유리 이미지")]
    [SerializeField] private Image glassImage;
    [Header("유리 파편 리소스")]
    [SerializeField] private Sprite[] glassSprites;
    private float glassWaitTime = 0.05f;

    private Camera cam;
    private EnemyZone battleZone;

    private bool isBattle;

    private bool isBattling = false;

    [Header("BGM")]
    [SerializeField] private AudioClip enemySound;
    [SerializeField] private AudioClip BossSound;

    protected override void DoAwake()
    {
        base.DoAwake();
        cam = Camera.main;
        isBattle = false;
    }

    private void Start()
    {
        waitTimer = new(waitTime);
    }

    public void FadeoutEntryBattle(E_TurnBase turnBase)
    {
        if (isBattling) return;
        isBattling = true;
        StartCoroutine(FadeOutCoroutine(turnBase));
    }

    public void BrokenEntryBattle(E_TurnBase turnBase)
    {
        if (isBattling) return;
        isBattling = true;
        StartCoroutine(GlassBrokenEffect(turnBase));
    }

    #region EnemyBattle System
    private IEnumerator FadeOutCoroutine(E_TurnBase turnBase)
    {
        ToggleOtherZones(false);

        var system = FindAnyObjectByType<SetPlayerSystem>();
        var player = system.GetPlayer();
        PlayerDataManager.Instance.SetPlayerTransform(player.transform.position, player.transform.rotation);

        nextScene = $"Battle";

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        battleText.gameObject.SetActive(true);
        if (turnBase == E_TurnBase.Player)
        {
            battleText.text = attackSuccessText;
        }
        else
        {
            battleText.text = attackFailText;
        }

        var movement = player.GetComponentInChildren<CharacterMovement>();
        var animator = player.GetComponent<Animator>();
        movement.SetCurrentSpeed(0f);
        animator.speed = 0.05f;

        var fsmControllers = battleZone.GetComponentsInChildren<EnemyFSM>();

        foreach (var fsmController in fsmControllers)
        {
            var ani = fsmController.GetComponent<Animator>();
            ani.speed = 0.05f;

            var nav = fsmController.GetComponent<NavMeshAgent>();
            nav.speed = 0.1f;
        }

        yield return waitTimer;

        fadePanel.gameObject.SetActive(true);

        float alpha = 0f;
        Color currentColor = fadePanel.color;

        while (!op.isDone)
        {
            if (op.progress >= 0.9f && !op.allowSceneActivation)
            {
                ShowMouse();

                while (alpha < 1f)
                {
                    alpha += fadePanelSpeed * Time.deltaTime;
                    fadePanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Clamp01(alpha));
                    yield return null;
                }

                battleText.gameObject.SetActive(false);
                animator.speed = 1f;
                movement.IsHit = false;

                StartCoroutine(FadeInCoroutine(op, turnBase));

                AudioManager.instance.Stop();
            }

            yield return null;
        }
    }

    private IEnumerator FadeInCoroutine(AsyncOperation _op, E_TurnBase turnBase)
    {
        float alpha = 1f;
        Color currentColor = fadePanel.color;

        BattleSceneChange(_op, turnBase);

        while (!_op.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        var normalZone = battleZone as NormalEnemyZone;
        var poses = normalZone.GetSpawnPos();
        foreach(var pos in poses)
        {
            pos.gameObject.SetActive(false);
        }
        
        while (alpha > 0f)
        {
            alpha -= fadePanelSpeed * Time.deltaTime;
            fadePanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Clamp01(alpha));

            yield return null;
        }

        if (alpha <= 0f)
        {
            var go = PopupManager.Instance.InstantPopUp($"Next Battle Panel");
            var panel = go.GetComponentInChildren<AlramPanel>();
            panel.SetAlramText("1 스테이지");
        }

        yield return new WaitForSeconds(0.5f);
        fadePanel.gameObject.SetActive(false);
        isBattling = false;

        AudioManager.instance.BgmPlay(enemySound);
    }
    #endregion

    #region BossBattle System
    private IEnumerator GlassBrokenEffect(E_TurnBase turnBase)
    {
        ToggleOtherZones(false);

        StartCoroutine(nameof(CaptureCoroutine));

        nextScene = $"Battle";

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        var playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerAnimator.speed = 0f;

        while (!op.isDone)
        {
            if (op.progress >= 0.9f && !op.allowSceneActivation)
            {
                glassImage.gameObject.SetActive(true);

                for (int i = 0; i < glassSprites.Length; i++)
                {
                    glassImage.sprite = glassSprites[i];
                    yield return new WaitForSeconds(glassWaitTime);

                    if ((glassSprites.Length / 2) - 2 == i)
                    {
                        playerAnimator.speed = 1f;
                        ShowMouse();
                        BattleSceneChange(op, turnBase);
                    }

                    if ((glassSprites.Length / 2) - 1 == i)
                    {
                        captureImage.gameObject.SetActive(false);
                    }
                }

                glassImage.gameObject.SetActive(false);
            }

            yield return null;
        }
    }

    private IEnumerator CaptureCoroutine()
    {
        yield return new WaitForEndOfFrame();

        captureImage.gameObject.SetActive(true);

        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = renderTexture;
        cam.Render();

        RenderTexture.active = renderTexture;
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        Sprite captureSprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f));

        captureImage.sprite = captureSprite;
    }
    #endregion

    #region Battle Start CheckZone

    public EnemyZone SetZone(EnemyZone zone)
    {
        battleZone = zone;
        return battleZone;
    }

    #endregion

    private void BattleSceneChange(AsyncOperation op, E_TurnBase isTurn)
    {
        op.allowSceneActivation = true;
        isBattle = true;
        StartCoroutine(FindBattleSystemCoroutine(isTurn));
    }

    private IEnumerator FindBattleSystemCoroutine(E_TurnBase turnType)
    {
        yield return new WaitForSeconds(0.5f);

        var battleSystem = FindObjectOfType<BattleSystem>();

        if (battleSystem == null)
        {
            Debug.LogError("BattleSystem not found.");
            yield break;
        }

        battleSystem.SetAttackTurn(turnType);

        
        yield return null;
    }

    public void BattleEndCoroutine()
    {
        StartCoroutine(nameof(BattleEnd));
    }

    private void ToggleOtherZones(bool isOn)
    {
        var allZones = FindObjectsOfType<EnemyZone>();

        foreach (var zone in allZones)
        {
            if (zone != battleZone)
            {
                zone.gameObject.SetActive(isOn);
            }
        }
    }

    public void EnemyZone(bool isOn)
    {
        var zone = transform.GetChild(0).gameObject;
        zone.SetActive(isOn);
    }

    private IEnumerator BattleEnd()
    {
        isBattle = false;
        nextScene = $"Game";

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float alpha = 0f;
        Color currentColor = fadePanel.color;

        fadePanel.gameObject.SetActive(true);

        while (!op.isDone)
        {
            if (op.progress >= 0.9f && !op.allowSceneActivation)
            {
                HideMouse();

                while (alpha < 1f)
                {
                    alpha += fadePanelSpeed * Time.deltaTime;
                    fadePanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Clamp01(alpha));
                    yield return null;
                }

                ToggleOtherZones(true);

                GameObject dummySystem = new GameObject($"Dummy System");
                var system = dummySystem.AddComponent<PlayerBattleSystem>();
                system.CharacterFieldSet();
                yield return new WaitForSeconds(1f);
                Destroy(dummySystem);

                op.allowSceneActivation = true;

                var normalZone = battleZone as NormalEnemyZone;
                var poses = normalZone.GetSpawnPos();
                foreach (var pos in poses)
                {
                    pos.gameObject.SetActive(true);
                }

                yield return new WaitForSeconds(0.5f);

                battleZone.AllEnemiesDead = true;
                battleZone.CallRespawn();

                while (alpha > 0f)
                {
                    alpha -= fadePanelSpeed * Time.deltaTime;
                    fadePanel.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Clamp01(alpha));
                    yield return null;
                }

                yield return new WaitForSeconds(0.5f);
                fadePanel.gameObject.SetActive(false);

                yield break;
            }

            yield return null;
        }
    }

    public List<GameObject> GetBattleEnemies()
    {
        NormalEnemyZone enemies = battleZone as NormalEnemyZone;
        return enemies.GetBattleEnemies();
    }

    public int GetEnemiesCount()
    {
        NormalEnemyZone enemies = battleZone as NormalEnemyZone;
        return enemies.GetEnemiesCount();
    }

    public bool SetBattle(bool isOn) => isBattle = isOn;
    public bool GetBattle() => isBattle;
}
