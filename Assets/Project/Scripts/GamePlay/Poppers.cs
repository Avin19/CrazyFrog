using UnityEngine;

public class Poppers : MonoBehaviour
{
    [SerializeField] private PopperView view;
    [SerializeField] private Projectile projectilePrefab;

    private BoardManager board;
    private int gridX, gridY;
    private PopperColor color;
    private int hitsRemaining;
    private bool isExploded = false;   // ← ADD THIS
    private Collider2D col;

    public void Initialize(BoardManager board, int x, int y, PopperColor color)
    {
        this.board = board;
        this.gridX = x;
        this.gridY = y;
        this.color = color;
        hitsRemaining = (int)color;

        if (col == null)
            col = GetComponent<Collider2D>();

        view.SetColor(color);
        isExploded = false;

        if (col != null)
            col.enabled = true;
    }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.TryUseTap()) return;
        Hit();
    }

    public void HitByProjectile()
    {
        Hit();
    }

    private void Hit()
    {
        if (isExploded) return;   // ← ignore extra hits after explosion

        hitsRemaining--;
        if (hitsRemaining > 0)
        {
            color = (PopperColor)hitsRemaining;
            view.SetColor(color);
        }
        else
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (isExploded) return;   // safe guard
        isExploded = true;

        // disable collider so projectiles cannot hit this again
        if (col != null)
            col.enabled = false;

        // play explosion puff
        view.PlayExplosionPuff(() =>
        {
            gameObject.SetActive(false);
        });

        // notify board
        board.NotifyPopperExploded(this);

        // spawn projectiles AFTER collider is disabled
        board.SpawnExplosionProjectiles(gridX, gridY, transform);
    }
}
