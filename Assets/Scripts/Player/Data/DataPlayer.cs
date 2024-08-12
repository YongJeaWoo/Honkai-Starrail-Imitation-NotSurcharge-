using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    [SerializeField] private CharacterStateSOJ sojData;

    public CharacterStateSOJ GetSOJData() => sojData;
}
