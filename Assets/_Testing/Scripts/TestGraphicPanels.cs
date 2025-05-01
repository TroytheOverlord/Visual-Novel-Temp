using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGraphicPanels : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());
    }

    IEnumerator Running()
    {
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
        GraphicLayer layer = panel.GetLayer(0,true);

        yield return new WaitForSeconds(1);

        Texture blendTex = Resources.Load<Texture>("Graphics/Transition Effects/hurricane");
        layer.SetTexture("Graphics/BG Images/Abandoned Store", blendingTexture: blendTex); 

        yield return new WaitForSeconds(10);

        layer.SetTexture("Graphics/BG Images/BG Bath", blendingTexture: blendTex);

         yield return new WaitForSeconds(10);

        layer.SetTexture("Graphics/BG Images/outside", blendingTexture: blendTex);

         yield return new WaitForSeconds(10);

        layer.SetTexture("Graphics/BG Images/Courtyard", blendingTexture: blendTex);
        
        

    }
}
