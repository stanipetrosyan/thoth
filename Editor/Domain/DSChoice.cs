using System;
using UnityEngine;

namespace Editor.Domain {
    [Serializable]
    public class DSChoice {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}