using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.LogicalLines
{
    public class LL_Input : ILogicalLine
    {
        public string keyword => "input";
        public bool Matches(DialogueLine line)
        {
            return (line.hasSpeaker && line.speakerData.name.ToLower() == keyword);
        }
        public IEnumerator Execute(DialogueLine line)
        {
            string title = line.dialogueData.rawData;

            InputPanel panel = InputPanel.instance;
            panel.Show(title);

            while(panel.isWaitingOnUserInput)
                yield return null;
        }
    }
}

