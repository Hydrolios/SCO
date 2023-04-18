using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{

    public static BGMusic instance;
    public bool musicPlaying = false;
    public bool sceneChanged = false;
    public AudioClip tavern1MusicStart;
    public AudioClip tavern1MusicLoop; // Take some rest - xDeviruchi
    public AudioClip battle1MusicStart;
    public AudioClip battle1MusicLoop; // Decisive Battle - xDeviruchi
    public AudioClip battle2MusicStart;
    public AudioClip battle2MusicLoop; // Minigame - xDeviruchi
    public AudioClip menuMusicStart;
    public AudioClip menuMusicLoop; // And the Journey Begins - xDeviruchi
    public AudioClip dungeon1MusicStart;
    public AudioClip dungeon1MusicLoop; // Mysterious Dungeon - xDeviruchi

    public Coroutine loopCoroutine;

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
        sceneChanged = false;
        currentTrackName = "";
        //PlayMusicForScene(SceneManager.GetActiveScene().name);
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
        sceneChanged = true;
        audioSource.loop = false;
        StopLoop();
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {

        switch (sceneName)
        {
            case "Menu":
                PlayMusic(menuMusicStart, menuMusicLoop);
                break;
            case "Tavern_Hysteria":
            case "Tavern_Hysteria_F2":
            case "Tavern_HysteriaTut":
            case "THysteria_F2_Perm":
            case "Sarah_House":
            case "Town_Saleria":
            case "Town_SaleriaV2":
                PlayMusic(tavern1MusicStart, tavern1MusicLoop);
                break;
            case "TutBattle_THysteria":
            case "DreamCombat":
            case "RBDCombatGuard":
            case "RBDCombatGuardRage":
            case "RBDCombatShade":
                PlayMusic(battle1MusicStart, battle1MusicLoop);
                break;
            default:
                if (sceneName.Contains("RBD_"))
                {
                    PlayMusic(dungeon1MusicStart, dungeon1MusicLoop);
                }
                else
                {
                    audioSource.Stop();
                }
                break;
        }
        currentTrackName = audioSource.clip.ToString();
    }

    private void PlayMusic(AudioClip startAudio, AudioClip loopAudio)
    {
        //checking if the audio is the same upon scene transition to continue playing
        if (sceneChanged)
        {
            if (loopAudio.ToString() == currentTrackName || startAudio.ToString() == currentTrackName)
            {
                Debug.Log("same bgm");
                audioSource.UnPause();
                return;
            }
            if (loopAudio.ToString() == startAudio.ToString())
            {
                Debug.Log("start and loop is the same");
                audioSource.clip = startAudio;
                audioSource.Play();
                audioSource.loop = true;
                currentTrackName = audioSource.clip.ToString();
                return;
            }
        }
        audioSource.clip = startAudio;
        audioSource.Play();
        currentTrackName = audioSource.clip.ToString();
        Debug.Log("playing audio");
        
        
        
        

        
        musicPlaying = true;
        loopCoroutine = StartCoroutine(PlayLoop(audioSource.clip.length));
        IEnumerator PlayLoop(float length)
        {
            Debug.Log("waiting to loop");
            yield return new WaitForSeconds(length-0.3f);
            if(sceneChanged)
            {
                Debug.Log("stopping coroutine");
                StopCoroutine(loopCoroutine);
                sceneChanged = false;
                yield break;
                
            }
            Debug.Log("looping");
            audioSource.clip = loopAudio;
            currentTrackName = audioSource.clip.ToString();
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    private void StopLoop()
    {
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
    }
}





