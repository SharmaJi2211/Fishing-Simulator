using System;
using UnityEngine;

public static class InteractionEvent
{

    // Subscribe the event for the function you want to fire
    public static event Action OnInteractionPressed;

    // Fire the event using the Function i.e. InteractionEvent.InteractPressed
    public static void InteractPressed() => OnInteractionPressed?.Invoke();
}
