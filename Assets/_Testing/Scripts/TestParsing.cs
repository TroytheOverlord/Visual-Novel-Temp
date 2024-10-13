using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace TESTING{
    public class TestParsing : MonoBehaviour
    {
        [SerializeField] private TextAsset file;

        // Start is called before the first frame update
        void Start()
        {
            SendFileToParse();
        }

       void SendFileToParse(){
            List<string> lines = FileManager.ReadTextAsset("FirstMeeting");

            foreach(string line in lines){

                if(line == string.Empty)
                    continue;
                DialogueLine dl = DialogueParser.Parse(line);
            }
       }
    }
}
