using System;
using UnityEngine;


public static class GameEvents
{
    #region Player

    // This is what is used to call the OnPlayerMoveToggled in this case to pass a bool
    // Only using the function we can call this list outside this class
    // When called itll pass in the relative parameter to OnPlayerMoveToggled and Invoke it making that paramter to be passed to all the subscribed functions
    public static event Action<bool> OnPlayerMoveToggled;

    // A variable that holds a list of functions that are subscribed to it eg: GameEvents.OnPlayerMoveToggled += SetCanMove
    // After subscribing itll look like OnPlayerMoveToggled = [ SetCanMove, HandleMovement, etc ]
    // We can use the same variable for multiple actions but that could go messy if subscribers are expecting different inputs like some expect true while other false
    public static void PlayerMoveToggled(bool canMove) => OnPlayerMoveToggled?.Invoke(canMove);
    #endregion

    #region Input
    public static event Action OnCastInput;
    public static void CastInput() => OnCastInput?.Invoke();

    public static event Action OnReelHoldDown;
    public static void ReelHoldDown() => OnReelHoldDown?.Invoke();

    public static event Action OnReelHoldUp;
    public static void ReelHoldUp() => OnReelHoldUp?.Invoke();
    #endregion

    #region Fishing State
    public static event Action<FishingState> OnFishingStateChanged;
    public static void FishingStateChanged(FishingState state) => OnFishingStateChanged?.Invoke(state);
    #endregion

    #region Hook
    public static event Action OnHookLandedInWater;
    public static void HookLandedInWater() => OnHookLandedInWater?.Invoke();

    public static event Action OnHookLandedOnGround;
    public static void HookLandedOnGround() => OnHookLandedOnGround?.Invoke();
    #endregion

    #region Fishing Loop
    public static event Action OnBite;
    public static void Bite() => OnBite?.Invoke();

    public static event Action OnFishEscaped;
    public static void FishEscaped() => OnFishEscaped?.Invoke();

    public static event Action<FishData> OnFishCaught;
    public static void FishCaught(FishData fish) => OnFishCaught?.Invoke(fish);

    public static event Action<FishData> OnReelStarted;
    public static void ReelStarted(FishData fish) => OnReelStarted?.Invoke(fish);
    #endregion

    #region Reel Minigame UI
    public static event Action<float> OnFishIconPositionChanged;
    public static void FishIconPositionChanged(float pos) => OnFishIconPositionChanged?.Invoke(pos);

    public static event Action<float, float> OnZonePositionChanged;
    public static void ZonePositionChanged(float min, float max) => OnZonePositionChanged?.Invoke(min, max);

    public static event Action<float> OnProgressChanged;
    public static void ProgressChanged(float progress) => OnProgressChanged?.Invoke(progress);
    #endregion

    #region UI Notifications
    public static event Action<string, Color> OnNotifyMessage;
    public static void NotifyMessage(string msg, Color color) => OnNotifyMessage?.Invoke(msg, color);
    #endregion
}