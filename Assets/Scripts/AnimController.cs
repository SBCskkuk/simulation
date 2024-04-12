/*using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    internal bool isGrounded;
    private Animator animator; // 애니메이터 참조

    internal void Move(Vector3 vector3)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 참조를 가져옴
    }

    void Update()
    {
        // 걷기 입력을 받았을 때 (예: "W" 키를 누르고 있을 때)
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("IsWalking", true); // "IsWalking" 불리언 파라미터를 참으로 설정
        }
        else
        {
            animator.SetBool("IsWalking", false); // "IsWalking" 불리언 파라미터를 거짓으로 설정
        }
    }
}
*/