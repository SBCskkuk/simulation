using UnityEngine;

public class LookAtCCTV : MonoBehaviour
{
    public string targetTag = "MainCamera";
    private Transform target;

    void Start()
    {
        // CCTV 태그를 가진 첫 번째 오브젝트를 찾아서 그 오브젝트의 Transform을 가져옵니다.
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            Debug.LogError("MainCamera 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        // 타겟 오브젝트가 존재한다면, 매 프레임마다 해당 오브젝트를 바라봅니다.
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
