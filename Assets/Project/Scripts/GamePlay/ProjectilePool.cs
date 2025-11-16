using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int initialSize = 20;

    private readonly Queue<Projectile> pool = new Queue<Projectile>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Pre-warm
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewProjectile();
        }
    }

    private Projectile CreateNewProjectile()
    {
        Projectile proj = Instantiate(projectilePrefab, transform);
        proj.gameObject.SetActive(false);
        pool.Enqueue(proj);
        return proj;
    }

    public Projectile SpawnProjectile(Vector3 position, Vector2 direction)
    {
        if (pool.Count == 0)
        {
            CreateNewProjectile();
        }

        Projectile proj = pool.Dequeue();
        proj.transform.position = position;
        proj.transform.rotation = Quaternion.identity;
        proj.gameObject.SetActive(true);

        proj.Initialize(direction);         // will also RegisterProjectile in GameManager
        return proj;
    }

    public void ReleaseProjectile(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        pool.Enqueue(proj);

        GameManager.Instance.UnregisterProjectile();
    }
}
