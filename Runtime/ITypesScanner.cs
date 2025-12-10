using System;
using System.Collections.Generic;

namespace TypesRecord {
    public interface ITypesScanner {
        string ID { get; }
        Type[] ScanTypes(List<Type> allTypes);
    }
}