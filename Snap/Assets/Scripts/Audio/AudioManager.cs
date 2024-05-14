using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySoundOneShot(AudioSource source, SpatialSound spatialSound = null, bool overrideSource = true)
    {
        // Play sound always if override is on, else, only play sound if not already playing
        if (!overrideSource && source.isPlaying) return;

        // Emit spatial sound to environment if exists
        if (spatialSound != null) SoundEmitter.EmitSound(spatialSound);

        // Play audio clip
        source.PlayOneShot(source.clip);
    }
    public void StopPlaying(AudioSource source)
    {
        source.Stop();
    }
}
