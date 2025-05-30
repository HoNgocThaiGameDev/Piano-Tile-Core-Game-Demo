using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



public class AudioController : BySingleton<AudioController>
{
    public bool isSoundOff;
    AudioSource source;
    string IS_UNLOCK = "isUnlock";
    bool isSync;
    List<ISound> sounds = new List<ISound>();
    [SerializeField]
    List<ISong> songList= new List<ISong>();
    private List<ConfigSongRecord> configSongs;
    public AudioClip fullMusic;

    void Start()
    {
        configSongs = ConfigManager.instance.configSong.GetAllRecordSong();
        LoadConfigSong();
        source = GetComponent<AudioSource>();
        SynchroSongs();
    }

    private void LoadConfigSong()
    {
        for(int i = 0; i < configSongs.Count; i++)
        {
            songList[i].id = configSongs[i].id;
            songList[i].name = configSongs[i].Name;
            songList[i].type = configSongs[i].Type;
            songList[i].totalNote = configSongs[i].TotalNote;
            songList[i].tempo = configSongs[i].Tempo;
            songList[i].file = Resources.Load("Music/" + configSongs[i].FileID) as AudioClip;
            songList[i].isUnlock=configSongs[i].IsUnlock;
        }
    }
    public void Play(string name)
    {
        if (isSoundOff) return;
        if (sounds.Find(s => s.name == name) == null) return;
        AudioClip clip = sounds.Find(s => s.name == name).clip;
        if (clip != null)
            source.PlayOneShot(clip);



    }

    public void PlayFullMusic()
    {
        if (isSoundOff) return;
        for (int i = 0; i < songList.Count; i++)
        {
            if (songList[i].name == GameController.instance.SONG_NAME)
            {
                fullMusic = songList[i].file;
                break;
            }
        }
        source.clip = fullMusic;
        source.Stop();
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
    public void Pause()
    {
        source.Pause();
    }

    public ISong GetSong(string name)
    {
        ISong song = songList.Find(x => x.name == name);
        int unlock = PlayerPrefs.GetInt(IS_UNLOCK + name, -1);
        if (unlock > -1)
        {
            song.isUnlock = unlock == 1;
        }
        return song;
    }
    public List<ISong> GetSongs()
    {
        if (!isSync)
            SynchroSongs();
        return songList;
    }

    private void SynchroSongs()
    {
        for (int i = 0; i < songList.Count; i++)
        {
            string name = songList[i].name;
            songList[i].id = i + 1;
            Debug.LogError(songList[i].id);
            GetSong(name);
        }
        isSync = true;

    }

    public void ResumeMusic()
    {
        if (source != null && !source.isPlaying && source.clip == fullMusic)
        {
            source.UnPause();
        }
    }

    public void PauseFullMusic()
    {
        if (source != null && source.isPlaying)
        {
            source.Pause();
        }
    }

}
