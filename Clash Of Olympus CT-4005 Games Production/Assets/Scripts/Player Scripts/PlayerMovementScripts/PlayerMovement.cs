using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    #region Variable declerations [SerializeFields]
    [SerializeField]
    private float slowDownSpeed = 0.45f;
    [SerializeField]
    private float playerSpeed = 5f;
    #endregion

    #region Variable declerations private

    #endregion

    //Ignore //////
    #region Function(s)
    void Movement () {
        #region Basic WASD movement
        /*//Creates a tiny sphere where the game object is within the player
        //If the sphere collides with anything in the ground mask grounded will equal to true, if not, it will be false.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Ground check to reset the velocity back to 0 as long as the player is stood on the "ground"
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Direction in which the player will want to move on
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);*/
        #endregion

        #region Player Movement w/ Rotation
        /*//Creates a tiny sphere where the game object is within the player
        //If the sphere collides with anything in the ground mask grounded will equal to true, if not, it will be false.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Ground check to reset the velocity back to 0 as long as the player is stood on the "ground"
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        velocity.y += gravity * Time.deltaTime;

        if (direction.magnitude >= 0.1f) {
            //Allows the player to rotate and rotate in the direction that it is moving
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //Smoothing of the rotation so that it isn't jagged and looks unrealistic.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotSpeed);
            //Allows to input the numbers around each axis 
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Character movement and making sure that the player has gravity applied to it.
            controller.Move(direction * speed * Time.deltaTime);
            controller.Move(velocity * Time.deltaTime);
        }
    }*/

        #endregion
    }
    #endregion
    //Ignore //////

    #region Region for the player controls (controller & keyboard)

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool moveNow = true;
    private float gravityValue = -9.81f;
    private Vector2 movementInput = Vector2.zero;
    private Vector3 addedVelocity = Vector3.zero;

    private void Start () {
        controller = gameObject.GetComponent<CharacterController>();
    }

    
    private void OnMovement (InputValue value) {
        movementInput = value.Get<Vector2>();
    }
    void Update () {
        if (moveNow)
        {
            //If player is grounded, set Y velocity to 0.
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            //Every frame, reduce player speed by slowDownSpeed, a float value between 0 and 1.
            playerVelocity -= playerVelocity * slowDownSpeed;
            //Take input from controller and rotate accordingly.
            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
            //Add input, gravity and extra velocity to velocity and move the player that amount.
            playerVelocity += move * playerSpeed * Time.deltaTime;
            playerVelocity.y = gravityValue * Time.deltaTime;
            playerVelocity += addedVelocity;
            controller.Move(playerVelocity);
            //Reset added velocity.
            addedVelocity = Vector3.zero;
        }
    }

    public void AddVelocity(Vector3 movementVector) {
        addedVelocity += movementVector;
    }

    public void SetBool(bool a_bBool)
    {
        moveNow = a_bBool;
    }

    #endregion
}
