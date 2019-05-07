using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType
{
    DIALOGSTATIC = 0,
    DIALOGCHOICE
}


[System.Serializable]
public struct Reply
{
    public Parametre parametre;
    public string reply;
}

[System.Serializable]
public class DialogueStatic
{
    public DialogueStatic()
    {
    }

    public DialogueStatic CreateCopy()
    {
        return new DialogueStatic(this);
    }

    DialogueStatic(DialogueStatic dialogueStatic)
    {
        dialogueStatic.Replies = Replies;
    }


    public List<Reply> replies = new List<Reply>();

    public List<Reply> Replies { get => replies; set => replies = value; }
}

[System.Serializable]
public class DialogueChoice
{
    public DialogueChoice(string str)
    {
        Relplies = new List<string>();
        Relplies.Add(str);
    }

    public List<string> Relplies { get; set; }
}






