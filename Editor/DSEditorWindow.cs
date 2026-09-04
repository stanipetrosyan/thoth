using System;
using System.IO;
using System.Web;
using Editor.Adapters;
using Style;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace thot.DS.Windows {
    public class DSEditorWindow : EditorWindow {
        private const string DefaultDialogueFilename = "Dialogue Filename";
        private TextField filenameTextField;
        private FileSystemGraph fsGraph;
        private DSGraphView graphView { get; set; }

        [MenuItem("Tests/Dialogue Graph")]
        public static void OpenDialogueGraphWindow() {
            var window = GetWindow<DSEditorWindow>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        public void OnEnable() {
            AddGraphView();
            AddToolbar();
        }

        void OnDestroy() {
            graphView.Clear();
        }

        private void AddGraphView() {
            DSGraphView dsGraphView = new DSGraphView(this);

            dsGraphView.StretchToParentSize();
            rootVisualElement.Add(dsGraphView);

            graphView = dsGraphView;
        }

        private void AddToolbar() {
            fsGraph = new FileSystemGraph(graphView);
            fsGraph.Initialize();

            Toolbar toolbar = new Toolbar();

            TextField filename = new DSTextField(DefaultDialogueFilename);
            filenameTextField = filename;

            Button saveButton = new DSButton("Save", Save);
            Button clearButton = new DSButton("Clear", Clear);
            Button loadButton = new DSButton("Load", Load);

            toolbar.Add(filename);
            toolbar.Add(saveButton);
            toolbar.Add(clearButton);
            toolbar.Add(loadButton);

            rootVisualElement.Add(toolbar);
        }

        private void Load() {
            var path = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/DialogueSystem/Dialogues/Graphs",
                "asset");
            if (string.IsNullOrEmpty(path)) {
                EditorUtility.DisplayDialog("Error", "No filename selected", "OK");
                return;
            }

            var graphFilename = Path.GetFileNameWithoutExtension(path);
            if (fsGraph.Load(graphFilename)) {
                filenameTextField.value = graphFilename;
            }
            else {
                EditorUtility.DisplayDialog("Error", "Failed to load graph file: " + graphFilename, "Ok");
            }
        }

        private void Clear() {
            throw new NotImplementedException();
        }

        private void Save() {
            if (string.IsNullOrEmpty(filenameTextField.value)) {
                EditorUtility.DisplayDialog("Error", "No filename selected", "OK");
            }

            fsGraph.Save(filenameTextField.value);
        }
    }
}