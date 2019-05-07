using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RPGDialogueManager : RPGObjectInteractableComponent
{
    [SerializeField]
    Button ChoicePrefab;

    [SerializeField]
    Text textCharacter;

    [SerializeField]
    Text textDialogue;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    Image avatar;

    [SerializeField]
    Text debug;

    [SerializeField]
    Transform choiceListUI;


    int currentchoice = -1;
    int currentproposition = -1;

    [SerializeField]
    float m_waitTime = 0.1f;
    [SerializeField]
    float m_waitTimeDialogue = 5.0f;

    bool dialogueRun = false;

    bool choice = false;

    RPGCharacter currentNPC;

    static RPGDialogueManager instanse = null;
    public static RPGDialogueManager Instanse
    {
        get
        {
            if (instanse == null)
                instanse = GameObject.Find("RPGDialogManager").GetComponent<RPGDialogueManager>();

            return instanse;
        }
    }

    public void LauchDialogue(RPGCharacter npcCharacter)
    {
        dialogueRun = true;

        currentNPC = npcCharacter;
        canvas.enabled = true;
        StartCoroutine(WriteSubTitle(npcCharacter.CurrentDialogue));
    }

    IEnumerator WriteSubTitle(RPGCharacter.DialogueHUD dialogueHUD)
    {
        currentNPC.CurrentDialogue = dialogueHUD;
        if (dialogueHUD.type == DialogueType.DIALOGSTATIC)
        {
            DialogueStatic dialogue = dialogueHUD.DialogueStatic;

            foreach (Reply reply in dialogue.Replies)
            {
                string name = reply.parametre.parameterString;
                Debug.Log(name);
                Character character;
                if (name == "Player")
                {
                    name = RPGManager.Instanse.RPGPlayerCharacter.character.name;
                    character = RPGManager.Instanse.RPGPlayerCharacter.character;
                }
                else
                    character = currentNPC.character;
                    
                if (character != null)
                    avatar.sprite = Sprite.Create(character.characterAvatar, new Rect(0, 0, character.characterAvatar.width, character.characterAvatar.height), new Vector2(0.5f, 0.5f));
                textCharacter.text = name;
                textDialogue.text = "";
                    foreach (char letter in reply.reply)
                    {
                        textDialogue.text += letter;
                        yield return new WaitForSeconds(m_waitTime);
                    }
                yield return new WaitForSeconds(m_waitTimeDialogue);
            }
        }

        else
        {
            currentchoice++;
            DialogueChoice dialogueChoice = dialogueHUD.DialogueChoice;

            choice = true;
            for (int i = 0; i < dialogueChoice.Relplies.Count;i++)
            {
              //  string choice in dialogueChoice.Relplies
              
                Button button = Instantiate(ChoicePrefab, choiceListUI);
                button.transform.localPosition -= new Vector3(0, i * 80, 0);
                Text text =  button.gameObject.transform.GetComponentInChildren<Text>();
                text.text = dialogueChoice.Relplies[i];

                int temp = i;

                button.onClick.AddListener(() => { currentproposition = temp; choice = false; Clear(choiceListUI); });
            }

            choiceListUI.transform.localPosition = new Vector3(0, (dialogueChoice.Relplies.Count - 1) / 2 * 80, 0);
            while(choice)
            {
                yield return null;
            }
            choice = false;

           // yield return null;
         //   yield return null;

            if (dialogueHUD.NextDialogueHUD != null)
                yield return WriteSubTitle(dialogueHUD.NextDialogueHUD);


            Debug.Log("END");
            currentchoice++;
            currentproposition = -1;

        }

        if (dialogueHUD.NextDialogueHUD != null)
            yield return WriteSubTitle(dialogueHUD.NextDialogueHUD);

        currentchoice = -1;
        currentproposition = -1;
        yield return null;

        canvas.enabled = false;

    }

    void LauchDialogue(string message)
    {

    }

    public void Clear(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public bool ChoiceSelectionned(RPGCharacter character, int choice, int proposition)
    {
        if (currentNPC == character && choice == currentchoice && proposition == currentproposition)
            return true;

        return false;
    }

    public void DebugLog(string str)
    {
        debug.text += str + '\n';
    }

}








