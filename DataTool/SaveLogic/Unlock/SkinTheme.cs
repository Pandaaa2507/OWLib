﻿using System.Collections.Generic;
using System.IO;
using DataTool.DataModels.Hero;
using DataTool.Flag;
using DataTool.Helper;
using TankLib;
using TankLib.STU.Types;
using static DataTool.Helper.Logger;
using static DataTool.Helper.STUHelper;

namespace DataTool.SaveLogic.Unlock {
    public static class SkinTheme {
        public static void Save(ICLIFlags flags, string directory, DataModels.Unlock unlock, STUHero hero) {
            if (!(unlock.STU is STUUnlock_SkinTheme unlockSkinTheme)) return;
            STUSkinTheme skinTheme = GetInstance<STUSkinTheme>(unlockSkinTheme.m_skinTheme);
            if (skinTheme == null) return;
            
            LoudLog($"\tExtracting skin {unlock.Name}");
            Save(flags, directory, skinTheme, hero);
        }

        public static void Save(ICLIFlags flags, string directory, STU_63172E83 skin, STUHero hero) {
            STUSkinTheme skinTheme = GetInstance<STUSkinTheme>(skin.m_5E9665E3);
            if (skinTheme == null) return;
            LoudLog($"\tExtracting skin {IO.GetFileName(skin.m_5E9665E3)}");
            Save(flags, directory, skinTheme, hero);
        }

        public static void Save(ICLIFlags flags, string directory, STUSkinBase skin, STUHero hero) {
            Dictionary<ulong, ulong> replacements = GetReplacements(skin);
            
            LoudLog("\t\tFinding");
            
            FindLogic.Combo.ComboInfo info = new FindLogic.Combo.ComboInfo();
            var saveContext = new Combo.SaveContext(info);

            FindLogic.Combo.Find(info, hero.m_gameplayEntity, replacements);
            info.SetEntityName(hero.m_gameplayEntity, "Gameplay3P");
            
            FindLogic.Combo.Find(info, hero.m_previewEmoteEntity, replacements);
            info.SetEntityName(hero.m_previewEmoteEntity, "PreviewEmote");
            
            FindLogic.Combo.Find(info, hero.m_322C521A, replacements);
            info.SetEntityName(hero.m_322C521A, "Showcase");
            
            FindLogic.Combo.Find(info, hero.m_26D71549, replacements);
            info.SetEntityName(hero.m_26D71549, "HeroGallery");
            
            FindLogic.Combo.Find(info, hero.m_8125713E, replacements);
            info.SetEntityName(hero.m_8125713E, "HighlightIntro");
            
            if (skin is STUSkinTheme skinTheme) {
                info.m_processExistingEntities = true;
                foreach (var weaponOverrideGUID in skinTheme.m_heroWeapons) {
                    STUHeroWeapon heroWeapon = GetInstance<STUHeroWeapon>(weaponOverrideGUID);
                    if (heroWeapon == null) continue;

                    Dictionary<ulong, ulong> weaponReplacements = GetReplacements(heroWeapon);

                    SetPreviewWeaponNames(info, weaponReplacements, hero.m_previewWeaponEntities);
                    SetPreviewWeaponNames(info, weaponReplacements, hero.m_C2FE396F);
                }
                info.m_processExistingEntities = false;
            }

            foreach (STU_1A496D3C tex in hero.m_8203BFE1) { // find GUI
                FindLogic.Combo.Find(info, tex.m_texture, replacements);
                info.SetTextureName(tex.m_texture, teResourceGUID.AsString(tex.m_id));
            }
            
            if (replacements != null) {
                string soundDirectory = Path.Combine(directory, "Sound");
            
                FindLogic.Combo.ComboInfo diffInfoBefore = new FindLogic.Combo.ComboInfo();
                FindLogic.Combo.ComboInfo diffInfoAfter = new FindLogic.Combo.ComboInfo();
                var diffInfoAfterContext = new Combo.SaveContext(diffInfoAfter); // todo: remove
                
                foreach (KeyValuePair<ulong,ulong> replacement in replacements) {
                    uint diffReplacementType = new teResourceGUID(replacement.Value).Type;
                    if (diffReplacementType != 0x2C && diffReplacementType != 0x3F &&
                        diffReplacementType != 0xB2) continue; // no voice sets, use extract-hero-voice
                    FindLogic.Combo.Find(diffInfoAfter, replacement.Value);
                    FindLogic.Combo.Find(diffInfoBefore, replacement.Key);
                }
                
                foreach (KeyValuePair<ulong, FindLogic.Combo.VoiceSetAsset> voiceSet in diffInfoAfter.m_voiceSets) {
                    if (diffInfoBefore.m_voiceSets.ContainsKey(voiceSet.Key)) continue;
                    Combo.SaveVoiceSet(flags, soundDirectory, diffInfoAfterContext, voiceSet.Key);
                }

                foreach (KeyValuePair<ulong,FindLogic.Combo.SoundFileAsset> soundFile in diffInfoAfter.m_soundFiles) {
                    if (diffInfoBefore.m_soundFiles.ContainsKey(soundFile.Key)) continue;
                    Combo.SaveSoundFile(flags, soundDirectory, diffInfoAfterContext, soundFile.Key, false);
                }
            
                foreach (KeyValuePair<ulong,FindLogic.Combo.SoundFileAsset> soundFile in diffInfoAfter.m_voiceSoundFiles) {
                    if (diffInfoBefore.m_voiceSoundFiles.ContainsKey(soundFile.Key)) continue;
                    Combo.SaveSoundFile(flags, soundDirectory, diffInfoAfterContext, soundFile.Key, true);
                }
                diffInfoAfterContext.Wait();
            }
            LoudLog("\t\tSaving");
            Combo.SaveLooseTextures(flags, Path.Combine(directory, "GUI"), saveContext);
            Combo.Save(flags, directory, saveContext);
            saveContext.Wait();
            LoudLog("\t\tDone");
        }

        private static void SetPreviewWeaponNames(FindLogic.Combo.ComboInfo info, Dictionary<ulong, ulong> weaponReplacements, STU_A0872511[] entities) {
            if (entities == null) return;
            foreach (STU_A0872511 weaponEntity in entities) {
                FindLogic.Combo.Find(info, weaponEntity.m_entityDefinition, weaponReplacements);

                if (weaponEntity.m_loadout == 0) continue;
                Loadout loadout = Loadout.GetLoadout(weaponEntity.m_loadout);
                if (loadout == null) continue;
                info.SetEntityName(weaponEntity.m_entityDefinition, $"{loadout.Name}-{new teResourceGUID(weaponEntity.m_entityDefinition).Index}");
            }
        }

        public static Dictionary<ulong, ulong> GetReplacements(STUSkinBase skin) {
            if (skin.m_runtimeOverrides != null) {
                Dictionary<ulong, ulong> replacements = new Dictionary<ulong, ulong>();
                foreach (KeyValuePair<ulong,STUSkinRuntimeOverride> @override in skin.m_runtimeOverrides) {
                    replacements[@override.Key] = @override.Value.m_3D884507;
                }
                return replacements;
            }
            return null;
        }
    }
}