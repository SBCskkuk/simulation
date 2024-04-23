using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Transform thirdPersonPosition; // 3인칭 카메라 위치
    public Transform firstPersonPosition; // 1인칭 카메라 위치
    public Transform cameraTransform; // 실제 카메라의 Transform

    private bool isFirstPerson = false; // 현재 카메라 상태를 저장

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson; // 카메라 상태 전환
            SwitchView();
        }
    }

    void SwitchView()
    {
        if (isFirstPerson)
        {
            // 1인칭 시점으로 전환
            cameraTransform.position = firstPersonPosition.position;
            cameraTransform.rotation = firstPersonPosition.rotation;
        }
        else
        {
            // 3인칭 시점으로 전환
            cameraTransform.position = thirdPersonPosition.position;
            cameraTransform.rotation = thirdPersonPosition.rotation;
        }
    }
}
