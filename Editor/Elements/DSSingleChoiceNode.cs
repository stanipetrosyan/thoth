using Editor.Domain;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Elements {
    class DSSingleChoiceNode : DSNode {
        protected override void Initialize(Vector2 position) {
            base.Initialize(position);

            DialogueType = DSDialogueType.Single;
            DSChoice choice = new DSChoice() {
                Text = "Next Dialogue"
            };

            Choices.Add(choice);
        }

        public override void Draw() {
            base.Draw();

            foreach (var choice in Choices) {
                var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                    typeof(bool));
                port.portName = choice.Text;

                port.userData = choice;
                outputContainer.Add(port);
            }

            RefreshExpandedState();
        }
    }
}