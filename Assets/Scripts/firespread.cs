using UnityEngine;

using System.Collections;

public class FireSpread : MonoBehaviour
{
    public Transform targetPosition; // ��ƼŬ�� ������ ���� ������Ʈ�� ��ġ
    public ParticleSystem prefabWildFire; // ��ƼŬ ������ (�̸��� "WildFire(2)"�� ��ƼŬ)

    private void Update()
    {
        // �� �����Ӹ��� �÷��̾��� ��ġ�� Ÿ�� ��ġ�� ��
        if (Vector3.Distance(transform.position, targetPosition.position) < 1f) // 1���� �̳��� �����ϸ�
        {
            CreateAndMoveParticle(); // ��ƼŬ ���� �� �̵� �Լ� ȣ��
        }
    }

    private void CreateAndMoveParticle()
    {
        // �� ��ƼŬ ����
        ParticleSystem newParticle = Instantiate(prefabWildFire, targetPosition.position, Quaternion.identity);
        newParticle.Play(); // ��ƼŬ ���

        // Coroutine�� �����Ͽ� ��ƼŬ�� X������ õõ�� 2��ŭ �̵�
        StartCoroutine(MoveParticle(newParticle));
    }

    private IEnumerator MoveParticle(ParticleSystem particle)
    {
        float moveAmount = 2.0f; // �� �̵� �Ÿ�
        float moveSpeed = 0.5f; // �̵� �ӵ� (�ʴ� �̵��� �Ÿ�)

        // ���� ��ġ���� X��ǥ�� ����
        Vector3 startPosition = particle.transform.position;
        Vector3 endPosition = new Vector3(startPosition.x + moveAmount, startPosition.y, startPosition.z);

        float startTime = Time.time; // ���� �ð�
        float journeyLength = Vector3.Distance(startPosition, endPosition); // �� �̵� �Ÿ� ���

        while (Vector3.Distance(particle.transform.position, endPosition) > 0.01f)
        {
            // �̵� ��� �ð��� ���� ���� ���
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            // �ε巯�� �̵��� ���� Lerp �Լ� ���
            particle.transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);

            yield return null; // ���� �����ӱ��� ��ٸ�
        }

        particle.transform.position = endPosition; // ���� ��ġ�� ��Ȯ�ϰ� ����
    }
}
