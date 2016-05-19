using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] levelMusicChangeArray;

	private AudioSource audioSource;

	void Awake() {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PlayerPrefsManager.GetMasterVolume ();
	}
	
	void OnLevelWasLoaded (int level) {
		AudioClip thisLevelMusic = levelMusicChangeArray[level];

		if (thisLevelMusic) { // If there's some music attached
			audioSource.clip = thisLevelMusic;
			audioSource.volume = PlayerPrefsManager.GetMasterVolume ();
			audioSource.loop = true;
			audioSource.Play ();
		}
	}
	
	public void SetVolume (float volume) {
		audioSource.volume = volume;
	}
}
