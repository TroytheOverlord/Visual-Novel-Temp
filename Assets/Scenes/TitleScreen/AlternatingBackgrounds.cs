using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenBackground : MonoBehaviour
{
    [SerializeField] private RawImage backgroundImage; // Assign in Inspector
    [SerializeField] private Texture[] backgroundTextures; // Assign in Inspector
    [SerializeField] private float changeInterval = 5f; // Time interval in seconds

    private int currentIndex = 0;

    private void Start()
    {
        if (backgroundTextures.Length == 0)
        {
            Debug.LogError("No background textures assigned!");
            return;
        }

        StartCoroutine(ChangeBackgroundRoutine());
    }

    private IEnumerator ChangeBackgroundRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval);

            currentIndex = (currentIndex + 1) % backgroundTextures.Length;
            backgroundImage.texture = backgroundTextures[currentIndex];
        }
    }
}
