using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue{
    /* 
        Class Description:
        Storage for dialogueData information 
        that's been parsed and ripped from a string 
    */


    public class DialogueLine
    {
        public string rawData { get; private set;} = string.Empty;
        public DL_Speaker_Data speakerData;
        public DL_Dialogue_Data dialogueData;
        public DL_Command_Data commandData;

        public bool hasSpeaker => speakerData != null; 
        public bool hasDialogue => dialogueData != null;
        public bool hasCommands => commandData != null;

        public DialogueLine(string rawLine, string speakerData, string dialogueData, string commandData){
            rawData = rawLine;
            this.speakerData = (string.IsNullOrWhiteSpace(speakerData) ? null : new DL_Speaker_Data(speakerData));
            this.dialogueData = (string.IsNullOrWhiteSpace(dialogueData) ? null : new DL_Dialogue_Data(dialogueData));
            this.commandData = (string.IsNullOrWhiteSpace(commandData) ? null : new DL_Command_Data(commandData));
        }
    }
}
