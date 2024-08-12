using UnityEngine;

public class FieldComponent : MonoBehaviour
{
    [SerializeField] private GameObject fieldObj;

    public void ToggleFieldObject(bool isOn)
    {
        fieldObj.SetActive(isOn);
    }
}
