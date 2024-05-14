using System;
using UnityEngine;

public static class SoundEmitter
{ 
    public static void EmitSound(SpatialSound spatialSound)
    {
        // Get all colliders within sphere with the spatial sound range
        Collider[] col = Physics.OverlapSphere(spatialSound.pos, spatialSound.range);

        // Loop through colliders and try-get IHear component
        for (int i = 0; i < col.Length; i++)
        {
            if (!col[i].TryGetComponent(out IHear hearer)) continue;

            // Allow the hearer to respond to the sound
            hearer.RespondToSound(spatialSound);
        }
    }
}
