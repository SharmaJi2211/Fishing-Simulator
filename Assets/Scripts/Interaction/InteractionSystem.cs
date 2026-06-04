using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    // For OverlapSphere
    [SerializeField] private float sphereRadius;

    // For Layer
    [SerializeField] LayerMask interactableLayer;

    // For Raycast
    [SerializeField] private float raycastMaxDistance;
    [SerializeField] private Transform raycastOrigin;

    
    private IInteractable currentInteractable;

    void Update()
    {
        FindInteractable();
    }

    void FindInteractable()
    {
        // Works every frame
        // Find all the interactables inside the overlapping sphere
        Collider[] hits = Physics.OverlapSphere(transform.position, sphereRadius, interactableLayer);
        
        
        // Finds wht the player looking at
        bool hit = Physics.Raycast(raycastOrigin.position, Camera.main.transform.forward, out RaycastHit Hit, raycastMaxDistance,interactableLayer);

        if (hit)
        {
            // If Raycast hits something and player is looking at it then set the currectinteractble to that object 
            currentInteractable = Hit.collider.GetComponent<IInteractable>();
            Debug.Log("Raycast hit");
        }
        else
        {

            // Else picks the first object from the collider and marks it as CurrentInteractable
            if (hits.Length > 0)
            {
                currentInteractable = hits[0].GetComponent<IInteractable>();
                Debug.Log("Sphere hit");
            }
            else
                currentInteractable = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
    void HandleInteract()
    {
        // If currentinteractable has something then Interact function is called from inherited interface class
        currentInteractable?.Interact();
    }
    void OnEnable()
    {
        // Fires only if E/Dedicated button is pressed
        InteractionEvent.OnInteractionPressed += HandleInteract;
    }

    void OnDisable()
    {
        InteractionEvent.OnInteractionPressed -= HandleInteract;
    }
}
