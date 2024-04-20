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
    
    private Queue<DialogueLine> lineQueue;

    public bool isDialogueActive = false;

    public float typingSpeed = 2.0f;

    public Animator animator;
    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        animator.Play("show");
        lineQueue.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            lineQueue.Enqueue(line);
            
        }
        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (lineQueue.Count == 0) 
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lineQueue.Dequeue();

        characterSprite.sprite = currentLine.character.sprite;
        CharacterName.name = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.textLine.ToCharArray())
        {
            dialogueArea.text += ;
            yield return new WaitForSeconds(typingSpeed);
        }        
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        animator.PLay("hide");
    }
}
