using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Handles the logic to run dialogue 
    on screen one line at a time 
*/

namespace Dialogue{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;

        public bool isRunning => process != null;

        private TextArchitect architect = null;
        private bool userPrompt = false;

        public ConversationManager(TextArchitect architect){
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        public void OnUserPrompt_Next(){
            userPrompt = true;
        }
        
        public void StartConversation(List<string> conversation){
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }

        public void StopConversation(){
            if(!isRunning){
                return;
            }

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation){
            // To find the length of an array use .Length but for Lists use .Count
            for(int i = 0;i < conversation.Count; i++){
                // Skips over balnk lines 
                if(string.IsNullOrWhiteSpace(conversation[i])){
                    continue;
                }
                DialogueLine line = DialogueParser.Parse(conversation[i]);

                //Shows Dialogue
                if(line.hasDialogue){
                   yield return LineRunDialogue(line);
                }

                // Runs Commands
                if(line.hasCommands){
                    yield return LineRunCommands(line);
                }  
            }
            //TriggerSceneTransition();
        }

        /*
        private void TriggerSceneTransition()
        {
            LevelManager.instance.LoadScene("NextSceneName", "CrossFade");
        }
        */

        IEnumerator LineRunDialogue(DialogueLine line){
            if(line.hasSpeaker){
                dialogueSystem.ShowSpeakerName(line.speaker);
            }

            // Build Dialogue
           yield return BuildLineSegments(line.dialogue);

            // Waits for user input 
            yield return WaitForUserInput();

           
        }

        IEnumerator LineRunCommands(DialogueLine line){
            Debug.Log(line.commands);
            yield return null;
        }

        /*
            if(line.hasCommands){
                // Example command format: /transition NextSceneName
                string command = line.commands.Trim();
                if(command.StartsWith("/transition ")){
                    string sceneName = command.Replace("/transition ", "");
                    LevelManager.instance.LoadScene(sceneName, "CrossFade");
                }
            }
            yield return null;
        */

        IEnumerator BuildLineSegments(DL_Dialogue_Data line){
            for(int i = 0; i < line.segments.Count; i++){
                DL_Dialogue_Data.Dialogue_Segment segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTrigger(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToBeTrigger(DL_Dialogue_Data.Dialogue_Segment segment){
            switch(segment.startSignal){
                case DL_Dialogue_Data.Dialogue_Segment.StartSignal.C:
                case DL_Dialogue_Data.Dialogue_Segment.StartSignal.A:
                    yield return WaitForUserInput();
                    break;

                case DL_Dialogue_Data.Dialogue_Segment.StartSignal.WC:
                case DL_Dialogue_Data.Dialogue_Segment.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false){
            // Build the dialogue 
            if(!append)
                architect.Build(dialogue);

            else
                architect.Append(dialogue);

            // Waits for dialogue to complete
            while(architect.isBuilding){
                if(userPrompt){
                    if(!architect.hurryUp){
                        architect.hurryUp = true;
                    }
                    else{
                        architect.ForceComplete();
                    }
                    userPrompt = false;
                }
                 yield return null;
            }
        }

        IEnumerator WaitForUserInput(){
            while(!userPrompt){
                yield return null;
            }
            userPrompt = false;
        }
    }
}

// Beginning of new changes 
