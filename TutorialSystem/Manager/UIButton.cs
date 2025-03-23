using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SentryToolkit
{
    [RequireComponent(typeof(Button))]
    public class UIButton : MonoBehaviour
    {
        public ButtonID buttonID;
        GraphicRaycaster graphicRaycaster;
        Canvas btnCanvas;
        bool isFocused = false;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(Click);
        }

        void OnEnable()
        {
            UITutorialManager.OnButtonFocus += TriggerButtonFocus;
        }

        void OnDisable()
        {
            UITutorialManager.OnButtonFocus -= TriggerButtonFocus;
        }

        public void TriggerButtonFocus(ButtonID id)
        {
            if (id == buttonID && buttonID != ButtonID.None)
            {
                // Add components or adjust settings to highlight the button
                graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
                btnCanvas = GetComponent<Canvas>();
                btnCanvas.overrideSorting = true;
                btnCanvas.sortingOrder = 10;
                isFocused = true;
            }
            else if (isFocused)
            {
                // Remove focus effects safely
                if (graphicRaycaster != null)
                {
                    Destroy(graphicRaycaster);
                    graphicRaycaster = null;
                }

                if (btnCanvas != null)
                {
                    StartCoroutine(DestroyCanvasAfterFrame());
                }

                isFocused = false;
                isFocused = false;
            }
        }

        private IEnumerator DestroyCanvasAfterFrame()
        {
            yield return null; // Wait until the next frame
            if (btnCanvas != null)
            {
                Destroy(btnCanvas);
                btnCanvas = null;
            }
        }


        void Click()
        {
            if (isFocused)
            {
                FindAnyObjectByType<UITutorialManager>().OnFocusedButtonClicked(buttonID);

                // Remove GraphicRaycaster first before removing Canvas
                if (graphicRaycaster != null)
                {
                    Destroy(graphicRaycaster);
                    graphicRaycaster = null;
                }

                if (btnCanvas != null)
                {
                    StartCoroutine(DestroyCanvasAfterFrame());
                }
            }
        }

    }
}
