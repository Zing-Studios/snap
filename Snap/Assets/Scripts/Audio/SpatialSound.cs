using UnityEngine;

public class SpatialSound
{
    public readonly Vector3 pos;
    public float range;
    public SoundType type = SoundType.Default;

    public enum SoundType
    {
        Default = -1,
        Interesting,
        Player
    }

    public SpatialSound(Vector3 _pos, float _range, SoundType _type = SoundType.Default)
    {
        pos = _pos;
        range = _range;
        type = _type;
    }
}
