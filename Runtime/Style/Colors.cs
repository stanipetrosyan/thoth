using UnityEngine;

namespace Style {
    public static class Colors {
        public static Color FromRGBToColor(float red, float green, float blue, float alpha = 255) {
            return new Color(red / 255f, green / 255f, blue / 255f, alpha / 255f);
        }
    }
}