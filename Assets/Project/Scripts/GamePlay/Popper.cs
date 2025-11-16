using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Popper : MonoBehaviour
{
    [SerializeField] private PopperView view;
    [SerializeField] private Projectile projectilePrefab;

    private BoardManager board;
    private int gridX, gridY;
    private PopperColor color;
    private int hitsRemaining;

    public void Initialize(BoardManager board, int x, int y, PopperColor color)
    {
        this.board = board;
        this.gridX = x;
        this.gridY = y;
        SetColor(color);
    }

    private void SetColor(PopperColor popColor)
    {
        color = popColor;
        hitsRemaining = (int)color; // Purple=1, Blue=2, Yellow=3
        view.SetColor(color);
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.TryUseTap()) return;
        Hit();
        AudioManager.Instance.PlayPop();
        CheckForLoseCondition();
    }

    public void HitByProjectile()
    {
        Hit();
    }

    private void Hit()
    {
        hitsRemaining--;
        if (hitsRemaining > 0)
        {
            // Yellow -> Blue -> Purple
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
        view.PlayExplosionPuff(() =>
        {
            // after puff you can disable sprite
            gameObject.SetActive(false);
        });

        board.NotifyPopperExploded(this);
        board.SpawnExplosionProjectiles(gridX, gridY, transform, projectilePrefab);
    }

    private void CheckForLoseCondition()
    {
        // user used a tap; if taps reach zero and board not cleared, we *might* lose.
        if (GameManager.Instance.RemainingTaps == 0)
        {
            // delay a bit in real game to allow chain reaction to finish (use coroutine).
            GameManager.Instance.OnNoMovesLeft();
        }
    }
}
