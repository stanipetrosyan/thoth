using System.Collections.Generic;
using Domain;
using Editor.Elements;
using Elements;
using thot.DS.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor {
    public class DSSearchWindow : ScriptableObject, ISearchWindowProvider {
        private DSGraphView graphView;
        private Texture2D indentationIcon;

        public void Initialize(DSGraphView dsGraphView) {
            this.graphView = dsGraphView;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
            var result = new List<SearchTreeEntry>() {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue node"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", indentationIcon)) {
                    level = 2,
                    userData = DSDialogueType.Single
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", indentationIcon)) {
                    level = 2,
                    userData = DSDialogueType.Multiple
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentationIcon)) {
                    level = 2,
                    userData = new Group()
                }
            };

            return result;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
            var localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);

            graphView.CreateElementNode(DSNode.From((DSDialogueType)searchTreeEntry.userData, localMousePosition));
            return true;
        }
    }
}