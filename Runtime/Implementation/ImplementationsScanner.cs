using System;
using System.Collections.Generic;
using System.Linq;

namespace TypesRecord.Implementation {
    public class ImplementationsScanner<T> : ITypesScanner {
        public string ID => nameof(T);
        public Type[] ScanTypes(List<Type> allTypes) => allTypes.Where(x => typeof(T).IsAssignableFrom(x) && !x.IsAbstract).ToArray();
    }
}