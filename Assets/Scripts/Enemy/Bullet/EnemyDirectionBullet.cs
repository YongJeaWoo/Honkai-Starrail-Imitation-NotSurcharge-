using UnityEngine;

public class EnemyDirectionBullet : DirectionBullet
{
    private EnemyZone zone;

    private void Start()
    {
        zone = transform.parent.parent.GetComponent<NormalEnemyZone>();
        BattleEntryManager.Instance.SetZone(zone);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals(targetName))
        {
            collision.gameObject.GetComponentInChildren<IHit>().Hit();
        }

        CollisionActive(collision);
    }
}
