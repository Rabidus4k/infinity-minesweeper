using UnityEngine;
using UnityEngine.UI;

public class ForceLayout : MonoBehaviour
{
    [SerializeField] private LayoutGroup layout;

    void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());
    }
}
