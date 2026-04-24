using System.Collections;
using UnityEngine;


public class ReelMechanic : MonoBehaviour
{
    [Header("Catching Bar Feel")]
    [SerializeField] float pullForce  = 1.8f;
    [SerializeField] float driftForce = 1.2f;

    [Header("Fish Movement")]
    [SerializeField] float fishMoveSpeed = 4f;
    [SerializeField] float fishMinWait   = 0.4f;
    [SerializeField] float fishMaxWait   = 1.6f;

    float fishPos;
    float fishDestination;
    float barPos;
    float barSize;
    float progress;
    bool  holding;
    bool  active;
    bool canFail;           // check for failure after progress has had a chance to rise

    FishData currentFish;

    void OnEnable()
    {
        GameEvents.OnReelStarted  += StartReel;
        GameEvents.OnReelHoldDown += OnHoldDown;
        GameEvents.OnReelHoldUp   += OnHoldUp;
    }

    void OnDisable()
    {
        GameEvents.OnReelStarted  -= StartReel;
        GameEvents.OnReelHoldDown -= OnHoldDown;
        GameEvents.OnReelHoldUp   -= OnHoldUp;
    }

    void StartReel(FishData fish)
    {
        canFail = false;
        Invoke(nameof(AllowFail), 1.5f);

        currentFish     = fish;
        fishPos         = 0.5f;
        fishDestination = 0.5f;
        barPos          = 0.3f;
        barSize         = currentFish.zoneWidth;
        progress        = 0f;
        holding         = false;
        active          = true;

        StartCoroutine(FishMovementRoutine());
    }

    void Update()
    {
        if (!active) return;

        // Fish smoothly lerps toward its random destination
        fishPos = Mathf.Lerp(fishPos, fishDestination, fishMoveSpeed * Time.deltaTime);

        // Space = up, release = gravity down
        if (holding) barPos += pullForce  * Time.deltaTime;
        else         barPos -= driftForce * Time.deltaTime;
        

        // ensures the bottom of the bar plus its height never goes past the top of the track
        barPos = Mathf.Clamp(barPos, 0f, 1f - barSize);

        // Overlap check is fish inside the catching bar
        bool inBar = fishPos >= barPos && fishPos <= (barPos + barSize);

        if (inBar) progress += currentFish.catchFillRate  * Time.deltaTime;
        else       progress -= currentFish.catchDrainRate * Time.deltaTime;
        progress = Mathf.Clamp01(progress);

        // Broadcast every frame to UIManager
        GameEvents.FishIconPositionChanged(fishPos);
        GameEvents.ZonePositionChanged(barPos, barPos + barSize);
        GameEvents.ProgressChanged(progress);

        if (progress >= 1f) EndReel(true);
        if (progress <= 0f && canFail) EndReel(false);
    }

    // Fish picks a new random destination every few seconds
    IEnumerator FishMovementRoutine()
    {
        while (active)
        {
            yield return new WaitForSeconds(Random.Range(fishMinWait, fishMaxWait));
            if (!active) yield break;
            fishDestination = Random.Range(0.05f, 0.95f);
        }
    }

    void OnHoldDown() { if (active) holding = true;  }
    void OnHoldUp()   { if (active) holding = false; }
    void AllowFail() => canFail = true;

    void EndReel(bool success)
    {
        active = false;
        canFail  = false;
        StopAllCoroutines();
        holding = false;
        if (success) GameEvents.FishCaught(currentFish);
        else         GameEvents.FishEscaped();
    }
}
