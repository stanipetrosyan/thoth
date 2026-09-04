using UnityEngine.UIElements;

namespace Style {
    public class DSTextField : TextField {
        public DSTextField(string text, EventCallback<ChangeEvent<string>> onChange = null) {
            this.text = text;
            this.value = text;

            if (onChange != null) {
                this.RegisterValueChangedCallback(onChange);
            }

            ApplyStyle();
        }

        private void ApplyStyle() {
            style.backgroundColor = Colors.FromRGBToColor(29, 29, 30);
            style.borderTopColor = Colors.FromRGBToColor(39, 41, 44);
            style.borderBottomColor = Colors.FromRGBToColor(39, 41, 44);
            style.borderLeftColor = Colors.FromRGBToColor(39, 41, 44);
            style.borderRightColor = Colors.FromRGBToColor(39, 41, 44);
            style.borderBottomLeftRadius = Sizes.Pixels(4);
            style.borderBottomRightRadius = Sizes.Pixels(4);
            style.borderTopRightRadius = Sizes.Pixels(4);
            style.borderTopLeftRadius = Sizes.Pixels(4);
            /*style.paddingBottom = Sizes.Pixels(8);
            style.paddingTop = Sizes.Pixels(8);
            style.paddingLeft = Sizes.Pixels(8);
            style.paddingRight = Sizes.Pixels(8);*/
        }
    }
}