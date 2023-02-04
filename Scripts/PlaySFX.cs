using UnityEngine;

public class PlaySFX : MonoBehaviour
{

    public AudioSource audioSource;

    //public AudioClip[] movingOnStoneSFX = new AudioClip[3];
    public AudioClip doorCreak, doorClose;
    public AudioClip pianoKey;

    public AudioClip elevatorOpen;

    public AudioClip buttonPressed;

    public AudioClip tileUp, tileDown;

    private float originalPitch;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            
        }

        originalPitch = audioSource.pitch;
    }

    public void OpenDoor()
    {
        audioSource.pitch = originalPitch;
        audioSource.PlayOneShot(doorCreak);
    }

    public void CloseDoor()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(doorClose);
    }

    private void PlayKey()
    {
        audioSource.PlayOneShot(pianoKey);
    }

    private void ElevatorDoorOpen()
    {
        audioSource.PlayOneShot(elevatorOpen);
    }

    private void ButtonPress()
    {
        audioSource.PlayOneShot(buttonPressed);
    }

    private void TranslateUpSFX()
    {
        audioSource.pitch = originalPitch;
        audioSource.PlayOneShot(tileUp);
    }

    private void TranslateDownSFX()
    {
        audioSource.pitch = originalPitch;
        audioSource.PlayOneShot(tileDown);
    }



}
