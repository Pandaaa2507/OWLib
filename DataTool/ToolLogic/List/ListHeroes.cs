﻿using System;
using System.Collections.Generic;
using DataTool.DataModels;
using DataTool.Flag;
using DataTool.Helper;
using DataTool.JSON;
using Newtonsoft.Json;
using OWLib;
using TankLib.Math;
using TankLib.STU.Types;
using static DataTool.Helper.IO;
using static DataTool.Program;
using static DataTool.Helper.Logger;
using static DataTool.Helper.STUHelper;

namespace DataTool.ToolLogic.List {
    [Tool("list-heroes", Description = "List heroes", TrackTypes = new ushort[] {0x75}, CustomFlags = typeof(ListFlags))]
    public class ListHeroes : JSONTool, ITool {
        public void IntegrateView(object sender) {
            throw new NotImplementedException();
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class HeroInfo {
            public string Name;
            public string Description;
            public teColorRGBA Color;
            public List<Loadout> Loadouts;

            [JsonConverter(typeof(GUIDConverter))]
            public ulong GUID;
            
            public HeroInfo(ulong guid, string name, string description, teColorRGBA color, List<Loadout> loadouts) {
                GUID = guid;
                Name = name;
                Description = description;
                Color = color;
                Loadouts = loadouts;
            }
        }

        public void Parse(ICLIFlags toolFlags) {
            Dictionary<string, Hero> heroes = GetHeroes();

            if (toolFlags is ListFlags flags)
                if (flags.JSON) {
                    ParseJSON(heroes, flags);
                    return;
                }

            IndentHelper indentLevel = new IndentHelper();
            
            foreach (KeyValuePair<string, Hero> hero in heroes) {
                Log($"{hero.Value.Name}");
                if (hero.Value.Description != null)
                    Log($"{indentLevel + 1}Description: {hero.Value.Description}");
                
                Log($"{indentLevel + 1}Color: {hero.Value.GalleryColor}");

                if (hero.Value.Loadouts != null) {

                    Log($"{indentLevel + 1}Loadouts:");
                    foreach (Loadout loadout in hero.Value.Loadouts) {
                        Log($"{indentLevel + 2}{loadout.Name}: {loadout.Category}");
                        Log($"{indentLevel + 3}{loadout.Description}");
                    }
                }

                Log();
            }
        }

        public Dictionary<string, Hero> GetHeroes() {
            Dictionary<string, Hero> @return = new Dictionary<string, Hero>();

            foreach (ulong key in TrackedFiles[0x75]) {
                STUHero hero = GetInstanceNew<STUHero>(key);
                if (hero == null) continue;

                string name = GetString(hero.m_0EDCE350) ?? $"Unknown{GUID.Index(key):X}";

                @return[name] = new Hero(key, hero);
            }

            return @return;
        }
    }
}