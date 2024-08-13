using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetUISystem : MonoBehaviour
{
    // �� UI ������ ����
    [SerializeField] private GameObject enemyCycleUI;
    // �÷��̾� UI ������ ����
    [SerializeField] private GameObject playerCycleUI;
    // UI�� ���� ĵ������ RectTransform
    [SerializeField] private RectTransform canvasRectTransform;
    // UI�� ������ ī�޶� ����
    [SerializeField] private Camera uiCamera;

    // ������ �� UI ������Ʈ ����Ʈ
    private List<GameObject> enemyCycleUIs = new List<GameObject>();
    // ������ �÷��̾� UI ������Ʈ ����Ʈ
    private List<GameObject> playerCycleUIs = new List<GameObject>();

    // �� UI �ʱ�ȭ �޼���
    public void InitializeEnemyUI(int count)
    {
        // ������ ��� �� UI ������Ʈ ����
        ClearUIs(enemyCycleUIs);
        // �ʿ��� ������ŭ �� UI ������Ʈ ����
        for (int i = 0; i < count; i++)
        {
            GameObject uiClone = Instantiate(enemyCycleUI, canvasRectTransform);
            enemyCycleUIs.Add(uiClone);

            Debug.Log(count);
        }
    }

    // �÷��̾� UI �ʱ�ȭ �޼���
    public void InitializePlayerUI(int count)
    {
        // ������ ��� �÷��̾� UI ������Ʈ ����
        ClearUIs(playerCycleUIs);
        // �ʿ��� ������ŭ �÷��̾� UI ������Ʈ ����
        for (int i = 0; i < count; i++)
        {
            GameObject uiClone = Instantiate(playerCycleUI, canvasRectTransform);
            playerCycleUIs.Add(uiClone);
        }
    }

    // UI ����Ʈ�� ���� ������Ʈ ����
    private void ClearUIs(List<GameObject> uiList)
    {
        // ����Ʈ�� ��� UI ������Ʈ�� ����
        foreach (var ui in uiList)
        {
            Destroy(ui);
        }
        // ����Ʈ �ʱ�ȭ
        uiList.Clear();
    }

    // �� Ÿ���� UI ���� �޼���
    public void EnemyTargeting(BattleBehaviourComponent target, int index)
    {
        // ���ο� Ÿ�� ����
        if (index >= 0 && index < enemyCycleUIs.Count)
        {
            // ��� �� UI ��Ȱ��ȭ
            for (int i = 0; i < enemyCycleUIs.Count; i++)
            {
                enemyCycleUIs[i].SetActive(i == index);
            }

            SetTargetingUI(target, enemyCycleUIs[index]);
        }
    }

    // �÷��̾� Ÿ���� UI ���� �޼���
    public void PlayerTargeting(BattleBehaviourComponent target, int index)
    {
        // ���ο� Ÿ�� ����
        if (index >= 0 && index < playerCycleUIs.Count)
        {
            // ��� �÷��̾� UI ��Ȱ��ȭ
            for (int i = 0; i < playerCycleUIs.Count; i++)
            {
                playerCycleUIs[i].SetActive(i == index);
            }

            SetTargetingUI(target, playerCycleUIs[index]);
        }
    }

    // Ÿ���� UI ���� ���� �޼���
    private void SetTargetingUI(BattleBehaviourComponent target, GameObject cycleUI)
    {
        // UI ī�޶� null�̸� ��ȯ
        if (uiCamera == null) return;

        // Ÿ�� ��ġ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 targetPosition = target.GetTargetTransform();
        Vector3 screenPosition = uiCamera.WorldToScreenPoint(targetPosition);

        // ȭ�� ��ǥ�� ĵ���� ��ǥ�� ��ȯ
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, uiCamera, out Vector2 canvasPosition))
        {
            // ��ȯ�� ĵ���� ��ǥ�� UI�� ��Ŀ ��ġ�� ����
            RectTransform cycleRectTransform = cycleUI.GetComponent<RectTransform>();
            cycleRectTransform.anchoredPosition = canvasPosition;

            // UI Ȱ��ȭ
            cycleUI.SetActive(true);
        }
    }

    // ��� �� UI Ȱ��ȭ/��Ȱ��ȭ �޼���
    public void EnemyAllCycleUI(bool isOn)
    {
        // ����Ʈ�� ��� �� UI ������Ʈ�� ������ ���·� ����
        foreach (var ui in enemyCycleUIs)
        {
            ui.SetActive(isOn);
        }
    }

    // ��� �÷��̾� UI Ȱ��ȭ/��Ȱ��ȭ �޼���
    public void PlayerAllCycleUI(bool isOn)
    {
        // ����Ʈ�� ��� �÷��̾� UI ������Ʈ�� ������ ���·� ����
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
