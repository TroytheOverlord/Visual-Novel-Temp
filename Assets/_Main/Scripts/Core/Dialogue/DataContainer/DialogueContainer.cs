using System.Collections;
using UnityEngine;
using TMPro;

namespace Dialogue{

   [System.Serializable] 

   public class DialogueContainer 
   {
      private const float DEFAULT_FADE_SPEED = 1f;
      public GameObject Root;
      public NameContainer nameContainer;
      public TextMeshProUGUI DialogueText;

      private CanvasGroupController cgController;

      public void SetDialogueColor(Color color) => DialogueText.color = color;
      public void SetDialogueFont(TMP_FontAsset font) => DialogueText.font = font;
      
      private bool initialized = false;

      public void Initialize()
      {
         if(initialized)
            return;

         cgController = new CanvasGroupController(DialogueSystem.instance, Root.GetComponent<CanvasGroup>());
      }

      public bool isVisible => cgController.isVisible;
      public Coroutine Show() => cgController.Show();
      public Coroutine Hide() => cgController.Hide();

   }
}
