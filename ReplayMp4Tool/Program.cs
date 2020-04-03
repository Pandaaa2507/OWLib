﻿using System;
using DataTool;

namespace ReplayMp4Tool {
    internal class Program {
        public static void Main(string[] args) {
            if (args.Length < 2) {
                Console.Out.WriteLine("Usage: Mp4Tool {owverwatch dir} {file}");
                return;
            }
            
            string gameDir = args[0];
            string filePath = args[1];

            if (!filePath.EndsWith(".mp4")) {
                Console.Out.WriteLine("Only MP4s are supported");
                return;
            }

            const string locale = "enUS";

            DataTool.Program.Flags = new ToolFlags {
                OverwatchDirectory = gameDir,
                Language = locale,
                SpeechLanguage = locale,
                UseCache = true,
                CacheCDNData = true,
                Quiet = true
            };

            DataTool.Program.InitStorage(false);

            ReplayThing.ParseReplay(filePath);
        }
    }
}