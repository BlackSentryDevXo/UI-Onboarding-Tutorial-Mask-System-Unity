using UnityEngine;
using TMPro;

namespace SentryToolkit{
    public class TooltipUI : MonoBehaviour
    {
        public RectTransform arrowImage;
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private RectTransform backgroundRectTransform;

        private void Awake()
        {
            HideTooltip();
        }

        public void SetText(string tooltipText)
        {
            textMeshPro.SetText(tooltipText);
            textMeshPro.ForceMeshUpdate(); // Force text update to get correct size
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }
    }
}