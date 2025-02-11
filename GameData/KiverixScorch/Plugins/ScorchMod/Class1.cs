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

        // Get the part's renderer component to access its material
        Renderer renderer = part.FindModelComponent<Renderer>();
        if (renderer != null)
        {
            partMaterial = renderer.material;

            // Load the correct scorch textures
            scorchTextureUV1 = GameDatabase.Instance.GetTexture("KiverixScorch/Textures/shuttleUV1_diff_Discovery", false);
            scorchTextureUV2 = GameDatabase.Instance.GetTexture("KiverixScorch/Textures/shuttleUV2_diff_Discovery", false);

            // Apply the initial texture and ensure the scorch effect is invisible at start
            if (scorchTextureUV1 != null && scorchTextureUV2 != null)
            {
                partMaterial.SetTexture("_DetailTex", scorchTextureUV1); // Default to UV1 texture
                partMaterial.SetFloat("_DetailBlend", 0f);  // Start with no scorch marks
            }
        }
    }

    public void Update()
    {
        // Check if reentry conditions are met and start scorch effect if not already triggered
        if (IsReentryHappening() && !isScorching)
        {
            isScorching = true;
            StartCoroutine(ApplyScorchOverTime());
        }
    }

    private bool IsReentryHappening()
    {
        // Ensure vessel exists and is in an atmosphere
        if (vessel == null || !vessel.mainBody.atmosphere)
            return false;

        double velocity = vessel.srfSpeed; // Get surface speed
        double atmoDensity = vessel.atmDensity; // Get atmospheric density

        // Check if velocity and atmospheric density indicate reentry conditions
        return velocity > 2800 && atmoDensity > 0.02; // Adjusted for KSRSS reentry speeds
        Debug.Log($"[KiverixScorch] Velocity: {vessel.srfSpeed}, AtmoDensity: {vessel.atmDensity}");
    }

    private IEnumerator ApplyScorchOverTime()
    {
        // Gradually increase the scorch effect over time
        while (scorchLevel < 1f)
        {
            scorchLevel += Time.deltaTime * 0.05f;
            ApplyScorchMarks();
            yield return null;
        }
    }

    private void ApplyScorchMarks()
    {
        // Update the material property to make the scorch marks visible
        if (partMaterial != null)
        {
            partMaterial.SetFloat("_DetailBlend", scorchLevel); // Fade in scorch effect over time
        }
        Debug.Log($"[KiverixScorch] Scorch Level: {scorchLevel}");
    }
}
