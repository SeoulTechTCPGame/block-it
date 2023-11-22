using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("프로필 화면 정보")]
    [SerializeField] TMP_Text userName;
    [SerializeField] TMP_Text userRecord;
    [SerializeField] Image targetProfileImage; // UI Image의 경우

    [Header("사용자 정보 변경 화면")]
    [SerializeField] private TMP_InputField _newNicknameField;
    [SerializeField] private TMP_InputField _newPasswordField;
    [SerializeField] private TMP_InputField _newPasswordCheckField;
    [SerializeField] private Button _changeImageBtn;
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private TMP_Text warningText;

    [Header("씬 이동")]
    [SerializeField] private MoveScene _ms;

    [Header("제한할 닉네임 글자수")]
    [SerializeField] int minNicknameLen = 3;
    [SerializeField] int maxNicknameLen = 15;

    private Texture2D _selectedImage; // 선택된 이미지
    private GameObject _curPanel;

    private void Start()
    {
        BlockItUser user = CurrentLoginSession.Singleton.User;
        Texture2D texture = new Texture2D(2, 2);

        userName.text = user.Nickname; // 닉네임 표시
        
        if (!user.IsGuest) // 게스트가 아닌 경우
        {
            texture.LoadImage(user.ProfileImg);
            targetProfileImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            if (user.PlayCount == 0)
            {
                userRecord.text = "플레이 기록이 없습니다.";
            }
            else
            {
                userRecord.text = user.PlayCount + " 게임, 승률: " + user.WinCount * 100 / user.PlayCount + "%";
            }
            _changeProfile.interactable = true;
        } 
        else // 게스트인 경우
        {
            userRecord.text = "게스트 모드입니다.";
            _changeProfile.interactable = false;
        }
        

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
                _selectedImage = NativeGallery.LoadImageAtPath(path, default, false);
                byte[] image = _selectedImage.EncodeToJPG();
                for (int quality = 100; quality > 0 && image.Length > 524288; quality -= 5)
                {
                    image = _selectedImage.EncodeToJPG(quality);
                }

                // 크기 제한
                if (image.Length > 524288)
                {
                    warningText.text = "프로필 사진의 최대 크기는 512KB 입니다.";
                }
                else
                {
                    BlockItUserDataManager.Singleton.UploadProfileImage(CurrentLoginSession.Singleton.User, image, () =>
                    {
                        if (CurrentLoginSession.Singleton.User.IsSuccess)
                        {
                            warningText.text = "프로필 사진 변경 성공";
                            BlockItUserDataManager.Singleton.GetProfileImage(CurrentLoginSession.Singleton.User);
                        }
                        else
                        {
                            warningText.text = "프로필 사진 변경 실패, 다시 시도하세요.";
                        }
                    });
                }
            }
        });
    }

    // 닉네임 혹은 비밀번호, 혹은 둘다 변경 시 이벤트
    private void ConfirmEvent()
    {
        // TODO: 비밀번호 일치 시 해당 동작을 수행하고, 아닐 경우 오류 메세지 출력
        // 현재는 비밀번호 입력하지 않아도 닉네임 변경 동작을 수행하도록 되어있습니다...

        // 글자 수 제한 체크
        if (_newNicknameField.text.Length >= minNicknameLen && _newNicknameField.text.Length <= maxNicknameLen)
        {
            BlockItUserDataManager.Singleton.UpdateUserName(CurrentLoginSession.Singleton.User, _newNicknameField.text, () =>
            {
                if (CurrentLoginSession.Singleton.User.IsSuccess)
                {
                    warningText.text = "닉네임 변경 완료.";
                }
                else
                {
                    warningText.text = "해당 닉네임은 이미 사용 중인 닉네임입니다.";
                }
            });
        }
        else
        {
            warningText.text = "닉네임의 글자수는 " + minNicknameLen + "글자와 " + maxNicknameLen + "글자 사이여야 합니다.";
        }
    }

    // 로그아웃 시 이벤트
    private void OnClickLogoutButton()
    {
        CurrentLoginSession.Singleton.Logout();
        _ms.ToLoading();
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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