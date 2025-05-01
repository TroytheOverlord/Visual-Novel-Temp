using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using Dialogue;

public class TestCharacters : MonoBehaviour
{
    private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

    // Start is called before the first frame update
    void Start()
    {
        //Character Sayori = CharacterManager.instance.CreateCharacter("Sayori");
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
       Character_Sprite Scarlet = CreateCharacter("Scarlet") as Character_Sprite;
       //Character Sayori2 = CreateCharacter("Sayori as Sayori");

        Sprite body = Scarlet.GetSprite("Scarlet_Body");
        Sprite eyes = Scarlet.GetSprite("Scarlet_Eyes_Joy");
        Sprite mouth = Scarlet.GetSprite("Scarlet_Mouth_Upset1");

        yield return Scarlet.TransitionSprite(body, 0);
        yield return Scarlet.TransitionSprite(eyes, 1);
        yield return Scarlet.TransitionSprite(mouth, 2);

        Scarlet.Show();

       /*Sayori.SetPosition(Vector2.zero);
       Sayori2.SetPosition(new Vector2(0.5f, 0.5f));

       yield return Sayori.MoveToPosition(Vector2.one, smooth: true);
       yield return Sayori.MoveToPosition(Vector2.zero, smooth: true);

       Sayori.Show();
       Sayori2.Show();*/

       yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
