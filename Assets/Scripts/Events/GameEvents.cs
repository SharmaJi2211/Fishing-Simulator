using System;

public static class GameEvents
{
    // This is what is used to call the OnPlayerMoveToggled in this case to pass a bool
    // Only using the function we can call this list outside this class
    // When called itll pass in the relative parameter to OnPlayerMoveToggled and Invoke it making that paramter to be passed to all the subscribed functions
    public static void PlayerMoveToggled(bool canMove) => OnPlayerMoveToggled?.Invoke(canMove);
    
    
    // A variable that holds a list of functions that are subscribed to it eg: GameEvents.OnPlayerMoveToggled += SetCanMove
    // After subscribing itll look like OnPlayerMoveToggled = [ SetCanMove, HandleMovement, etc ]
    // We can use the same variable for multiple actions but that could go messy if subscribers are expecting different inputs like some expect true while other false
    public static event Action<bool> OnPlayerMoveToggled;

    // Cast When player is Idle
    public static event Action OnCastInput;
    public static void CastInput() => OnCastInput?.Invoke();

    // Click Input when the fish bit on
    public static event Action OnClickInput;
    public static void ClickInput() => OnClickInput?.Invoke();

    public static event Action OnReelHoldDown;
    public static void ReelHoldDown() => OnReelHoldDown?.Invoke();

    public static event Action OnReelHoldUp;
    public static void ReelHoldUp() => OnReelHoldUp?.Invoke();



    public static event Action OnBite;
    public static void Bite() => OnBite?.Invoke();

    public static event Action<FishData> OnReelStarted;
    public static void ReelStarted(FishData fish) => OnReelStarted?.Invoke(fish);

    public static event Action OnFishEscaped;
    public static void FishEscaped() => OnFishEscaped?.Invoke(); 

    public static event Action OnFishCaught;
    public static void FishCaught() => OnFishCaught?.Invoke();

    // Fish UI Minigame Events
    public static event Action<float> OnFishIconPositionChanged;
    public static void FishIconPositionChanged(float pos) => OnFishIconPositionChanged?.Invoke(pos);

    public static event Action<float, float> OnZonePositionChanged;
    public static void ZonePositionChanged(float min, float max) => OnZonePositionChanged?.Invoke(min, max);

    public static event Action<float> OnProgressChanged;
    public static void ProgressChanged(float progress) => OnProgressChanged?.Invoke(progress);
}