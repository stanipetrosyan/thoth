using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Editor;
using Editor.Domain;
using Editor.Elements;
using Editor.Style;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace thot.DS.Windows {
    public class DSGraphView : GraphView {
        private DSSearchWindow _searchWindow;
        private readonly DSEditorWindow _editorWindow;

        private Dictionary<string, DSNode> ungroupedNodes = new Dictionary<string, DSNode>();

        public DSGraphView(DSEditorWindow dsEditorWindow) {
            this._editorWindow = dsEditorWindow;

            AddGridBackground();

            AddManipulators();
            AddSearchWindow();

            OnGraphViewChanged();
            OnElementDeleted();
        }

        public void Clear() {
            ungroupedNodes.Clear();
        }


        private void AddManipulators() {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateContextualMenu(
                title: "Add Single Choice Node",
                action => CreateElementNode(DSNode.From(DSDialogueType.Single, action.eventInfo.localMousePosition))
            ));

            this.AddManipulator(CreateContextualMenu(
                title: "Add Multiple Choice Node",
                action => CreateElementNode(DSNode.From(DSDialogueType.Multiple, action.eventInfo.localMousePosition))
            ));

            this.AddManipulator(CreateContextualMenu(
                title: "Add Group",
                action => CreateGroup("DialogueName")
            ));
        }


        private IManipulator CreateContextualMenu(string title, Action<DropdownMenuAction> callback) {
            var contextualMenuManipulator =
                new ContextualMenuManipulator(@event => @event.menu.AppendAction(title, callback)
                );

            return contextualMenuManipulator;
        }

        private void AddSearchWindow() {
            if (_searchWindow == null) {
                _searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
                _searchWindow.Initialize(this);
            }

            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        #region Graph View Elements

        public void CreateElementNode(DSNode node) {
            node.Draw();

            AddElement(node);
            AddUngroupedNode(node);
        }

        //TODO: duplicated node NAME case
        private void AddUngroupedNode(DSNode node) {
            ungroupedNodes.Add(node.ID, node);
        }

        // TODO: remove all edge and open ports
        private void RemoveUngroupedNode(DSNode node) {
            ungroupedNodes.Remove(node.ID);
            RemoveElement(node);
        }

        private void CreateEdges(List<Edge> edgesToCreate) {
            foreach (Edge edge in edgesToCreate) {
                DSNode nextNode = (DSNode)edge.input.node;
                DSChoice choiceData = (DSChoice)edge.output.userData;
                choiceData.NodeID = nextNode.ID;
            }
        }

        private void CreateGroup(string groupName) {
            DSGroup group = new DSGroup {
                title = groupName
            };

            AddElement(group);
        }

        #endregion

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false) {
            var worldMousePosition = mousePosition;

            if (isSearchWindow) {
                worldMousePosition -= _editorWindow.position.position;
            }

            return contentViewContainer.WorldToLocal(worldMousePosition);
        }


        #region Graph View Events

        private void OnGraphViewChanged() {
            graphViewChanged = (changes) => {
                if (changes.edgesToCreate != null) {
                    CreateEdges(changes.edgesToCreate);
                }

                return changes;
            };
        }

        private void OnElementDeleted() {
            deleteSelection = (operationName, askUser) => {
                var nodeToDelete = new List<DSNode>();
                var edgeToDelete = new List<Edge>();

                foreach (var element in selection) {
                    switch (element) {
                        case DSNode node:
                            nodeToDelete.Add(node);
                            break;
                        case Edge edge:
                            edgeToDelete.Add(edge);
                            break;
                    }
                }

                foreach (var node in nodeToDelete) {
                    RemoveUngroupedNode(node);
                    RemoveElement(node);
                }

                foreach (var edge in edgeToDelete) {
                    RemoveElement(edge);
                }
            };
        }

        #endregion

        #region Graph View Method Overrides

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port => {
                if (startPort == port || startPort.node == port.node || startPort.direction == port.direction) {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        #endregion

        #region Graph View Style

        private void AddGridBackground() {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            gridBackground.style.backgroundColor = Colors.FromRGBToColor(43, 43, 43);

            Insert(0, gridBackground);
        }

        #endregion

        #region Graph Utility Methods

        public List<DSNode> GetNodes() {
            return ungroupedNodes.Values.ToList();
        }

        #endregion
    }
}