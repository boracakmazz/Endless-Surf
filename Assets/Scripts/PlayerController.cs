using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    private const float LANE_DISTANCE = 2.5f;
    private const float TURN_SPEED = 0.5f;

    private bool isPlayerRunning = false;

    private CharacterController controller; 

    //Movement
    [SerializeField] float jumpForce = 8.0f;
    [SerializeField] float gravity = 15.0f;
    private float verticalVelocity;

    //Speed Modifier
    [SerializeField] float originalSpeed = 7.0f; //In order to keep what was our original speed since it changes over time;
    private float speed;
    private float speedIncreaseLastTime;
    private float speedIncreaseTime = 5.0f;
    private float speedIncreaseAmount = 0.1f;

    private int switchLane = 1; // 0 = Left, 1 = Middle, 2 = Right

    // Start is called before the first frame update
    void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerRunning)
            return;

        if(Time.time - speedIncreaseLastTime > speedIncreaseTime) //Speeds up over time
        {
            speedIncreaseLastTime = Time.time;
            speed += speedIncreaseAmount;
            GameManager.Instance.UpdateModifier(speed - originalSpeed);
        }

        if (MobileInput.Instance.SwipeLeft)//Move Left
            moveLane(false);

        if (MobileInput.Instance.SwipeRight)//Move Right
            moveLane(true); 

        Vector3 targetPosition = transform.position.z * Vector3.forward;

        if (switchLane == 0)  //future position
            targetPosition += Vector3.left * LANE_DISTANCE;
        else if (switchLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;

        movePlayer(targetPosition);
        playerRotation();
    }

    private void moveLane(bool goingRight)
    {
        /* if (!goingRight)
         {
             switchLane--;
             if (switchLane == -1)
                 switchLane = 0;
         }
         else
         {
             switchLane++;
             if (switchLane == 3)
                 switchLane = 2;
         }   */

        switchLane += (goingRight) ? 1 : -1;    //Optimized version of the upper code; Checks if player going right, if it is false switch lane to left else switch other direciton
        switchLane = Mathf.Clamp(switchLane, 0, 2); //Restricts the value between 0 and 2
    }

    private void movePlayer(Vector3 targetPosition)
    {
        Vector3 moveVector = Vector3.zero;  //Calculating delta position
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;
        
        if (isGrounded())
        {
            verticalVelocity = -0.1f;
            if (MobileInput.Instance.SwipeUp)
            {
                verticalVelocity = jumpForce;
            }
           
        }
        
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            if (MobileInput.Instance.SwipeDown)
            {
                verticalVelocity -= jumpForce;
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime);
    }

    private bool isGrounded()   //Checks if the player model is on the ground
    {
        Ray groundRay = new Ray(new Vector3(
            controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
            controller.bounds.center.z),
            Vector3.down);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
    }

    private void playerRotation()   //Rotate the face of the player model when it switches direction.
    {
        Vector3 direction = controller.velocity;

        if (direction != Vector3.zero)
        {
            direction.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, direction, TURN_SPEED);
        }
   
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                crash();
                break;
            
        }
    }


    private void crash()
    {
        isPlayerRunning = false;
        GameManager.Instance.onDeath();
        
    }

    public void startGame()
    {
        isPlayerRunning = true;
    }
   
}
