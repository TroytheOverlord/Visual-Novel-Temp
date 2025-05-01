using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

public class AudioTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());
    }

    Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

    IEnumerator Running()
    {
        Character_Sprite Sayori = CreateCharacter("Sayori") as Character_Sprite;
        Sayori.Show();

        yield return new WaitForSeconds(0.5f);

        GraphicPanelManager.instance.GetPanel("background").GetLayer(0, true).SetTexture("Graphics/BG Images/5");

        AudioManager.instance.PlayTrack("Audio/Music/Calm", startingVolume: 0.7f);

        //AudioManager.instance.PlaySoundEffect("Audio/SFX/thunder_strong_01");
        //AudioManager.instance.StopSoundEffect("thunder_strong_01");
        
        AudioManager.instance.PlaySoundEffect("Audio/Voices/wakeup");
    }
}
