using System.Collections.Generic;
using System.Linq;
using Editor.Domain;
using thot.DS.Domain;
using UnityEngine;

public class DSDialogueContainerSO : ScriptableObject {
    [field: SerializeField] public string FileName { get; set; }


    [field: SerializeField] public List<DSDialogueSO> UngroupedDialogues { get; set; }

    public void Initialize(string filename) {
        this.FileName = filename;
        this.UngroupedDialogues = new List<DSDialogueSO>();
    }

    public void AddDialogue(DSDialogueSO dialogue) {
        this.UngroupedDialogues.Add(dialogue);
    }

    public List<string> GetDialogueGroupNames() {
        return new List<string>();
    }

    public List<string> GetGroupedDialogueNames(DSDialogueGroupSO group) {
        return new List<string>();
    }
        
    public List<string> GetUngroupedDialogueNames() {
        return UngroupedDialogues.Select(item => item.DialogueName).ToList();
    }
}