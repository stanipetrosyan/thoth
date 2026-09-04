using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Domain
{
    public class DSDialogueContainerSO : ScriptableObject {
        [field: SerializeField] public string FileName { get; set; }


        [field: SerializeField] public List<DSDialogueSo> UngroupedDialogues { get; set; }

        public void Initialize(string filename) {
            this.FileName = filename;
            this.UngroupedDialogues = new List<DSDialogueSo>();
        }

        public void AddDialogue(DSDialogueSo dialogue) {
            this.UngroupedDialogues.Add(dialogue);
        }

        public List<string> GetDialogueGroupNames() {
            return this.UngroupedDialogues.Select(item => item.DialogueName).ToList();
        }

        public List<string> GetGroupedDialogueNames(DSDialogueGroupSO group) {
            return new List<string>();
        }
        
        public List<string> GetUngroupedDialogueNames() {
            return UngroupedDialogues.Select(item => item.DialogueName).ToList();
        }
    }
}