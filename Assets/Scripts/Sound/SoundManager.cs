using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public GameObject PreviousBackgroundMusicObject;
    private AudioSource PreviousBackgroundMusic; 
    private AudioSource MainAudioSource;
    [SerializeField] float FadeTime = 2f;
    float OriginalVolue;
    public void Awake()
    {
        MainAudioSource = GetComponent<AudioSource>();
        PreviousBackgroundMusic = PreviousBackgroundMusicObject.GetComponent<AudioSource>();
        OriginalVolue = MainAudioSource.volume;
        DontDestroyOnLoad(gameObject); // 오브젝트 파괴 방지
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject BackgroundAudio = GameObject.FindWithTag("BackgroundMusic");
        if (BackgroundAudio == null) {  // 배경음악이 있는지 확인
            return;
        }

        AudioSource CurrentBackgroundMusic = BackgroundAudio.GetComponent<AudioSource>();

        if (CurrentBackgroundMusic == null || (PreviousBackgroundMusic != null && CurrentBackgroundMusic.clip == PreviousBackgroundMusic.clip)) {
            return; // 오디오 소스가 없거나 클립이 같은 경우 리턴
        }

        // 배경음악 변경: 새로운 클립 재생
        MainAudioSource.clip = CurrentBackgroundMusic.clip;
        MainAudioSource.Play();
        StartCoroutine(FadeIn(MainAudioSource, FadeTime));
        PreviousBackgroundMusic.clip = CurrentBackgroundMusic.clip; // 현재 음악을 이전 음악으로 설정
    }

    IEnumerator FadeIn(AudioSource AudioSource, float FadeTime) { // 배경음악 변경 시 볼륨이 서서히 커지도록 설정
        float StartVolume = 0f;
        AudioSource.volume = 0f; // 초기 볼륨을 0으로 설정

        while (AudioSource.volume < OriginalVolue) { // 기존 볼륨까지 점진적으로 증가
            AudioSource.volume += StartVolume + Time.deltaTime / FadeTime;
            yield return null;
        }

        AudioSource.volume = OriginalVolue;
    }
}
