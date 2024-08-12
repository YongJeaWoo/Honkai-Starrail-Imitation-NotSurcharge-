using UnityEngine;

public class DirectionBullet : Bullet
{
    [SerializeField] protected string targetName;
    [SerializeField] private float moveSpeed = 10f;

    private bool startCheck = false;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (!startCheck)
        {
            if (flash != null)
            {
                flash.transform.parent = null;
            }
        }

        if (notDestroy)
        {
            StartCoroutine(DisableTimer(5));
        }
        else
        {
            Destroy(gameObject, 5);
        }

        startCheck = true;
    }

    private void OnEnable()
    {
        if (startCheck)
        {
            if (flash != null)
            {
                flash.transform.parent = null;
                }

            if (lightSource != null)
            {
                lightSource.enabled = true;
            }

            col.enabled = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (moveSpeed != 0)
        {
            rb.velocity = transform.forward * moveSpeed;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals(targetName))
        {
            collision.gameObject.GetComponent<IHit>().Hit();
        }

        CollisionActive(collision);
    }
}
