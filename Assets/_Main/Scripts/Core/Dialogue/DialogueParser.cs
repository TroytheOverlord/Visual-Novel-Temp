using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


namespace Dialogue{
    /*
        Class Description:
        Handles parsing functions to 
        convert strings into Dialogue Lines 
    */

    public class DialogueParser{

        private const string commandRegexPattern = @"[\w\[\]]*[^\s]\(";
        
        public static DialogueLine Parse(string rawLine){
            Debug.Log($"Parsing line - '{rawLine}'");

            (string speaker, string dialogue, string commands) = RipContent(rawLine);

            Debug.Log($"Speaker = '{speaker}'\nDialogue = '{dialogue}'\nCommands = '{commands}'");

            return new DialogueLine(rawLine, speaker, dialogue, commands);
        }

        private static(string, string, string) RipContent(string rawLine){
            string speaker = "";
            string dialogue = "";
            string commands = "";

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            for(int i = 0; i < rawLine.Length;i++){
                char current = rawLine[i];
                if(current == '\\'){
                    isEscaped = !isEscaped;
                }
                    
                else if(current == '"' && !isEscaped){
                    if(dialogueStart == -1){
                         dialogueStart = i;
                    }  
                    else if(dialogueEnd == -1){
                        dialogueEnd = i;
                        break;
                    } 
                }
                else{
                    isEscaped = false;
                }
            }

            // Identify Command Patterns
            Regex commandRegex = new Regex(commandRegexPattern);
            MatchCollection matches = commandRegex.Matches(rawLine);
            int commandStart = -1;
            foreach(Match match in matches)
            {
                if(match.Index < dialogueStart || match.Index > dialogueEnd){
                     commandStart = match.Index;
                     break;
                }
            }

            if(commandStart != -1 && (dialogueStart == -1 && dialogueEnd == -1)){
                return("", "", rawLine.Trim());
            }

            // If we are here then we either have dialogue or a multi word argument in a command.
            // This figures out if this is dialogue or not

            if(dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd)){
                // Valid Dialogue 
                speaker = rawLine.Substring(0, dialogueStart).Trim();
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"", "\"");
                if(commandStart != -1){
                    commands = rawLine.Substring(commandStart).Trim();
                }
            }

            else if(commandStart != -1 && dialogueStart > commandStart){
                commands = rawLine;
            }

            else{
                speaker = rawLine;
            }



            return(speaker,dialogue,commands);
        }
    }
}
