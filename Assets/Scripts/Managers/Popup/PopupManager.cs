using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [Header("사용할 패널들")]
    [SerializeField] private List<GameObject> popUps;
    [Header("패널이 생성될 위치")]
    [SerializeField] private Transform popUpCanvas;

    private List<GameObject> activePopUps = new List<GameObject>();

    private PanelActivate panelActive;

    protected override void DoAwake()
    {
        base.DoAwake();
        panelActive = GetComponentInChildren<PanelActivate>();
    }

    public GameObject InstantPopUp(string popUpName)
    {
        if (panelActive.GetIsFirst()) return null;

        RemovePopUp(popUpName);

        var popUpPrefab = popUps.FirstOrDefault(p => p.name == popUpName);

        if (popUpPrefab != null)
        {
            var ins = Instantiate(popUpPrefab, popUpCanvas);
            ins.name = popUpName;
            activePopUps.Add(ins);
            return ins;
        }

        return null;
    }

    public GameObject InstantPopUp(string popUpName, Transform customParent)
    {
        if (panelActive.GetIsFirst()) return null;

        RemovePopUp(popUpName);

        var popUpPrefab = popUps.FirstOrDefault(p => p.name == popUpName);

        if (popUpPrefab != null)
        {
            var ins = Instantiate(popUpPrefab, customParent);
            ins.name = popUpName;
            activePopUps.Add(ins);
            return ins;
        }

        return null;
    } 

    public bool RemovePopUp(string popUpName)
    {
        GameObject popUpRemove = activePopUps.FirstOrDefault(p => p.name == popUpName);

        if (popUpRemove != null)
        {
            activePopUps.Remove(popUpRemove);
            Destroy(popUpRemove);

            return true;
        }

        return false;
    }

    public PanelActivate GetPanelActivation() => panelActive;
}
