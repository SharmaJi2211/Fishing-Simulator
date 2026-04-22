using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;

    Camera mainCamera;
    bool canMove = true;

    // Called via Event manager to lock/unlock Movement
    public void SetCanMove(bool value) => canMove = value;

    void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!canMove) return;

        // For the Movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = (transform.forward * v + transform.right * h).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            // Smooth the Rotation of the Player
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), rotationSpeed * Time.deltaTime);

            // Moves the Player
            transform.Translate(moveDir * movementSpeed * Time.deltaTime, Space.World);
        }
    }

    void OnEnable()
    {
        GameEvents.OnPlayerMoveToggled += SetCanMove;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerMoveToggled -= SetCanMove;
    }
}
