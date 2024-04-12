using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject disasterReportPanel; // Inspector에서 할당

    // DisasterReport 패널을 활성화하는 메서드
    public void OpenDisasterReport()
    {
        Debug.Log("Opening Disaster Report Panel");
        SetActiveRecursively(disasterReportPanel, true);
    }

    // DisasterReport 패널을 비활성화하는 메서드
    public void CloseDisasterReport()
    {
        Debug.Log("Closing Disaster Report Panel");
        SetActiveRecursively(disasterReportPanel, false);
    }

    // 모든 자식 오브젝트를 재귀적으로 활성화/비활성화하는 메서드
    private void SetActiveRecursively(GameObject root, bool state)
    {
        root.SetActive(state);
        Debug.Log($"Setting {root.name} to {(state ? "active" : "inactive")}");

        // 모든 자식에 대해 동일한 처리
        foreach (Transform child in root.transform)
        {
            SetActiveRecursively(child.gameObject, state);
        }
    }
}
