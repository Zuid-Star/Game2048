using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 音频管理器，管理音乐和音效的播放
/// </summary>
public class AudioManager : SingletonBase<AudioManager>
{
    private AudioSource _backMusic;
    private float _musicVolume = 0.5f;//背景音乐音量
    
    private float _soundVolume = 0.5f;//音效音量
    
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="musicName">音乐(切片)名称</param>
    public void PlayBackMusic(string musicName)
    {
        if (_backMusic == null)
        {
            //如果场景中没有背景音乐对象，就创建gameObject并挂载AudioSource对象
            GameObject obj = new GameObject("BackMusic");
            _backMusic = obj.AddComponent<AudioSource>();
        }
        //异步加载背景音乐
        ResourceManager.GetInstance().LoadAsync<AudioClip>("Music/BackMusic/" + musicName,(audioClip) =>
        {
            _backMusic.clip = audioClip;
            _backMusic.volume = _musicVolume;
            _backMusic.Play();
        });
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBackMusic()
    {
        if (_backMusic == null)
            return;
        _backMusic.Pause();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBackMusic()
    {
        if (_backMusic == null)
            return;
        _backMusic.Stop();
    }

    /// <summary>
    /// 改变背景音乐音量
    /// </summary>
    /// <param name="volume">音量大小（0-1）</param>
    public void ChangeMusicVolume(float volume)
    {
        _musicVolume =  volume;
        if (_backMusic == null)
            return;
        _backMusic.volume = _musicVolume;
    }

    //=============================================音效部分===============================================
    
    /// <summary>
    /// 播放音效(需要制作音效预制体)
    /// </summary>
    /// <param name="soundName">音效(预制体)名称</param>
    public void PlaySound(string soundName)
    {
        PoolManager.GetInstance().GetObject(soundName,"Music/SoundPrefab/" + soundName, (gameObject) =>
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.volume = _soundVolume;
            audioSource.Play();
            //使用公共mono类启动协程
            MonoManager.GetInstance().StartCoroutine(PlaySoundEnd(audioSource, (audioSource) =>
            {
                if (audioSource != null)
                {
                    PoolManager.GetInstance().ReturnObject(soundName,audioSource.gameObject);//播放完成后将预制体放回到缓冲池中
                }
            }));
            
        });
    }

    /// <summary>
    /// audioSource没有自带的回调函数，通过封装到协程中，实现播放完毕后的回调函数
    /// </summary>
    /// <param name="audioSource">音频播放器</param>
    /// <param name="callback">回调函数</param>
    /// <returns></returns>
    private IEnumerator PlaySoundEnd(AudioSource audioSource, UnityAction<AudioSource> callback)
    {
        // yield return new WaitUntil(() => audioSource.isPlaying);
        yield return new WaitForSeconds(audioSource.clip.length); // 直接等待音频播放完成
        callback(audioSource);
    }

    /// <summary>
    /// 改变音效的音量大小
    /// </summary>
    /// <param name="volume">音量大小</param>
    public void ChangeSoundVolume(float volume)
    {
        _soundVolume = volume;
    }
}

