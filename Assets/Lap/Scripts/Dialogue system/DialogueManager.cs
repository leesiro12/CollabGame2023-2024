using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterSprite;
    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI dialogueArea;
    
    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        animator.Play("show");
        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);            
        }
        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (lines.Count == 0) 
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterSprite.sprite = currentLine.character.sprite;
        CharacterName.text = currentLine.character.name;
        

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.textLine.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }        
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        animator.Play("hide");
    }
}
