using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


namespace Dialogue{

    /*
        Player inputs controls
    */

    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerInput input;
        private List<(InputAction action, Action<InputAction.CallbackContext> command)> actions = new List<(InputAction action, Action<InputAction.CallbackContext> command)> ();

        // Start is called before the first frame update
        void Awake()
        {
            input = GetComponent<PlayerInput>();
            InitializeActions();
        }

        private void InitializeActions()
        {
            actions.Add((input.actions["Next"], PromptAdvance));
        }

        private void OnEnable()
        {
            foreach(var inputAction in actions)
                inputAction.action.performed += inputAction.command;
        }

         private void OnDisable()
        {
            foreach(var inputAction in actions)
                inputAction.action.performed -= inputAction.command;
        }

        public void PromptAdvance(InputAction.CallbackContext c){
            DialogueSystem.instance.OnUserPrompt_Next();
        }
    }
}
