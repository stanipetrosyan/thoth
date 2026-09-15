using System;
using System.Collections.Generic;
using System.Linq;
using Exp;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Editor.Exp {
    
    [ScriptedImporter(1, DialogueGraph.AssetExtension)]
    public class DialogueGraphImporter: ScriptedImporter {
        
        public override void OnImportAsset(AssetImportContext ctx) {
            DialogueGraph editorGraph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);
            RuntimeDialogueGraph runtimeGraph = ScriptableObject.CreateInstance<RuntimeDialogueGraph>();

            var nodeIDMap = new Dictionary<INode, string>();

            foreach (var node in editorGraph.GetNodes()) {
                nodeIDMap[node] = Guid.NewGuid().ToString();
            }
            
            var startNode = editorGraph.GetNodes().OfType<StartNode>().FirstOrDefault();
            if (startNode != null) {
                var entryPort = startNode.GetOutputPorts().FirstOrDefault()?.firstConnectedPort;
                if (entryPort != null) {
                    runtimeGraph.EntryNodeID = nodeIDMap[entryPort.GetNode()];
                }
            }

            foreach (var node in editorGraph.GetNodes()) {
                if (node is StartNode || node is EndNode) continue;

                var runtimeNode = new RuntimeDialogueNode { NodeID = nodeIDMap[node] };
                if (node is DialogueNode dialogueNode) {
                    ProcessDialogueNode(dialogueNode, runtimeNode, nodeIDMap);
                }
                
                runtimeGraph.AllNodes.Add(runtimeNode);
            }
            
            ctx.AddObjectToAsset("RuntimeData", runtimeGraph);
            ctx.SetMainObject(runtimeGraph);
        }

        private void ProcessDialogueNode(DialogueNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap) {
            runtimeNode.Speaker = GetPortValue<string>(node.GetInputPortByName("Speaker"));
            runtimeNode.DialogueText = GetPortValue<string>(node.GetInputPortByName("Dialogue"));
            
            var nextNodePort = node.GetOutputPortByName("out")?.firstConnectedPort;
            if (nextNodePort != null) {
                runtimeNode.NextNodeID = nodeIDMap[nextNodePort.GetNode()];
            }
        }

        private void ProcessChoideNode(ChoiceNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap) {
            
        }

        private T GetPortValue<T>(IPort port) {
            if (port == null) return default;

            if (port.isConnected) {
                if (port.firstConnectedPort.GetNode() is IVariableNode variableNode) {
                    variableNode.variable.TryGetDefaultValue(out T value);
                    return value;
                }
            }
            
            port.TryGetValue(out T fallbackValue);
            return fallbackValue;
        }
    }
}