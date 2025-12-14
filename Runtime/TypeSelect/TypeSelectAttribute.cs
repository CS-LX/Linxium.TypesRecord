using System;
using UnityEngine;

namespace TypesRecord.TypeSelect {
    [AttributeUsage(AttributeTargets.Field )]
    public class TypeSelectAttribute : PropertyAttribute {
        public bool FullTypeName { get; }
        public Type BaseType { get; }

        public TypeSelectAttribute(Type baseTyp, bool fullTypeName) {
            BaseType = baseTyp;
            FullTypeName = fullTypeName;
        }
    }
}