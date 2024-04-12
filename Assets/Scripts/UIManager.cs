using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject disasterReportPanel; // Inspector���� �Ҵ�

    // DisasterReport �г��� Ȱ��ȭ�ϴ� �޼���
    public void OpenDisasterReport()
    {
        disasterReportPanel.SetActive(true);
    }

    // DisasterReport �г��� ��Ȱ��ȭ�ϴ� �޼���
    public void CloseDisasterReport()
    {
        disasterReportPanel.SetActive(false);
    }
}
