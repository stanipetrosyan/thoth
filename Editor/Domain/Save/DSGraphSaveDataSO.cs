using System.Collections.Generic;
using System.Linq;
using Editor.Elements;
using UnityEngine;

namespace thot.DS.Domain.Save {
    public class DSGraphSaveDataSO : ScriptableObject {
        [field: SerializeField] public string Filename { get; set; }

        //[field: SerializeField] public List<DSGroupSaveData> Groups { get; set; }
        [field: SerializeField] public List<DSNodeSaveData> Nodes { get; set; }


        public void Initialize(string filename) {
            Filename = filename;
            //Groups = new List<DSGroupSaveData>();
            Nodes = new List<DSNodeSaveData>();
        }

        public void AddNodes(List<DSNode> nodes) {
            nodes.ForEach(SaveNodeToGraph);
        }

        private void SaveNodeToGraph(DSNode node) {
            var choices = node.Choices
                .Select(choice => new DSChoiceSaveData() { Text = choice.Text, NodeID = choice.NodeID }).ToList();

            DSNodeSaveData nodeData = new DSNodeSaveData() {
                ID = node.ID,
                Name = node.DialogueName,
                Choices = choices,
                Text = node.DialogueText,
                Position = node.GetPosition().position,
                DialogueType = node.DialogueType,
                //GroupID = node.Group?.ID
            };

            Nodes.Add(nodeData);
        }
    }
}