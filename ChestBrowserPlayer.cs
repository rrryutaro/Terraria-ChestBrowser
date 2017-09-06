﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace ChestBrowser
{
	public class ChestBrowserPlayer : ModPlayer
	{
        private TagCompound chestBrowserData;

        public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (ChestBrowser.instance.HotKey.JustPressed)
			{
                ChestBrowser.instance.chestBrowserTool.visible = !ChestBrowser.instance.chestBrowserTool.visible;
                if (ChestBrowser.instance.chestBrowserTool.visible)
                {
                    ChestBrowserUI.instance.updateNeeded = true;
                }
            }
        }

		public override TagCompound Save()
		{
            return new TagCompound
            {
                ["ChestBrowserUI"] = ChestBrowser.instance.chestBrowserTool.uistate.Save(),
            };
        }

		public override void Load(TagCompound tag)
		{
            if (tag.ContainsKey("ChestBrowserUI"))
            {
                if (tag.Get<object>("ChestBrowserUI").GetType().Equals(typeof(TagCompound)))
                {
                    chestBrowserData = tag.Get<TagCompound>("ChestBrowserUI");
                }
            }
        }

        public override void OnEnterWorld(Player player)
        {
            ChestBrowserUI.instance.InitializeUI();
            if (chestBrowserData != null)
            {
                ChestBrowser.instance.chestBrowserTool.uistate.Load(chestBrowserData);
            }
        }

        /// <summary>
        /// チェストブラウザーを表示中はタイルレンジを無制限にする
        /// </summary>
        /// <remarks>
        /// HEROsMod の InfiniteReach.cs の public override void ResetEffects() 部分を流用
        /// </remarks>
        public override void ResetEffects()
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (ChestBrowser.instance.chestBrowserTool.visible)
                {
                    Player.tileRangeX = Config.isInfinityRange ? ChestBrowserUtils.InfinityRange : Config.searchRangeX / 2;
                    Player.tileRangeY = Config.isInfinityRange ? ChestBrowserUtils.InfinityRange : Config.searchRangeY / 2;
                }
            }
        }

    }
}
