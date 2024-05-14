using System;
using UnityEngine;

[Serializable]
public class SoundInfo
{
    public string name;
    public AudioClip clip;
    public float minVolume;
    public float maxVolume;
    public float minPitch;
    public float maxPitch;
    public float baseRange;
    public SurfaceType surfaceType;

    public enum SurfaceType
    {
        NA,
        Grass,
        Gravel,
        Metal,
        Concrete,
        Wood
    }
}
