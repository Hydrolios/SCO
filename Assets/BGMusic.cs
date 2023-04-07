using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{

    public static BGMusic instance;

    public AudioClip tavern1Music; // Take some rest - xDeviruchi
    public AudioClip battle1Music; // Decisive Battle - xDeviruchi
    public AudioClip battle2Music; // Minigame - xDeviruchi
    public AudioClip menuMusic; // And the Journey Begins - xDeviruchi
    public AudioClip dungeon1Music; // Mysterious Dungeon - xDeviruchi

    public AudioSource audioSource;
    public string currentTrackName;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentTrackName = "";
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable() //for handling scene changes
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() //for handling scene changes
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {

        switch (sceneName)
        {
            case "Menu":
                audioSource.clip = menuMusic;
                if (audioSource.clip.ToString() == currentTrackName)
                {
                    audioSource.UnPause();
                    return;
                }
                audioSource.Play();
                break;
            case "Tavern_Hysteria":
            case "Tavern_Hysteria_F2":
            case "Tavern_HysteriaTut":
            case "THysteria_F2_Perm":
            case "Sarah_House":
            case "Town_Saleria":
            case "Town_SaleriaV2":
                audioSource.clip = tavern1Music;
                if (audioSource.clip.ToString() == currentTrackName)
                {
                    audioSource.UnPause();
                    return;
                }
                audioSource.Play();
                break;
            case "TutBattle_THysteria":
            case "DreamCombat":
            case "RBDCombatGuard":
            case "RBDCombatGuardRage":
            case "RBDCombatShade":
                audioSource.clip = battle1Music;
                if (audioSource.clip.ToString() == currentTrackName)
                {
                    audioSource.UnPause();
                    return;
                }
                audioSource.Play();
                break;
            default:
                if (sceneName.Contains("RBD_"))
                {
                    Debug.Log("it is the dungeon");
                    audioSource.clip = dungeon1Music;
                    if (audioSource.clip.ToString() == currentTrackName)
                    {
                        audioSource.UnPause();
                        return;
                    }
                    audioSource.Play();
                }
                else
                {
                    Debug.Log("it is NOT the dungeon");
                    audioSource.Stop();
                }
                break;
        }
        currentTrackName = audioSource.clip.ToString();
    }
}






