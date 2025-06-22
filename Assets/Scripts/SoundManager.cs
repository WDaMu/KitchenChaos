using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipSO audioClipSO;
    private void Start()
    {
        // 切菜音效
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        // 交付成功音效

        // 交付失败音效

        // 脚步声音效
        //Player.Instance.OnWalking += Player_OnWalking;
        // 放置物品声音效
        BaseCounter.OnAnyObjectDropped += BaseCounter_OnAnyObjectDropped;
        // 拾取物品声音效
        BaseCounter.OnAnyObjectPicked += BaseCounter_OnAnyObjectPicked;

    }

    private void BaseCounter_OnAnyObjectPicked(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipSO.pickupClip, baseCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectDropped(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipSO.dropClip, baseCounter.transform.position);
    }

    private void Player_OnWalking(object sender, EventArgs e)
    {
        PlaySound(audioClipSO.footstepClip, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipSO.chopClip, cuttingCounter.transform.position);
    }

    private void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}
