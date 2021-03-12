using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    #region SERIALIZED FIELDS
    [SerializeField]
    private float speed = 600.0f;
    [SerializeField]
    private float turnSpeed = 400.0f;
    [SerializeField]
    private float gravity = 20.0f;
    [SerializeField]
    private float jumpHeight = 5f;
    #endregion

    #region PRIVATE FIELDS
    private Vector3 moveDirection = Vector3.zero;
    private bool toGround;
    private bool jump;

    private Animator animator;
    private float prevSpeed;
    private CharacterController controller;
    #endregion

    #region MONO BEHAVIOURS
    void Start()
    {
        prevSpeed = speed;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (transform.position.y < -1)
            SceneManager.LoadScene("waveformScene");
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            moveDirection.y = jumpHeight;
            jump = true;
        }
        else if (controller.isGrounded)
        {
            if (toGround)
            {
                jump = false;
                toGround = false;
            }
            moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
        }
        if (Input.GetKey("w"))
        {
            animator.SetBool("run", true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 3;
            }
            else
                speed = prevSpeed;
        }
        else
        {
            animator.SetBool("run", false);
        }
        


        float turn = Input.GetAxis("Horizontal");
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
        controller.Move(moveDirection * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;
    }
    #endregion
}
