using UnityEngine;
using TMPro;

public class LookAtObject : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public TextMeshProUGUI[] textDisplays; // �޽����� ǥ���� TextMeshProUGUI ��ü �迭

    void Update()
    {
        RaycastHit hit;
        Vector3 forward = player.transform.TransformDirection(Vector3.forward) * 10;

        // �÷��̾��� ��ġ���� �������� Raycast�� �߻�
        if (Physics.Raycast(player.position, forward, out hit))
        {
            // �±װ� 'WildFire'�� ������Ʈ�� �ٶ󺸰� �ִ��� Ȯ��
            if (hit.collider.CompareTag("WildFire"))
            {
                // �� TextMeshProUGUI�� �޽����� �ۼ�
                textDisplays[0].text = "2024-nn-nn";
                textDisplays[1].text = "�ϳ���Ÿ�ʵ� CGV";
                textDisplays[2].text = "ȭ�� �߻�";
            }
        }
    }

    // ������� ���� Ray�� �ð�ȭ
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 forward = player.transform.TransformDirection(Vector3.forward) * 10;
        Gizmos.DrawRay(player.position, forward);
    }
}
