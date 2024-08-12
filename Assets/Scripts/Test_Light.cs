using UnityEngine;

public class Test_Light : MonoBehaviour
{
    [SerializeField] private SetPlayerSystem setPlayerSystem;
    private Vector3 originPos;
    private Quaternion originRot;
    private Light lightComponent;

    private void Start()
    {
        originPos = transform.position;
        originRot = gameObject.transform.rotation;
        lightComponent = GetComponent<Light>();
    }

    public void PlaceLight(bool isPlace)
    {
        var player = setPlayerSystem.GetPlayer();

        if (isPlace)
        {
            lightComponent.shadowStrength = 0.2f;

            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2f, player.transform.position.z + 2f);
            transform.LookAt(player.transform);
        }
        else
        {
            lightComponent.shadowStrength = 1f;
            transform.position = originPos;
            transform.rotation = originRot;
        }
    }
}
