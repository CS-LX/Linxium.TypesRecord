using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using TypesRecord.TypeSelect;

namespace TypesRecord.Editor.TypeSelect {
    [CustomPropertyDrawer(typeof(TypeSelectAttribute))]
    public class TypeSelectPropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // 获取 TypeSelectAttribute
            if (attribute is not TypeSelectAttribute typeSelectAttribute) {
                return;
            }

            // 确保字段类型是字符串
            if (property.propertyType != SerializedPropertyType.String) {
                EditorGUI.LabelField(position, label.text, "Only string fields can use TypeSelectAttribute.");
                return;
            }

            // 绘制标签和字符串输入框
            Rect fieldRect = new Rect(position.x, position.y, position.width - 75, position.height);
            EditorGUI.PropertyField(fieldRect, property, label);

            // 绘制按钮
            Rect buttonRect = new Rect(position.x + position.width - 70, position.y, 70, position.height);
            if (GUI.Button(buttonRect, "[Type...]")) {
                // 打开类型选择窗口
                TypeSelectionWindow.Open(typeSelectAttribute.BaseType, property.stringValue, selectedType => {
                    // 选择类型后，根据 FullTypeName 设置字段值
                    string valueToSet = typeSelectAttribute.FullTypeName ? selectedType.AssemblyQualifiedName : selectedType.FullName;
                    property.stringValue = valueToSet;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
        }
    }
}