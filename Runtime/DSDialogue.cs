using UnityEngine;

public class DSDialogue : MonoBehaviour {
    [SerializeField] private DSDialogueContainerSO dialogueContainer;
    [SerializeField] private DSDialogueGroupSO dialogueGroup;
    [SerializeField] private DSDialogueSO dialogue;

    [SerializeField] private bool groupedDialogues;
    [SerializeField] private bool startingDialoguesOnly;

    [SerializeField] private int selectedDialogueGroupIndex;
    [SerializeField] private int selectedDialogueIndex;


    public string GetStartDialogue() {
        return dialogue.Text;
    }

    public string GetNextDialogue() {
        return dialogue.Choices[0].NextDialogue.Text;
    }
}