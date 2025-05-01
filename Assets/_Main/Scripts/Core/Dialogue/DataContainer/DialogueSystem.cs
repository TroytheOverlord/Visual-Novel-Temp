using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CHARACTERS;

namespace Dialogue{

    public class DialogueSystem : MonoBehaviour
    {

    [SerializeField]private DialogueSystemConfigurationSO _config;
    public DialogueSystemConfigurationSO config => _config;
    public DialogueContainer dialogueContainer = new DialogueContainer();
    public ConversationManager conversationManager { get; private set; }
    private TextArchitect architect;
    [SerializeField] private CanvasGroup mainCanvas;
    public static DialogueSystem instance {get; private set;}
    public delegate void DialogueSystemEvent();
    public event DialogueSystemEvent onUserPrompt_Next;
    public bool isRunningConversation => conversationManager.isRunning;

    public DialogueContinuePrompt prompt;
    private CanvasGroupController cgController;

        private void Awake(){
            if(instance == null){
                instance = this;
                Initialize();
            }
               
            else
                DestroyImmediate(gameObject);
        }

        bool _initialized = false;
        private void Initialize(){
            if(_initialized){
                return;
            }
            architect = new TextArchitect(dialogueContainer.DialogueText);
            conversationManager = new ConversationManager(architect);

            cgController = new CanvasGroupController(this, mainCanvas);
            dialogueContainer.Initialize();
        }

        public void OnUserPrompt_Next(){
            onUserPrompt_Next?.Invoke();
        }

        public void ApplySpeakerDataToDialogueContainer(string speakerName)
        {
            Character character = CharacterManager.instance.GetCharacter(speakerName);
            CharacterConfigData config = character != null ? character.config : CharacterManager.instance.GetCharacterConfig(speakerName);

            ApplySpeakerDataToDialogueContainer(config);
        }

        public void ApplySpeakerDataToDialogueContainer(CharacterConfigData config)
        {
            dialogueContainer.SetDialogueColor(config.dialogueColor);
            dialogueContainer.SetDialogueFont(config.dialogueFont);

            dialogueContainer.nameContainer.SetNameColor(config.nameColor);
            dialogueContainer.nameContainer.SetNameFont(config.nameFont);
        }

        public void ShowSpeakerName(string speakerName = " "){
            if(speakerName.ToLower() != "narrator")
                dialogueContainer .nameContainer.Show(speakerName);

            else{
                HideSpeakerName();
            }
        }
            
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        public Coroutine Say(string speaker, string dialogue){
            List<string> conversation = new List<string>() {$"{speaker} \"{dialogue}\""};
            return Say(conversation);
        }

        public Coroutine Say(List<string> lines){
            Conversation conversation = new Conversation(lines);
            return conversationManager.StartConversation(conversation);
        }

        public Coroutine Say(Conversation conversation){
            return conversationManager.StartConversation(conversation);
        }

        public bool isVisible => cgController.isVisible;
        public Coroutine Show(float speed = 1f, bool immediate = false) => cgController.Show(speed, immediate); 

        public Coroutine Hide(float speed = 1f, bool immediate = false) => cgController.Hide(speed, immediate);

        //public Coroutine Show(float speed = 1f, bool immediate = false) => cgController.Show(speed,immediate);

        //public Coroutine Hide(float speed = 1f, bool immediate = false) => cgController.Hide(speed, immediate);


    }
}
