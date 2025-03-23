using System.Collections;
using SentryToolkit;
using UnityEngine;

public class MenuTutorialController : MonoBehaviour
{
    public UITutorialManager tutorialManager;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        //let's tell the player to click to start if they are new to the game
        if (PlayerPrefs.GetInt("tut_firstplay") != 1){
            //tutorial has been played
            PlayerPrefs.SetInt("tut_firstplay", 1);
            tutorialManager.StartTutorial(SequenceID.seq_abcdef);
        }
    }
}
