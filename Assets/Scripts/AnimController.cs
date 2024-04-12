using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator animator; // 애니메이터 컴포넌트를 참조할 변수
    private float movementSpeed; // 캐릭터의 움직임 속도를 저장할 변수

    void Start()
    {
        animator = GetComponent<Animator>(); // 시작할 때 애니메이터 컴포넌트를 가져옵니다.
    }

    void Update()
    {
        // 플레이어 입력 받기
        float horizontal = Input.GetAxis("Horizontal"); // 수평 입력
        float vertical = Input.GetAxis("Vertical"); // 수직 입력
        movementSpeed = Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical)); // 가장 큰 입력 값을 속도로 사용

        // 애니메이터에 속도 값 설정
        animator.SetFloat("Speed", movementSpeed);

        // 캐릭터 움직임 처리
        Vector3 movement = new Vector3(horizontal, 0, vertical) * Time.deltaTime;
        transform.Translate(movement, Space.World); // 캐릭터 위치 이동

        // 애니메이션 상태 제어
        if (movementSpeed > 0)
        {
            animator.SetBool("IsWalking", true); // 움직이면 walk 애니메이션
        }
        else
        {
            animator.SetBool("IsWalking", false); // 정지하면 idle 애니메이션
        }
    }
}