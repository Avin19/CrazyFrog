using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        GameManager.Instance.RegisterProjectile();
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Simple off-screen release
        if (Mathf.Abs(transform.position.x) > 20f ||
            Mathf.Abs(transform.position.y) > 20f)
        {
            Release();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var popper = other.GetComponent<Poppers>();
        if (popper != null)
        {
            Debug.Log("Projectile hit: " + other.name);
            popper.HitByProjectile();
            Destroy(gameObject);   // or Release() if pooling
        }
    }

    private void Release()
    {
        // Return to pool instead of Destroy
        ProjectilePool.Instance.ReleaseProjectile(this);
    }
}
