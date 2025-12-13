using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TypesRecord {
    public static class TypesRecordReader {
        static Dictionary<string, Type[]> typesRecord = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init() => ReadTypes();

        static void ReadTypes() {
            var config = Resources.Load<TextAsset>(TypesRecordUtils.RecordPath[..^Path.GetExtension(TypesRecordUtils.RecordPath).Length]);
            if (!config) {
                Debug.LogError("TypesRecord.json not found in Resources/Settings/");
                return;
            }

            var cfg = JsonUtility.FromJson<TypesRecord>(config.text);
            foreach (TypesCollection collection in cfg.collections) {
                string id = collection.id;
                Type[] types = collection.types.Select(TypesRecordUtils.ResolveFromString).ToArray();
                typesRecord.Add(id, types);
            }
        }

        public static Type[] GetRecordedTypes(string id) => typesRecord[id];

        public static T CreateInstance<T>(string id, Type type) {
            HashSet<Type> types = typesRecord[id].ToHashSet();
            if (!types.Contains(type)) {
                throw new ArgumentException($"Type {type} not found in the type record of {id}");
            }
            return (T)Activator.CreateInstance(type);
        }

        public static T CreateInstance<T>(string id, string type) => CreateInstance<T>(id, TypesRecordUtils.ResolveFromString(type));
    }
}