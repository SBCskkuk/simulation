using StarterAssets;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject disasterReportPanel; // Inspector���� �Ҵ�

    private Vector3 originalScale; // ���� ������ ����
    private Vector2 originalPosition; // ���� ��ġ ����

    public TMP_InputField inputField;
    public ThirdPersonController playerController;


    
    private void Awake()
    {
        // ���� �����ϰ� ��ġ�� �����մϴ�.
        originalScale = disasterReportPanel.transform.localScale;
        originalPosition = disasterReportPanel.GetComponent<RectTransform>().anchoredPosition;

        inputField.onSelect.AddListener(delegate { DisablePlayerControls(); });
        inputField.onDeselect.AddListener(delegate { EnablePlayerControls(); });
    }
    private void DisablePlayerControls()
    {
        // �÷��̾� ��Ʈ���� ��Ȱ��ȭ�մϴ�.
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        // Ŀ�� ������ �����մϴ�.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void EnablePlayerControls()
    {
        // �Է� �ʵ忡�� ����� �÷��̾� ��Ʈ���� �ٽ� Ȱ��ȭ�մϴ�.
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        // ���ӿ� �°� Ŀ�� ������ �ٽ� �����մϴ�.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        // ����Ű�� ���ȴ��� Ȯ���մϴ�.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleDisasterReport();
        }

        // '+' Ű(Shift + '=')�� ���ȴ��� Ȯ���մϴ�.
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                // �������� ��� 1�� �����ϰ�, Y ��ġ�� 70���� �����Ͽ� �߾ӿ� ��ġ�մϴ�.
                SetDisasterReportScale(Vector3.one, 70f);
                // Ŀ�� Ȱ��ȭ
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        // '-' Ű�� ���ȴ��� Ȯ���մϴ�.
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            // �����ϰ� ��ġ�� ������� �����մϴ�.
            RestoreDisasterReportScaleAndPosition();
            // Ŀ�� ��Ȱ��ȭ
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // DisasterReport �г��� �����ϰ� ��ġ�� �����ϴ� �޼���
    private void SetDisasterReportScale(Vector3 newScale, float yPosition)
    {
        disasterReportPanel.transform.localScale = newScale;
        RectTransform rectTransform = disasterReportPanel.GetComponent<RectTransform>();

        // X�� �߾�(0)�� ��ġ�ϰ�, Y�� ���ڷ� ���� ������ �����մϴ�.
        rectTransform.anchoredPosition = new Vector2(0, yPosition);
    }

    // DisasterReport �г��� �����ϰ� ��ġ�� ������� �����ϴ� �޼���
    private void RestoreDisasterReportScaleAndPosition()
    {
        disasterReportPanel.transform.localScale = originalScale;
        RectTransform rectTransform = disasterReportPanel.GetComponent<RectTransform>();

        // ���� ��ġ�� �ǵ����ϴ�.
        rectTransform.anchoredPosition = originalPosition;
    }

    // DisasterReport �г��� ����ϴ� �޼���
    private void ToggleDisasterReport()
    {
        // �г��� Ȱ��ȭ ���¸� ����մϴ�.
        bool newState = !disasterReportPanel.activeSelf;
        disasterReportPanel.SetActive(newState);

        // �ֿܼ� �α׸� ����մϴ�.
        Debug.Log($"Disaster Report Panel is now " + (newState ? "opened" : "closed"));
    }
}
