/*using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    internal bool isGrounded;
    private Animator animator; // �ִϸ����� ����

    internal void Move(Vector3 vector3)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ������ ������
    }

    void Update()
    {
        // �ȱ� �Է��� �޾��� �� (��: "W" Ű�� ������ ���� ��)
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("IsWalking", true); // "IsWalking" �Ҹ��� �Ķ���͸� ������ ����
        }
        else
        {
            animator.SetBool("IsWalking", false); // "IsWalking" �Ҹ��� �Ķ���͸� �������� ����
        }
    }
}
*/