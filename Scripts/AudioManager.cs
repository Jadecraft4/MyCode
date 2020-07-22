using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioClip Music1;
    public AudioClip Music2;
    public AudioSource Player;
    private void Awake()
    {
        StartCoroutine(PlayMusic());
    }
    IEnumerator PlayMusic()
    {
        Player.clip = Music1;
        Player.Play();
        yield return new WaitForSeconds(Player.clip.length);
        Player.clip = Music2;
        Player.Play();
    }
}
