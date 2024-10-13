using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue{
    /* 
        Class Description:
        Storage for dialogue information 
        that's been parsed and ripped from a string 
    */


    public class DialogueLine
    {
        public string speaker;
        public DL_Dialogue_Data dialogue;
        public string commands;

        public bool hasSpeaker => speaker != string.Empty;
        public bool hasDialogue => dialogue.hasDialogue;
        public bool hasCommands => commands != string.Empty;

        public DialogueLine(string speaker, string dialogue, string commands){
            this.speaker = speaker;
            this.dialogue = new DL_Dialogue_Data(dialogue);
            this.commands = commands;
        }
    }
}
