using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using Febucci.UI;
using TMPro;

public class DialoguesCreatorScriptManager : MonoBehaviour
{

    public GameObject shadow;

    public MMF_Player moveCreatorToScreenFeedbacks1;

    public MMF_Player moveCreatorOutOfScreenFeedbacks1;

    public MMF_Player creatorTalkingFeedbacks;

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


    void Start()
    {
        shadow.SetActive(false);
        DialoguesBubble.SetActive(false);
        continueText.SetActive(false);
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

    public void StartDialogue(bool needShadow)
    {


        canPlayerCloseDialogue = false;
        GameManager.instance.isInDialogueWithMonster = true;
        moveCreatorToScreenFeedbacks1.PlayFeedbacks();

        if(needShadow)
        {
            shadow.SetActive(true);
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


}
