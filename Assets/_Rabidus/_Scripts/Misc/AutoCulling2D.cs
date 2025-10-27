using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoCulling2D : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    void OnBecameVisible()
    {
        spriteRenderer.enabled = true;
    }

    void OnBecameInvisible()
    {
        spriteRenderer.enabled = false;
    }
}
