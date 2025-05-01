using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COMMANDS;

public class CommandTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(Running());
    }

    IEnumerator Running(){
        yield return CommandsManager.instance.Execute("print");
        yield return CommandsManager.instance.Execute("print_1p", "Hello World");
        yield return CommandsManager.instance.Execute("print_mp", "Line1", "Line2", "Line3");

        yield return CommandsManager.instance.Execute("lambda");
        yield return CommandsManager.instance.Execute("lambda_1p", "3");
        yield return CommandsManager.instance.Execute("lambda_mp", "Lambda1", "Lambda2", "Lambda3");

        yield return CommandsManager.instance.Execute("process");
        yield return CommandsManager.instance.Execute("process_1p", "Hello Process");
        yield return CommandsManager.instance.Execute("process_mp", "Process1", "Process2", "Process3");
        
        //yield return CommandsManager.instance.Execute("playSFX_truckHorn");  // Play truck horn sound
        //yield return CommandsManager.instance.Execute("playSFX_buildingCrash"); 
    }
}
