using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 1, 0);
    private Vector3 randomIntensity = new Vector3(.8f, 0, 0);
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
        RandomPos();
    }

    public void RandomPos()
    {
        Vector3 newPos = initialPosition + offset;
        newPos += new Vector3(Random.Range(-randomIntensity.x, randomIntensity.x),
            Random.Range(-randomIntensity.y, randomIntensity.y),
            Random.Range(-randomIntensity.z, randomIntensity.z));

        transform.localPosition = newPos;
    }
}
