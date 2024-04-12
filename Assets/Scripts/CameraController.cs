using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerBody; // 캐릭터의 Transform 참조
    public float sensitivity = 500f; // 마우스 민감도
    private float rotationX = 0f; // 초기 X축 회전 각도 설정 (시작 각도)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 게임 중 커서 숨김
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // 마우스 X축 움직임
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; // 마우스 Y축 움직임

        rotationX -= mouseY; // 수직 회전 계산
        rotationX = Mathf.Clamp(rotationX, -75f, 60f); // 인간의 시야 범위에 맞춰 수직 회전을 -75도에서 60도로 제한

        transform.localEulerAngles = new Vector3(rotationX, playerBody.eulerAngles.y, 0f); // 카메라의 수직 회전 적용, 수평 회전은 캐릭터의 회전을 따름
        playerBody.Rotate(Vector3.up * mouseX); // 캐릭터의 수평 회전 적용
    }
}
