using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RPGCharacter : RPGComponent
{
    [SerializeField]
    public Character character = null;

    public class DialogueHUD
    {
        DialogueHUD nextDialogueHUD;

        DialogueStatic dialogueStatic;
        DialogueChoice dialogueChoice;
        public DialogueType type;

        public DialogueHUD(DialogueStatic _dialogue) { dialogueStatic = _dialogue; type = DialogueType.DIALOGSTATIC; }
        public DialogueHUD(string str) { dialogueChoice = new DialogueChoice(str); type = DialogueType.DIALOGCHOICE; }

        public DialogueHUD NextDialogueHUD { get => nextDialogueHUD; set => nextDialogueHUD = value; }
        public DialogueStatic DialogueStatic { get => dialogueStatic; set => dialogueStatic = value; }
        public DialogueChoice DialogueChoice { get => dialogueChoice; set => dialogueChoice = value; }

    }

    DialogueHUD currentDialogue;
    DialogueHUD lastDialogueAdded;

    public DialogueHUD CurrentDialogue { get => currentDialogue; set => currentDialogue = value; }

    void Awake()
    {
        if(character)
            RPGManager.Instanse.AddEntity(this, character.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void AddDialogue(int index)
    {
        RPGDialogueManager.Instanse.DebugLog("AddDialog : " + index);
        Debug.Log("AddDialog : " + index);
        if (index > character.dialogues.Count-1)
            return;

        DialogueStatic dialog = character.dialogues[index];

        if (CurrentDialogue == null)
        {
            CurrentDialogue = new DialogueHUD(dialog);
            lastDialogueAdded = CurrentDialogue;
        }
        else
        {
            lastDialogueAdded.NextDialogueHUD = new DialogueHUD(dialog);
            lastDialogueAdded = lastDialogueAdded.NextDialogueHUD;
        }
    }

    public void AddProposition(string proposition)
    {
        RPGDialogueManager.Instanse.DebugLog("AddProposition : " + proposition);

        if (CurrentDialogue == null)
        {
            CurrentDialogue = new DialogueHUD(proposition);
            lastDialogueAdded = CurrentDialogue;
        }
        else
        {
            if(lastDialogueAdded.DialogueStatic != null)
            {
                lastDialogueAdded.NextDialogueHUD = new DialogueHUD(proposition);
                lastDialogueAdded = lastDialogueAdded.NextDialogueHUD;
            }

            else
            {
                DialogueChoice dialogueChoice = lastDialogueAdded.DialogueChoice;
                dialogueChoice.Relplies.Add(proposition);
            }

            
        }
    }

    
    public void Debugtest(string str)
    {
        Debug.Log(str);
    }

    public bool Condition1(bool booleen)
    {
        return booleen;
    }

}

