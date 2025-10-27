using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoTiledSprite : MonoBehaviour
{
    [SerializeField] private float padding = 2f;
    [SerializeField] private Vector2 parallax = Vector2.one;
    [SerializeField] private Vector2 offset;

    private Camera cam;
    private SpriteRenderer sr;
    private Vector2 tileSize;
    private Material mat;
    private static string texProp = "_MainTex";

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (!cam) cam = Camera.main;

        sr.drawMode = SpriteDrawMode.Tiled;
        transform.localScale = Vector3.one;
        tileSize = sr.sprite.bounds.size;

        mat = sr.material;
    }

    private void LateUpdate()
    {
        // Размер текущего экрана в юнитах
        float h = cam.orthographicSize * 2f;
        float w = h * cam.aspect;

        // Сколько тайлов нужно, чтобы покрыть экран + запас СПРАВА и СНИЗУ
        int tilesX = Mathf.CeilToInt((w + padding) / tileSize.x);
        int tilesY = Mathf.CeilToInt((h + padding) / tileSize.y);

        // Итоговый размер полотна
        Vector2 areaSize = new Vector2(tilesX * tileSize.x, tilesY * tileSize.y);
        sr.size = areaSize;

        // ---- ПИВОТ СЛЕВА-СВЕРХУ ----
        // Координата левого-верхнего угла в мире для текущей камеры
        Vector3 camPos = cam.transform.position;
        Vector2 topLeft = new Vector2(
            camPos.x - w * 0.5f,
            camPos.y + h * 0.5f
        );

        // Так как SpriteRenderer позиционируется по ЦЕНТРУ области,
        // смещаем трансформ к центру из левого-верхнего угла.
        Vector3 centerFromTopLeft = new Vector3(areaSize.x * 0.5f, -areaSize.y * 0.5f, 0f);
        transform.position = new Vector3(
            topLeft.x + centerFromTopLeft.x,
            topLeft.y + centerFromTopLeft.y,
            transform.position.z
        );

        // ---- ОФФСЕТ МАТЕРИАЛА ----
        // Мировой сдвиг worldOffset применяем как фазу (в юнитах).
        // Pinned-точка для узора = левый-верхний угол минус мировой оффсет.
        Vector2 pinnedPoint = topLeft - offset;

        Vector2 worldToUV = new Vector2(1f / tileSize.x, 1f / tileSize.y);
        Vector2 uvOffset = new Vector2(
            Mathf.Repeat(-pinnedPoint.x * worldToUV.x * parallax.x, 1f),
            Mathf.Repeat(-pinnedPoint.y * worldToUV.y * parallax.y, 1f)
        );

        if (mat && mat.HasProperty(texProp))
            mat.SetTextureOffset(texProp, uvOffset);
    }
}
