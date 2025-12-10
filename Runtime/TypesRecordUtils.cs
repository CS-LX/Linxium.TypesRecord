using System;

namespace TypesRecord {
    public static class TypesRecordUtils {
        public const string RecordPath = "Settings/TypesRecord.json";

        public static Type ResolveFromString(string data) {
            if (string.IsNullOrWhiteSpace(data)) return null;

            // 先尝试通过全名解析
            Type resolved = Type.GetType(data, throwOnError: false);
            if (resolved != null) return resolved;

            // 尝试只用类型名匹配当前已加载的程序集
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
                resolved = asm.GetType(data, throwOnError: false);
                if (resolved != null) return resolved;
            }
            throw new ArgumentException($"Cannot resolve Type from string: {data}");
        }

        public static string ConvertToString(Type value) {
            if (value == null) return string.Empty;
            // 输出简洁类型名（带命名空间）
            return value.FullName ?? value.Name;
        }
    }
}