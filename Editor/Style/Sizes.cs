using UnityEngine.UIElements;

namespace Editor.Style {
    public static class Sizes {
        public static StyleLength Pixels(float value) {
            return new StyleLength(new Length(value, LengthUnit.Pixel));
        }
    }
}