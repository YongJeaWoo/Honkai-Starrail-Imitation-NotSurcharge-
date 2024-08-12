using Cinemachine;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameInputSystem : MonoBehaviour
{
    private CinemachineFreeLook vCamera;

    private float originX;
    private float originY;

    private void Awake()
    {
        InitValues();
    }

    private void Update()
    {
        InputKey();   
    }

    private void InitValues()
    {
        PopupManager.Instance.HideMouse();

        originX = 600f;
        originY = 2.5f;

        vCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            PopupManager.Instance.ShowMouse();
            SetFreeCamera(0, 0);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            PopupManager.Instance.HideMouse();
            SetFreeCamera(originX, originY);
        }
    }

    public void SetFreeCamera(float xValue, float yValue)
    {
        vCamera.m_XAxis.m_MaxSpeed = xValue;
        vCamera.m_YAxis.m_MaxSpeed = yValue;
    }

    public float GetXValue() => originX;
    public float GetYValue() => originY;
}
