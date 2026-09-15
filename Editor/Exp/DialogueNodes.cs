using System;
using Unity.GraphToolkit.Editor;

namespace Editor.Exp {
    [Serializable]
    public class StartNode : Node {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddOutputPort("out").Build();
        }
    }


    [Serializable]
    public class EndNode : Node {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort("in").Build();
        }
    }


    [Serializable]
    public class DialogueNode : Node {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort("in").Build();
            context.AddOutputPort("out").Build();

            context.AddInputPort<string>("Speaker").Build();
            context.AddInputPort<string>("Dialogue").Build();
        }
    }

    [Serializable]
    public class ChoiceNode : Node {
        private const string optionID = "portCount";
        
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort("in").Build();

            context.AddInputPort<string>("Speaker").Build();
            context.AddInputPort<string>("Dialogue").Build();
            
            var option = GetNodeOptionByName(optionID);
            option.TryGetValue(out int portCount);
            for (int i = 0; i < portCount; i++) {
                context.AddInputPort<string>($"Choice Text {i}").Build();
                context.AddOutputPort($"Choice {i}").Build();
            }
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context) {
            context.AddOption<int>(optionID);
        }
    }
}