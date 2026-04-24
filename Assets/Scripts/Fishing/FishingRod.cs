using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [SerializeField] Transform rodTip;              //  The line starts here and the hook rests here when idle
    [SerializeField] Transform hook;                // We move this through the air during the cast.
    [SerializeField] LineRenderer fishingLine;      // The LineRenderer component that draws the visible line

    [SerializeField] float castDistance = 8f;       // how far the hook flies
    [SerializeField] float castHeight   = 4f;       // how high the arc peaks
    [SerializeField] float castDuration = 0.8f;     // duration is how long the cast takes
    [SerializeField] LayerMask waterLayerMask;
    
    bool hookLanded = false;
    bool isCasting = false;
    FishingState currentState;
    Vector3 hookRestPosition;


    void Start()
    {
        hookRestPosition = rodTip.position;
        hook.position    = hookRestPosition;

        if (fishingLine != null)
        {
            fishingLine.positionCount = 2;
            fishingLine.startWidth    = 0.02f;
            fishingLine.endWidth      = 0.02f;
        }
        
        GameEvents.OnFishingStateChanged += OnStateChanged;
        GameEvents.OnFishEscaped += ReelHookBack;
        GameEvents.OnFishCaught  += OnFishCaught;
    }

    void OnDestroy()
    {
        GameEvents.OnFishEscaped -= ReelHookBack;
        GameEvents.OnFishCaught -= OnFishCaught;
        GameEvents.OnFishingStateChanged -= OnStateChanged;
    }

    void Update()
    {
        // Every single frame we update the line two points
        if (currentState == FishingState.Idle)
            hook.position = rodTip.position;

        if (fishingLine != null)
        {
            fishingLine.SetPosition(0, rodTip.position);
            fishingLine.SetPosition(1, hook.position);
        }
    }


    IEnumerator CastRoutine()
    {
        isCasting = true;
 
        // Cast target forward from camera, castDistance away
        Camera cam  = Camera.main;
        Vector3 start = rodTip.position;
        Vector3 target = cam.transform.position + cam.transform.forward * castDistance;
        target.y = rodTip.position.y; 
 
        float elapsed = 0f;
 
        while (elapsed < castDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / castDuration;
 
            // lerp position and add height curve
            Vector3 pos = Vector3.Lerp(start, target, t);
            pos.y += castHeight * Mathf.Sin(Mathf.PI * t);  // arc peak in middle
 
            hook.position = pos;
            yield return null;
        }
 
        // Hook has landed at target
        hook.position = target;
        
        if (Physics.Raycast(target + Vector3.up * 10f, Vector3.down, out RaycastHit groundHit, 20f, ~0, QueryTriggerInteraction.Collide))
        {
            target.y = groundHit.point.y;
            hook.position = target;
            
            bool hitWater = ((1 << groundHit.collider.gameObject.layer) & waterLayerMask) != 0;
            if (hitWater) GameEvents.HookLandedInWater();
            else GameEvents.HookLandedOnGround();
        }
        else
        {
            hook.position = target;
            GameEvents.HookLandedOnGround();
        }
        hookLanded = true;
        isCasting = false;
    }

    void ReelHookBack()
    {
        if (isCasting) return;
        hook.position = rodTip.position;
    }

    void OnFishCaught(FishData fish)
    {
        if (isCasting) return;
        hook.position = rodTip.position;
    }

    void OnStateChanged(FishingState state)
    {
        currentState = state;
        
        if (state == FishingState.Casting)
        {
            hookLanded = false;
            StartCoroutine(CastRoutine());
        }
    }
}
