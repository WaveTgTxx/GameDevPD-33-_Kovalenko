using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Movement_Controller))]
public class PC_InputController : MonoBehaviour
{
    Movement_Controller playerMovement;
    float move;
    bool jump;
    bool crouch;

    private void Start()
    {
        playerMovement = GetComponent<Movement_Controller>();
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonUp("Jump"))
        {
            jump = true;
        }

        crouch = Input.GetKey(KeyCode.C);

        if (!IsPointerOverUI())
        {
            if (Input.GetButtonDown("Fire1") && move == 0 && crouch == false)
                playerMovement.StartShooting();
            else if (Input.GetButtonDown("Fire1") && Mathf.Abs(move) > 0 && crouch == false)
                playerMovement.StartRunShooting();
            else if (Input.GetButtonDown("Fire1") && crouch == true)
                playerMovement.StartCrouchShooting();
            else if (Input.GetButtonDown("Fire2") && move == 0 && crouch == false)
                playerMovement.StartPowerShooting();
            else if (Input.GetButtonDown("Fire2") && crouch == true)
                playerMovement.StartPowerShooting();
        }
    }

    private void FixedUpdate()
    {
        playerMovement.Move(move, jump, crouch);
        jump = false;
    }

    private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
}
