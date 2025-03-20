using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UITutorialStep
{
    public ButtonID buttonID;
    [TextArea] public string message;
    public UnityEvent OnStepCompleted;
}

[Serializable]
public class TutorialSequence
{
    public SequenceID sequenceName;
    public List<UITutorialStep> steps = new List<UITutorialStep>();
}
