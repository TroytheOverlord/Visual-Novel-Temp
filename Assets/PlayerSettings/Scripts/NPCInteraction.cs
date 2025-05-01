using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Animator dialogueAnimator; // Reference to the Animator controlling the dialogue box animation
    private bool isPlayerInRange = false; 

    void Update()
    {
        // Check if the player is in range and presses the "A" key (or any key you prefer)
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }

         if (Input.GetKeyDown(KeyCode.R))
        {
            TriggerDialogueEnd();
        }
    }

    // Called when the player enters the NPC's trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the object is tagged as "Player"
        {
            isPlayerInRange = true;
            Debug.Log("Player in range of NPC");
        }
    }

    // Called when the player leaves the NPC's trigger area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left range of NPC");
        }
    }

    // Method to trigger the dialogue animation
    void TriggerDialogue()
    {
        dialogueAnimator.SetTrigger("ShowDialogue");
    }

     void TriggerDialogueEnd()
    {
        dialogueAnimator.SetTrigger("HideDialogue");
    }
}

