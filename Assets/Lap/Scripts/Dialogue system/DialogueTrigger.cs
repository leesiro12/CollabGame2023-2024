using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name; public Sprite sprite;
    
}



[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string textLine;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool canRun = true;
    private int i = 0;
    
    public void TriggerDialogue(SimpleMovement movementScript)
    {
        if (!canRun)
        {
            return;
        }
        else
        {
            canRun = false;
        }

        if (dialogue == null) Debug.Log("Cannot found Dialogue");


        if (i == 0)
        {
            i++;
            movementScript.IgnoreMovementInout();
            DialogueManager.Instance.StartDialogue(dialogue);
        }
        else if (i < dialogue.dialogueLines.Count)
        {
           if(DialogueManager.Instance.DisplayNextDialogue())
           {
               i++;
           }
        }
        else
        {
            movementScript.SubscribeToMovement();
            //Debug.Log("maxed lines");
            DialogueManager.Instance.EndDialogue();
            i = 0;
        }

        canRun = true;
        

        //if (dialogueIsPlaying == true)
        //{
        //    DialogueManager.Instance.DisplayNextDialogue();
        //}
        //else if (dialogueIsPlaying == false)
        //{
        //    DialogueManager.Instance.StartDialogue(dialogue);
        //    dialogueIsPlaying = true;
        //}
        
    }
}
