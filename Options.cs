using System.Collections.Generic;
using Menu.Remix.MixedUI;
using UnityEngine;

namespace SCDSforthfix
{
    // Based on the options script from SBCameraScroll by SchuhBaum
    // https://github.com/SchuhBaum/SBCameraScroll/blob/Rain-World-v1.9/SourceCode/MainModOptions.cs
    public class Options : OptionInterface
    {
        public static Options instance = new Options();
        private const string AUTHORS_NAME = "forthbridge";

        #region Options

        public static Configurable<KeyCode> keybindEnterSim = instance.config.Bind("keybindEnterSim", KeyCode.D, new ConfigurableInfo(
            "Keybind to enter the dating sim.", null, "", "Enter Dating Sim"));

        public static Configurable<KeyCode> keybindExitSim = instance.config.Bind("keybindExitSim", KeyCode.Escape, new ConfigurableInfo(
            "Additional keybind to exit the dating sim. The pause key also works.", null, "", "Exit Dating Sim"));

        public static Configurable<KeyCode> keybindResetSim = instance.config.Bind("keybindResetSim", KeyCode.R, new ConfigurableInfo(
            "Keybind to reset the dating sim.", null, "", "Reset Dating Sim"));

        #endregion

        #region Parameters

        private readonly float spacing = 20f;
        private readonly float fontHeight = 20f;

        #endregion

        #region Variables

        private Vector2 marginX = new();
        private Vector2 pos = new();

        private readonly List<float> boxEndPositions = new();
        private readonly List<OpLabel> textLabels = new();

        #endregion

        private const int NUMBER_OF_TABS = 1;

        public override void Initialize()
        {
            base.Initialize();
            Tabs = new OpTab[NUMBER_OF_TABS];
            int tabIndex = -1;

            AddTab(ref tabIndex, "Input");

            AddNewLine(3);

            DrawKeybinders(keybindEnterSim, ref Tabs[tabIndex]);
            AddNewLine(1);
            
            DrawKeybinders(keybindExitSim, ref Tabs[tabIndex]);
            AddNewLine(1);

            DrawKeybinders(keybindResetSim, ref Tabs[tabIndex]);
            AddNewLine(1);

            AddNewLine(9);
            DrawBox(ref Tabs[tabIndex]);
        }

        private void AddTab(ref int tabIndex, string tabName)
        {
            tabIndex++;
            Tabs[tabIndex] = new OpTab(this, tabName);
            InitializeMarginAndPos();

            AddNewLine();
            AddTextLabel(Plugin.MOD_NAME, bigText: true);
            DrawTextLabels(ref Tabs[tabIndex]);

            AddNewLine(0.5f);
            AddTextLabel("Version " + Plugin.VERSION, FLabelAlignment.Left);
            AddTextLabel("by " + AUTHORS_NAME, FLabelAlignment.Right);
            DrawTextLabels(ref Tabs[tabIndex]);

            AddNewLine();
            AddBox();
        }

        private void InitializeMarginAndPos()
        {
            marginX = new Vector2(50f, 550f);
            pos = new Vector2(50f, 600f);
        }

        private void AddNewLine(float spacingModifier = 1f)
        {
            pos.x = marginX.x; // left margin
            pos.y -= spacingModifier * spacing;
        }

        private void AddBox()
        {
            marginX += new Vector2(spacing, -spacing);
            boxEndPositions.Add(pos.y); // end position > start position
            AddNewLine();
        }

        private void DrawBox(ref OpTab tab)
        {
            marginX += new Vector2(-spacing, spacing);
            AddNewLine();

            float boxWidth = marginX.y - marginX.x;
            int lastIndex = boxEndPositions.Count - 1;

            tab.AddItems(new OpRect(pos, new Vector2(boxWidth, boxEndPositions[lastIndex] - pos.y)));
            boxEndPositions.RemoveAt(lastIndex);
        }

        private void DrawKeybinders(Configurable<KeyCode> configurable, ref OpTab tab)
        {
            string name = (string)configurable.info.Tags[0];

            tab.AddItems(
                new OpLabel(new Vector2(115.0f, pos.y), new Vector2(100f, 34f), name)
                {
                    alignment = FLabelAlignment.Right,
                    verticalAlignment = OpLabel.LabelVAlignment.Center,
                    description = configurable.info?.description
                },
                new OpKeyBinder(configurable, new Vector2(235.0f, pos.y), new Vector2(146f, 30f), false)
            );

            AddNewLine(2);
        }

        private void AddTextLabel(string text, FLabelAlignment alignment = FLabelAlignment.Center, bool bigText = false)
        {
            float textHeight = (bigText ? 2f : 1f) * fontHeight;
            if (textLabels.Count == 0)
            {
                pos.y -= textHeight;
            }

            OpLabel textLabel = new(new Vector2(), new Vector2(20f, textHeight), text, alignment, bigText) // minimal size.x = 20f
            {
                autoWrap = true
            };
            textLabels.Add(textLabel);
        }

        private void DrawTextLabels(ref OpTab tab)
        {
            if (textLabels.Count == 0)
            {
                return;
            }

            float width = (marginX.y - marginX.x) / textLabels.Count;
            foreach (OpLabel textLabel in textLabels)
            {
                textLabel.pos = pos;
                textLabel.size += new Vector2(width - 20f, 0.0f);
                tab.AddItems(textLabel);
                pos.x += width;
            }

            pos.x = marginX.x;
            textLabels.Clear();
        }
    }
}