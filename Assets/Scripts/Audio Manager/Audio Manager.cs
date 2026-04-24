using Cinemachine;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] CinemachineImpulseSource impulseSource;

    [Header("Clips")]
    [SerializeField] AudioClip castClip;
    [SerializeField] AudioClip biteClip;
    [SerializeField] AudioClip successClip;
    [SerializeField] AudioClip failClip;

    void OnEnable()
    {
        GameEvents.OnHookLandedInWater += PlayCast;
        GameEvents.OnBite              += PlayBite;
        GameEvents.OnFishCaught        += PlaySuccess;
        GameEvents.OnFishEscaped       += PlayFail;
    }

    void OnDisable()
    {
        GameEvents.OnHookLandedInWater -= PlayCast;
        GameEvents.OnBite              -= PlayBite;
        GameEvents.OnFishCaught        -= PlaySuccess;
        GameEvents.OnFishEscaped       -= PlayFail;
    }

    void PlayCast() => audioSource.PlayOneShot(castClip);
    void PlayBite()
    {
        audioSource.PlayOneShot(biteClip);
        impulseSource.GenerateImpulse();
    }
    void PlaySuccess(FishData f) => audioSource.PlayOneShot(successClip);
    void PlayFail() => audioSource.PlayOneShot(failClip);
}