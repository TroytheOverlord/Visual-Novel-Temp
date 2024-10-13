using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace TESTING{
    public class Testing_Architect : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;

        public TextArchitect.BuildMethod bm = TextArchitect.BuildMethod.instant;

        string[] lines = new string[5]{
            "Welcome this is the beginning",
            "the beginning of what you ask",
            "the beginning of the retelling of",
            "your story everyone wishes for a redo",
            "in life and this is your have fun"
        };

        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.DialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.fade;
            
            // Will slow down the speed of the text
            //architect.speed = 0.5;
        }

        // Update is called once per frame
        void Update()
        {
            if(bm != architect.buildMethod){
                architect.buildMethod = bm;
                architect.Stop();
            }

            if(Input.GetKeyDown(KeyCode.S))
                architect.Stop();

            //string longLine = "ive been working on this shit for almost three hours now I drained mentally and physically I dont wanna look at code anymore but alas thus is my burden";
            if(Input.GetKeyDown(KeyCode.Space)){
                if(architect.isBuilding){
                    if(!architect.hurryUp)
                    architect.hurryUp = true;
                    else   
                        architect.ForceComplete();
                }
                else
                   //architect.Build(longLine);
                    architect.Build(lines[Random.Range(0,lines.Length)]);
            }

            else if(Input.GetKeyDown(KeyCode.A)){
                 // architect.Append(longLine);
                 architect.Append(lines[Random.Range(0,lines.Length)]);
            }
            
        }
    }
}
