using UnityEngine;

public class PopperView : MonoBehaviour
{
    [Header("Body Sprites")]
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private Sprite purpleSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite yellowSprite;

    [Header("Eyes")]
    [SerializeField] private Sprite leftEyeSprite;
    [SerializeField] private Sprite rightEyeSprite;
    [SerializeField] private Vector2 leftEyeLocalPos;
    [SerializeField] private Vector2 rightEyeLocalPos;

    [Header("Explosion")]
    [SerializeField] private SpriteRenderer explosionRenderer;
    [SerializeField] private Sprite explosionSprite;
    [SerializeField] private float explosionDuration = 0.25f;

    private GameObject leftEyeGO;
    private GameObject rightEyeGO;

    private void Awake()
    {
        CreateEyes();
        explosionRenderer.enabled = false;
    }

    private void CreateEyes()
    {
        // Left eye
        leftEyeGO = new GameObject("LeftEye");
        leftEyeGO.transform.SetParent(transform);
        leftEyeGO.transform.localPosition = leftEyeLocalPos;
        var leftSR = leftEyeGO.AddComponent<SpriteRenderer>();
        leftSR.sprite = leftEyeSprite;
        leftSR.sortingOrder = bodyRenderer.sortingOrder + 1;

        // Right eye
        rightEyeGO = new GameObject("RightEye");
        rightEyeGO.transform.SetParent(transform);
        rightEyeGO.transform.localPosition = rightEyeLocalPos;
        var rightSR = rightEyeGO.AddComponent<SpriteRenderer>();
        rightSR.sprite = rightEyeSprite;
        rightSR.sortingOrder = bodyRenderer.sortingOrder + 1;
    }

    public void SetColor(PopperColor color)
    {
        switch (color)
        {
            case PopperColor.Purple:
                bodyRenderer.sprite = purpleSprite;
                break;
            case PopperColor.Blue:
                bodyRenderer.sprite = blueSprite;
                break;
            case PopperColor.Yellow:
                bodyRenderer.sprite = yellowSprite;
                break;
        }
    }

    public void PlayExplosionPuff(System.Action onComplete)
    {
        StartCoroutine(ExplosionRoutine(onComplete));
    }

    private System.Collections.IEnumerator ExplosionRoutine(System.Action onComplete)
    {
        bodyRenderer.enabled = false;
        leftEyeGO.SetActive(false);
        rightEyeGO.SetActive(false);

        explosionRenderer.sprite = explosionSprite;
        explosionRenderer.enabled = true;

        yield return new WaitForSeconds(explosionDuration);

        explosionRenderer.enabled = false;
        onComplete?.Invoke();
    }
}
