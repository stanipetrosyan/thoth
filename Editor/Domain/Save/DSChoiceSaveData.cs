using System;
using UnityEngine;

namespace thot.DS.Domain.Save {
    
    [Serializable]
    public class DSChoiceSaveData {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}