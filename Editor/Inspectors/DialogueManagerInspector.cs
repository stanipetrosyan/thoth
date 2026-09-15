using System.Collections.Generic;
using System.Linq;
using Domain;
using Exp;
using UnityEditor;
using UnityEngine;

namespace Editor.Inspectors {
    [CustomEditor(typeof(DialogueManager))]
    public class DialogueManagerInspector : UnityEditor.Editor {
        private SerializedProperty runtimeDialogueProperty;
        private SerializedProperty selectedDialogueIdProperty;

        private void OnEnable() {
            runtimeDialogueProperty =
                serializedObject.FindProperty("runtimeDialogue");

            selectedDialogueIdProperty =
                serializedObject.FindProperty("selectedDialogueId");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawDialogueContainerArea();

            RuntimeDialogueGraph runtimeDialogueGraph =
                runtimeDialogueProperty.objectReferenceValue
                    as RuntimeDialogueGraph;

            if (runtimeDialogueGraph == null) {
                StopDrawing(
                    "Select a Dialogue Container to see the rest of the inspector."
                );

                return;
            }

            List<RuntimeDialogueNode> nodes =
                runtimeDialogueGraph.AllNodes;

            if (nodes == null || nodes.Count == 0) {
                StopDrawing(
                    "The selected Dialogue Container contains no dialogue nodes."
                );

                return;
            }

            DrawDialogueArea(runtimeDialogueGraph);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDialogueContainerArea() {
            EditorGUILayout.LabelField(
                "Dialogue Container",
                EditorStyles.boldLabel
            );

            EditorGUILayout.PropertyField(
                runtimeDialogueProperty
            );

            EditorGUILayout.Space(8);
        }

        private void DrawDialogueArea(
            RuntimeDialogueGraph runtimeDialogueGraph) {
            EditorGUILayout.LabelField(
                "Dialogue",
                EditorStyles.boldLabel
            );

            List<string> dialogueNames =
                runtimeDialogueGraph.AllNodes
                    .Select(node => node.NodeID)
                    .ToList();

            int currentIndex =
                dialogueNames.IndexOf(
                    selectedDialogueIdProperty.stringValue
                );

            if (currentIndex < 0)
                currentIndex = 0;

            int selectedIndex = EditorGUILayout.Popup(
                "Dialogue",
                currentIndex,
                dialogueNames.ToArray()
            );

            selectedDialogueIdProperty.stringValue =
                dialogueNames[selectedIndex];

            EditorGUILayout.Space(8);

            RuntimeDialogueNode selectedDialogue =
                runtimeDialogueGraph.AllNodes.Find(node =>
                    node.NodeID ==
                    selectedDialogueIdProperty.stringValue
                );

            if (selectedDialogue == null) {
                EditorGUILayout.HelpBox(
                    "The selected dialogue node could not be found.",
                    MessageType.Warning
                );

                return;
            }

            DrawSelectedDialogue(selectedDialogue);
        }

        private void DrawSelectedDialogue(
            RuntimeDialogueNode selectedDialogue) {
            EditorGUILayout.LabelField(
                "Selected Dialogue",
                EditorStyles.boldLabel
            );

            EditorGUILayout.Space(4);

            EditorGUILayout.LabelField(
                "Node ID",
                selectedDialogue.NodeID
            );

            EditorGUILayout.LabelField(
                "Speaker",
                selectedDialogue.Speaker
            );

            EditorGUILayout.LabelField(
                "Dialogue Text"
            );

            EditorGUILayout.HelpBox(
                selectedDialogue.DialogueText,
                MessageType.None
            );

            EditorGUILayout.Space(8);

            DrawChoices(selectedDialogue);

            EditorGUILayout.Space(4);

            EditorGUILayout.LabelField(
                "Next Node",
                selectedDialogue.NextNodeID
            );
        }

        private void DrawChoices(
            RuntimeDialogueNode selectedDialogue) {
            EditorGUILayout.LabelField(
                "Choices",
                EditorStyles.boldLabel
            );

            if (selectedDialogue.Choices == null ||
                selectedDialogue.Choices.Count == 0) {
                EditorGUILayout.LabelField(
                    "No choices."
                );

                return;
            }

            for (int i = 0;
                 i < selectedDialogue.Choices.Count;
                 i++) {
                ChoiceData choice =
                    selectedDialogue.Choices[i];

                EditorGUILayout.BeginVertical(
                    EditorStyles.helpBox
                );

                EditorGUILayout.LabelField(
                    $"Choice {i + 1}",
                    EditorStyles.boldLabel
                );

                EditorGUILayout.LabelField(
                    "Text",
                    choice.ChoiceText
                );

                EditorGUILayout.LabelField(
                    "Destination",
                    choice.DesinationNodeId
                );

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(4);
            }
        }

        private void StopDrawing(string reason) {
            EditorGUILayout.HelpBox(
                reason,
                MessageType.Info
            );

            serializedObject.ApplyModifiedProperties();
        }
    }
}