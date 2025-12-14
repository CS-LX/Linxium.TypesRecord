# LXLib - Types Record

[中文](#简介) | [English](#english-version)

---

## 简介

本库是一个高效、安全且兼容IL2CPP模式的类型管理系统，旨在通过编辑器时提前扫描并记录类型（将扫描到的符合要求的类型序列化成JSON资源文件）来提升性能，特别是在大型Unity项目中。系统的两个主要优点如下：

- 提前扫描与配置文件读取：在编辑器时提前扫描类型并记录到配置文件中，避免了在玩家游玩时逐一扫描类型。游戏运行时直接读取已记录的类型数据，极大提高了性能。
  
- IL2CPP兼容性与稳健性：在IL2CPP模式下，反射和动态类型查找可能导致稳健性问题，使用JSON文件来读取类型信息，不仅避免了反射的性能损失，还能提高类型识别的稳健性，确保在IL2CPP下的更高稳定性。
  

---

## 安装

### ✅ 方法一：通过 `manifest.json` 引用

在你的 Unity 项目的 `Packages/manifest.json` 中添加依赖：

```json
{
  "dependencies": {
    "com.linxium.types-record": "https://github.com/CS-LX/Linxium.TypesRecord.git"
  }
}
```

---

### ✅ 方法二：通过 Unity 包管理器 UI 添加

1. 打开 Unity 编辑器
2. 菜单栏选择：**Window → Package Manager**
3. 点击左上角的 **“+” → “Add package from git URL...”**
4. 输入以下地址并确认：

```
https://github.com/CS-LX/Linxium.TypesRecord.git
```

Unity 会自动下载并导入该包。

---

## 使用方式

1. 定义自定义类型扫描器

为了扩展类型扫描逻辑，可以创建实现`ITypeScanner`接口的类。每个扫描器将扫描特定类型集合，并记录相关类型。

请记住你自定义扫描器中的ID，这是在下文获取到扫描到的类型集合的唯一凭证。

```csharp
using System;
using System.Collections.Generic;

namespace TypesRecord {
    public class MyCustomTypeScanner : ITypeScanner {
        public string ID => "MyCustomScanner";

        public Type[] ScanTypes(List<Type> allTypes) {
            // 根据需要筛选出特定的类型
            return allTypes.FindAll(t => t.Namespace == "MyNamespace").ToArray();
        }
    }
}
```

2. 读取类型记录

在游戏运行时，你可以使用`TypesRecordReader`来读取已记录的类型信息。你只需通过ID来获取已记录的类型数组。

```csharp
using System;

namespace TypesRecord {
    public class ExampleUsage : MonoBehaviour {
        void Start() {
            // 使用扫描器的ID来获取记录的类型
            Type[] types = TypesRecordReader.GetRecordedTypes("MyCustomScanner");

            // 执行与类型相关的操作
            foreach (var type in types) {
                Debug.Log($"Loaded Type: {type.FullName}");
            }
        }
    }
}
```

3. 生成类型记录 **（重要）**

在编辑器中，你可以通过菜单项自动生成类型记录文件。**每次修改或添加新的类型扫描器时，记得重新生成类型记录文件。** 否则在编辑器时运行游戏会发生类型未预加载的问题。

需要通过点击菜单栏 `Tools/Generate Types Record` 或 `Assets/Create/Generate Types Record` 生成/更新类型记录文件。

![](https://github.com/CS-LX/Linxium.LXLib-Docs/blob/main/TypesRecord1.png?raw=true)
![](https://github.com/CS-LX/Linxium.LXLib-Docs/blob/main/TypesRecord2.png?raw=true)

生成的类型记录文件会放在`Assets/Resources/Settings/`下，名称为`TypesRecord.json`

![](https://github.com/CS-LX/Linxium.LXLib-Docs/blob/main/TypesRecord3.png?raw=true)

---

## 注意事项

- **类型记录的更新**：每次修改类型扫描规则后，需要重新生成`TypesRecord.json`文件，以确保游戏运行时使用最新的类型信息。
  
  ## 贡献
  

如果你发现了问题或者有任何改进建议，欢迎提交问题或PR。

---

## 额外小工具

#### 类型选择字段

`TypeSelectAttribute` 让你无需手动输入类型名。在 Unity 编辑器中，应用该属性的字符串字段会自动在输入框后面添加一个 `[Type...]` 按钮，点击按钮后可以通过窗口选择一个类型，选定后自动将类型的全名或程序集限定名填入该字段。

该属性的构造函数接收两个参数：

- `baseType`：指定允许选择的类型的基类或接口类型，只有继承自这个类型的类可以被选择。
  
- `fullTypeName`：决定将类型的 **全名**（`false`）还是 **程序集限定名**（`true`）赋值给字段。
  

通过这种方式，你可以轻松选择类型并避免手动输入错误。

---

## 许可

MIT License. See [LICENSE](https://github.com/CS-LX/Linxium.TypesRecord/blob/main/LICENSE) for more details.

---

English Version

# LXLib - Types Record

[中文](#简介) | [English](#english-version)

---

## Introduction

This library is an efficient, safe, and IL2CPP-compatible type management system designed to improve performance by scanning and recording types during the editor phase (serializing relevant types into a JSON resource file). It is particularly useful for large Unity projects. The two main benefits of the system are as follows:

- **Pre-scan and Configuration File Reading**: Types are pre-scanned and recorded into configuration files during the editor phase, avoiding the need to scan types during runtime. The game reads the recorded types directly during gameplay, which significantly improves performance.
  
- **IL2CPP Compatibility and Robustness**: In IL2CPP mode, reflection and dynamic type lookups may cause robustness issues. Using a JSON file to read type information avoids the performance overhead of reflection and improves type resolution reliability, ensuring better stability in IL2CPP mode.
  

---

## Installation

### ✅ Method 1: Via `manifest.json`

Add the following dependency in your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.linxium.types-record": "https://github.com/CS-LX/Linxium.TypesRecord.git"
  }
}
```

---

### ✅ Method 2: Via Unity Package Manager UI

1. Open the Unity Editor.
  
2. Go to the menu bar: **Window → Package Manager**.
  
3. Click the top-left **“+” → “Add package from git URL...”**.
  
4. Enter the following URL and confirm:
  

```
https://github.com/CS-LX/Linxium.TypesRecord.git
```

Unity will automatically download and import the package.

---

## Usage

1. **Define a Custom Type Scanner**

To extend the type scanning logic, you can create a class that implements the `ITypeScanner` interface. Each scanner will scan a specific set of types and record the relevant ones.

Remember to keep the `ID` in your custom scanner, as it will be the unique reference to retrieve the scanned type collection.

```csharp
using System;
using System.Collections.Generic;

namespace TypesRecord {
    public class MyCustomTypeScanner : ITypeScanner {
        public string ID => "MyCustomScanner";

        public Type[] ScanTypes(List<Type> allTypes) {
            // Filter specific types as needed
            return allTypes.FindAll(t => t.Namespace == "MyNamespace").ToArray();
        }
    }
}
```

2. **Reading Type Records**

At runtime, you can use `TypesRecordReader` to read the recorded type information. Simply use the ID to get the recorded array of types.

```csharp
using System;

namespace TypesRecord {
    public class ExampleUsage : MonoBehaviour {
        void Start() {
            // Use the scanner's ID to get the recorded types
            Type[] types = TypesRecordReader.GetRecordedTypes("MyCustomScanner");

            // Perform operations with the types
            foreach (var type in types) {
                Debug.Log($"Loaded Type: {type.FullName}");
            }
        }
    }
}
```

3. **Generate Type Records** **(Important)**

In the editor, you can automatically generate the type record file using the menu option. **Remember to regenerate the type record file each time you modify or add a new type scanner.** Otherwise, types may not be pre-loaded when the game is run in the editor.

Click the menu option `Tools/Generate Types Record` or `Assets/Create/Generate Types Record` to generate/update the type record file.

![](https://github.com/CS-LX/Linxium.LXLib-Docs/blob/main/TypesRecord1.png?raw=true)  
![](https://github.com/CS-LX/Linxium.LXLib-Docs/blob/main/TypesRecord2.png?raw=true)

The generated type record file will be located at `Assets/Resources/Settings/` and will be named `TypesRecord.json`.

![](https://github.com/CS-LX/Linxium.LXLib-Docs/blob/main/TypesRecord3.png?raw=true)

---

## Notes

- **Updating Type Records**: Each time you modify the type scanning rules, make sure to regenerate the `TypesRecord.json` file to ensure the latest type information is used at runtime.

## Contributions

If you find any issues or have suggestions for improvements, feel free to submit issues or PRs.

---

## Additional Tools

#### Type Selection Field

`TypeSelectAttribute` allows you to avoid manually typing type names. In the Unity editor, when this attribute is applied to a string field, a `[Type...]` button will automatically appear next to the input field. Clicking the button opens a window to select a type, and the selected type’s full name or assembly-qualified name will be automatically assigned to the field.

The attribute’s constructor takes two parameters:

- `baseType`: Specifies the base type or interface type, only classes that inherit from this type can be selected.
  
- `fullTypeName`: Determines whether the **full name** (`false`) or **assembly-qualified name** (`true`) of the type will be assigned to the field.
  

This way, you can easily select types and avoid manual entry errors.

---

## License

MIT License. See [LICENSE](https://github.com/CS-LX/Linxium.TypesRecord/blob/main/LICENSE) for more details.
