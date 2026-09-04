using System.Collections.Generic;
using Editor.Adapters;
using Editor.Domain;
using thot.DS.Domain;
using UnityEditor;

namespace Editor.Inspectors {
    [CustomEditor(typeof(DSDialogue))]
    public class DSInspector : UnityEditor.Editor {
        private SerializedProperty dialogueContainerProperty;
        private SerializedProperty dialogueGroupProperty;
        private SerializedProperty dialogueProperty;

        private SerializedProperty groupedDialogueProperty;
        private SerializedProperty startingDialogueProperty;

        private SerializedProperty selectedDialogueGroupIndexProperty;
        private SerializedProperty selectedDialogueIndexProperty;

        private void OnEnable() {
            dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
            dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");

            groupedDialogueProperty = serializedObject.FindProperty("groupedDialogues");
            startingDialogueProperty = serializedObject.FindProperty("startingDialoguesOnly");

            selectedDialogueGroupIndexProperty = serializedObject.FindProperty("selectedDialogueGroupIndex");
            selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawDialogueContainerArea();

            DSDialogueContainerSO dialogueContainer =
                (DSDialogueContainerSO)dialogueContainerProperty.objectReferenceValue;

            if (dialogueContainerProperty.objectReferenceValue == null) {
                StopDrawing("Select a dialogue Container to see the rest of the inspector");

                return;
            }

            DrawFiltersArea();

            List<string> dialogueNames;
            string dialogueFolderPath = $"Assets/DialogueSystem/Dialogues/";

            if (groupedDialogueProperty.boolValue) {
                var dialogueGroupNames = dialogueContainer.GetDialogueGroupNames();
                if (dialogueGroupNames.Count == 0) {
                    StopDrawing("Select a dialogue Group containing the dialogue groups");

                    return;
                }

                DrawDialogueGroupArea(dialogueContainer, dialogueGroupNames);

                var dialogueGroup = (DSDialogueGroupSO)dialogueGroupProperty.objectReferenceValue;
                dialogueNames = dialogueContainer.GetGroupedDialogueNames(dialogueGroup);
                dialogueFolderPath += $"/Groups/{dialogueGroup.GroupName}/Dialogues";
            }
            else {
                dialogueNames = dialogueContainer.GetUngroupedDialogueNames();
                dialogueFolderPath += "/Global/Dialogues";
            }

            if (dialogueNames.Count == 0) {
                StopDrawing("Select a dialogue Name");

                return;
            }


            DrawDialogueArea(dialogueNames, dialogueFolderPath);

            serializedObject.ApplyModifiedProperties();
        }

        private void StopDrawing(string reason) {
            EditorGUILayout.HelpBox(reason, MessageType.Info);

            serializedObject.ApplyModifiedProperties();
        }


        private void DrawDialogueContainerArea() {
            EditorGUILayout.LabelField("Dialogue Container", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(dialogueContainerProperty);
            EditorGUILayout.Space(4);
        }


        private void DrawFiltersArea() {
            EditorGUILayout.LabelField("Filters", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(groupedDialogueProperty);
            EditorGUILayout.PropertyField(startingDialogueProperty);
            EditorGUILayout.Space(4);
        }


        private void DrawDialogueGroupArea(DSDialogueContainerSO dialogueContainer, List<string> dialogueGroupNames) {
            EditorGUILayout.LabelField("Dialogue Groups", EditorStyles.boldLabel);

            selectedDialogueGroupIndexProperty.intValue = EditorGUILayout.Popup("Dialogue Group",
                selectedDialogueGroupIndexProperty.intValue, dialogueGroupNames.ToArray());

            var selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];
            DSDialogueGroupSO selectedDialogueGroup = Assets.LoadAsset<DSDialogueGroupSO>(
                $"Assets/DialogueSystem/Dialogues/Groups/{selectedDialogueGroupName}",
                selectedDialogueGroupName);

            dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;

            EditorGUILayout.PropertyField(dialogueGroupProperty);
            EditorGUILayout.Space(4);
        }

        private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath) {
            EditorGUILayout.LabelField("Dialogue", EditorStyles.boldLabel);

            selectedDialogueIndexProperty.intValue = EditorGUILayout.Popup("Dialogue",
                selectedDialogueIndexProperty.intValue, dialogueNames.ToArray());

            var selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];
            DSDialogueSO selectedDialogue = Assets.LoadAsset<DSDialogueSO>(dialogueFolderPath, selectedDialogueName);

            dialogueProperty.objectReferenceValue = selectedDialogue;

            EditorGUILayout.PropertyField(dialogueProperty);
            EditorGUILayout.Space(4);
        }
    }
}