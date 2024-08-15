using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BridgeController : MonoBehaviour
{
    [Header("스포너 정보")]
    [SerializeField] private Transform spawner;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float spawnTime;

    [Header("타이틀 텍스트")]
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("첫번째 프리팹")]
    [SerializeField] private GameObject firstBridgePrefab;
    [Header("두번째 프리팹")]
    [SerializeField] private GameObject secondsBridgePrefab;
    [Header("마지막 프리팹")]
    [SerializeField] private GameObject finalBridgePrefab;
    
    [Header("다리 리스트 및 레이어")]
    [SerializeField] private List<GameObject> bridges = new List<GameObject>();

    [Header("메인 카메라")]
    [SerializeField] private Transform mainCamera;

    [Header("페이드 아웃 패널")]
    [SerializeField] private Image fadePanel;

    public int gamePhase = 1;
    private int bridgesSpawned;
    private bool canSpawnNextBridge;

    // phase 1
    private float cameraSpeed;
    private Vector3 cameraPosition;

    private Ray cameraRay;
    private RaycastHit cameraRayInfo;
    private float rayDistance = 5f;
    [SerializeField] private LayerMask bridgeLayer;

    // phase 2
    private Vector3 finalBridgeCamPosition;
    [Header("마지막 카메라 무빙")]
    [Range(0.001f, 1f)]
    [SerializeField] private float cameraDecelerationSpeed = 0.5f;

    private bool spawnedLastBridge;

    // phase 3
    private Vector3 entryCamPosition;

    private void Start()
    {
        canSpawnNextBridge = true;
        cameraPosition = Vector3.zero;
        cameraSpeed = spawnOffset.z / spawnTime;

        fadePanel.color = new Color(0, 0, 0, 0);
    }

    private void OnEnable()
    {
        gamePhase = 1;
        bridgesSpawned = 0;
        spawnedLastBridge = false;
        canSpawnNextBridge = true;
    }

    private void Update()
    {
        SpawnBridge();
        MoveCamera();
        DeleteBridge();
    }

    private void LateUpdate()
    {
        TitleTextInfomation();
    }

    private void SpawnBridge()
    {
        if (gamePhase == 1)
        {
            if (canSpawnNextBridge)
            {
                canSpawnNextBridge = false;

                GameObject newBridge;
                if (bridgesSpawned < 3)
                {
                    newBridge = Instantiate(firstBridgePrefab, spawner);
                    bridgesSpawned++;
                }
                else
                {
                    newBridge = Instantiate(secondsBridgePrefab, spawner);
                    bridgesSpawned = 0;
                }

                newBridge.name = newBridge.name.Replace("(Clone)", "");

                bridges.Add(newBridge);

                foreach (GameObject bridge in bridges)
                {
                    bridge.transform.parent = null;
                }

                spawner.position += spawnOffset;
                StartCoroutine(WaitForTime());
            }
        }
        else if (gamePhase == 2)
        {
            if (!spawnedLastBridge)
            {
                spawnedLastBridge = true;

                GameObject finalBridge = Instantiate(finalBridgePrefab, spawner);
                finalBridge.name = finalBridge.name.Replace("(Clone)", ""); 

                bridges.Add(finalBridge);

                foreach (GameObject bridge in bridges)
                {
                    bridge.transform.parent = null;
                }

                finalBridgeCamPosition = GameObject.Find("FinalBridgeCamPosition").transform.position;
                entryCamPosition = GameObject.Find("EntryCamPosition").transform.position;
            }
        }
    }

    private IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(spawnTime);

        canSpawnNextBridge = true;
    }

    private void MoveCamera()
    {
        if (gamePhase == 1)
        {
            cameraPosition.z = cameraSpeed * Time.deltaTime;
            mainCamera.transform.position += cameraPosition;
        }
        else if (gamePhase == 2)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.position, finalBridgeCamPosition, cameraDecelerationSpeed * Time.deltaTime);
        }
        else if (gamePhase == 3)
        {
            float moveSpeed = 1.2f;

            mainCamera.transform.position = Vector3.Lerp(mainCamera.position, entryCamPosition, cameraDecelerationSpeed * Time.deltaTime * moveSpeed);
        }
    }

    private void TitleTextInfomation()
    {
        switch (gamePhase)
        {
            case 1:
                infoText.text = $"아무 키를 눌러 게임을 실행하세요.";
                break;
            case 2:
                infoText.text = $"게임 시작";
                break;
            case 3:
                infoText.text = string.Empty;
                infoText.gameObject.SetActive(false);
                break;
        }
    }

    private void DeleteBridge()
    {
        cameraRay.origin = mainCamera.transform.position;
        cameraRay.direction = -mainCamera.transform.up;

        if (Physics.Raycast(cameraRay.origin, cameraRay.direction, out cameraRayInfo, rayDistance, bridgeLayer))
        {
            if (bridges.Contains(cameraRayInfo.collider.gameObject))
            {
                if (bridges.IndexOf(cameraRayInfo.collider.gameObject) > 0)
                {
                    Destroy(bridges[0]);
                    bridges.RemoveAt(0);
                }
            }
        }
    }

    public void NextPhase()
    {
        gamePhase++;

        if (gamePhase >= 3)
        {
            StartCoroutine(FadeOutAndStartGame());
        }
    }

    private IEnumerator FadeOutAndStartGame()
    {
        GameObject finalBridge = null;

        foreach (GameObject bridge in bridges)
        {
            if (bridge.name == finalBridgePrefab.name)
            {
                finalBridge = bridge;
                break;
            }
        }

        if (finalBridge != null)
        {
            var door = finalBridge.transform.GetChild(0).GetChild(0);
            if (door != null)
            {
                var doorAnimator = door.GetComponent<Animator>();
                if (doorAnimator != null)
                {
                    AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
                    if (stateInfo.IsName("None"))
                    {
                        doorAnimator.SetTrigger("Open");

                        while (!stateInfo.IsName("Open"))
                        {
                            stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
                            yield return null; 
                        }

                        AnimatorClipInfo[] clipInfoArray = doorAnimator.GetCurrentAnimatorClipInfo(0);
                        if (clipInfoArray.Length > 0)
                        {
                            AnimatorClipInfo clipInfo = clipInfoArray[0];
                            float doorAnimationLength = clipInfo.clip.length;

                            float fadeStartTime = doorAnimationLength - 0.2f;
                            fadeStartTime = Mathf.Max(0, fadeStartTime);

                            yield return new WaitForSeconds(fadeStartTime);

                            float fadeDuration = 1f;
                            float elapsedTime = 0f;

                            Color panelColor = fadePanel.color;

                            while (elapsedTime < fadeDuration)
                            {
                                elapsedTime += Time.deltaTime;
                                panelColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                                fadePanel.color = panelColor;
                                yield return null;
                            }

                            StartGame();
                        }
                        else
                        {
                            Debug.LogError("No animation clip info found for the door.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Door is not in the expected 'None' state.");
                    }
                }
            }
        }
    }

    private void StartGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}