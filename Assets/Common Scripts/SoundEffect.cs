using UnityEngine;

// ���� ����� ������ ��ư Ŭ�� �� ������ ȿ���� ó��
public class SoundEffect: MonoBehaviour
{
    [SerializeField] AudioClip _Clip;

    public void PlayButtonSound()
    {
        SoundManager.instance.PlaySoundEffect(_Clip);
    }
}