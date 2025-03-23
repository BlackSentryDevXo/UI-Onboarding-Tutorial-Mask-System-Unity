using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace SentryToolkit
{
    public class UITutorialManager : MonoBehaviour
    {
        public GameObject overlayPanel; // The overlay panel to dim the screen and focus on the button
        public GameObject tooltipPrefab; // Prefab for the tooltip
        public List<TutorialSequence> TutorialSequences = new List<TutorialSequence>();
        public static Action<ButtonID> OnButtonFocus = delegate { };

        private bool isTutorialActive = false; // Whether the tutorial is currently active
        private ButtonID currentlyFocusedButtonID; // The ID of the button currently in focus
        private Action currentlyFocusedButtonCallback; // Callback for the currently focused button
        private GameObject currentTooltip; // The currently active tooltip
        private TooltipUI tooltip;
        private Button overlayButton;

        [Header("Debug")]
        public SequenceID testSequenceID;
        public ButtonID testButtonID;

        public int currentStepIndex = 0;
        private TutorialSequence currentSequence;
        private Dictionary<ButtonID, UIButton> buttonDictionary = new Dictionary<ButtonID, UIButton>();
        private Dictionary<Button, UnityEngine.Events.UnityAction> tutorialClickActions = new();

        void Awake()
        {
            CacheAllButtons();
        }

        private void CacheAllButtons()
        {
            buttonDictionary.Clear();

            UIButton[] allButtons = FindObjectsByType<UIButton>(FindObjectsSortMode.None);

            foreach (UIButton button in allButtons)
            {
                if (!buttonDictionary.ContainsKey(button.buttonID))
                {
                    buttonDictionary[button.buttonID] = button;
                }
            }
            ResetButtonAction();
        }

        [Button]
        public void DebugTutorialSequence()
        {
            StartTutorial(testSequenceID);
        }

        [Button]
        public void DebugFocusOnButton()
        {
            FocusOnButton(testButtonID, "This is a debug tooltip!");
        }

        // Start a tutorial sequence
        public void StartTutorial(SequenceID sequenceID)
        {
            TutorialSequence sequence = TutorialSequences.Find(seq => seq.sequenceName == sequenceID);
            if (sequence == null || sequence.steps.Count == 0)
            {
                Debug.LogWarning("Tutorial sequence is empty or null.");
                return;
            }

            isTutorialActive = true;
            currentSequence = sequence;
            currentStepIndex = 0;
            FocusOnNextButton();
        }

        public void FocusOnNextButton()
        {
            if (!isTutorialActive || currentStepIndex >= currentSequence.steps.Count)
            {
                EndTutorial();
                return;
            }

            var step = currentSequence.steps[currentStepIndex];

            // Enable overlay panel ad add a button to it for skipping panels without buttons
            if (overlayPanel != null)
            {
                overlayPanel.SetActive(true);
                if (overlayPanel.GetComponent<Button>() == null)
                {
                    overlayButton = overlayPanel.AddComponent<Button>();
                }
                else
                {
                    overlayButton = overlayPanel.GetComponent<Button>();
                }
                overlayButton.interactable = false;
            }

            // Find target button
            if (step.focusButtonID != ButtonID.None)
            {
                UIButton targetButton = FindButton(step.focusButtonID);
                if (targetButton == null)
                {
                    Debug.LogError($"Button with ID {step.focusButtonID} not found!");
                    return;
                }

                // Activate parent if it's inactive
                if (!targetButton.gameObject.activeInHierarchy)
                {
                    Transform parent = targetButton.transform.parent;
                    while (parent != null)
                    {
                        if (!parent.gameObject.activeSelf)
                        {
                            parent.gameObject.SetActive(true);
                        }
                        parent = parent.parent;
                    }
                }

                // Highlight button
                OnButtonFocus?.Invoke(step.focusButtonID);

                // Get the Button component and remove any previous listeners to avoid stacking events
                Button buttonComponent = targetButton.GetComponent<Button>();
                if (tutorialClickActions.TryGetValue(buttonComponent, out var existingAction))
                {
                    buttonComponent.onClick.RemoveListener(existingAction);
                    tutorialClickActions.Remove(buttonComponent);
                }

                // Create a new action for this step
                UnityEngine.Events.UnityAction clickAction = () => OnStepCompleted(step);

                // Add and store it
                buttonComponent.onClick.AddListener(clickAction);
                tutorialClickActions[buttonComponent] = clickAction;
            }
            else
            {
                // Move to next step when we tap on the overlay panel
                if (overlayButton)
                {
                    overlayButton.interactable = true;
                    overlayButton.onClick.RemoveAllListeners(); // Add this before adding new one
                    overlayButton.onClick.AddListener(() => OnStepCompleted(step));
                }
            }

            // Show tooltip
            ShowTooltip(step.focusButtonID, step.message);
        }

        // Called when a tutorial step is completed
        private void OnStepCompleted(UITutorialStep step)
        {
            step.OnStepCompleted?.Invoke(); // Call any extra actions
            currentStepIndex++;
            FocusOnNextButton();
        }

        // End the tutorial
        public void EndTutorial()
        {
            isTutorialActive = false;
            if (overlayPanel != null) overlayPanel.SetActive(false);
            Debug.Log("Tutorial Completed.");
            ResetButtonAction();
        }

        // Focus on a specific button by its ButtonID and provide a callback for when it's clicked
        public void FocusOnButton(ButtonID buttonID, string tooltipText = "", Action OnFocusedButtonClicked = null)
        {
            currentlyFocusedButtonID = buttonID;
            currentlyFocusedButtonCallback = OnFocusedButtonClicked;

            if (overlayPanel != null)
            {
                overlayPanel.SetActive(true); // Activate the overlay to dim the screen
            }

            // Trigger the OnButtonFocus event to highlight the button
            OnButtonFocus?.Invoke(buttonID);

            // Show tooltip if text is provided
            if (!string.IsNullOrEmpty(tooltipText))
            {
                ShowTooltip(buttonID, tooltipText);
            }
        }

        // Show a tooltip for the focused button
        private void ShowTooltip(ButtonID buttonID, string tooltipText)
        {
            if (currentTooltip == null)
            {
                // Instantiate the tooltip
                currentTooltip = Instantiate(tooltipPrefab, overlayPanel.transform);
                tooltip = currentTooltip.GetComponent<TooltipUI>();
            }
            else
            {
                tooltip.gameObject.SetActive(false);
            }

            // Find the target button
            if (buttonID != ButtonID.None)
            {
                UIButton targetButton = FindButton(buttonID);
                if (targetButton == null)
                {
                    Debug.LogError($"Button with ID {buttonID} not found!");
                    return;
                }

                // Set tooltip position
                PositionNearButton(targetButton.GetComponent<RectTransform>());
            }
            else
            {
                // Set tooltip position to the center of the screen
                PositionAtCenter();
            }
            tooltip.SetText(tooltipText);
            tooltip.gameObject.SetActive(true);
        }

        private void PositionAtCenter()
        {
            RectTransform toolTipRect = tooltip.GetComponent<RectTransform>();

            // Ensure the tooltip anchors and pivot are centered
            toolTipRect.anchorMin = new Vector2(0.5f, 0.5f);
            toolTipRect.anchorMax = new Vector2(0.5f, 0.5f);
            toolTipRect.pivot = new Vector2(0.5f, 0.5f);

            // Center it relative to the parent (canvas/overlayPanel)
            toolTipRect.anchoredPosition = Vector2.zero;

            // Hide arrow if it exists
            if (tooltip.arrowImage != null)
            {
                tooltip.arrowImage.gameObject.SetActive(false);
            }
        }

        public void PositionNearButton(RectTransform targetButtonRect)
        {
            RectTransform toolTipRect = tooltip.GetComponent<RectTransform>();
            RectTransform canvasRect = overlayPanel.GetComponent<RectTransform>();

            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(null, targetButtonRect.position);
            Vector2 localPoint;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, null, out localPoint))
                return;

            float buttonHeight = targetButtonRect.rect.height;
            float tooltipHeight = toolTipRect.rect.height;
            float tooltipWidth = toolTipRect.rect.width;

            // Calculate available space
            float buttonTopEdge = localPoint.y + (buttonHeight / 2);
            float spaceAbove = (canvasRect.rect.height / 2) - buttonTopEdge;
            float spaceBelow = buttonTopEdge - (buttonHeight / 2) + (canvasRect.rect.height / 2);

            bool placeAbove = spaceAbove >= tooltipHeight || spaceAbove >= spaceBelow;
            float verticalOffset = 10f;

            // Calculate desired position
            float desiredY = placeAbove ?
                localPoint.y + (buttonHeight / 2) + (tooltipHeight / 2) + verticalOffset :
                localPoint.y - (buttonHeight / 2) - (tooltipHeight / 2) - verticalOffset;

            float desiredX = localPoint.x;

            // Horizontal clamping
            float halfTooltipWidth = tooltipWidth / 2;
            desiredX = Mathf.Clamp(
                desiredX,
                -canvasRect.rect.width / 2 + halfTooltipWidth,
                canvasRect.rect.width / 2 - halfTooltipWidth
            );

            // Vertical clamping
            float halfTooltipHeight = tooltipHeight / 2;
            desiredY = Mathf.Clamp(
                desiredY,
                -canvasRect.rect.height / 2 + halfTooltipHeight,
                canvasRect.rect.height / 2 - halfTooltipHeight
            );

            // Apply tooltip position
            toolTipRect.anchoredPosition = new Vector2(desiredX, desiredY);

            // Position arrow
            if (tooltip.arrowImage != null)
            {
                tooltip.arrowImage.gameObject.SetActive(true);
                RectTransform arrowRect = tooltip.arrowImage;

                // Calculate horizontal offset to maintain arrow pointing at button
                float deltaX = localPoint.x - desiredX;
                float maxOffset = halfTooltipWidth - arrowRect.rect.width / 2;
                deltaX = Mathf.Clamp(deltaX, -maxOffset, maxOffset);

                // Position and rotate arrow
                if (placeAbove)
                {
                    // Point down (default rotation)
                    arrowRect.localEulerAngles = Vector3.zero;
                    // Position at bottom of tooltip
                    arrowRect.anchoredPosition = new Vector2(
                        deltaX,
                        -halfTooltipHeight + arrowRect.rect.height / 2
                    );
                }
                else
                {
                    // Point up (180 degree rotation)
                    arrowRect.localEulerAngles = new Vector3(0, 0, 180);
                    // Position at top of tooltip
                    arrowRect.anchoredPosition = new Vector2(
                        deltaX,
                        halfTooltipHeight - arrowRect.rect.height / 2
                    );
                }
            }
        }

        // Call this method to stop focusing on any button
        public void StopFocus()
        {
            if (overlayPanel != null)
            {
                overlayPanel.SetActive(false); // Deactivate the overlay
            }

            // Trigger the OnButtonFocus event with a default value to reset focus
            OnButtonFocus?.Invoke(ButtonID.None); // Assuming ButtonID has a None or default value
            currentlyFocusedButtonID = ButtonID.None;
            currentlyFocusedButtonCallback = null; // Clear the callback
        }

        // Call this method when the focused button is clicked
        public void OnFocusedButtonClicked(ButtonID buttonID)
        {
            if (buttonID == currentlyFocusedButtonID)
            {
                // Invoke the callback if it exists
                currentlyFocusedButtonCallback?.Invoke();

                // Stop focusing on the button
                StopFocus();
                tooltip?.gameObject.SetActive(false);
            }
        }

        // Helper method to find a button by its ID
        private UIButton FindButton(ButtonID id)
        {
            if (buttonDictionary.TryGetValue(id, out UIButton button))
            {
                return button;
            }

            // If not found, attempt to find an inactive one
            UIButton[] allButtons = Resources.FindObjectsOfTypeAll<UIButton>();
            foreach (UIButton btn in allButtons)
            {
                if (btn.buttonID == id)
                {
                    buttonDictionary[id] = btn; // Cache it for future use
                    return btn;
                }
            }
            return null; // Button not found
        }

        void ResetButtonAction()
        {
            foreach (var pair in tutorialClickActions)
            {
                pair.Key.onClick.RemoveListener(pair.Value);
            }
            tutorialClickActions.Clear();
        }
    }
}
