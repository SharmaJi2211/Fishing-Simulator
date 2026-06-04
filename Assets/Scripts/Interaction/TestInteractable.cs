using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public void Interact() => Debug.Log("Interacted with " + gameObject.name);
    public string GetInteractPrompt() => "Press E to interact";
}