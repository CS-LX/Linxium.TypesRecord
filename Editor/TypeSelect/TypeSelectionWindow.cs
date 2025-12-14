using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace TypesRecord.Editor.TypeSelect {
    public class TypeSelectionWindow : EditorWindow {
        string searchText = "";
        Vector2 scroll;
        Action<Type> onSelected;

        Dictionary<string, List<Type>> namespaceGroups;
        Dictionary<string, bool> foldoutStates = new();

        Type baseType;

        public static void Open([CanBeNull] Type baseType, string currentTypeName, Action<Type> onSelected) {
            var window = CreateInstance<TypeSelectionWindow>();
            window.titleContent = new GUIContent("选择类型");
            window.baseType = baseType ?? typeof(object);
            window.onSelected = onSelected;

            window.Initialize();
            window.ShowUtility();
        }

        void Initialize() {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => {
                    try {
                        return a.GetTypes();
                    }
                    catch {
                        return Array.Empty<Type>();
                    }
                }
            )
            .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
            .OrderBy(t => t.Namespace)
            .ThenBy(t => t.Name)
            .ToList();

            namespaceGroups = allTypes.GroupBy(t => t.Namespace ?? "(Global)").ToDictionary(g => g.Key, g => g.ToList());

            // 初始化折叠状态
            foreach (var ns in namespaceGroups.Keys) {
                foldoutStates.TryAdd(ns, false);
            }
        }

        void OnGUI() {
            DrawSearchBar();

            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (var pair in namespaceGroups) {
                string ns = pair.Key;
                List<Type> types = pair.Value;

                if (!string.IsNullOrEmpty(searchText)) {
                    types = types.Where(t => t.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (t.Namespace ?? "").IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    if (types.Count == 0) continue;
                }

                foldoutStates[ns] = EditorGUILayout.Foldout(foldoutStates[ns], ns, true);

                if (foldoutStates[ns]) {
                    EditorGUI.indentLevel++;
                    foreach (var t in types) {
                        if (GUILayout.Button(t.Name, EditorStyles.label)) {
                            onSelected?.Invoke(t);
                            Close();
                        }
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(3);
            }

            EditorGUILayout.EndScrollView();
        }

        void DrawSearchBar() {
            GUILayout.BeginHorizontal("HelpBox");
            searchText = GUILayout.TextField(searchText, GUI.skin.FindStyle("SearchTextField"));
            if (GUILayout.Button("", GUI.skin.FindStyle("SearchCancelButton"))) {
                searchText = "";
                GUI.FocusControl(null);
            }
            GUILayout.EndHorizontal();
        }
    }
}