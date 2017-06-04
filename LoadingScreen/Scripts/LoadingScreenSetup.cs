﻿// Project:         Loading Screen for Daggerfall Unity
// Web Site:        http://forums.dfworkshop.net/viewtopic.php?f=14&t=469
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/TheLacus/loadingscreen-du-mod
// Original Author: TheLacus (TheLacus@yandex.com)
// Contributors:    

using UnityEngine;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;

namespace LoadingScreen
{
    /// <summary>
    /// Load settings for Loading Screen mod and its optional plugins.
    /// </summary>
    public class LoadingScreenSetup
    {
        private ModSettings settings;

        const string
            LoadingLabelSection      = "LoadingLabel",
            TipsSection              = "Tips",
            experimentalSection      = "Experimental";

        #region Public Methods

        /// <summary>
        /// Constructor for LoadingScreenSetup.
        /// Use user settings for plugins to create GUI controls.
        /// </summary>
        /// <param name="settings"></param>
        public LoadingScreenSetup (ModSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Get status of plugins from user settings.
        /// </summary>
        /// <returns></returns>
        public PluginsStatus GetSettingsPluginsStatus()
        {
            return new PluginsStatus()
            {
                loadingCounter = settings.GetBool(LoadingLabelSection, "LoadingCounter"),
                tips = settings.GetBool(TipsSection, "Tips"),
                questMessages = settings.GetBool(experimentalSection, "QuestMessages"),
                levelCounter = settings.GetBool(experimentalSection, "LevelCounter")
            };
        }

        /// <summary>
        /// Get settings for Loading Counter
        /// </summary>
        /// <param name="labelText">Text shown while loading.</param>
        /// <param name="labelTextFinish">Text shown after loading.</param>
        public void InitLabel(out Rect rect, out GUIStyle style, out string labelText, out string labelTextFinish)
        {
            labelText = settings.GetString(LoadingLabelSection, "LabelText");
            labelTextFinish = settings.GetString(LoadingLabelSection, "LabelTextFinish");

            var position = settings.GetTupleFloat(LoadingLabelSection, "Position");
            rect = new Rect(Screen.width - position.First, Screen.height - position.Second, 50, 10);
            style = new GUIStyle()
            {
                alignment = TextAnchor.LowerRight,
                font = GetFont(settings.GetInt(LoadingLabelSection, "Font")),
                fontSize = settings.GetInt(LoadingLabelSection, "FontSize"),
                fontStyle = (FontStyle)settings.GetInt(LoadingLabelSection, "FontStyle", 0, 3)
            };
            style.normal.textColor = settings.GetColor(LoadingLabelSection, "FontColor");
        }

        /// <summary>
        /// Get settings for Tips
        /// </summary>
        /// <param name="language">Language of tips.</param>
        public void InitTips(out Rect rect, out GUIStyle style, out string language)
        {
            int fontSize = settings.GetInt(TipsSection, "FontSize");
            language = settings.GetString(TipsSection, "Language");

            TextAnchor alignment;
            var position = settings.GetTupleFloat(TipsSection, "Position");
            var size = settings.GetTupleFloat(TipsSection, "Size");
            rect = GetRect(position, size.First, size.Second, out alignment);

            style = new GUIStyle()
            {
                alignment = alignment,
                font = GetFont(settings.GetInt(LoadingLabelSection, "Font")), //TODO: font
                fontSize = fontSize,
                fontStyle = (FontStyle)settings.GetInt(TipsSection, "FontStyle", 0, 3),
                wordWrap = true,
            };
            style.normal.textColor = settings.GetColor(TipsSection, "FontColor");
        }

        /// <summary>
        /// Get settings for Quest Messages.
        /// </summary>
        public void InitQuestMessages(out Rect rect, out GUIStyle style)
        {
            TextAnchor alignment;
            var position = settings.GetTupleFloat(experimentalSection, "QuestPosition");
            rect = GetRect(position, 1000, 100, out alignment);

            // TODO: use own style
            Rect tipsRect;
            GUIStyle tipStyle;
            string language;
            InitTips(out tipsRect, out tipStyle, out language);

            style = tipStyle;
            style.alignment = alignment;
            style.font = GetFont(settings.GetInt(LoadingLabelSection, "Font")); //TODO: font
            style.normal.textColor = settings.GetColor(experimentalSection, "QuestColor");
        }

        /// <summary>
        /// Get settings for Level Counter.
        /// </summary>
        public void InitLevelCounter(out Rect rect, out GUIStyle style, out bool upperCase)
        {
            upperCase = settings.GetBool(experimentalSection, "LcUppercase");
            var position = settings.GetTupleFloat(experimentalSection, "LevelPosition");

            TextAnchor alignment;
            rect = GetRect(position, 50, 10, out alignment);
            style = new GUIStyle()
            {
                font = GetFont(settings.GetInt(LoadingLabelSection, "Font")), //TODO: font
                fontSize = 35,
                fontStyle = FontStyle.Bold,
                alignment = alignment
            };
            style.normal.textColor = settings.GetColor(experimentalSection, "LevelColor");
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Get Rect and text alignment.
        /// Positives values starts from the left/up side of the screen,
        /// Negatives values starts from the right/bottom side.
        /// </summary>
        private static Rect GetRect(Tuple<float, float> position, float width, float height, out TextAnchor textAlignment)
        {
            float rectX, rectY;

            if (position.First > 0)
            {
                rectX = position.First;
                if (position.Second > 0)
                {
                    rectY = position.Second;
                    textAlignment = TextAnchor.UpperLeft;
                }
                else
                {
                    rectY = Screen.height + position.Second;
                    textAlignment = TextAnchor.LowerLeft;
                }
            }
            else
            {
                rectX = Screen.width + position.First;
                if (position.Second > 0)
                {
                    rectY = position.Second;
                    textAlignment = TextAnchor.UpperRight;
                }
                else
                {
                    rectY = Screen.height + position.Second;
                    textAlignment = TextAnchor.LowerRight;
                }
            }

            return new Rect(rectX, rectY, width, height);
        }

        /// <summary>
        /// Import a font from resources.
        /// </summary>
        private static Font GetFont(int fontID)
        {
            string name = GetFontName(fontID);
            if (name != null)
                return (Font)Resources.Load(name);

            return null; // Use unity default
        }

        /// <summary>
        /// Convert font id to font path.
        /// </summary>
        private static string GetFontName(int fontID)
        {
            switch (fontID)
            {
                case 1:
                    return "Fonts/OpenSans/OpenSans-ExtraBold";
                case 2:
                    return "Fonts/OpenSans/OpenSansBold";
                case 3:
                    return "Fonts/OpenSans/OpenSansSemibold";
                case 4:
                    return "Fonts/OpenSans/OpenSansRegular";
                case 5:
                    return "Fonts/OpenSans/OpenSansLight";
                case 6:
                    return "Fonts/TESFonts/Kingthings Exeter";
                case 7:
                    return "Fonts/TESFonts/Kingthings Petrock";
                case 8:
                    return "Fonts/TESFonts/Kingthings Petrock light";
                case 9:
                    return "Fonts/TESFonts/MorrisRomanBlack";
                case 10:
                    return "Fonts/TESFonts/oblivion-font";
                case 11:
                    return "Fonts/TESFonts/Planewalker";
                default:
                    return null;
            }
        }
        
        #endregion

    }
}