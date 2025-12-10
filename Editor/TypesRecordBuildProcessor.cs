using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace TypesRecord.Editor {
    public class TypesRecordBuildProcessor : IPreprocessBuildWithReport {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report) {
            Debug.Log($"[TypesRecordBuildProcessor] Starting build: {report.summary.platform} at {report.summary.totalTime}.");
            TypesRecordGenerator.Generate();
            Debug.Log($"[TypesRecordBuildProcessor] Build completed. Build result: {report.summary.result}");
        }
    }
}