using UnityEngine;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        // Fired when mouse button is pressed once
        if (Input.GetKeyDown(KeyCode.Space))
            GameEvents.ReelHoldDown();

        // Fired when mouse button is held down 
        if (Input.GetKeyUp(KeyCode.Space))
            GameEvents.ReelHoldUp();
    }
}