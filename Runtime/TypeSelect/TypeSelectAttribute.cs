using System;
using UnityEngine;

namespace TypesRecord.TypeSelect {
    [AttributeUsage(AttributeTargets.Field )]
    public class TypeSelectAttribute : PropertyAttribute {
        public bool FullTypeName { get; }
        public Type BaseType { get; }

        public TypeSelectAttribute(Type baseType, bool fullTypeName) {
            BaseType = baseType;
            FullTypeName = fullTypeName;
        }
    }
}