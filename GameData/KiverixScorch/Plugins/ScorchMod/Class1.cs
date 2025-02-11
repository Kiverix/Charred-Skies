using UnityEngine;
using System.Collections;

[KSPAddon(KSPAddon.Startup.Flight, false)]  // Ensures this runs when flight starts
public class ScorchModDebugger : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[KiverixScorch] Plugin has been loaded into the game!");
    }
}

public class SOCKScorchMarks : PartModule
{
    private Material partMaterial;
    private Texture scorchTextureUV1;
    private Texture scorchTextureUV2;
    private float scorchLevel = 0f;
    private bool isScorching = false;

    public override void OnStart(StartState state)
    {
        base.OnStart(state);
        Debug.Log("[KiverixScorch] Mod initialized on part: " + part.partInfo.title);

        Renderer renderer = part.FindModelComponent<Renderer>();
        if (renderer != null)
        {
            partMaterial = renderer.material;
            scorchTextureUV1 = GameDatabase.Instance.GetTexture("KiverixScorch/Textures/shuttleUV1_diff_Discovery", false);
            scorchTextureUV2 = GameDatabase.Instance.GetTexture("KiverixScorch/Textures/shuttleUV2_diff_Discovery", false);

            if (scorchTextureUV1 != null && scorchTextureUV2 != null)
            {
                Debug.Log("[KiverixScorch] Scorch textures loaded successfully.");
                partMaterial.SetTexture("_DetailTex", scorchTextureUV1);
                partMaterial.SetFloat("_DetailBlend", 0f);
            }
            else
            {
                Debug.LogError("[KiverixScorch] Error loading scorch textures!");
            }
        }
        else
        {
            Debug.LogError("[KiverixScorch] Renderer not found!");
        }
    }
}
