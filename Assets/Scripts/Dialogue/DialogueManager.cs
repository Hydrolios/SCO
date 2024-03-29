using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class DialogueManager : MonoBehaviour
{

    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;
    
    public GameObject shopUI;
    public GameObject lvlSys;
    public GameObject npctoggle;
    public Player uiChecker;
    public LevelSys levelSys;
   
    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;
    public GameObject player;
    public Vector2 playerposition;

    public FastTravel ftMenu;

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    Dialogue dialogue;
    int currentLine = 0;
    bool isTyping; //to make sure players can't skip dialogue while it is being typed
    bool loadReq;
    bool shopNpc;
    bool expGiver;
    bool battle;
    string loadScene;
    public IEnumerator ShowDialogue(Dialogue dialogue, string scene, bool load, bool shop, bool exp) // show dialogue and sets conditions for any additional actions required
    {
        uiChecker.openedDialog = true;
        uiChecker.StopSpeed();
        yield return new WaitForEndOfFrame();

        OnShowDialogue?.Invoke();
        this.dialogue = dialogue;
        dialogueBox.SetActive(true);
        loadReq = load;
        loadScene = scene;
        shopNpc = shop;
        expGiver = exp;
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));

    }
    public IEnumerator ShowDialogueV2(Dialogue dialogue) // simplified check for more niche uses which doesn't require much
    {
        uiChecker.openedDialog = true;
        uiChecker.StopSpeed();
        yield return new WaitForEndOfFrame();

        OnShowDialogue?.Invoke();
        this.dialogue = dialogue;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));

    }
    public IEnumerator ShowDialogueV3(Dialogue dialogue, string scene, bool encounter) // simplified check for more niche uses which doesn't require much
    {
        uiChecker.openedDialog = true;
        uiChecker.StopSpeed();
        yield return new WaitForEndOfFrame();

        OnShowDialogue?.Invoke();
        this.dialogue = dialogue;
        loadReq = true;
        battle = encounter;
        loadScene = scene;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));

    }

    public void HandleUpdate()
    {
        levelSys = lvlSys.GetComponent<LevelSys>();
        if ((Input.GetKeyDown(KeyCode.E) && !isTyping) || (Input.GetKeyDown(KeyCode.Space) && !isTyping) || (Input.GetMouseButtonDown(0) && !isTyping)) //Inside player class which is what i assigned to allow players to interact
        {
            ++currentLine;
            if (currentLine < dialogue.Lines.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                dialogueBox.SetActive(false);
                uiChecker.openedDialog = false;
                uiChecker.ResumeSpeed();
                OnCloseDialogue?.Invoke();
                if (loadReq)
                {
                    if (battle) //check if its a battle to save previous position and scene otherwise not needed
                    {

                        int activeScene = SceneManager.GetActiveScene().buildIndex;
                        PlayerPrefs.SetInt("PreBattleScene", activeScene);
                        PlayerPrefs.SetFloat("PBx", player.transform.position.x);
                        PlayerPrefs.SetFloat("PBy", player.transform.position.y);
                        PlayerPrefs.SetInt("encounter", (battle ? 1 : 0));
                    }
                    SceneManager.LoadScene(loadScene);
                    PlayerPrefs.SetFloat("interact_range", 0.6f);
                }
                else if (shopNpc)
                {
                    PlayerPrefs.SetFloat("interact_range", 0f);
                    uiChecker.openedUIShop = true;
                    shopUI.SetActive(true);
                    Text curcash = shopUI.transform.Find("PlayerCash").GetComponent<Text>();
                    curcash.text = "$" + PlayerPrefs.GetInt("currentcash");
                    Time.timeScale = 0f;
                    /*shop UI opens
                     * shopUI.SetActive(true);
                     * Time.timeScale = 1f;
                     * we will allow pausing during this
                     * to close the ui, we use the function called 
                     */
                }
                else if (PlayerPrefs.GetInt("ftNPC") != 0)  // pop up window for fast travel window
                {
                    ftMenu.Selection();
                    
                }
            }
        }
        
    }

    //Method to print out dialogue character by character so there is a flow to the dialogue box
    public IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        /*string[] splitLines = line.Split('\n'); // Split the line at '\n'
        foreach (var letter in line.ToCharArray()) // displays text one letter at a time
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }*/
        string[] splitLines = line.Split(new[] { "\\n" }, StringSplitOptions.None); // Split the line at "\n" and puts each part into an array

        for (int i = 0; i < splitLines.Length; i++)
        {
            for (int j = 0; j < splitLines[i].Length; j++) // this loops prints out the text in the array element
            {
                if (splitLines[i][j] == '\\' && j + 1 < splitLines[i].Length && splitLines[i][j + 1] == 'n') // replaces and removes the \n
                {
                    dialogueText.text += "\n"; // Add a new line if "\\n" is detected
                    j++; // Skip the 'n' character
                }
                else
                {
                    dialogueText.text += splitLines[i][j];
                }

                yield return new WaitForSeconds(1f / lettersPerSecond);
            }

            if (i < splitLines.Length - 1)
            {
                dialogueText.text += "\n"; // Add a new line after each split
            }
        }

        isTyping = false;
        
    }

}
