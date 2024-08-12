using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    private bool isfirstEvent;
    
    public bool SetFirstEvent(bool value) => isfirstEvent = value;
    public bool GetFirstEvent() => isfirstEvent;
}
