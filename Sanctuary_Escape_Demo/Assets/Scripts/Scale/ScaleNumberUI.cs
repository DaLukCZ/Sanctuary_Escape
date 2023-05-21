using UnityEngine;
using TMPro;

public class ScaleNumberUI : MonoBehaviour
{
    private TextMeshPro scaleText;

    private void Awake()
    {
        scaleText = GetComponent<TextMeshPro>();
    }

    public void UpdateScaleNumber(float scale)
    {
        scaleText.text = scale.ToString("F2");
    }
}
