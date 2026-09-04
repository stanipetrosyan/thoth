using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Editor.Domain;
using Editor.Style;
using Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Elements {
    public class DSNode : Node {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public string DialogueText { get; set; }
        public DSDialogueType DialogueType { get; set; }
        public List<DSChoice> Choices { get; set; }


        private TextField dialogueNameField;
        private TextField dialogueTextField;
        private VisualElement dialogueContainer;
        private Foldout foldout;


        protected virtual void Initialize(Vector2 position) {
            ID = Guid.NewGuid().ToString();
            DialogueName = "Dialogue Name";
            DialogueText = "Dialogue Text";
            Choices = new List<DSChoice>();
            SetPosition(new Rect(position, Vector2.zero));
        }

        public static DSNode From(DSDialogueType dialogueType, Vector2 position) {
            DSNode node;
            switch (dialogueType) {
                case DSDialogueType.Single:
                    node = new DSSingleChoiceNode();
                    break;
                case DSDialogueType.Multiple:
                    node = new DSMultipleChoiceNode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dialogueType), dialogueType, null);
            }
            node.Initialize(position);
            
            return node;
        }


        public virtual void Draw() {
            dialogueNameField = new DSTextField(DialogueName, (callback) => DialogueName = callback.newValue);
            titleContainer.Add(dialogueNameField);
            
            Port input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            inputContainer.Add(input);

            dialogueContainer = new VisualElement();

            foldout = new Foldout {
                text = "Dialogue Area"
            };

            TextField dialogueText = new DSTextField(DialogueText, (callback) => DialogueText = callback.newValue) {
                multiline = true
            };
            foldout.Add(dialogueText);
            dialogueContainer.Add(foldout);
            extensionContainer.Add(dialogueContainer);

            SetStyle();
            RefreshExpandedState();
        }

        private void SetStyle() {
            mainContainer.style.backgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            /*dialogueContainer.style.marginLeft = new StyleLength(new Length(-4f));
            dialogueContainer.style.marginTop = new StyleLength(new Length(4f));*/
        
            foldout.style.marginLeft = new StyleLength(new Length(-4f));
            foldout.style.marginTop = new StyleLength(new Length(4f));
        }
        
        public bool IsStartingNode() {
            Port inputPort = (Port) inputContainer.Children().First();
            return !inputPort.connected;
        }
    }
}