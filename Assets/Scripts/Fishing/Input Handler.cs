using UnityEngine;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        // Cast or react to bite
        if (Input.GetMouseButtonDown(0))
            GameEvents.CastInput();
            
        // Fired when mouse button is pressed once
        if (Input.GetKeyDown(KeyCode.Space))
            GameEvents.ReelHoldDown();

        // Fired when mouse button is held down 
        if (Input.GetKeyUp(KeyCode.Space))
            GameEvents.ReelHoldUp();
    }
}