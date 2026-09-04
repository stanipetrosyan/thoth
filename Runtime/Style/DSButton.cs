using System;
using UnityEngine.UIElements;

namespace Editor.Style {
    public class DSButton : Button {
        public DSButton(string text, Action onClick = null) {
            this.text = text;
            clickable = new Clickable(onClick);

            style.backgroundColor = Colors.FromRGBToColor(37, 39, 41);
        
            style.borderBottomColor = Colors.FromRGBToColor(29, 29, 30);
            style.borderTopColor = Colors.FromRGBToColor(29, 29, 30);
            style.borderLeftColor = Colors.FromRGBToColor(29, 29, 30);
            style.borderRightColor = Colors.FromRGBToColor(29, 29, 30);

            style.marginLeft = Sizes.Pixels(4);
            style.marginTop = Sizes.Pixels(4);
            style.marginRight = Sizes.Pixels(4);
            style.marginBottom = Sizes.Pixels(4);

            style.maxHeight = Sizes.Pixels(25);
        }
    }
}