using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Domain;
using Editor.Elements;
using thot.DS.Domain;
using thot.DS.Domain.Save;
using thot.DS.Windows;
using UnityEditor.Experimental.GraphView;

namespace Editor.Adapters {
    public class FileSystemGraph {
        private DSGraphView graphView;
        private const string containerFolderPath = "Assets/DialogueSystem/Dialogues";

        private Dictionary<string, DSDialogueSO> createdDialogues;
        private Dictionary<string, DSNode> loadedNodes;

        public FileSystemGraph(DSGraphView dsGraphView) {
            this.graphView = dsGraphView;

            createdDialogues = new Dictionary<string, DSDialogueSO>();
            loadedNodes = new Dictionary<string, DSNode>();
        }

        public void Initialize() {
            CreateStaticFolders();
        }

        private void CreateStaticFolders() {
            Assets.CreateFolder("Assets", "DialogueSystem");
            Assets.CreateFolder("Assets/DialogueSystem", "Dialogues");
            Assets.CreateFolder("Assets/DialogueSystem/Dialogues", "Graphs");
            Assets.CreateFolder(containerFolderPath, "Global");
            Assets.CreateFolder($"{containerFolderPath}/Global", "Dialogues");
            // CreateFolder(containerFolderPath, "Groups");
        }

        #region Save Methods

        public void Save(string filename) {
            DSGraphSaveDataSO graphData =
                Assets.UpsertAsset<DSGraphSaveDataSO>("Assets/DialogueSystem/Dialogues/Graphs", $"{filename}");
            graphData.Initialize(filename);

            DSDialogueContainerSO dialogueContainer =
                Assets.CreateAsset<DSDialogueContainerSO>(containerFolderPath, filename);
            dialogueContainer.Initialize(filename);

            SaveNodes(graphData, dialogueContainer);

            Assets.SaveAsset(graphData);
        }


        private void SaveNodes(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer) {
            var nodes = graphView.GetNodes();
            graphData.AddNodes(nodes);
            Assets.SaveAsset(graphData);

            foreach (var node in nodes) {
                SaveNodeToScriptableObject(node, dialogueContainer);
            }

            UpdateDialogChoicesConnections(nodes);
        }

        private void SaveNodeToScriptableObject(DSNode node, DSDialogueContainerSO dialogueContainer) {
            var dialogue =
                Assets.UpsertAsset<DSDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
            dialogueContainer.AddDialogue(dialogue);

            dialogue.Initialize(
                node.DialogueName,
                node.DialogueText,
                FromNodeChoices(node.Choices),
                node.DialogueType,
                node.IsStartingNode()
            );

            createdDialogues[node.ID] = dialogue;

            Assets.SaveAsset(dialogue);
        }

        private static List<DSDialogueSO.DSDialogueChoiceData> FromNodeChoices(List<DSChoice> nodeChoices) {
            return nodeChoices.Select(node => new DSDialogueSO.DSDialogueChoiceData {
                    Text = node.Text
                }
            ).ToList();
        }

        private void UpdateDialogChoicesConnections(List<DSNode> nodes) {
            foreach (DSNode node in nodes) {
                DSDialogueSO dialogue = createdDialogues[node.ID];

                for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex) {
                    var nodeChoice = node.Choices[choiceIndex];

                    if (string.IsNullOrEmpty(nodeChoice.NodeID)) {
                        continue;
                    }

                    dialogue.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];
                }

                Assets.SaveAsset(dialogue);
            }
        }

        #endregion

        #region Clear Methods

        public void Clear() {
            throw new NotImplementedException();
        }

        #endregion

        #region Load Methods

        public bool Load(string graphFilename) {
            var graphData =
                Assets.LoadAsset<DSGraphSaveDataSO>("Assets/DialogueSystem/Dialogues/Graphs", graphFilename);
            if (graphData == null) {
                return false;
            }

            LoadNodes(graphData.Nodes);
            LoadNodesConnections();

            return true;
        }

        private void LoadNodes(List<DSNodeSaveData> graphDataNodes) {
            graphDataNodes.ForEach(nodeData => {
                var dsChoices = FromNodeChoices(nodeData.Choices);

                var node = DSNode.From(nodeData.DialogueType, nodeData.Position);
                node.ID = nodeData.ID;
                node.DialogueName = nodeData.Name;
                node.DialogueText = nodeData.Text;
                node.Choices = dsChoices;

                graphView.CreateElementNode(node);

                loadedNodes.Add(node.ID, node);

                /*if (string.IsNullOrEmpty(nodeData.GroupID)) {
                    continue;
                }

                DSGroup group = loadedGroups[nodeData.GroupID];
                node.Group = group;

                group.AddElement(node);*/
            });
        }

        private List<DSChoice> FromNodeChoices(List<DSChoiceSaveData> nodeChoices) {
            return nodeChoices.Select(choice =>
                new DSChoice {
                    Text = choice.Text,
                    NodeID = choice.NodeID
                }
            ).ToList();
        }

        private void LoadNodesConnections() {
            foreach (KeyValuePair<string, DSNode> loadedNode in loadedNodes) {
                foreach (var visualElement in loadedNode.Value.outputContainer.Children()) {
                    var choicePort = (Port)visualElement;
                    DSChoice choiceData = (DSChoice)choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NodeID)) {
                        return;
                    }

                    DSNode nextNode = loadedNodes[choiceData.NodeID];

                    Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();
                    Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                    graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        #endregion
    }
}