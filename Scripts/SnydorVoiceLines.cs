using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnydorVoiceLines : MonoBehaviour
{

    public AudioSource voiceLineSource;

    public AudioClip zhieltropolisClip;
    public AudioClip[] stage2Grunts;
    public AudioClip stage4Clip;

    private float initialPitch;

    private void Start()
    {
        initialPitch = voiceLineSource.pitch;   
    }

    public void PlayZhieltropolisClip()
    {
        voiceLineSource.PlayOneShot(zhieltropolisClip);
    }

    public void PlayStage2Grunt()
    {
        int index = Random.Range(0, stage2Grunts.Length);
        voiceLineSource.PlayOneShot(stage2Grunts[index]);
    }

    public void PlayStage4Clip()
    {
        voiceLineSource.pitch = initialPitch;
        voiceLineSource.PlayOneShot(stage4Clip);
    }

    public IEnumerator PlayVoiceLine(AudioClip line, float delayTime, float pitch)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        voiceLineSource.pitch = pitch;
        voiceLineSource.PlayOneShot(line);
    }
}
