using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject cam; // ī�޶� ������Ʈ
    public float speed = 5.0f;  // �̵� �ӵ�
    public float jumpPower = 5.0f;
    public float gravity = 10.0f; // �߷�

    private CharacterController characterController;
    private Animator animator; // �ִϸ����� ����
    private Vector3 moveDirection; // ĳ������ �����̴� ����
    private bool jumpButtonPressed;  // ���� ���� ��ư ���� ����
    private bool flyingMode;  // ��۶��̴� ��� ����

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ������ ������
        moveDirection = Vector3.zero;
    }

    void Update()
    {
        // �Է� ó��
        if (cam != null)
        {
            var offset = cam.transform.forward;
            offset.y = 0;
            transform.LookAt(transform.position + offset);
        }

        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (!jumpButtonPressed && Input.GetButton("Jump"))
            {
                jumpButtonPressed = true;
                moveDirection.y = jumpPower;
            }
        }
        else
        {
            if (moveDirection.y < 0 && !jumpButtonPressed && Input.GetButton("Jump"))
            {
                flyingMode = true;
            }

            if (flyingMode)
            {
                jumpButtonPressed = true;
                moveDirection.y *= 0.95f;
                if (moveDirection.y > -1) moveDirection.y = -1;
                moveDirection.x = Input.GetAxis("Horizontal");
                moveDirection.z = Input.GetAxis("Vertical");
            }
            else
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
        }

        if (!Input.GetButton("Jump"))
        {
            jumpButtonPressed = false;
            flyingMode = false;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // �ִϸ��̼� ó��
        bool isWalking = moveDirection.x != 0f || moveDirection.z != 0f;
        animator.SetBool("IsWalking", isWalking);
    }
}