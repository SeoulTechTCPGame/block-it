using System.Security.Cryptography;
using System;
using Newtonsoft.Json.Linq;

[Serializable]
public class BlockItUser
{
    #region 유저의 정보를 담을 Data Field와 Getter & Setter
    private string _id;                                 // Firebase UID (유저 ID)
    private string _nickname;                           // 유저 네임
    private int _playCount = 0;                         // 플레이 횟수
    private int _winCount = 0;                          // 승리 횟수
    private byte[] _profileImg = null;                  // 프로필 사진
    // private string _profileImagePath;                // 프로필 사진 경로

    private bool _isGuest = false;                      // 게스트 확인 플래그
    private bool _isSuccess = false;                    // 요청 성공 여부 확인 플래그

    public string Id
    { 
        get { return _id; }
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

    public byte[] ProfileImg
    {
        get { return _profileImg; }
        set { _profileImg = value; }
    }

    public bool IsGuest
    {
        get { return _isGuest; }
    }

    public bool IsSuccess
    {
        get { return _isSuccess; }
        set { _isSuccess = value; }
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
    public BlockItUser(string id)
    {
        _id = id;
    }

    // 회원 가입일 경우 생성자 (닉네임을 저장해야하므로)
    public BlockItUser(string id, string nickname)
    {
        _id = id;
        _nickname = nickname;
    }
    #endregion

    public void UpdateData(string nickname, int playcount, int wincount)
    {
        _nickname = nickname;
        _winCount = wincount;
        _playCount = playcount;
    }
}