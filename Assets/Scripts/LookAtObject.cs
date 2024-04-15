using UnityEngine;
using TMPro;

public class LookAtObject : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public TextMeshProUGUI[] textDisplays; // 메시지를 표시할 TextMeshProUGUI 객체 배열

    void Update()
    {
        RaycastHit hit;
        Vector3 forward = player.transform.TransformDirection(Vector3.forward) * 10;

        // 플레이어의 위치에서 전방으로 Raycast를 발사
        if (Physics.Raycast(player.position, forward, out hit))
        {
            // 태그가 'WildFire'인 오브젝트를 바라보고 있는지 확인
            if (hit.collider.CompareTag("WildFire"))
            {
                // 각 TextMeshProUGUI에 메시지를 작성
                textDisplays[0].text = "2024-nn-nn";
                textDisplays[1].text = "하남스타필드 CGV";
                textDisplays[2].text = "화재 발생";
            }
        }
    }

    // 디버깅을 위해 Ray를 시각화
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 forward = player.transform.TransformDirection(Vector3.forward) * 10;
        Gizmos.DrawRay(player.position, forward);
    }
}
