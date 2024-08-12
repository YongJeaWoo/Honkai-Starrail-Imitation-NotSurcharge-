using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
    public float wobbleSpeed = 1.0f;
    public float wobbleAmount = 0.1f;
    private RectTransform rectTransform;
    private Vector3 initialScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }

    void Update()
    {
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
        rectTransform.localScale = initialScale + new Vector3(wobble, wobble, 0);
    }
}
