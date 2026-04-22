using System.Collections;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    FishingState currentState;
    FishData currentFish; // the fish we picked when player cast, used throughout Bite and Reeling

    [SerializeField] FishData[] fishPool; // array of all FishData assets
    [SerializeField] float minWaitTime = 2f;
    [SerializeField] float maxWaitTime = 6f;
    [SerializeField] float biteWindow  = 1.5f;


    void Start()
    {
        // when game starts set state to Idle.
        SetState(FishingState.Idle);
    }

    void SetState(FishingState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            // when we enter Idle fire the event to unlock player
            case FishingState.Idle:
                GameEvents.PlayerMoveToggled(true);
                break;

            // when we enter Casting fire the event to lock player
            case FishingState.Casting:
                GameEvents.PlayerMoveToggled(false);
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
}