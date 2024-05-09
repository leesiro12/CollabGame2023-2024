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
    
    public void TriggerDialogue(bool dialogueIsPlaying)
    {
        if (dialogue == null) Debug.Log("Cannot found Dialogue");

        if (dialogueIsPlaying == true)
        {
            DialogueManager.Instance.DisplayNextDialogue();
        }
        else if (dialogueIsPlaying == false)
        {
            DialogueManager.Instance.StartDialogue(dialogue);            
        }
        
    }
}
