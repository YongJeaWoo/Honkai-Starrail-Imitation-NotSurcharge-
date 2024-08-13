using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetUISystem : MonoBehaviour
{
    // 적 UI 프리팹 참조
    [SerializeField] private GameObject enemyCycleUI;
    // 플레이어 UI 프리팹 참조
    [SerializeField] private GameObject playerCycleUI;
    // UI가 속한 캔버스의 RectTransform
    [SerializeField] private RectTransform canvasRectTransform;
    // UI가 보여질 카메라 참조
    [SerializeField] private Camera uiCamera;

    // 복제된 적 UI 오브젝트 리스트
    private List<GameObject> enemyCycleUIs = new List<GameObject>();
    // 복제된 플레이어 UI 오브젝트 리스트
    private List<GameObject> playerCycleUIs = new List<GameObject>();

    // 적 UI 초기화 메서드
    public void InitializeEnemyUI(int count)
    {
        // 기존의 모든 적 UI 오브젝트 제거
        ClearUIs(enemyCycleUIs);
        // 필요한 개수만큼 적 UI 오브젝트 생성
        for (int i = 0; i < count; i++)
        {
            GameObject uiClone = Instantiate(enemyCycleUI, canvasRectTransform);
            enemyCycleUIs.Add(uiClone);

            Debug.Log(count);
        }
    }

    // 플레이어 UI 초기화 메서드
    public void InitializePlayerUI(int count)
    {
        // 기존의 모든 플레이어 UI 오브젝트 제거
        ClearUIs(playerCycleUIs);
        // 필요한 개수만큼 플레이어 UI 오브젝트 생성
        for (int i = 0; i < count; i++)
        {
            GameObject uiClone = Instantiate(playerCycleUI, canvasRectTransform);
            playerCycleUIs.Add(uiClone);
        }
    }

    // UI 리스트를 비우고 오브젝트 제거
    private void ClearUIs(List<GameObject> uiList)
    {
        // 리스트의 모든 UI 오브젝트를 제거
        foreach (var ui in uiList)
        {
            Destroy(ui);
        }
        // 리스트 초기화
        uiList.Clear();
    }

    // 적 타겟팅 UI 설정 메서드
    public void EnemyTargeting(BattleBehaviourComponent target, int index)
    {
        // 새로운 타겟 설정
        if (index >= 0 && index < enemyCycleUIs.Count)
        {
            // 모든 적 UI 비활성화
            for (int i = 0; i < enemyCycleUIs.Count; i++)
            {
                enemyCycleUIs[i].SetActive(i == index);
            }

            SetTargetingUI(target, enemyCycleUIs[index]);
        }
    }

    // 플레이어 타겟팅 UI 설정 메서드
    public void PlayerTargeting(BattleBehaviourComponent target, int index)
    {
        // 새로운 타겟 설정
        if (index >= 0 && index < playerCycleUIs.Count)
        {
            // 모든 플레이어 UI 비활성화
            for (int i = 0; i < playerCycleUIs.Count; i++)
            {
                playerCycleUIs[i].SetActive(i == index);
            }

            SetTargetingUI(target, playerCycleUIs[index]);
        }
    }

    // 타겟팅 UI 설정 공통 메서드
    private void SetTargetingUI(BattleBehaviourComponent target, GameObject cycleUI)
    {
        // UI 카메라가 null이면 반환
        if (uiCamera == null) return;

        // 타겟 위치를 화면 좌표로 변환
        Vector3 targetPosition = target.GetTargetTransform();
        Vector3 screenPosition = uiCamera.WorldToScreenPoint(targetPosition);

        // 화면 좌표를 캔버스 좌표로 변환
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, uiCamera, out Vector2 canvasPosition))
        {
            // 변환된 캔버스 좌표를 UI의 앵커 위치로 설정
            RectTransform cycleRectTransform = cycleUI.GetComponent<RectTransform>();
            cycleRectTransform.anchoredPosition = canvasPosition;

            // UI 활성화
            cycleUI.SetActive(true);
        }
    }

    // 모든 적 UI 활성화/비활성화 메서드
    public void EnemyAllCycleUI(bool isOn)
    {
        // 리스트의 모든 적 UI 오브젝트를 설정된 상태로 변경
        foreach (var ui in enemyCycleUIs)
        {
            ui.SetActive(isOn);
        }
    }

    // 모든 플레이어 UI 활성화/비활성화 메서드
    public void PlayerAllCycleUI(bool isOn)
    {
        // 리스트의 모든 플레이어 UI 오브젝트를 설정된 상태로 변경
        foreach (var ui in playerCycleUIs)
        {
            ui.SetActive(isOn);
        }
    }

    public void ShowAliveEnemyUIs(List<BattleBehaviourComponent> enemies)
    {
        ClearUIs(enemyCycleUIs);
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                GameObject uiClone = Instantiate(enemyCycleUI, canvasRectTransform);
                enemyCycleUIs.Add(uiClone);
                SetTargetingUI(enemy, uiClone);
                uiClone.SetActive(true);
            }
        }
    }

    public void ShowAlivePlayerUIs(List<BattleBehaviourComponent> players)
    {
        ClearUIs(playerCycleUIs);
        foreach (var player in players)
        {
            if (player.IsAlive)
            {
                GameObject uiClone = Instantiate(playerCycleUI, canvasRectTransform);
                playerCycleUIs.Add(uiClone);
                SetTargetingUI(player, uiClone);
                uiClone.SetActive(true);
            }
        }
    }

}
