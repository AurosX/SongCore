﻿using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BS_Utils.Utilities;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static BeatSaberMarkupLanguage.Components.CustomListTableData;

namespace SongCore.UI
{
    public class RequirementsUI : PersistentSingleton<RequirementsUI>
    {
        private StandardLevelDetailViewController standardLevel;


        internal static Config ModPrefs = new Config("SongCore/SongCore");
        internal Button infoButton;

        internal Sprite HaveReqIcon;
        internal Sprite MissingReqIcon;
        internal Sprite HaveSuggestionIcon;
        internal Sprite MissingSuggestionIcon;
        internal Sprite WarningIcon;
        internal Sprite InfoIcon;
        internal Sprite MissingCharIcon;
        internal Sprite LightshowIcon;
        internal Sprite ExtraDiffsIcon;
        internal Sprite WIPIcon;
        internal Sprite FolderIcon;

        [UIComponent("modal")]
        internal ModalView modal;

        [UIComponent("list")]
        public CustomListTableData customListTableData;

        internal void Setup()
        {
            GetIcons();
            standardLevel = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().First();
            BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "SongCore.UI.requirements.bsml"), standardLevel.gameObject, this);
        }

        internal void GetIcons()
        {
            if (!MissingReqIcon)
                MissingReqIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.RedX.png");
            if (!HaveReqIcon)
                HaveReqIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.GreenCheck.png");
            if (!HaveSuggestionIcon)
                HaveSuggestionIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.YellowCheck.png");
            if (!MissingSuggestionIcon)
                MissingSuggestionIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.YellowX.png");
            if (!WarningIcon)
                WarningIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.Warning.png");
            if (!InfoIcon)
                InfoIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.Info.png");
            if (!MissingCharIcon)
                MissingCharIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.MissingChar.png");
            if (!LightshowIcon)
                LightshowIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.Lightshow.png");
            if (!ExtraDiffsIcon)
                ExtraDiffsIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.ExtraDiffsIcon.png");
            if (!WIPIcon)
                WIPIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.squek.png");
            if (!FolderIcon)
                FolderIcon = Utilities.Utils.LoadSpriteFromResources("SongCore.Icons.FolderIcon.png");
        }

        internal void ShowRequirements(CustomPreviewBeatmapLevel level, Data.ExtraSongData songData, Data.ExtraSongData.DifficultyData diffData, bool wipFolder)
        {
            //   suggestionsList.text = "";

            customListTableData.data.Clear();
            //Requirements
            if (diffData != null)
            {
                if (diffData.additionalDifficultyData._requirements.Count() > 0)
                {
                    foreach (string req in diffData.additionalDifficultyData._requirements)
                    {
                        //    Console.WriteLine(req);
                        if (!Collections.capabilities.Contains(req))
                            customListTableData.data.Add(new CustomCellInfo("<size=75%>" + req, "Missing Requirement", MissingReqIcon.texture));
                        else
                            customListTableData.data.Add(new CustomCellInfo("<size=75%>" + req, "Requirement", HaveReqIcon.texture));
                    }
                }
            }
            //Contributors
            if (songData.contributors.Count() > 0)
            {
                foreach (Data.ExtraSongData.Contributor author in songData.contributors)
                {
                    if (author.icon == null)
                        if (!string.IsNullOrWhiteSpace(author._iconPath))
                        {
                            author.icon = Utilities.Utils.LoadSpriteFromFile(level.customLevelPath + "/" + author._iconPath);
                            customListTableData.data.Add(new CustomCellInfo(author._name, author._role, author.icon.texture));
                        }
                        else
                            customListTableData.data.Add(new CustomCellInfo(author._name, author._role, InfoIcon.texture));
                    else
                        customListTableData.data.Add(new CustomCellInfo(author._name, author._role, author.icon.texture));
                }
            }
            //WIP Check
            if (wipFolder)
                customListTableData.data.Add(new CustomCellInfo("<size=70%>" + "WIP Song. Please Play in Practice Mode", "Warning", WarningIcon.texture));
            //Additional Diff Info
            if (diffData != null)
            {
                if (diffData.additionalDifficultyData._warnings.Count() > 0)
                {
                    foreach (string req in diffData.additionalDifficultyData._warnings)
                    {

                        //    Console.WriteLine(req);

                        customListTableData.data.Add(new CustomCellInfo("<size=75%>" + req, "Warning", WarningIcon.texture));
                    }
                }
                if (diffData.additionalDifficultyData._information.Count() > 0)
                {
                    foreach (string req in diffData.additionalDifficultyData._information)
                    {

                        //    Console.WriteLine(req);

                        customListTableData.data.Add(new CustomCellInfo("<size=75%>" + req, "Info", InfoIcon.texture));
                    }
                }
                if (diffData.additionalDifficultyData._suggestions.Count() > 0)
                {
                    foreach (string req in diffData.additionalDifficultyData._suggestions)
                    {

                        //    Console.WriteLine(req);
                        if (!Collections.capabilities.Contains(req))
                            customListTableData.data.Add(new CustomCellInfo("<size=75%>" + req, "Missing Suggestion", MissingSuggestionIcon.texture));
                        else
                            customListTableData.data.Add(new CustomCellInfo("<size=75%>" + req, "Suggestion", HaveSuggestionIcon.texture));
                    }
                }
            }
            customListTableData.tableView.ReloadData();

        }
    }
}