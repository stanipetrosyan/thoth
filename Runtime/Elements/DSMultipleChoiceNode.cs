using Domain;
using Style;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Elements {
    class DSMultipleChoiceNode : DSNode {
        protected override void Initialize(Vector2 position) {
            base.Initialize(position);

            DialogueType = DSDialogueType.Multiple;

            var choice = new DSChoice() {
                Text = "Next Choice"
            };

            Choices.Add(choice);
        }

        public override void Draw() {
            base.Draw();

            Button addChoiceButton = new DSButton("Add Choice", () => {
                DSChoice choice = new DSChoice {
                    Text = "Next Choice"
                };

                Choices.Add(choice);
            
                CreateChoice(choice);
            });
        
            mainContainer.Add(addChoiceButton);
            foreach (var choice in Choices) {
                CreateChoice(choice);
            }
        
        }

        private void CreateChoice(DSChoice choice) {
            Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            choicePort.portName = "";

            choicePort.userData = choice;

            TextField choiceTextField = new DSTextField("New Choice");
            choicePort.Add(choiceTextField);
            outputContainer.Add(choicePort);
        }
    }
}