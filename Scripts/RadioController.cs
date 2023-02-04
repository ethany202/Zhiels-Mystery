using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    public AudioSource source;
    public AudioClip newtonLine;

    public string[] subtitles;
    public TMP_Text subtitleBox;

    public void PlayVoiceLine()
    {
        source.PlayOneShot(newtonLine);
    }

    public IEnumerator PlayVoiceLineDelay(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        PlayVoiceLine();
    }

    public IEnumerator UpdateSubtitles()
    {
        for(int i = 0; i < subtitles.Length; i++)
        {
            subtitleBox.text = subtitles[i];
            yield return new WaitForSecondsRealtime(1f);
        }
    }

}
