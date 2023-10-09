using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    [Header("패널")]
    [SerializeField] private GameObject _myProfilePanel;
    [SerializeField] private GameObject _changeIDPanel;
    [SerializeField] private GameObject _changeImagePanel;

    [Header("프로필 화면")]
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _changeProfile;
    [SerializeField] private Button _logoutBtn;

    [Header("사용자 정보 변경 화면")]
    [SerializeField] private TMP_InputField _newNicknameField;
    [SerializeField] private TMP_InputField _newPasswordField;
    [SerializeField] private TMP_InputField _newPasswordCheckField;
    [SerializeField] private Button _changeImageBtn;
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private TMP_Text warningText;

    [SerializeField] private MoveScene _ms;

    private Texture2D _selectedImage; // 선택된 이미지
    private GameObject _curPanel;

    private void Start()
    {
        ActivatePanel(_myProfilePanel);

        _backBtn.onClick.AddListener(BackEvent);
        _changeProfile.onClick.AddListener(() => ActivatePanel(_changeIDPanel));
        _changeImageBtn.onClick.AddListener(SelectImage);
        _confirmBtn.onClick.AddListener(ConfirmEvent);
        _logoutBtn.onClick.AddListener(OnClickLogoutButton);
    }

    // 이미지 선택 후 자동 업로드
    private void SelectImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                _selectedImage = ResizeTexture(NativeGallery.LoadImageAtPath(path, default, false), 500, 540);
                byte[] image = _selectedImage.EncodeToJPG();
                for (int quality = 100; quality > 0 && image.Length > 16373; quality -= 5)
                {
                    image = _selectedImage.EncodeToJPG(quality);
                }
                CurrentLoginSession.Instance.User.UploadProfileImageToServer(image);
            }
        });
    }

    // 닉네임 혹은 비밀번호, 혹은 둘다 변경 시 이벤트
    private void ConfirmEvent()
    {
        // TODO: 비밀번호 일치 시 해당 동작을 수행하고, 아닐 경우 오류 메세지 출력
        // 현재는 비밀번호 입력하지 않아도 닉네임 변경 동작을 수행하도록 되어있습니다...
        CurrentLoginSession.Instance.User.ChangeUserNameToServer(_newNicknameField.text);
        if (CurrentLoginSession.Instance.User.IsDupcliateName)
        {
            warningText.text = "해당 닉네임은 이미 사용 중인 닉네임입니다.";
        }
        else
        {
            warningText.text = "닉네임 변경 완료.";
        }
    }

    // 로그아웃 시 이벤트
    private void OnClickLogoutButton()
    {
        CurrentLoginSession.Instance.Logout();
        _ms.ToLobby();
    }


    #region 패널 관리
    private void BackEvent()
    {
        if (_curPanel == _myProfilePanel)
        {
            _ms.ToHome();
        }
        if (_curPanel == _changeIDPanel)
        {
            ActivatePanel(_myProfilePanel);
        }
        if (_curPanel == _changeImagePanel)
        {
            ActivatePanel(_changeIDPanel);
        }
    }

    private void ActivatePanel(GameObject panel)
    {
        _myProfilePanel.SetActive(panel == _myProfilePanel);
        _changeIDPanel.SetActive(panel == _changeIDPanel);
        _changeImagePanel.SetActive(panel == _changeImagePanel);
        _curPanel = panel;
    }
    #endregion

    // 사진의 해상도를 조절한 후 Texture2D 반환
    public Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        // 원본 Texture2D를 Sprite로 변환
        Sprite sprite = Sprite.Create(source, new Rect(0, 0, source.width, source.height), new Vector2(0.5f, 0.5f));

        // 새 Texture2D 생성
        Texture2D result = new Texture2D(newWidth, newHeight);

        // RenderTexture 생성 및 설정
        RenderTexture renderTexture = RenderTexture.GetTemporary(newWidth, newHeight);
        renderTexture.filterMode = FilterMode.Bilinear;

        // RenderTexture에 Sprite 렌더링
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Graphics.DrawTexture(new Rect(0, 0, newWidth, newHeight), sprite.texture, new Rect(0, 0, 1, 1), 0, 0, 0, 0);

        // RenderTexture에서 결과를 Texture2D로 복사
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();

        // RenderTexture 해제
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        return result;
    }
}