using UnityEngine;

public class LookAtCCTV : MonoBehaviour
{
    public string targetTag = "MainCamera";
    private Transform target;

    void Start()
    {
        // CCTV �±׸� ���� ù ��° ������Ʈ�� ã�Ƽ� �� ������Ʈ�� Transform�� �����ɴϴ�.
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            Debug.LogError("MainCamera �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        // Ÿ�� ������Ʈ�� �����Ѵٸ�, �� �����Ӹ��� �ش� ������Ʈ�� �ٶ󺾴ϴ�.
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
