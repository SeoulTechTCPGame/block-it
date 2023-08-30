using Mirror;
using System.Threading;
using System.Threading.Tasks;

public class BlockItUser
{
    #region ������ ������ ���� Data Field�� Getter & Setter
    private string _userId;                             // Firebase UID (���� ID)
    private string _nickname;                           // ������ �г���
    private int _playCount = 0;                         // �÷��� Ƚ��
    private int _winCount = 0;                          // �¸� Ƚ��
    private string _profilePicturePath = string.Empty;  // ������ ���� ���
    private bool _isRecived = false;                    // ���� ���� ���� Flag

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

    public string ProfilePicturePath
    {
        get { return _profilePicturePath; }
        private set { _profilePicturePath = value; }
    }

    #endregion

    #region ������
    // �α����� ��� ������
    public BlockItUser(string userId)
    {
        _userId = userId;
    }

    // ȸ�� ������ ��� ������ (�г����� �����ؾ��ϹǷ�)
    public BlockItUser(string userId, string nickname)
    {
        _userId = userId;
        _nickname = nickname;
    }
    #endregion

    // ������ ����� ������ ��� ��Ű�� �񵿱� �Լ�
    private async Task WaitForConnectionAsync(CancellationToken token)
    {
        while (!NetworkClient.isConnected)
        {
            if (token.IsCancellationRequested)
            {
                // ��� ��û�� ���� ���, �۾��� ����
                throw new TaskCanceledException();
            }
            await Task.Delay(100); // 100ms ��� (���� �ʿ信 ���� ���� ����)
        }
    }

    #region ���� ���� ��û�� ���� �� �̺�Ʈ
    public async void getUserData(CancellationToken token = default)
    {
        // ������ �ҷ����� ���� Mirror Ŭ���̾�Ʈ ����
        NetworkManager.singleton.StartClient();

        // �������� ���� ������ �ޱ� ���� Handler ���
        NetworkClient.RegisterHandler<ResponseUserDataMessage>(OnReceiveUserDataResponse);

        // ������ ����Ǹ� UID�� ����
        await WaitForConnectionAsync(token);
        SendUserDataRequest(_userId);

        // ���� �ҷ������� Ŭ���̾�Ʈ ����
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
        _profilePicturePath = msg.profilePicturePath;
        _isRecived = msg.success;
    }
    #endregion

    #region ȸ�� ���� ��û�� ���� �� �̺�Ʈ
    // ���� ������ DB�� �����ϱ� ���� ������ �޼��� ����
    public async void SignUpUserToServer(CancellationToken token = default)
    {
        // ������ �ҷ����� ���� Mirror Ŭ���̾�Ʈ ����
        NetworkManager.singleton.StartClient();

        // �������� ���� ������ �ޱ� ���� Handler ���
        NetworkClient.RegisterHandler<ResponseUserSignUpMessage>(OnReceiveUserSignUpResponse);

        // ������ ����Ǹ� UID�� ����� �̸��� ����
        await WaitForConnectionAsync(token);
        SendUserSignUpRequest(_userId, _nickname);

        // ���� �ҷ������� Ŭ���̾�Ʈ ����
        if (_isRecived)
        {
            NetworkManager.singleton.StopClient();
            _isRecived = false;
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
}