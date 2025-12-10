using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TypesRecord.Editor {
    public static class TypesRecordGenerator {
        public static string ConfigPath = "Assets/Resources/" + TypesRecordUtils.RecordPath;

        [MenuItem("Assets/Create/Generate Types Record"), MenuItem("Tools/Generate Types Record")]
        public static void Generate() {
            var config = new TypesRecord { collections = new List<TypesCollection>() };
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<Type> allTypes = new();
            List<ITypesScanner> allScanners = new();

            // 获取所有类型并扫描 ITypedScanner
            int totalAssemblies = assemblies.Length;
            for (int i = 0; i < totalAssemblies; i++) {
                var assembly = assemblies[i];
                EditorUtility.DisplayProgressBar("Generating Types Record", $"Scanning assembly {i + 1} of {totalAssemblies}: {assembly.GetName().Name}", (float)i / totalAssemblies);

                var types = assembly.GetTypes();
                foreach (Type type in types) {
                    allTypes.Add(type);
                    if (typeof(ITypesScanner).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract) {
                        ITypesScanner scanner = Activator.CreateInstance(type) as ITypesScanner;
                        allScanners.Add(scanner);
                    }
                }
            }

            // 处理扫描器
            int totalScanners = allScanners.Count;
            for (int i = 0; i < totalScanners; i++) {
                var scanner = allScanners[i];
                string id = scanner.ID;
                Type[] types = scanner.ScanTypes(allTypes);

                config.collections.Add(new TypesCollection {
                    id = id,
                    types = types.Select(TypesRecordUtils.ConvertToString).ToArray()
                });

                EditorUtility.DisplayProgressBar("Generating Types Record", $"Scanning scanner {i + 1} of {totalScanners}: {scanner.ID}", (float)i / totalScanners);
            }

            // 完成生成
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            File.WriteAllText(ConfigPath, JsonUtility.ToJson(config, true));
            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar(); // 清除进度条

            Debug.Log($"[TypesRecordGenerator] Generated {config.collections.Count} reader type collections.");
        }
    }
}