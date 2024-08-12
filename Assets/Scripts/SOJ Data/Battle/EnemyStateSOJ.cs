using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy State", menuName = "ENE State / Enemy State")]
public class EnemyStateSOJ : ScriptableObject
{
    [SerializeField] protected int hp;
    [SerializeField] protected float maxhp;
    [SerializeField] protected int damage;
    [SerializeField] protected int acting;

    public int Hp { get => hp; set => hp = value; }
    public float Maxhp { get => maxhp; set => maxhp = value; }
    public int Damage { get => damage; set => damage = value; }
    public int Acting { get => acting; set => acting = value; }
}
