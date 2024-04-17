using StarterAssets;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject disasterReportPanel; // Inspector에서 할당

    private Vector3 originalScale; // 원래 스케일 저장
    private Vector2 originalPosition; // 원래 위치 저장

    public TMP_InputField inputField;
    public ThirdPersonController playerController;


    
    private void Awake()
    {
        // 원래 스케일과 위치를 저장합니다.
        originalScale = disasterReportPanel.transform.localScale;
        originalPosition = disasterReportPanel.GetComponent<RectTransform>().anchoredPosition;

        inputField.onSelect.AddListener(delegate { DisablePlayerControls(); });
        inputField.onDeselect.AddListener(delegate { EnablePlayerControls(); });
    }
    private void DisablePlayerControls()
    {
        // 플레이어 컨트롤을 비활성화합니다.
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        // 커서 설정을 해제합니다.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void EnablePlayerControls()
    {
        // 입력 필드에서 벗어나면 플레이어 컨트롤을 다시 활성화합니다.
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        // 게임에 맞게 커서 설정을 다시 적용합니다.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        // 엔터키가 눌렸는지 확인합니다.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleDisasterReport();
        }

        // '+' 키(Shift + '=')가 눌렸는지 확인합니다.
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                // 스케일을 모두 1로 설정하고, Y 위치를 70으로 조정하여 중앙에 위치합니다.
                SetDisasterReportScale(Vector3.one, 70f);
                // 커서 활성화
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        // '-' 키가 눌렸는지 확인합니다.
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            // 스케일과 위치를 원래대로 복원합니다.
            RestoreDisasterReportScaleAndPosition();
            // 커서 비활성화
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // DisasterReport 패널의 스케일과 위치를 설정하는 메서드
    private void SetDisasterReportScale(Vector3 newScale, float yPosition)
    {
        disasterReportPanel.transform.localScale = newScale;
        RectTransform rectTransform = disasterReportPanel.GetComponent<RectTransform>();

        // X는 중앙(0)에 위치하고, Y는 인자로 받은 값으로 설정합니다.
        rectTransform.anchoredPosition = new Vector2(0, yPosition);
    }

    // DisasterReport 패널의 스케일과 위치를 원래대로 복원하는 메서드
    private void RestoreDisasterReportScaleAndPosition()
    {
        disasterReportPanel.transform.localScale = originalScale;
        RectTransform rectTransform = disasterReportPanel.GetComponent<RectTransform>();

        // 원래 위치로 되돌립니다.
        rectTransform.anchoredPosition = originalPosition;
    }

    // DisasterReport 패널을 토글하는 메서드
    private void ToggleDisasterReport()
    {
        // 패널의 활성화 상태를 토글합니다.
        bool newState = !disasterReportPanel.activeSelf;
        disasterReportPanel.SetActive(newState);

        // 콘솔에 로그를 출력합니다.
        Debug.Log($"Disaster Report Panel is now " + (newState ? "opened" : "closed"));
    }
}
