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


    public List<string> linesOfCreator = new List<string>();

    public List<int> linesOfCreatorADDLIGHT = new List<int>();

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

    [Header("Light For Dialogues")]

    public GameObject prefabLightForDialogues;

    public List<GameObject> listOfLightsOnScene = new List<GameObject>();

    public List<int> lenghtOfDialogues = new List<int>();


    void Start()
    {
        GameManager.LaunchDialogue += SetupDialogueWithIndex; // Abonnement


        shadow.SetActive(false);
        DialoguesBubble.SetActive(false);
        continueText.SetActive(false);

        GameManager.instance.indexDialogues = 0; // RESET
        GameManager.instance.TriggerEvent();

        StartCoroutine(WaitingForSpecialDialogueStartScene(0.01f)); // COOLDOWN DE 0.01SEC CAR YA LE LOAD SCENE QUI RESETE AVANT


    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.instance.isInDialogueWithMonster && npcIsTyping == false && indexOfLine != linesOfCreator.Count && canPlayerCloseDialogue == false)
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
        foreach(GameObject itemLighted in listOfLightsOnScene) // CLEAR LES LUMIERES
        {
            Destroy(itemLighted);
        }


        if(indexOfLine != linesOfCreator.Count)
        {
            StartCoroutine(isPlayerAbleToSkipDialogue(0.1f));
            continueText.SetActive(false);
            npcIsTyping = true;

            typewriterByCharacterMainText.ShowText(linesOfCreator[indexOfLine]);
            typewriterByCharacterMainText.StartShowingText();


            if(linesOfCreatorADDLIGHT[indexOfLine] != 0) // SPAWN LES LUMIERES SI BESOIN
            {
                for(int i = 0; i < GameManager.instance.posToLightUp.Count; i++) 
                {
                    if(linesOfCreatorADDLIGHT[indexOfLine] == GameManager.instance.indexToLightUp[i]) // SI LITEM A LE BON INDEX PAR RAPPORT A SON ORDRE DE LUMIERE ON L'ALLUME
                    {
                        GameObject newLight = Instantiate(prefabLightForDialogues, Vector3.zero, Quaternion.identity);
                        listOfLightsOnScene.Add(newLight);
                        RectTransform newLightRTTransform = newLight.GetComponent<RectTransform>();

            

                        newLight.transform.SetParent(GameManager.instance.posToLightUp[i].transform.parent);

                        RectTransform posLightRT = GameManager.instance.posToLightUp[i].GetComponent<RectTransform>();
                        newLightRTTransform.anchoredPosition = new Vector2(posLightRT.anchoredPosition.x, posLightRT.anchoredPosition.y); 

                        RectTransform newLightRT = newLight.GetComponent<RectTransform>();
                        newLightRT.localScale = posLightRT.localScale * 0.5f;
                        newLight.transform.SetAsFirstSibling();
                    }

                }
            }

            indexOfLine++;

            
        }
        else
        {
            Debug.LogWarning(indexOfLine);
            Debug.LogWarning("Out of range : MAIS ARRETE SA TOUT DE SUITE MON PETIT !!");
        }

        
    }

    public void finishedTalking()
    {
        npcIsTyping = false;
        continueText.SetActive(true);

        continueTextWritingSpeedManager = continueText.GetComponent<TypewriterByCharacter>();
        if(indexOfLine != linesOfCreator.Count)
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

    IEnumerator WaitingForSpecialDialogueStartScene(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        if(SceneManager.GetActiveScene().name == "Level2")
        {
            Debug.Log("WAHHHHHH");
            GameManager.instance.indexDialogues = 9993; // LAUNCH DIALOGUE
            GameManager.instance.TriggerEvent();
            GameManager.instance.indexDialogues = 122774647; // LAUNCH DIALOGUE
            GameManager.instance.TriggerEvent();
        }
    }


    void SetupDialogueWithIndex() // INDEX STARTING WITH 1000X ARE BONUS SCENES // 999X = DEBUT DU NIVEAU // 666X = DIDACTICIEL || 122774647 CODE POUR LANCER DIALOGUE
    { // 0 = RESET
        if(this != null)
        {
            bool isShadowed = true;

            if(GameManager.instance.indexDialogues == 0)
            {
                linesOfCreator = new List<string>();
                linesOfCreatorADDLIGHT = new List<int>();
                lenghtOfDialogues = new List<int>();

                GameManager.instance.posToLightUp = new List<GameObject>();
                GameManager.instance.indexToLightUp = new List<int>();
            }
            if(GameManager.instance.indexDialogues == 9991) // TUTORIEL LEVEL 1
            {
                linesOfCreator.Add("Let me introduce you to the game. And <size=125%>NO</size> <waitfor=0.2> I'm not doing that because I'm the tutorial guy or because I'm friendly. <size=90%>I just have my own reasons okay ?</size>");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("So, this drawer is your <color=#2d89ea>inventory</color>, in it you will always see 2 types of object.");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("The <bounce a=1.5><color=#2d89ea>boxes</color></bounce>, that will gives you items to stack on each others in the level.");
                linesOfCreatorADDLIGHT.Add(1); // INDEX BOX
                linesOfCreator.Add("The <bounce a=1.5><color=#2d89ea>potions</color></bounce>, that will help/annoy you during each levels by making objects doing weird stuffs.");
                linesOfCreatorADDLIGHT.Add(2); // INDEX POTIONS
                linesOfCreator.Add("To complete a level you need to make your drawer <wave a=1.25>empty</wave> AND that nothing <size=60%>(except furnitures)</size> touches the floor.");
                linesOfCreatorADDLIGHT.Add(0);

            }

            if(GameManager.instance.indexDialogues == 9992) // TUTORIEL LEVEL 8
            {
                linesOfCreator.Add("<waitfor=0.35>OH <waitfor=0.35>MY <waitfor=0.35>GOD !!! You got a <shake a=1.65><color=#2d89ea>shiny box</color></shake> ??!!!");
                linesOfCreatorADDLIGHT.Add(4);
                linesOfCreator.Add("Ah, I got you, hehe. This is just a <bounce a=1.5><color=#2d89ea>furniture box</color></bounce>.");
                linesOfCreatorADDLIGHT.Add(4);
                linesOfCreator.Add("In this box you will always get furnitures, you <size=125%>can't</size> use any potion on those furnitures and they <size=125%>can</size> touch the ground !");
                linesOfCreatorADDLIGHT.Add(0); // INDEX BOX
                linesOfCreator.Add("I recommend you to use them to stack other items, but you can also stack them on each others, which is kinda dumb honestly.");
                linesOfCreatorADDLIGHT.Add(0); // INDEX POTIONS
                linesOfCreator.Add("Ah and, you can't rotate them. Wait... IT'S A <shake a=1.75><rainb f=0.95>SHINY FURNITURE BOX</rainb></shake> ??!!");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("Hehe. <size=75%>It's not.</size>");
                linesOfCreatorADDLIGHT.Add(0);

            }

            if(GameManager.instance.indexDialogues == 9993) // TUTORIEL LEVEL 3 - Molette
            {
                linesOfCreator.Add("Oh, do you want to know something funny and maybe a little useful ?");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("You can just scroll to <rot>rotate</rot> items when you have them on your cursor !");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("You can try right now !");
                linesOfCreatorADDLIGHT.Add(0);
            }
    
            if(GameManager.instance.indexDialogues == 6661) // TUTORIEL POPO ROUGE
            {
                linesOfCreator.Add("Ah, its the first time you meet this little fellow ? This <bounce a=1.5><color=#2d89ea>potion</color></bounce> makes things <size=150%>bigger</size>, you can accumulate up to 2 !");
                linesOfCreatorADDLIGHT.Add(3); // INDEX RED POTIONS
                linesOfCreator.Add("Trust me, you prefer it little brother... the <bounce a=1.5><color=#2d89ea>blue potion</color></bounce>.");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("Good luck trying to makes things not going everywhere with this one!");
                linesOfCreatorADDLIGHT.Add(0);
            }

            if(GameManager.instance.indexDialogues == 998) // TUTORIEL POPO VERTE MAIS ATTENTION L'INDEX!!
            {
                linesOfCreator.Add("The <color=#1b9115>green potion</color> . . . This one is pretty annoying im not gonna lie.");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("The developer of the game was <size=140%>SO <waitfor=0.3> LAZY</size> that he made it only working on <color=#d68a06><bounce>blank items.</bounce></color>");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("Eh, At least it has a silly effect, I'll let you find out what it does.");
                linesOfCreatorADDLIGHT.Add(0);


            }

            if(GameManager.instance.indexDialogues == 999)
            {
                linesOfCreator.Add("You... <size=120%>YOU JUST BROKE THE GAME!!!</size>");
                linesOfCreator.Add("I just told you that the <color=#1b9115>green potion</color> needs to be used on BLANK items.");
                linesOfCreator.Add("<size=70%>Ugh, if I'm stuck with someone that dumb its gonna take so long to reach the-</size>");
                linesOfCreator.Add("Oops! I was talking about the last guy that played the game <size=80%>not you...</size>");
                linesOfCreator.Add("So, I will give you the chance to retry the level once more so don't break everything <size=110%>AGAIN</size>, <waitfor=0.5> please.");

                needToAnnounceLooseAtTheEnd = true;
            }

            if(GameManager.instance.indexDialogues == 10001)
            {
                linesOfCreator.Add("Yeah, I've locked the QUIT button.");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("<size=250%><shake a=4>WHY ?</size></shake>");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("You <waitfor=0.25>LITERALLY bought this game for <shake a=2><size=150%><waitfor=0.2>0.<waitfor=0.2>0<waitfor=0.2>0<waitfor=0.2>$<waitfor=0.2> B<waitfor=0.2>U<waitfor=0.2>C<waitfor=0.2>K<waitfor=0.2>S</size></shake> and <shake a=1.25> you don't even TRY playing it</shake> ? <size=80%>I've never seen something like this in my whole career...</size>");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("Now try to clear <size=150%>AT LEAST</size> one level and I will make you able to QUIT, but you know, you will love the game so no need to worry...");
                linesOfCreatorADDLIGHT.Add(0);
                linesOfCreator.Add("<size=80%>Don't even dare ALT+F4.</size>");
                linesOfCreatorADDLIGHT.Add(0);

            }

            if(GameManager.instance.indexDialogues == 10002)
            {
                int random = Random.Range(0, 2);
                if(random == 0)
                {
                    linesOfCreator.Add("Nah.");
                    linesOfCreatorADDLIGHT.Add(0);

                }
                
                else if(random == 1)
                {
                    linesOfCreator.Add("No.");
                    linesOfCreatorADDLIGHT.Add(0);

                }

                else if(random == 2)
                {
                    linesOfCreator.Add("Nope.");
                    linesOfCreatorADDLIGHT.Add(0);

                }
                isShadowed = false;

            }

            if(GameManager.instance.indexDialogues == 122774647)
            {
                if(linesOfCreator.Count > 0)
                {
                    StartCoroutine(StartDialogue(isShadowed, 0.35f+0f)); // NEED SHADOW OFF + COOLDOWN 0.35 COOLDOWN + PERSO
                }
                
            }
        }

    }



}
