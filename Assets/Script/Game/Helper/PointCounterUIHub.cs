using UnityEngine;
using TMPro;

public class PointUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointText;

    private void Start()
    {
        if (PointCounter.instance != null)
        {
            PointCounter.instance.OnPointChanged += UpdatePointText;
            UpdatePointText();
        }
        else
        {
            Debug.LogWarning("PointCounter instance not found!");
        }
    }

    private void OnDestroy()
    {
        if (PointCounter.instance != null)
        {
            PointCounter.instance.OnPointChanged -= UpdatePointText;
        }
    }

    private void UpdatePointText()
    {
        if (pointText == null)
            return;

        pointText.text = $"Score: {PointCounter.instance.point}";
    }
}