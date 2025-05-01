using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        private const string SPRITE_RENDERER_PARENT_NAME = "Renderers";
        private const string SPRITESHEET_DEFAULT_SHEETNAME = "Default";
        private const char SPRITESHEET_TEX_SPRITE_DELIMITTER = '-';
        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();

        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        private string artAssetsDirectory = "";

        public override bool isVisible
        {
            get { return isRevealing || rootCG.alpha == 1; }
            //set { rootCG.alpha = value ? 1 : 0; }
        }

        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            rootCG.alpha = ENABLE_ON_START ? 1 : 0;
            artAssetsDirectory = rootAssetsFolder + "/Image";

            GetLayers();
            
            Debug.Log($"Created Sprite Character: '{name}'");
        }

        private void GetLayers()
{
    if (animator == null)
    {
        Debug.LogError("Animator is null.");
        return;
    }

    Transform rendererRoot = animator.transform.Find(SPRITE_RENDERER_PARENT_NAME);

    if (rendererRoot == null)
    {
        Debug.LogError($"Renderers object '{SPRITE_RENDERER_PARENT_NAME}' not found in prefab for character '{name}'");
        return;
    }

    for (int i = 0; i < rendererRoot.childCount; i++)
    {
        Transform child = rendererRoot.GetChild(i);
        Image rendererImage = child.GetComponent<Image>();

        if (rendererImage != null)
        {
            CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
            layers.Add(layer);
            child.name = $"Layer: {i}";
        }
    }

    Debug.Log($"Loaded {layers.Count} sprite layers for character '{name}'");
}


        public void SetSprite(Sprite sprite, int layer = 0)
{
    Debug.Log($"Setting sprite {sprite.name} on layer {layer} for {name}");
    layers[layer].SetSprite(sprite);
}

       public Sprite GetSprite(string spriteName)
{
    string path = $"{artAssetsDirectory}/{spriteName}";
    Debug.Log($"Loading sprite from path: {path}");
    Sprite sprite = Resources.Load<Sprite>(path);
    
    if (sprite == null)
    {
        Debug.LogError($"Sprite '{spriteName}' not found at {path}");
    }

    return sprite;
}


        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1)
        {
            CharacterSpriteLayer spriteLayer = layers[layer];

            return spriteLayer.TransitionSprite(sprite, speed);
        }

       public override IEnumerator ShowingOrHiding(bool show)
{
    float targetAlpha = show ? 1f : 0;
    Debug.Log($"Character {name} visibility transitioning to {targetAlpha}");

    CanvasGroup self = rootCG;

    while (self.alpha != targetAlpha)
    {
        self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime);
        yield return null;
    }

    Debug.Log($"Character {name} visibility set to {self.alpha}");
    co_revealing = null;
    co_hiding = null;
}


/*
        public override void SetColor(Color color)
        {
            base.SetColor(color);

            color = displayColor;

            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.StopChangingColor();
                layer.SetColor(color);
            }
        }

        public override IEnumerator ChangingColor(Color color, float speed)
        {
            foreach (CharacterSpriteLayer layer in layers)
                layer.TransitionColor(color, speed);

            yield return null;

            while (layers.Any(l => l.isChangingColor))
                yield return null;

            co_changingColor = null;
        }

        public override IEnumerator Highlighting(bool highlight, float speedMultiplier)
        {
            Color targetColor = displayColor;

            foreach (CharacterSpriteLayer layer in layers)
                layer.TransitionColor(targetColor, speedMultiplier);

            yield return null;

            while (layers.Any(l => l.isChangingColor))
                yield return null;

            co_highlighting = null;
        }
*/
    }
}
