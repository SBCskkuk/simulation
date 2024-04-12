using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject disasterReportPanel; // Inspector���� �Ҵ�

    // DisasterReport �г��� Ȱ��ȭ�ϴ� �޼���
    public void OpenDisasterReport()
    {
        Debug.Log("Opening Disaster Report Panel");
        SetActiveRecursively(disasterReportPanel, true);
    }

    // DisasterReport �г��� ��Ȱ��ȭ�ϴ� �޼���
    public void CloseDisasterReport()
    {
        Debug.Log("Closing Disaster Report Panel");
        SetActiveRecursively(disasterReportPanel, false);
    }

    // ��� �ڽ� ������Ʈ�� ��������� Ȱ��ȭ/��Ȱ��ȭ�ϴ� �޼���
    private void SetActiveRecursively(GameObject root, bool state)
    {
        root.SetActive(state);
        Debug.Log($"Setting {root.name} to {(state ? "active" : "inactive")}");

        // ��� �ڽĿ� ���� ������ ó��
        foreach (Transform child in root.transform)
        {
            SetActiveRecursively(child.gameObject, state);
        }
    }
}
