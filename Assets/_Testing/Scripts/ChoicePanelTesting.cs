using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicePanelTesting : MonoBehaviour
{
    ChoicePanel panel;

    // Start is called before the first frame update
    void Start()
    {
        panel = ChoicePanel.instance;
        

        string[] choices = new string[]
        {
            "I need five more minutes",
            "Crap I need to get up now",
            "Ahh what a beautiful day",
            
        };
        panel.Show("Your alarm goes off", choices);
        //StartCoroutine(WaitForUserChoice("Your alarm goes off", choices));
    }

    private IEnumerator WaitForUserChoice(string question, string[] choices)
    {
        panel.Show(question, choices);

        while (panel.isWaitingOnUserChoice)
        {
            yield return null;
        }

        var decision = panel.lastDecision;

        Debug.Log($"Made choice {decision.answerIndex} '{decision.choices[decision.answerIndex]}'");
    }
}

