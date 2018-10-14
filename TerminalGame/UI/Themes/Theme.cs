using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TerminalGame.UI.Themes
{
    public class Theme
    {
        private bool _isFlashing;
        private Color _oldHeaderBG, _oldStatusBG, _oldNodeColor, _oldOutlineColor;
        private float _lerpAmount;
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
        public Color WarningColor { get; private set; }

        public Theme(string themeName, Color? statusBarBackgroundColor = null, Color? moduleBackgroundColor = null, 
            Color? moduleFontColor = null, Color? moduleHeaderBackgroundColor = null, Color? moduleHeaderFontColor = null, 
            Color? moduleOutlineColor = null, Color? networkMapNodeColor = null, Color? networkMapHomeSpinnerColor = null,
            Color? networkMapHoverSpinnerColor = null, Color? networkMapConnectedSpinnerColor = null, Color? warningColor = null)
        {
            ThemeName = themeName;
            StatusBarBackgroundColor = statusBarBackgroundColor == null ? Color.DeepPink : (Color)statusBarBackgroundColor;
            ModuleBackgroundColor = moduleBackgroundColor == null ? Color.DeepPink * 0.75f : (Color)moduleBackgroundColor;
            ModuleFontColor = moduleFontColor == null ? Color.White : (Color)moduleFontColor;
            ModuleHeaderBackgroundColor = moduleHeaderBackgroundColor == null ? Color.HotPink : (Color)moduleHeaderBackgroundColor;
            ModuleHeaderFontColor = moduleHeaderFontColor == null ? Color.White : (Color)moduleHeaderFontColor;
            ModuleOutlineColor = moduleOutlineColor == null ? Color.LightPink : (Color)moduleOutlineColor;
            NetworkMapNodeColor = networkMapNodeColor == null ? Color.Pink : (Color)networkMapNodeColor;
            NetworkMapHomeSpinnerColor = networkMapHomeSpinnerColor == null ? Color.Red : (Color)networkMapHomeSpinnerColor;
            NetworkMapHoverSpinnerColor = networkMapHoverSpinnerColor == null ? Color.Firebrick : (Color)networkMapHoverSpinnerColor;
            NetworkMapConnectedSpinnerColor = networkMapConnectedSpinnerColor == null ? Color.Purple : (Color)networkMapConnectedSpinnerColor;
            WarningColor = warningColor == null ? Color.White : (Color)warningColor;
            _isFlashing = false;
            _oldHeaderBG = ModuleHeaderBackgroundColor;
            _oldStatusBG = StatusBarBackgroundColor;
            _oldNodeColor = NetworkMapNodeColor;
            _oldOutlineColor = ModuleOutlineColor;
        }

        public void Flash()
        {
            _isFlashing = true;
            _lerpAmount = 1f;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isFlashing)
            {
                return;
            }

            if (_lerpAmount > 0.0f)
            {
                //during flash
                ModuleHeaderBackgroundColor = Color.Lerp(_oldHeaderBG, WarningColor, _lerpAmount);
                StatusBarBackgroundColor = Color.Lerp(_oldStatusBG, WarningColor, _lerpAmount);
                NetworkMapNodeColor = Color.Lerp(_oldNodeColor, WarningColor, _lerpAmount);
                ModuleOutlineColor = Color.Lerp(_oldOutlineColor, WarningColor, _lerpAmount);
                _lerpAmount -= 0.025f;
                return;
            }
            //after flash
            ModuleHeaderBackgroundColor = _oldHeaderBG;
            StatusBarBackgroundColor = _oldStatusBG;
            NetworkMapNodeColor = _oldNodeColor;
            ModuleOutlineColor = _oldOutlineColor;
            _isFlashing = false;
        }
    }
}
