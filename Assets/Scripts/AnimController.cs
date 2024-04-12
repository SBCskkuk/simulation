using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator animator; // �ִϸ����� ������Ʈ�� ������ ����
    private float movementSpeed; // ĳ������ ������ �ӵ��� ������ ����

    void Start()
    {
        animator = GetComponent<Animator>(); // ������ �� �ִϸ����� ������Ʈ�� �����ɴϴ�.
    }

    void Update()
    {
        // �÷��̾� �Է� �ޱ�
        float horizontal = Input.GetAxis("Horizontal"); // ���� �Է�
        float vertical = Input.GetAxis("Vertical"); // ���� �Է�
        movementSpeed = Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical)); // ���� ū �Է� ���� �ӵ��� ���

        // �ִϸ����Ϳ� �ӵ� �� ����
        animator.SetFloat("Speed", movementSpeed);

        // ĳ���� ������ ó��
        Vector3 movement = new Vector3(horizontal, 0, vertical) * Time.deltaTime;
        transform.Translate(movement, Space.World); // ĳ���� ��ġ �̵�

        // �ִϸ��̼� ���� ����
        if (movementSpeed > 0)
        {
            animator.SetBool("IsWalking", true); // �����̸� walk �ִϸ��̼�
        }
        else
        {
            animator.SetBool("IsWalking", false); // �����ϸ� idle �ִϸ��̼�
        }
    }
}