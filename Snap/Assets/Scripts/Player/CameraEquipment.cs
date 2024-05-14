using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEquipment : MonoBehaviour, IUsable
{
    [Header("Camera")]
    [SerializeField] private Camera cameraSource;
    [SerializeField] private Transform cameraFocusPosition;
    public SoundInfo soundInfo;
    [SerializeField] private int width;
    [SerializeField] private int height;

    [Header("Flash")]
    [SerializeField] private GameObject flash;
    [SerializeField] private float flashBrightness;
    [SerializeField] private float flashDuration;
    [SerializeField] private float cooldown;

    private AudioManager audioManager;
    private AudioSource audioSource;
    private Texture2D capture;
    private bool canUse = true;

    private void Start()
    {
        Light spotlight = flash.GetComponent<Light>();
        spotlight.intensity = flashBrightness;

        audioManager = AudioManager.instance;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundInfo.clip;
    }

    public void Use()
    {
        // Return if the camera cannot be used (If just took photo)
        if (!canUse) return;

        StartCoroutine(TakePhoto());
    }

    public void HideFlash()
    {
        flash.SetActive(false);
    }

    private IEnumerator TakePhoto()
    {
        canUse = false;
        flash.SetActive(true);
        yield return new WaitForSeconds(0.01f);

        // Check for artifact
        PlayShutterSound();

        yield return new WaitForSeconds(flashDuration);
        flash.SetActive(false);
        yield return new WaitForSeconds(cooldown);
        canUse = true;
    }

    private void PlayShutterSound()
    {
        // Get volume and pitch parameters
        audioSource.volume = Random.Range(soundInfo.minVolume, soundInfo.maxVolume);
        audioSource.pitch = Random.Range(soundInfo.minPitch, soundInfo.maxPitch);

        // Calculate spatial range of the camera shutter
        float range = soundInfo.baseRange * (audioSource.volume + 0.5f);

        // Create a new spatial sound using the shutter sound
        SpatialSound spatialSound = new SpatialSound(transform.position, range);

        // Play the shutter sound
        audioManager.PlaySoundOneShot(audioSource, spatialSound, false);
    }

    private void CheckForArtifact()
    {
        GameObject[] artifacts = GameObject.FindGameObjectsWithTag("Artifacts");

        foreach (GameObject artifact in artifacts)
        {
            // Photograph artifact
        }
    }

    public void Focus()
    {
        gameObject.transform.DOLocalMove(cameraFocusPosition.localPosition, 0.2f).SetEase(Ease.InOutFlash);
    }

    public void Unfocus()
    {
        gameObject.transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.InOutFlash);
    }
}
