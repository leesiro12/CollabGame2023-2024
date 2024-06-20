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
    private DialogueLine currentLine;

    private Queue<DialogueLine> lines;

    public static bool isDialogueActive = false;

    public float typingTime = 0.5f;

    public Animator animator;

    private Coroutine typingCoroutine;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (isDialogueActive == false)
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
        
    }

    public bool DisplayNextDialogue()
    {
        if (typingCoroutine != null)
        {
            StopAllCoroutines();
            dialogueArea.text = currentLine.textLine;
            typingCoroutine = null;
            return false;
        }
        else
        {
            if (lines.Count == 0)
            {
                EndDialogue();
                return true;
            }

            currentLine = lines.Dequeue();
            //Debug.Log(currentLine);
            characterSprite.sprite = currentLine.character.sprite;
            CharacterName.text = currentLine.character.name;

            typingCoroutine = StartCoroutine(TypeSentence(currentLine));
        }

        return true;
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.textLine.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingTime);
        }

        typingCoroutine = null;
    }

    public void EndDialogue()
    {    
        isDialogueActive = false;
        animator.Play("hide");
    }

    
}
