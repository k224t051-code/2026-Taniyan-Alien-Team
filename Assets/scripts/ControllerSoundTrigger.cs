using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ControllerSoundTrigger : MonoBehaviour
{
    [Header("鳴らしたい音のデータ（AudioClip）を入れてください")]
    [SerializeField] private AudioClip soundEffect;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        // Meta Questの右コントローラー(RTouch)の、Aボタン(One)が「押された瞬間(GetDown)」を検知
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            if (soundEffect != null)
            {
                audioSource.PlayOneShot(soundEffect);
                Debug.Log("右のAボタンが押されました！");
            }
        }
    }
}