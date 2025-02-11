using UnityEngine;
using System.Collections;

public class SOCKScorchMarks : PartModule
{
    private Material partMaterial; // Stores the material of the part
    private Texture scorchTextureUV1; // Texture for UV1-based parts
    private Texture scorchTextureUV2; // Texture for UV2-based parts
    private float scorchLevel = 0f; // Controls the visibility of scorch marks
    private bool isScorching = false; // Flag to ensure scorching starts only once

    public override void OnStart(StartState state)
    {
        base.OnStart(state);
        Debug.Log("[KiverixScorch] Mod initialized on part: " + part.partInfo.title);

        // Get the part's renderer component to access its material
        Renderer renderer = part.FindModelComponent<Renderer>();
        if (renderer != null)
        {
            partMaterial = renderer.material;

            // Load the correct scorch textures
            scorchTextureUV1 = GameDatabase.Instance.GetTexture("KiverixScorch/Textures/shuttleUV1_diff_Discovery", false);
            scorchTextureUV2 = GameDatabase.Instance.GetTexture("KiverixScorch/Textures/shuttleUV2_diff_Discovery", false);

            if (scorchTextureUV1 != null && scorchTextureUV2 != null)
            {
                Debug.Log("[KiverixScorch] Scorch textures loaded successfully.");
                partMaterial.SetTexture("_DetailTex", scorchTextureUV1);
                partMaterial.SetFloat("_DetailBlend", 0f);  // Start with no scorch marks
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

    public void Update()
    {
        if (IsReentryHappening() && !isScorching)
        {
            isScorching = true;
            Debug.Log("[KiverixScorch] Reentry detected! Starting scorch effect...");
            StartCoroutine(ApplyScorchOverTime());
        }
    }

    private bool IsReentryHappening()
    {
        if (vessel == null || !vessel.mainBody.atmosphere)
            return false;

        double velocity = vessel.srfSpeed; // Get surface speed
        double atmoDensity = vessel.atmDensity; // Get atmospheric density

        Debug.Log($"[KiverixScorch] Checking Reentry - Velocity: {velocity}, Atmosphere Density: {atmoDensity}");

        return velocity > 2800 && atmoDensity > 0.02; // Adjusted for KSRSS
    }

    private IEnumerator ApplyScorchOverTime()
    {
        while (scorchLevel < 1f)
        {
            scorchLevel += Time.deltaTime * 0.05f;
            ApplyScorchMarks();
            yield return null;
        }
    }

    private void ApplyScorchMarks()
    {
        if (partMaterial != null)
        {
            partMaterial.SetFloat("_DetailBlend", scorchLevel);
            Debug.Log($"[KiverixScorch] Scorch Level Updated: {scorchLevel}");
        }
    }
}
