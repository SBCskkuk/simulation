using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerBody; // ĳ������ Transform ����
    public float sensitivity = 500f; // ���콺 �ΰ���
    private float rotationX = 0f; // �ʱ� X�� ȸ�� ���� ���� (���� ����)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���� �� Ŀ�� ����
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // ���콺 X�� ������
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; // ���콺 Y�� ������

        rotationX -= mouseY; // ���� ȸ�� ���
        rotationX = Mathf.Clamp(rotationX, -75f, 60f); // �ΰ��� �þ� ������ ���� ���� ȸ���� -75������ 60���� ����

        transform.localEulerAngles = new Vector3(rotationX, playerBody.eulerAngles.y, 0f); // ī�޶��� ���� ȸ�� ����, ���� ȸ���� ĳ������ ȸ���� ����
        playerBody.Rotate(Vector3.up * mouseX); // ĳ������ ���� ȸ�� ����
    }
}
