using UnityEngine;

using System.Collections;

public class FireSpread : MonoBehaviour
{
    public Transform targetPosition; // 파티클이 생성될 게임 오브젝트의 위치
    public ParticleSystem prefabWildFire; // 파티클 프리팹 (이름이 "WildFire(2)"인 파티클)

    private void Update()
    {
        // 매 프레임마다 플레이어의 위치와 타겟 위치를 비교
        if (Vector3.Distance(transform.position, targetPosition.position) < 1f) // 1미터 이내로 접근하면
        {
            CreateAndMoveParticle(); // 파티클 생성 및 이동 함수 호출
        }
    }

    private void CreateAndMoveParticle()
    {
        // 새 파티클 생성
        ParticleSystem newParticle = Instantiate(prefabWildFire, targetPosition.position, Quaternion.identity);
        newParticle.Play(); // 파티클 재생

        // Coroutine을 시작하여 파티클을 X축으로 천천히 2만큼 이동
        StartCoroutine(MoveParticle(newParticle));
    }

    private IEnumerator MoveParticle(ParticleSystem particle)
    {
        float moveAmount = 2.0f; // 총 이동 거리
        float moveSpeed = 0.5f; // 이동 속도 (초당 이동할 거리)

        // 현재 위치에서 X좌표만 조정
        Vector3 startPosition = particle.transform.position;
        Vector3 endPosition = new Vector3(startPosition.x + moveAmount, startPosition.y, startPosition.z);

        float startTime = Time.time; // 시작 시간
        float journeyLength = Vector3.Distance(startPosition, endPosition); // 총 이동 거리 계산

        while (Vector3.Distance(particle.transform.position, endPosition) > 0.01f)
        {
            // 이동 경과 시간에 따른 비율 계산
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            // 부드러운 이동을 위한 Lerp 함수 사용
            particle.transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);

            yield return null; // 다음 프레임까지 기다림
        }

        particle.transform.position = endPosition; // 최종 위치로 정확하게 설정
    }
}
