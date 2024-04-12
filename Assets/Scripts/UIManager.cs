using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject disasterReportPanel; // Inspector에서 할당

    // DisasterReport 패널을 활성화하는 메서드
    public void OpenDisasterReport()
    {
        disasterReportPanel.SetActive(true);
    }

    // DisasterReport 패널을 비활성화하는 메서드
    public void CloseDisasterReport()
    {
        disasterReportPanel.SetActive(false);
    }
}
