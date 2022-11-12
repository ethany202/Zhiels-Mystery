using UnityEngine;

public class PlaySFX : MonoBehaviour
{

    public AudioSource audioSource;

    //public AudioClip[] movingOnStoneSFX = new AudioClip[3];
    public AudioClip doorCreak;
    public AudioClip pianoKey;

    public AudioClip elevatorOpen;

    public AudioClip buttonPressed;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void OpenDoor()
    {
        audioSource.PlayOneShot(doorCreak);
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

}
