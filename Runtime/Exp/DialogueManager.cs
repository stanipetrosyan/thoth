using UnityEngine;

namespace Exp {
    public class DialogueManager : MonoBehaviour {
        [SerializeField] public RuntimeDialogueGraph runtimeDialogue;

        [SerializeField] private string selectedDialogueId;

        public RuntimeDialogueGraph RuntimeDialogue => runtimeDialogue;

        public string SelectedDialogueId => selectedDialogueId;

        public RuntimeDialogueNode SelectedDialogue {
            get {
                if (runtimeDialogue == null)
                    return null;

                return runtimeDialogue.AllNodes.Find(node => node.NodeID == selectedDialogueId
                );
            }
        }
    }
}