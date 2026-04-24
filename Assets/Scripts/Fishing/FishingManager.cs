using System.Collections;
using UnityEngine;

public class FishingManager : MonoBehaviour
{

    [SerializeField] FishData[] fishPool; // array of all FishData assets
    [SerializeField] float minWaitTime = 2f;
    [SerializeField] float maxWaitTime = 6f;
    [SerializeField] float biteWindow  = 1.5f;

    FishingState currentState;
    FishData currentFish; // the fish we picked when player cast, used throughout Bite and Reeling
    bool hookLanded = false;
    


    void Start()
    {
        // when game starts set state to Idle.
        SetState(FishingState.Idle);
    }



    void SetState(FishingState newState)
    {
        currentState = newState;
        GameEvents.FishingStateChanged(currentState);
        switch (currentState)
        {
            // when we enter Idle fire the event to unlock player
            case FishingState.Idle:
                GameEvents.PlayerMoveToggled(true);
                break;

            // when we enter Casting fire the event to lock player
            case FishingState.Casting:
                GameEvents.PlayerMoveToggled(false);
                StartCoroutine(CastTimeout());
                break;

            // When we enter waiting state wait for the fish to bite on
            case FishingState.Waiting:
                StartCoroutine(WaitForBite());
                break;
            
            // When we are in Bite State alert the player, start reaction window timer
            case FishingState.Bite:
                GameEvents.Bite();
                StartCoroutine(BiteWindow());
                break;

            // In reeling state start the minigame using the fish we already picked
            case FishingState.Reeling:
                GameEvents.ReelStarted(currentFish);
                break;

            // In Result we simply go back to idle state
            case FishingState.Result:
                StartCoroutine(ReturnToIdle());
                break;
        }
    }


    // If neither HookLandedInWater nor HookLandedOnGround fires within 3 seconds it forces back to Idle
    IEnumerator CastTimeout()
    {
        hookLanded = false;
        yield return new WaitForSeconds(3f);
        if (currentState == FishingState.Casting && !hookLanded)
            SetState(FishingState.Idle);
    }

    // The coroutine lets us pause for a random amount of time first then change state
    IEnumerator WaitForBite()
    {
        // picks a random fish the moment player casts
        currentFish = PickFish();

        // picks a random wait time
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        SetState(FishingState.Bite);
    }



    IEnumerator BiteWindow()
    {
        yield return new WaitForSeconds(biteWindow);
        
        if (currentState == FishingState.Bite)
            GameEvents.FishEscaped();
    }



    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(2f);
        SetState(FishingState.Idle);
    }



    void HandleHookInWater()
    {
        Debug.Log("Hook landed in water");
        if (currentState == FishingState.Casting)
        {
            hookLanded = true;
            SetState(FishingState.Waiting);
        }
    }

    void HandleHookOnGround()
    {
        Debug.Log("Hook landed on ground");
        if (currentState == FishingState.Casting)
        {
            hookLanded = true;
            SetState(FishingState.Idle);
        }
    }



    // Chooses up a random fish from the pool to bite onto the bait
    FishData PickFish()
    {
        int totalWeight = 0;

        for(int i = 0; i < fishPool.Length; i++)
        {
            totalWeight += fishPool[i].spawnWeight;
        }
        int roll = Random.Range(0, totalWeight);
        
        for(int i = 0; i < fishPool.Length; i++)
        {
            if(roll < fishPool[i].spawnWeight)
                return fishPool[i];  // this fish owns the roll
            
            roll -= fishPool[i].spawnWeight;  // not this fish, subtract and check next
        }
    
        return fishPool[0];
    }



    // Works only when idle lets player cast the fishing rod
    public void HandleCastInput()
    {
        if (currentState == FishingState.Idle)
            SetState(FishingState.Casting);
        else if (currentState == FishingState.Bite)
            SetState(FishingState.Reeling);
    }


    void HandleFishCaught(FishData fish) => SetState(FishingState.Result);
    void HandleFishEscaped() => SetState(FishingState.Idle);



    // Subscribing the event
    void OnEnable()
    {
        GameEvents.OnCastInput += HandleCastInput;
        GameEvents.OnFishCaught += HandleFishCaught;
        GameEvents.OnFishEscaped += HandleFishEscaped;
        GameEvents.OnHookLandedInWater  += HandleHookInWater;
        GameEvents.OnHookLandedOnGround += HandleHookOnGround;
    }

    // Unsubscribing the event
    void OnDisable()
    {
        GameEvents.OnCastInput -= HandleCastInput;
        GameEvents.OnFishCaught -= HandleFishCaught;
        GameEvents.OnFishEscaped -= HandleFishEscaped;
        GameEvents.OnHookLandedInWater  -= HandleHookInWater;
        GameEvents.OnHookLandedOnGround -= HandleHookOnGround;
    }
    
}