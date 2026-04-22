using System.Collections;
using UnityEngine;

public class FishingManager : MonoBehaviour
{

    [SerializeField] FishData[] fishPool; // array of all FishData assets
    [SerializeField] float minWaitTime = 2f;
    [SerializeField] float maxWaitTime = 6f;
    [SerializeField] float biteWindow  = 1.5f;
    [SerializeField] float castDuration = 1f;

    FishingState currentState;
    FishData currentFish; // the fish we picked when player cast, used throughout Bite and Reeling
    

    void Start()
    {
        // when game starts set state to Idle.
        SetState(FishingState.Idle);
    }

    void SetState(FishingState newState)
    {
        currentState = newState;
        Debug.Log("State changed to: " + currentState);
        switch (currentState)
        {
            // when we enter Idle fire the event to unlock player
            case FishingState.Idle:
                GameEvents.PlayerMoveToggled(true);
                break;

            // when we enter Casting fire the event to lock player
            case FishingState.Casting:
                GameEvents.PlayerMoveToggled(false);
                StartCoroutine(CastRoutine());
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
        {
            GameEvents.FishEscaped();
            SetState(FishingState.Idle);
        }
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(2f);
        SetState(FishingState.Idle);
    }

    IEnumerator CastRoutine()
    {
        yield return new WaitForSeconds(castDuration);
        SetState(FishingState.Waiting);
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
    public void OnCastInput()
    {
        if (currentState == FishingState.Idle)
            SetState(FishingState.Casting);
    }

    // Works only when fish bit on the bait prevents unnecessary clicks
    public void OnClickInput()
    {
        if (currentState == FishingState.Bite)
            SetState(FishingState.Reeling);
    }

    // Subscribing the event
    void OnEnable()
    {
        GameEvents.OnCastInput += OnCastInput;
        GameEvents.OnClickInput += OnClickInput;
    }

    // Unsubscribing the event
    void OnDisable()
    {
        GameEvents.OnCastInput -= OnCastInput;
        GameEvents.OnClickInput -= OnClickInput;
    }
    
}