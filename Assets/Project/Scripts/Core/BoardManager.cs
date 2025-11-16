using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Poppers popperPrefab;
    [SerializeField] private float cellSize = 1.0f;
    [SerializeField] private Vector2 boardOrigin = Vector2.zero;

    private Poppers[,] poppers;
    private int width, height;
    private int aliveCount;

    public void BuildBoard(LevelDefinition level)
    {
        width = level.width;
        height = level.height;
        poppers = new Poppers[width, height];
        aliveCount = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = x + y * width;
                PopperColor color = level.grid[index];
                if (color == PopperColor.None) continue;

                Vector3 pos = boardOrigin + new Vector2(x * cellSize, y * cellSize);
                Poppers popper = Instantiate(popperPrefab, pos, Quaternion.identity, transform);
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

    public void NotifyPopperExploded(Poppers popper)
    {
        aliveCount--;
        if (aliveCount <= 0)
        {
            GameManager.Instance.OnBoardCleared();
        }
    }

    public bool AllPoppersCleared() => aliveCount <= 0;

    public Poppers GetPopperAt(int x, int y)
    {
        if (poppers == null) return null;
        if (x < 0 || y < 0 || x >= width || y >= height) return null;
        return poppers[x, y];
    }

    public void SpawnExplosionProjectiles(int gridX, int gridY, Transform origin)
    {
        Spawn(origin.position, Vector2.up);
        Spawn(origin.position, Vector2.down);
        Spawn(origin.position, Vector2.left);
        Spawn(origin.position, Vector2.right);

        void Spawn(Vector3 pos, Vector2 dir)
        {
            ProjectilePool.Instance.SpawnProjectile(pos, dir);
        }
    }
}
