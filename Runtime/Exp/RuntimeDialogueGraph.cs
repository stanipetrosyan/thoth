using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exp {
    public class RuntimeDialogueGraph: ScriptableObject {
        public string EntryNodeID;
        public List<RuntimeDialogueNode> AllNodes = new List<RuntimeDialogueNode>();
    }

    [Serializable]
    public class RuntimeDialogueNode{
        public string NodeID;
        public string Speaker;
        public string DialogueText;
        public List<ChoiceData> Choices = new List<ChoiceData>();
        public string NextNodeID;
    }
    
    [Serializable]
    public class ChoiceData {
        public string ChoiceText;
        public string DesinationNodeId;
    }
}