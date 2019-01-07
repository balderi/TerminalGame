﻿//From https://github.com/UnterrainerInformatik/Monogame-Textbox <3 -b

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TerminalGame.Utils.TextHandler
{
    /// <summary>
    ///     All printable characters are returned using the CharPressed event and captured using the Game.Window.TextInput
    ///     event exposed by MonoGame.
    ///     Those automatically honor keyboard-layout and repeat-frequency of the keyboard for all platforms.
    ///     The flag filterSpecialCharactersFromCharPressed you may specify when calling Initialize tells the class to filter
    ///     those characters exposed in the SpecialCharacters char[] or not.
    ///     When running on a OpenGl-system those characters are not captured by the system. So the default for filtering them
    ///     out is for the sake of compatibility.
    ///     The repetition of the special characters as well as the arrow keys, etc, is handled by the class itself using the
    ///     values you pass it when calling Initialize.
    ///     The repetition is not done by sending the characters through the CharPressed event (since they may be not even
    ///     characters and some of them are omitted by the OpenGl platform), but through the KeyPressed event and the keys are
    ///     captured by getting the KeyboardState from MonoGame in the Update-method.
    ///     So if you want to capture those, use that one.
    ///     The KeyDown and KeyUp event are standard events being getting the KeyboardState from MonoGame in the Update-method.
    /// </summary>
    public static class KeyboardInput
    {
        /// <summary>
        /// Custom EventArgs for keyboard input
        /// </summary>
        public class CharacterEventArgs : EventArgs
        {
            /// <summary>
            /// OG author did not comment anything
            /// </summary>
            public char Character { get; private set; }

            /// <summary>
            /// OG author did not comment anything
            /// </summary>
            /// <param name="character"></param>
            public CharacterEventArgs(char character)
            {
                Character = character;
            }
        }

        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        public class KeyEventArgs : EventArgs
        {
            /// <summary>
            /// OG author did not comment anything
            /// </summary>
            public Keys KeyCode { get; private set; }

            /// <summary>
            /// OG author did not comment anything
            /// </summary>
            public KeyEventArgs(Keys keyCode)
            {
                KeyCode = keyCode;
            }
        }

        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        public delegate void CharEnteredHandler(object sender, CharacterEventArgs e, KeyboardState ks);

        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        public delegate void KeyEventHandler(object sender, KeyEventArgs e, KeyboardState ks);

        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        public static readonly char[] SPECIAL_CHARACTERS = { '\a', '\b', '\n', '\r', '\f', '\t', '\v' };

        private static Game game;

        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        public static event CharEnteredHandler CharPressed;
        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        public static event KeyEventHandler KeyPressed;
        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        public static event KeyEventHandler KeyDown;
        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        public static event KeyEventHandler KeyUp;

        private static KeyboardState prevKeyState;

        private static Keys? repChar;
        private static DateTime downSince = DateTime.Now;
        private static float timeUntilRepInMillis;
        private static int repsPerSec;
        private static DateTime lastRep = DateTime.Now;
        private static bool filterSpecialCharacters;

        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        /// <param name="g"></param>
        /// <param name="timeUntilRepInMilliseconds"></param>
        /// <param name="repsPerSecond"></param>
        /// <param name="filterSpecialCharactersFromCharPressed"></param>
        public static void Initialize(Game g, float timeUntilRepInMilliseconds, int repsPerSecond,
            bool filterSpecialCharactersFromCharPressed = true)
        {
            game = g;
            timeUntilRepInMillis = timeUntilRepInMilliseconds;
            repsPerSec = repsPerSecond;
            filterSpecialCharacters = filterSpecialCharactersFromCharPressed;
            game.Window.TextInput += TextEntered;
        }

        /// <summary>
        /// Is the shift button pressed?
        /// </summary>
        public static bool ShiftDown
        {
            get
            {
                KeyboardState state = Keyboard.GetState();
                return state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            }
        }

        /// <summary>
        /// Is the control button pressed?
        /// </summary>
        public static bool CtrlDown
        {
            get
            {
                KeyboardState state = Keyboard.GetState();
                return state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl);
            }
        }

        /// <summary>
        /// Is the alt button pressed?
        /// </summary>
        public static bool AltDown
        {
            get
            {
                KeyboardState state = Keyboard.GetState();
                return state.IsKeyDown(Keys.LeftAlt) || state.IsKeyDown(Keys.RightAlt);
            }
        }
        
        private static void TextEntered(object sender, TextInputEventArgs e)
        {
            if (CharPressed != null)
            {
                if (!filterSpecialCharacters || !SPECIAL_CHARACTERS.Contains(e.Character))
                {
                    CharPressed(null, new CharacterEventArgs(e.Character), Keyboard.GetState());
                }
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        public static void Update()
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (Keys key in (Keys[])Enum.GetValues(typeof(Keys)))
            {
                if (JustPressed(keyState, key))
                {
                    KeyDown?.Invoke(null, new KeyEventArgs(key), keyState);
                    if (KeyPressed != null)
                    {
                        downSince = DateTime.Now;
                        repChar = key;
                        KeyPressed(null, new KeyEventArgs(key), keyState);
                    }
                }
                else if (JustReleased(keyState, key))
                {
                    if (KeyUp != null)
                    {
                        if (repChar == key)
                        {
                            repChar = null;
                        }
                        KeyUp(null, new KeyEventArgs(key), keyState);
                    }
                }

                if (repChar != null && repChar == key && keyState.IsKeyDown(key))
                {
                    DateTime now = DateTime.Now;
                    TimeSpan downFor = now.Subtract(downSince);
                    if (downFor.CompareTo(TimeSpan.FromMilliseconds(timeUntilRepInMillis)) > 0)
                    {
                        // Should repeat since the wait time is over now.
                        TimeSpan repeatSince = now.Subtract(lastRep);
                        if (repeatSince.CompareTo(TimeSpan.FromMilliseconds(1000f / repsPerSec)) > 0)
                        {
                            // Time for another key-stroke.
                            if (KeyPressed != null)
                            {
                                lastRep = now;
                                KeyPressed(null, new KeyEventArgs(key), keyState);
                            }
                        }
                    }
                }
            }

            prevKeyState = keyState;
        }

        private static bool JustPressed(KeyboardState keyState, Keys key)
        {
            return keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key);
        }

        private static bool JustReleased(KeyboardState keyState, Keys key)
        {
            return prevKeyState.IsKeyDown(key) && keyState.IsKeyUp(key);
        }

        /// <summary>
        /// OG author did not comment anything
        /// </summary>
        public static void Dispose()
        {
            CharPressed = null;
            KeyDown = null;
            KeyPressed = null;
            KeyUp = null;
        }
    }
}