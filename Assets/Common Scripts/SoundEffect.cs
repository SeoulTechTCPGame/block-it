using UnityEngine;

// 사운드 출력의 예이자 버튼 클릭 시 나오는 효과음 처리
public class SoundEffect: MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    public void PlayButtonSound()
    {
        SoundManager.instance.PlaySoundEffect(_clip);
    }
}