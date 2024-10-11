using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Events;
using UnityEngine.SceneManagement;

public enum songs
{
    BubbleMerge,
    WinLevel,
    BlockedObstacle,
    Spawn,
    LoseLevel,
    MoveBubble,
    breakBubble,
    ChangeBubble,
    BombExplode,
    Warning,
    NegativeMerge,
    BackgroundMusic,
    InGameMusic,
}
[Serializable]
public class SoundList
{
    public songs song;
        public AudioClip clip;

}
[Serializable]
public class MusicOnAwake
{
    public songs song;
    public bool onAwake;
}
public class SongController : MonoBehaviour
{
    [Header("Clips of the songs")]
    [SerializeField] List<SoundList> soundClips = new List<SoundList>();
    [Header("Clips of the music PlayOnAwake")]
    [SerializeField] MusicOnAwake musicOnAwake = new MusicOnAwake();
    [SerializeField] public AudioSource Source,MusicSource;

    public bool SoundsOn;
    public bool MusicOn;
    public bool VibrOn;

    //bool onActiveSongs = true;

    private void Start()
    {

        if (musicOnAwake.onAwake)PlayMusic(musicOnAwake.song);
        Source = GetComponent<AudioSource>();
        EventManager.AddListener<songs>(SongsEvents.PlaySFX, EffectSong);
        EventManager.AddListener<songs>(SongsEvents.PlayMusic, PlayMusic);
        EventManager.AddListener<songs>(SongsEvents.LoopSong, LoopSong);
        //EventManager.AddListener<bool>(LevelFlowEvents.levelSongState, StateSongs);
        EventManager.AddListener(SongsEvents.StopSong, StopMusic);
        EventManager.AddListener(SongsEvents.EnableMusic, EnableMusic);
        EventManager.AddListener(SongsEvents.StopSounds, StopSounds);
        EventManager.AddListener(SongsEvents.EnableSounds, EnableSound);

        // The following is to not reset the maintheme each time a scene change
        GameObject[] musicObject = GameObject.FindGameObjectsWithTag("GameSounds");
        if (musicObject.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<songs>(SongsEvents.PlaySFX, EffectSong);
        EventManager.RemoveListener<songs>(SongsEvents.PlayMusic, PlayMusic);
        EventManager.RemoveListener<songs>(SongsEvents.LoopSong, LoopSong);
        //EventManager.RemoveListener<bool>(LevelFlowEvents.levelSongState, StateSongs);
        EventManager.RemoveListener(SongsEvents.StopSong, StopMusic);
        EventManager.RemoveListener(SongsEvents.EnableMusic, EnableMusic);
        EventManager.RemoveListener(SongsEvents.StopSounds, StopSounds);
        EventManager.RemoveListener(SongsEvents.EnableSounds, EnableSound);
    }

    void EffectSong(songs song) {
        //if (!MusicOn) return;
        AudioClip clip = soundClips.FirstOrDefault(data => data.song == song).clip;
        Source.PlayOneShot(clip);
    }

    void LoopSong(songs song)
    {
        //if (!MusicOn) return;
        AudioClip clip = soundClips.FirstOrDefault(data => data.song == song).clip;
        Source.loop = true;
        Source.clip = clip;
        Source.Play();
    }
    void PlayMusic(songs song)
    {
        //if (!MusicOn) return;
        MusicSource.enabled = true;
        AudioClip clip = soundClips.FirstOrDefault(data => data.song == song).clip;
        MusicSource.loop = true;
        MusicSource.clip = clip;
        MusicSource.Play();
    }
    public void StopMusic()
    {
        Source.loop = false;
        MusicSource.enabled = false;
    }

    public void EnableMusic()
    {
        MusicSource.enabled = true;
    }

    public void StopSounds()
    {
        Source.enabled = false;
    }

    public void EnableSound()
    {
        Source.enabled = true;
    }

}
