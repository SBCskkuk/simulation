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
                // 각 TextMeshProUGUI에 메시지를 작성 , 재난발생 개요 
                textDisplays[0].text = "2024-nn-nn";
                textDisplays[1].text = "하남스타필드 CGV";
                textDisplays[2].text = "화재 발생";
                // 피해 내용
                textDisplays[3].text = "인명 피해 현재 없음";
                textDisplays[4].text = "재산 피해 산정중";
                // 동원 자원 
                textDisplays[5].text = "현재 없음";
                textDisplays[6].text = "현장 인원1명 ";
                textDisplays[7].text = "현재 없음";
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
