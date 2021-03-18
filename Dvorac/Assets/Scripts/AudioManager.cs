using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject gameManager;
    private GameLoop gameLoopScript;

    public Sound[] sounds;

    // =========== Helper Functions ===========
    public static float Remap(float val, float in1, float in2, float out1, float out2)
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        gameLoopScript = gameManager.GetComponent<GameLoop>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    // Called on settings screen exit
    // Updates sound volumes with data from GameManager
    public void updateVolumes()
    {
        foreach (Sound s in sounds)
        {
            //StopSound(s.name);
            if (s.type == "OST") 
            {
                s.source.volume = Remap(gameLoopScript.volumeOST, 0, 1, 0, gameLoopScript.volumeMaster);
            }
            else if (s.type == "Effect")
            {
                s.source.volume = Remap(gameLoopScript.volumeEffects, 0, 1, 0, gameLoopScript.volumeMaster);
            }
            Debug.Log("Volume of " + s.name + " set to " + s.source.volume);
            //PlaySound(s.name);
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        if (s.source == null)
        {
            s.source.Play();
            return;
        }
        if(!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
}
