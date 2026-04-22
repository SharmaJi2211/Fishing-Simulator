using System.Collections;
using UnityEngine;

public class ReelMechanic : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float pullForce    = 1.8f;  // how fast icon moves up when holding
    [SerializeField] float driftForce   = 1.2f;  // how fast icon drifts down when released
    [SerializeField] float zoneSpeed    = 0.8f;  // how fast zone moves to new position
    [SerializeField] float struggleInterval = 2f; // seconds between zone movements

    float fishPos;      // current icon position 0-1
    float zoneMin;      // bottom of Current zone 0-1
    float zoneMax;  
    float zoneTargetMin;// next Zone
    float zoneTargetMax;
    float progress;     // catch progress 0-1
    bool  holding;      // is LMB held
    bool  active;       // is minigame running
    FishData currentFish;

    void OnHoldDown()
    {
        if (!active) return;  // ignore if minigame not running
        holding = true;
    }

    public void OnHoldUp()
    {
        if (!active) return;
        holding = false;
    }

    void StartReel(FishData fish)
    {
        currentFish = fish;
        fishPos     = 0.5f;  // start icon in middle
        progress    = 0f;
        holding     = false;
        active      = true;

        // place zone in random starting position
        float width = currentFish.zoneWidth;
        zoneMin = Random.Range(0f, 1f - width); // Zone always stays fully inside the 0-1 track when subtracted with width
        zoneMax = zoneMin + width;
        zoneTargetMin = zoneMin;
        zoneTargetMax = zoneMax;

        StartCoroutine(StruggleRoutine());
    }

    IEnumerator StruggleRoutine()
    {
        while (active)
        {
            yield return new WaitForSeconds(currentFish.struggleInterval);
            
            if (!active) yield break;
            
            // pick new target position for zone
            float width = currentFish.zoneWidth;
            zoneTargetMin = Random.Range(0f, 1f - width);
            zoneTargetMax = zoneTargetMin + width;
        }
    }

    void OnEnable()
    {
        GameEvents.OnReelStarted += StartReel;
        GameEvents.OnReelHoldDown += OnHoldDown;
        GameEvents.OnReelHoldUp += OnHoldUp;
    }

    void OnDisable()
    {
        GameEvents.OnReelStarted -= StartReel;
        GameEvents.OnReelHoldDown -= OnHoldDown;
        GameEvents.OnReelHoldUp -= OnHoldUp;
    }
}