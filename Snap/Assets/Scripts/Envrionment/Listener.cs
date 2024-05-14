using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener : MonoBehaviour, IHear
{
    public List<Material> mats = new List<Material>();
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void RespondToSound(SpatialSound spatialSound)
    {
        renderer.material = mats[1];
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        renderer.material = mats[0];
    }
}
