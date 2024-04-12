using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject cam; // 카메라 오브젝트
    public float speed = 5.0f;  // 이동 속도
    public float jumpPower = 5.0f;
    public float gravity = 10.0f; // 중력

    private CharacterController characterController;
    private Animator animator; // 애니메이터 참조
    private Vector3 moveDirection; // 캐릭터의 움직이는 방향
    private bool jumpButtonPressed;  // 최종 점프 버튼 눌림 상태
    private bool flyingMode;  // 행글라이더 모드 여부

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 참조를 가져옴
        moveDirection = Vector3.zero;
    }

    void Update()
    {
        // 입력 처리
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

        // 애니메이션 처리
        bool isWalking = moveDirection.x != 0f || moveDirection.z != 0f;
        animator.SetBool("IsWalking", isWalking);
    }
}