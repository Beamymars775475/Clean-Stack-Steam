using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using Febucci.UI;
using TMPro;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialoguesCreatorScriptManager : MonoBehaviour
{

    public GameObject shadow;

    [Header("Feedbacks")]

    public MMF_Player moveCreatorToScreenFeedbacks1;

    public MMF_Player moveCreatorOutOfScreenFeedbacks1;

    public MMF_Player creatorTalkingFeedbacks;

    [Header("Text")]

    public TypewriterByCharacter typewriterByCharacterMainText;


    public string[] linesOfCreator;

    public TextMeshProUGUI dialogueText;

    public MMF_Player feedbacksSounds;

    public AudioClip[] smallSoundsChar; 

    public GameObject DialoguesBubble;


    public int test;

    public int soundsPerChar;

    public bool npcIsTyping;

    public int indexOfLine;

    public GameObject continueText;

    public bool isTextSkipable;

    public bool canPlayerCloseDialogue;


    private TypewriterByCharacter continueTextWritingSpeedManager;



    public bool needToAnnounceLooseAtTheEnd;


    void Start()
    {
        GameManager.LaunchDialogue += OnSoftlockStrangePotion; // Abonnement


        shadow.SetActive(false);
        DialoguesBubble.SetActive(false);
        continueText.SetActive(false);

        if(SceneManager.GetActiveScene().name == "RedLevel3")
        {
            linesOfCreator = new string[3];
            linesOfCreator[0] = "The <color=#1b9115>green potion</color> . . . This one is pretty annoying im not gonna lie.";
            linesOfCreator[1] = "The developer of the game was <size=140%>SO <waitfor=0.3> LAZY</size> that he made it only working on <color=#d68a06><bounce>blank items.</bounce></color>";
            linesOfCreator[2] = "Eh, At least it has a silly effect, I'll let you find out what it does.";

            StartCoroutine(StartDialogue(true, 0.35f));
        }


    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.instance.isInDialogueWithMonster && npcIsTyping == false && indexOfLine != linesOfCreator.Length && canPlayerCloseDialogue == false)
        {
            typewriterByCharacterMainText.waitForNormalChars = 0.04f;
            typewriterByCharacterMainText.waitLong = 0.6f;
            typewriterByCharacterMainText.waitMiddle = 0.2f;
            soundsPerChar = 2;
            WriteNextDialogue();
        }
        else if (Input.GetMouseButtonDown(0) && GameManager.instance.isInDialogueWithMonster && npcIsTyping == true && isTextSkipable == true && canPlayerCloseDialogue == false)
        {
            isTextSkipable = false;
            Debug.Log("GRAA");
            typewriterByCharacterMainText.waitForNormalChars = 0.008f;
            typewriterByCharacterMainText.waitLong = 0.011f;
            typewriterByCharacterMainText.waitMiddle = 0.009f;           

            soundsPerChar = 5;
        }
        else if(Input.GetMouseButtonDown(0) && GameManager.instance.isInDialogueWithMonster && canPlayerCloseDialogue == true)
        {
            GameManager.instance.isInDialogueWithMonster = false;
            moveCreatorOutOfScreenFeedbacks1.PlayFeedbacks();
            DialoguesBubble.SetActive(false);
        }


    }


    public void AnimationShowingUpCharacterDone() // Start a new dialogue
    {
        indexOfLine = 0;
        DialoguesBubble.SetActive(true);

        soundsPerChar = 2;

        // Setup
        typewriterByCharacterMainText.StartShowingText();
        typewriterByCharacterMainText.waitForNormalChars = 0.04f;
        typewriterByCharacterMainText.waitLong = 0.6f;
        typewriterByCharacterMainText.waitMiddle = 0.2f;

        WriteNextDialogue();

    }


    public void WriteNextDialogue()
    {
        if(indexOfLine != linesOfCreator.Length)
        {
            StartCoroutine(isPlayerAbleToSkipDialogue(0.1f));
            continueText.SetActive(false);
            npcIsTyping = true;

            typewriterByCharacterMainText.ShowText(linesOfCreator[indexOfLine]);
            typewriterByCharacterMainText.StartShowingText();

            indexOfLine++;

            
        }
        else
        {
            Debug.LogWarning("Out of range : MAIS ARRETE SA TOUT DE SUITE MON PETIT !!");
        }

        
    }

    public void finishedTalking()
    {
        npcIsTyping = false;
        continueText.SetActive(true);

        continueTextWritingSpeedManager = continueText.GetComponent<TypewriterByCharacter>();
        if(indexOfLine != linesOfCreator.Length)
        {
            continueTextWritingSpeedManager.ShowText("Click to continue...");
        }
        else
        {
            continueTextWritingSpeedManager.ShowText("Click to close...");
            canPlayerCloseDialogue = true;
        }

    }

    public void finishedDialogue()
    {
        if(needToAnnounceLooseAtTheEnd)
        {
            needToAnnounceLooseAtTheEnd = false;
            GameManager.instance.isGameOver = true;
        }

        shadow.SetActive(false);
    }


    public void soundDialogueEffect()
    {
        test++;
        if(test%soundsPerChar==0)
        {
            feedbacksSounds.PlayFeedbacks();
            creatorTalkingFeedbacks.PlayFeedbacks();
        }
    }

    IEnumerator isPlayerAbleToSkipDialogue(float cooldown)
    {
        if(isTextSkipable == false)
        {
            yield return new WaitForSeconds(cooldown);
            isTextSkipable = true;
        }
    }


    IEnumerator StartDialogue(bool needShadow, float cooldown)
    {
        
        yield return new WaitForSeconds(cooldown);

        canPlayerCloseDialogue = false;
        GameManager.instance.isInDialogueWithMonster = true;
        moveCreatorToScreenFeedbacks1.PlayFeedbacks();

        shadow.SetActive(true);
        Image shadowImg = shadow.GetComponent<Image>();
        if(needShadow)
        {
            shadowImg.color = new Color(0, 0, 0, 0.65f);
        }
        else
        {
            shadowImg.color = new Color(0, 0, 0, 0.01f);
        }
    }

    void OnSoftlockStrangePotion()
    {
        linesOfCreator = new string[5];
        linesOfCreator[0] = "You... <size=120%>YOU JUST BROKE THE GAME!!!</size>";
        linesOfCreator[1] = "I just told you that the <color=#1b9115>green potion</color> needs to be used on BLANK items.";
        linesOfCreator[2] = "<size=70%>Ugh, if im stuck with someone that much dumb its gonna take so long to reach the -</size>";
        linesOfCreator[3] = "Oops! I was talking about the last guy that played the game <size=80%>not you...</size>";
        linesOfCreator[4] = "So, I will give you the chance to retry the level once more so don't break everything <size=110%>AGAIN</size>, <waitfor=0.5> please.";

        needToAnnounceLooseAtTheEnd = true;
        StartCoroutine(StartDialogue(true, 1f));
    }

}
