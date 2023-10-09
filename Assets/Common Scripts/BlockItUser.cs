using Mirror;
using System.Security.Cryptography;
using System;
using System.Threading;
using System.Threading.Tasks;

public class BlockItUser
{
    #region 유저의 정보를 담을 Data Field와 Getter & Setter
    private string _userId;                             // Firebase UID (유저 ID)
    private string _nickname;                           // 유저 네임
    private int _playCount = 0;                         // 플레이 횟수
    private int _winCount = 0;                          // 승리 횟수
    private byte[] _profileImage = null;                // 프로필 사진

    private bool _isGuest = false;                      // 게스트 확인 플래그

    private bool _isRecived = false;                    // 전송 확인 플래그
    private bool _isDupcliateName = false;              // 닉네임 중복 확인 플래그

    public string UserId
    { 
        get { return _userId; }
    }

    public string Nickname
    {
        get { return _nickname; }
        private set { _nickname = value; }
    }

    public int PlayCount
    {
        get { return _playCount; }
        private set { _playCount = value; }
    }

    public int WinCount
    {
        get { return _winCount; }
        private set { _winCount = value; }
    }

    public byte[] ProfileImage
    {
        get { return _profileImage; }
        private set { _profileImage = value; }
    }

    public bool IsGuest
    {
        get { return _isGuest; }
    }

    public bool IsDupcliateName
    {
        get { return _isDupcliateName; }
    }

    #endregion

    #region 생성자
    // Firebase GID 없을 경우 게스트로 판단 후 유저네임 랜덤 생성
    public BlockItUser()
    {
        _isGuest = true;
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);

            string randomHash = BitConverter.ToString(randomNumber).Replace("-", "").Substring(0, 8);
            _nickname = "Guest#" + randomHash;
        }
    }

    // 로그인일 경우 생성자
    public BlockItUser(string userId)
    {
        _userId = userId;
    }

    // 회원 가입일 경우 생성자 (닉네임을 저장해야하므로)
    public BlockItUser(string userId, string nickname)
    {
        _userId = userId;
        _nickname = nickname;
    }
    #endregion

    // 서버와 연결될 때까지 대기 시키는 비동기 함수
    private async Task WaitForConnectionAsync(CancellationToken token)
    {
        while (!NetworkClient.isConnected)
        {
            if (token.IsCancellationRequested)
            {
                // 취소 요청이 있을 경우, 작업을 중지
                throw new TaskCanceledException();
            }
            await Task.Delay(100); // 100ms 대기 (값은 필요에 따라 조절 가능)
        }
    }

    #region 유저 정보 요청과 응답 시 이벤트
    public async void getUserData(CancellationToken token = default)
    {
        // 정보를 불러오기 위해 Mirror 클라이언트 시작
        NetworkManager.singleton.StartClient();

        // 서버에서 보낸 정보를 받기 위한 Handler 등록
        NetworkClient.RegisterHandler<ResponseUserDataMessage>(OnReceiveUserDataResponse);

        // 서버와 연결되면 UID를 보냄
        await WaitForConnectionAsync(token);
        SendUserDataRequest(_userId);

        // 정보 불러왔으면 클라이언트 종료
        if (_isRecived)
        {
            NetworkManager.singleton.StopClient();
            _isRecived = false;
        }
    }

    private void SendUserDataRequest(string userId)
    {
        RequestUserDataMessage requestData = new RequestUserDataMessage { userId = userId };
        NetworkClient.Send(requestData);
    }

    private void OnReceiveUserDataResponse(ResponseUserDataMessage msg)
    {
        _nickname = msg.nickname;
        _playCount = msg.playCount;
        _winCount = msg.winCount;
        _profileImage = msg.profileImage;
        _isRecived = msg.success;
    }
    #endregion

    #region 회원 가입 요청과 응답 시 이벤트
    // 유저 정보를 DB에 저장하기 위해 서버에 메세지 전송
    public async void SignUpUserToServer(CancellationToken token = default)
    {
        if (!_isGuest)
        {
            // 정보를 불러오기 위해 Mirror 클라이언트 시작
            NetworkManager.singleton.StartClient();

            // 서버에서 보낸 정보를 받기 위한 Handler 등록
            NetworkClient.RegisterHandler<ResponseUserSignUpMessage>(OnReceiveUserSignUpResponse);

            // 서버와 연결되면 UID와 사용자 이름을 보냄
            await WaitForConnectionAsync(token);
            SendUserSignUpRequest(_userId, _nickname);

            // 정보 불러왔으면 클라이언트 종료
            if (_isRecived)
            {
                NetworkManager.singleton.StopClient();
                _isRecived = false;
            }
        }
    }

    private void SendUserSignUpRequest(string userId, string userName)
    {
        RequestUserSignUpMessage requestData = new RequestUserSignUpMessage { userId = userId, nickname = userName };
        NetworkClient.Send(requestData);
    }

    private void OnReceiveUserSignUpResponse(ResponseUserSignUpMessage msg)
    {
        _isRecived = msg.success;
    }
    #endregion

    #region 이미지 업로드와 응답 시 이벤트
    public async void UploadProfileImageToServer(byte[] profileImage, CancellationToken token = default)
    {
        if (!_isGuest)
        {
            _profileImage = profileImage;
            NetworkManager.singleton.StartClient();
            NetworkClient.RegisterHandler<ResponseProfileImageUploadMessage>(OnReceiveProfileImageUploadResponse);

            await WaitForConnectionAsync(token);
            UploadUserProfileImage(_userId, _profileImage);

            if (_isRecived)
            {
                NetworkManager.singleton.StopClient();
                _isRecived = false;
            }
        }
    }

    private void UploadUserProfileImage(string userId, byte[] profileImage)
    {
        RequestProfileImageUploadMessage requestData = new RequestProfileImageUploadMessage 
        { userId = userId, image = profileImage };
        NetworkClient.Send(requestData);
    }

    private void OnReceiveProfileImageUploadResponse(ResponseProfileImageUploadMessage msg)
    {
        _isRecived = msg.success;
    }
    #endregion

    #region 닉네임 변경 요청과 응답 시 이벤트
    public async void ChangeUserNameToServer(string newName, CancellationToken token = default)
    {
        if (!_isGuest)
        {
            NetworkManager.singleton.StartClient();
            NetworkClient.RegisterHandler<ResponseChangeUserNameMessage>(OnReceiveChangeUserNameResponse);

            await WaitForConnectionAsync(token);
            RequestChangeUserName(_userId, newName);

            if (_isRecived)
            {
                NetworkManager.singleton.StopClient();
                _isRecived = false;
                if (!_isDupcliateName)
                {
                    _nickname = newName;
                }
            }
        }
    }

    private void RequestChangeUserName(string userId, string username)
    {
        RequestChangeUserNameMessage requestData = new RequestChangeUserNameMessage
        { userId = userId, nickname = username };
        NetworkClient.Send(requestData);
    }

    private void OnReceiveChangeUserNameResponse(ResponseChangeUserNameMessage msg)
    {
        _isRecived = msg.success;
        _isDupcliateName = msg.isDupcliate;
    }
    #endregion
}