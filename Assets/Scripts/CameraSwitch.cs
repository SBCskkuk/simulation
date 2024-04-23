using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Transform thirdPersonPosition; // 3��Ī ī�޶� ��ġ
    public Transform firstPersonPosition; // 1��Ī ī�޶� ��ġ
    public Transform cameraTransform; // ���� ī�޶��� Transform

    private bool isFirstPerson = false; // ���� ī�޶� ���¸� ����

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson; // ī�޶� ���� ��ȯ
            SwitchView();
        }
    }

    void SwitchView()
    {
        if (isFirstPerson)
        {
            // 1��Ī �������� ��ȯ
            cameraTransform.position = firstPersonPosition.position;
            cameraTransform.rotation = firstPersonPosition.rotation;
        }
        else
        {
            // 3��Ī �������� ��ȯ
            cameraTransform.position = thirdPersonPosition.position;
            cameraTransform.rotation = thirdPersonPosition.rotation;
        }
    }
}
