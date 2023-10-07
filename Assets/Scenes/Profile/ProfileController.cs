using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    [SerializeField] private GameObject _myProfilePanel;
    [SerializeField] private GameObject _changeIDPanel;
    [SerializeField] private GameObject _changeImagePanel;
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _changeProfile;
    [SerializeField] private Button _changeImage;
    [SerializeField] private Button _confirmBtn;
    // [SerializeField] private Button _selectImgBtn;
    [SerializeField] private MoveScene _ms;

    private Texture2D _selectedImage;

    private GameObject _curPanel;

    private void Start()
    {
        ActivatePanel(_myProfilePanel);

        _backBtn.onClick.AddListener(BackEvent);
        _changeProfile.onClick.AddListener(() => ActivatePanel(_changeIDPanel));
        // _changeImage.onClick.AddListener(() => ActivatePanel(_changeImagePanel));
        _changeImage.onClick.AddListener(SelectImage);
        _confirmBtn.onClick.AddListener(ConfirmEvent);   
        // _selectImgBtn.onClick.AddListener(SelectImage);
    }

    // TODO: SelectImage() should be filled
    private void SelectImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                _selectedImage = NativeGallery.LoadImageAtPath(path, 256, false);
                byte[] image = _selectedImage.EncodeToJPG();
                CurrentLoginSession.Instance.User.UploadProfileImageToServer(image);
            }
        });
    }

    // TODO: ConfirmEvent() should be filled
    private void ConfirmEvent()
    {

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
}