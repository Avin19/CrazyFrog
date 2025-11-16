using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Popper popperPrefab;
    [SerializeField] private float cellSize = 1.0f;
    [SerializeField] private Vector2 boardOrigin = Vector2.zero;

    private Popper[,] poppers;
    private int width, height;
    private int aliveCount;

    public void BuildBoard(LevelDefinition level)
    {
        width = level.width;
        height = level.height;
        poppers = new Popper[width, height];
        aliveCount = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = x + y * width;
                PopperColor color = level.grid[index];
                if (color == PopperColor.None) continue;

                Vector3 pos = boardOrigin + new Vector2(x * cellSize, y * cellSize);
                Popper popper = Instantiate(popperPrefab, pos, Quaternion.identity, transform);
                popper.Initialize(this, x, y, color);
                poppers[x, y] = popper;
                aliveCount++;
            }
        }
    }

    public void ClearBoard()
    {
        if (poppers == null) return;
        foreach (var p in poppers)
        {
            if (p != null) Destroy(p.gameObject);
        }
        poppers = null;
    }

    public void NotifyPopperExploded(Popper popper)
    {
        aliveCount--;
        if (aliveCount <= 0)
        {
            GameManager.Instance.OnBoardCleared();
        }
    }

    public bool AllPoppersCleared() => aliveCount <= 0;

    public Popper GetPopperAt(int x, int y)
    {
        if (poppers == null) return null;
        if (x < 0 || y < 0 || x >= width || y >= height) return null;
        return poppers[x, y];
    }

    public void SpawnExplosionProjectiles(int gridX, int gridY, Transform origin, Projectile projectilePrefab)
    {
        // 4 directions
        SpawnProjectile(origin.position, Vector2.up);
        SpawnProjectile(origin.position, Vector2.down);
        SpawnProjectile(origin.position, Vector2.left);
        SpawnProjectile(origin.position, Vector2.right);

        void SpawnProjectile(Vector3 pos, Vector2 dir)
        {
            Projectile proj = Instantiate(projectilePrefab, pos, Quaternion.identity);
            proj.Initialize(dir);
        }
    }
}
