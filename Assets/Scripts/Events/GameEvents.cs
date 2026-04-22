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


    public static event Action OnFishEscaped;
    public static void FishEscaped() => OnFishEscaped?.Invoke();    
}