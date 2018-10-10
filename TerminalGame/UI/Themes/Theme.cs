using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalGame.UI.Themes
{
    class Theme
    {
        public string ThemeName { get; private set; }
        public Color StatusBarBackgroundColor { get; private set; }
        public Color ModuleBackgroundColor { get; private set; }
        public Color ModuleFontColor { get; private set; }
        public Color ModuleHeaderBackgroundColor { get; private set; }
        public Color ModuleHeaderFontColor { get; private set; }
        public Color ModuleOutlineColor { get; private set; }
        public Color NetworkMapNodeColor { get; private set; }
        public Color NetworkMapHomeSpinnerColor { get; private set; }
        public Color NetworkMapHoverSpinnerColor { get; private set; }
        public Color NetworkMapConnectedSpinnerColor { get; private set; }

        public Theme(string themeName, Color? statusBarBackgroundColor = null, Color? moduleBackgroundColor = null, 
            Color? moduleFontColor = null, Color? moduleHeaderBackgroundColor = null, Color? moduleHeaderFontColor = null, 
            Color? moduleOutlineColor = null, Color? networkMapNodeColor = null, Color? networkMapHomeSpinnerColor = null,
            Color? networkMapHoverSpinnerColor = null, Color? networkMapConnectedSpinnerColor = null)
        {
            ThemeName = themeName;
            StatusBarBackgroundColor = statusBarBackgroundColor == null ? Color.DeepPink : (Color)statusBarBackgroundColor;
            ModuleBackgroundColor = moduleBackgroundColor == null ? Color.Black * 0.75f : (Color)moduleBackgroundColor;
            ModuleFontColor = moduleFontColor == null ? Color.White : (Color)moduleFontColor;
            ModuleHeaderBackgroundColor = moduleHeaderBackgroundColor == null ? Color.HotPink : (Color)moduleHeaderBackgroundColor;
            ModuleHeaderFontColor = moduleHeaderFontColor == null ? Color.White : (Color)moduleHeaderFontColor;
            ModuleOutlineColor = moduleOutlineColor == null ? Color.LightPink : (Color)moduleOutlineColor;
            NetworkMapNodeColor = networkMapNodeColor == null ? Color.Pink : (Color)networkMapNodeColor;
            NetworkMapHomeSpinnerColor = networkMapHomeSpinnerColor == null ? Color.Red : (Color)networkMapHomeSpinnerColor;
            NetworkMapHoverSpinnerColor = networkMapHoverSpinnerColor == null ? Color.LightSeaGreen : (Color)networkMapHoverSpinnerColor;
            NetworkMapConnectedSpinnerColor = networkMapConnectedSpinnerColor == null ? Color.Khaki : (Color)networkMapConnectedSpinnerColor;
        }
    }
}
