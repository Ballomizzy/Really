using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private float moveVelocity = 10f, 
        rotateXSpeed = 30f, 
        rotateXValue,
        jumpForce = 10f;

    private GameManager gameManager;

    private bool isOnFloor = true, isWalking, hasWalked;

    [SerializeField]
    private Vector3 moveDir;

    [SerializeField]
    private Rigidbody rb;
    private CharacterController controller;
    private Animator anim;

    private GameSettings gameSettings;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        controller = GetComponent<CharacterController>();
        gameSettings = FindObjectOfType<GameSettings>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        /*if(transform.position.y > 23)
        {
            transform.position = new Vector3(transform.position.x, 23, transform.position.z);
        }*/
        if(gameManager.gameState == GameManager.GameState.Playing)
        {
            Move();
            PanView();
            isOnFloor = CheckIfOnFloor();
        }
    }

    public void FixedUpdate()
    {
        //if(gameManager.gameState == GameManager.GameState.Playing && rb != null && Input.GetButtonDown("Jump") && isOnFloor)
            //Jump();
    }

    private void Move()
    {
        //transform.position += (transform.TransformDirection(Input.GetAxis("Horizontal") * moveVelocity, 0, Input.GetAxis("Vertical") * moveVelocity)) * Time.deltaTime;
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(transform.TransformDirection(moveDir * Time.deltaTime * moveVelocity));

        if (moveDir.x > 0.1f || moveDir.z > 0.1f || moveDir.x < -0.1f || moveDir.z < -0.1f)
        {
            isWalking = true;
            if (!hasWalked)
            {
                anim.SetTrigger("walk");
                hasWalked = true;
            }
        }
        else
        {
            isWalking = false;
            if (hasWalked)
            {
                anim.SetTrigger("stopWalk");
                hasWalked = false;
            }
        }

       
    }
    private void PanView()
    {
        rotateXValue += Input.GetAxis("Mouse X");
        transform.localRotation = Quaternion.Euler(0, rotateXValue * rotateXSpeed * gameSettings.GetGameSensititvityValue(), 0);
    }
    private bool CheckIfOnFloor()
    {
        if (transform.position.y < 1.3f)
            return true;
        else
            return false;
    }
    private void Jump()
    {
        rb.AddForce(0, jumpForce, 0);
    }
}
