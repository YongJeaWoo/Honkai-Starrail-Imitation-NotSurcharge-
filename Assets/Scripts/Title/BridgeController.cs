using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BridgeController : MonoBehaviour
{
    [Header("스포너 정보")]
    [SerializeField] private Transform spawner;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float spawnTime;

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
    }

    private void Update()
    {
        SpawnBridge();
        MoveCamera();
        DeleteBridge();
    }

    private void SpawnBridge()
    {
        if (gamePhase == 1)
        {
            if (canSpawnNextBridge)
            {
                canSpawnNextBridge = false;

                if (bridgesSpawned < 3)
                {
                    bridges.Add(Instantiate(firstBridgePrefab, spawner));
                    bridgesSpawned++;
                }
                else
                {
                    bridges.Add(Instantiate(secondsBridgePrefab, spawner));
                    bridgesSpawned = 0;
                }

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

                bridges.Add(Instantiate(finalBridgePrefab, spawner));

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
            //SceneManager.LoadSceneAsync("Game");
        }
    }
}