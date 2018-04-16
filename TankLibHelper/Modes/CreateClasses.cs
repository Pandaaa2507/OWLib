﻿using System;
using System.Collections.Generic;
using System.IO;

namespace TankLibHelper.Modes {
    public class CreateClasses : IMode {
        public string Mode => "createclasses";
        private StructuredDataInfo _info;

        public ModeResult Run(string[] args) {
            if (args.Length < 2) {
                Console.Out.WriteLine("Missing required arg: \"output\"");
                return ModeResult.Fail;
            }
            string outDirectory = args[1];
            string dataDirectory;
            bool try14Hashes = false;

            if (args.Length >= 3) {
                dataDirectory = args[2];
            } else {
                dataDirectory = StructuredDataInfo.GetDefaultDirectory();
            }

            if (args.Length >= 4) {
                try14Hashes = args[3] == "true";
            }

            if (try14Hashes) {
                _info = new StructuredDataInfo(StructuredDataInfo.GetDefaultDirectory());
                _info.LoadExtra(dataDirectory);
            } else {
                _info = new StructuredDataInfo(dataDirectory);
            }
            BuilderConfig instanceBuilderConfig = new BuilderConfig {
                Namespace = "TankLib.STU.Types"
            };
            BuilderConfig enumBuilderConfig = new BuilderConfig {
                Namespace = "TankLib.STU.Types.Enums"
            };

            string generatedDirectory = Path.Combine(outDirectory, "Generated");
            string generatedEnumsDirectory = Path.Combine(outDirectory, "Generated", "Enums");
            Directory.CreateDirectory(generatedDirectory);
            Directory.CreateDirectory(generatedEnumsDirectory);

            Dictionary<string, STUFieldJSON> enumFields = new Dictionary<string, STUFieldJSON>();

            foreach (KeyValuePair<uint, STUInstanceJSON> instance in _info.Instances) {
                if (_info.BrokenInstances.Contains(instance.Key)) {
                    continue;
                }

                InstanceBuilder instanceBuilder = new InstanceBuilder(instanceBuilderConfig, _info, instance.Value);
                
                BuildAndWriteCSharp(instanceBuilder, generatedDirectory);

                foreach (var field in instance.Value.Fields) {
                    if (field.SerializationType != 8 && field.SerializationType != 9) continue;

                    if (!enumFields.ContainsKey(field.Type)) {
                        enumFields[field.Type] = field;
                        
                        EnumBuilder enumBuilder = new EnumBuilder(enumBuilderConfig, _info, field);
                        BuildAndWriteCSharp(enumBuilder, generatedEnumsDirectory);
                    }
                }
            }

            return ModeResult.Success;
        }

        public void BuildAndWriteCSharp(ClassBuilder builder, string directory) {
            string instanceCode = builder.BuildCSharp();

            using (StreamWriter file = new StreamWriter(Path.Combine(directory, builder.GetName()+".cs"))) {
                file.Write(instanceCode);
            }
        }
    }
}