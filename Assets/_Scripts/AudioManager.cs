using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public Sound[] sounds;
    public AudioSource backgroundSound;
    private bool mute = false;

	void Start ()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		} else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	public void Play (string sound)
	{
		// if (GameManager.mute)
		// 	return;
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Play();
	}
	public void Pause (string sound)
	{
		// if (GameManager.mute)
		// 	return;
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Pause();
	}

    public void toggleSound(){
        foreach( AudioSource audio in GetComponents<AudioSource>() ){
            audio.mute = !mute;
            mute = !mute;
        }
    }

    

    // Code will be to call the AudioManager in any script you need and pass a string to the Play() function. As thus:
// 
// 
// 
// 
// 
// 
// 
// 
// 
//     
    // NOW THESE ARE NOT REAL FUNCTIONS. THE FUNCTION CONTENT IS WHAT MATTERS, THEY ARE CALLED IN YOUR FUNCTIONS 
    // WHEREVER IT MAY HAVE BEEN WRITTEN
    public void End () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("End");
    }

    public void Coin () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Coin");
    }

    public void Coin2 () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Coin 2");
    }

    public void Background (bool pause = false) {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if(pause){
        sound.Pause("Background");
        }else{
        sound.Play("Background");
        }
    }

    public void Landing1 () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Landing 1");
    }

    public void Landing2 () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Landing 2");
    }

    public void MagnetLifetime () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Magnet Lifetime");
    }

    public void MagnetPowerup () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Magnet Powerup");
    }

    public void PlayerJump () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Player Jump");
    }

    public void RunningInWilderness () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Running");
    }

    public void SpeedPowerup () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Speed powerup");
    }

    public void CharacterPurchase () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Character purchase");
    }

    public void Explainer () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Explainer");
    }

    public void MoneyBagDrop () {
        AudioManager sound = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sound.Play("Money bag drop");
    }

}
