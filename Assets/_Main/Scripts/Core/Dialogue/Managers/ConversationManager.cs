using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using COMMANDS;
using Dialogue.LogicalLines;

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
        public Conversation conversation => (conversationQueue.IsEmpty() ? null : conversationQueue.top);
        public int conversationProgress => (conversationQueue.IsEmpty()? -1 : conversationQueue.top.GetProgress());
        private ConversationQueue conversationQueue;

        private TagManager tagManager;
        private LogicalLineManager logicalLineManager;

        public ConversationManager(TextArchitect architect){
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;

            tagManager = new TagManager();
            logicalLineManager = new LogicalLineManager();

            conversationQueue = new ConversationQueue();
        }

        public void Enqueue(Conversation conversation) => conversationQueue.Enqueue(conversation);
        public void EnqueuePriority(Conversation conversation) => conversationQueue.EnqueuePriority(conversation);

        public void OnUserPrompt_Next(){
            userPrompt = true;
        }
        
        public Coroutine StartConversation(Conversation conversation){
            StopConversation();
            conversationQueue.Clear();

            Enqueue(conversation);

            process = dialogueSystem.StartCoroutine(RunningConversation());

            return process;
        }

        public void StopConversation(){
            if(!isRunning){
                return;
            }

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(){
            // To find the length of an array use .Length but for Lists use .Count
            while(!conversationQueue.IsEmpty()){

                Conversation currentConversation = conversation;

                if(currentConversation.HasReachedEnd())
                {
                    conversationQueue.Dequeue();
                    continue;
                }

                string rawLine = conversation.CurrentLine();
                // Skips over blank lines 
                if(string.IsNullOrWhiteSpace(rawLine)){
                    TryAdvanceConversation(currentConversation);
                    continue;
                }
                DialogueLine line = DialogueParser.Parse(rawLine);

                if(logicalLineManager.TryGetLogic(line, out Coroutine logic))
                {
                    yield return logic;
                }

                else
                {
                    //Shows Dialogue
                    if(line.hasDialogue){
                    yield return LineRunDialogue(line);
                    }
                    // Runs Commands
                    if(line.hasCommands){
                        yield return LineRunCommands(line);
                    }
                    // Wait for User Input if dilaogue was in the line 
                    if(line.hasDialogue){
                        // Waits for user input 
                        yield return WaitForUserInput();  

                        CommandsManager.instance.StopAllProcesses();
                    }
                }

                TryAdvanceConversation(currentConversation);  
            }
            process = null;
        }

        private void TryAdvanceConversation(Conversation conversation)
        {
            conversation.IncrementProgress();

            if(conversation != conversationQueue.top)
                return;

            if(conversation.HasReachedEnd())
                conversationQueue.Dequeue();
        }

       

        IEnumerator LineRunDialogue(DialogueLine line){
            if(line.hasSpeaker)
            {
                HandleSpeakerLogic(line.speakerData);
            }

            //If the dialogue box is not visible - make sure it becomes visible automatically 
            if(!dialogueSystem.dialogueContainer.isVisible)
                dialogueSystem.dialogueContainer.Show();

            // Build Dialogue
           yield return BuildLineSegments(line.dialogueData);

        }

        private void HandleSpeakerLogic(DL_Speaker_Data speakerData)
        {
            bool characterMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition);

            Character character = CharacterManager.instance.GetCharacter(speakerData.name, createIfDoesNotExist: characterMustBeCreated);

                if(speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing))
                    character.Show();

                // Adds character name to UI
                dialogueSystem.ShowSpeakerName(tagManager.Inject(speakerData.displayname));

                DialogueSystem.instance.ApplySpeakerDataToDialogueContainer(speakerData.name);

                if(speakerData.isCastingPosition)
                    character.MoveToPosition(speakerData.castPosition);
        }

        IEnumerator LineRunCommands(DialogueLine line){
            List<DL_Command_Data.Command> commands = line.commandData.commands;

            foreach(DL_Command_Data.Command command in commands){

                if(command.waitForCompletion || command.name == "wait"){
                    CoroutineWrapper cw = CommandsManager.instance.Execute(command.name, command.arguments);
                    while(!cw.isDone)
                    {
                        if(userPrompt)
                        {
                            CommandsManager.instance.StopCurrentProcess();
                            userPrompt = false;
                        }
                        yield return null;
                    }
                }
                else{
                    CommandsManager.instance.Execute(command.name, command.arguments);
                }
                
            }
            yield return null;
        }

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

            dialogue = tagManager.Inject(dialogue);
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
            dialogueSystem.prompt.Show();

            while(!userPrompt){
                yield return null;
            }

             dialogueSystem.prompt.Hide();

            userPrompt = false;
        }
    }
}

// Beginning of new changes 
