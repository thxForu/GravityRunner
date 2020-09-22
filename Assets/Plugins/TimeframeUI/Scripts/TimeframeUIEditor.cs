#if UNITY_EDITOR
namespace Termway.TimeframeUI
{
    using Termway.Helper;

    using System;
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(TimeframeUI))]
    [CanEditMultipleObjects]
    public class TimeframeEditor : Editor
    {
        TimeframeUI timeframeUI;
        SerializedPropertyManager pm;

        SerializedProperty toggleUiKey;
        SerializedProperty defaultVisibility;
        SerializedProperty statsFrameRange;
        SerializedProperty targetFramerate;
        SerializedProperty uiAnchorPosition;
        SerializedProperty uiOffset;

        SerializedProperty gridUpdateDelaySeconds;
        SerializedProperty showLegend;
        SerializedProperty showMs;
        SerializedProperty showFps;
        SerializedProperty gridCellSize;
        SerializedProperty gridRowsData;

        SerializedProperty showGraph;
        SerializedProperty graphWidthAutoResize;
        SerializedProperty graphWidth;
        SerializedProperty graphTextureSize;
        SerializedProperty graphMsUpperLimit;
        SerializedProperty graphColorAlternateTint;

        void OnEnable()
        {
            timeframeUI = (TimeframeUI)target;

            toggleUiKey = serializedObject.FindProperty(Name.Of(() => timeframeUI.toggleUiKey));
            defaultVisibility = serializedObject.FindProperty(Name.Of(() => timeframeUI.defaultVisibility));
            statsFrameRange = serializedObject.FindProperty(Name.Of(() => timeframeUI.statsFrameRange));
            targetFramerate = serializedObject.FindProperty(Name.Of(() => timeframeUI.targetFramerate));
            uiAnchorPosition = serializedObject.FindProperty(Name.Of(() => timeframeUI.uiAnchorPosition));
            uiOffset = serializedObject.FindProperty(Name.Of(() => timeframeUI.uiOffset));

            gridUpdateDelaySeconds = serializedObject.FindProperty(Name.Of(() => timeframeUI.gridUpdateDelaySeconds));
            showLegend = serializedObject.FindProperty(Name.Of(() => timeframeUI.showLegend));
            showMs = serializedObject.FindProperty(Name.Of(() => timeframeUI.showMs));
            showFps = serializedObject.FindProperty(Name.Of(() => timeframeUI.showFps));
            gridCellSize = serializedObject.FindProperty(Name.Of(() => timeframeUI.gridCellSize));
            gridRowsData = serializedObject.FindProperty(Name.Of(() => timeframeUI.gridRowsData));

            showGraph = serializedObject.FindProperty(Name.Of(() => timeframeUI.showGraph));
            graphWidthAutoResize = serializedObject.FindProperty(Name.Of(() => timeframeUI.graphWidthAutoResize));
            graphWidth = serializedObject.FindProperty(Name.Of(() => timeframeUI.graphWidth));
            graphTextureSize = serializedObject.FindProperty(Name.Of(() => timeframeUI.graphTextureSize));
            graphMsUpperLimit = serializedObject.FindProperty(Name.Of(() => timeframeUI.graphMsUpperLimit));
            graphColorAlternateTint = serializedObject.FindProperty(Name.Of(() => timeframeUI.graphColorAlternateTint));

            Action buildUiAction = () => timeframeUI.BuildUI();

            pm = new SerializedPropertyManager(serializedObject);
            pm.Add(toggleUiKey);
            pm.Add(defaultVisibility, () => timeframeUI.SetUiState(defaultVisibility.boolValue));
            pm.Add(statsFrameRange, () => timeframeUI.InitStats());

            string tooltip = timeframeUI.GetType().GetField(Name.Of(() => timeframeUI.targetFramerate)).GetTooltip();
            pm.Add(targetFramerate,
                () => Application.targetFrameRate = (int)timeframeUI.targetFramerate,
                () => targetFramerate.intValue = EditorGUILayout.DelayedIntField(new GUIContent("Target Framerate", tooltip), targetFramerate.intValue));
            pm.Add(uiAnchorPosition, buildUiAction);
            pm.Add(uiOffset, buildUiAction);

            pm.Add(gridUpdateDelaySeconds, "Update Delay (s)", () => timeframeUI.CorrectGridUpdateDelay());
            pm.Add(showLegend, buildUiAction);
            pm.Add(showMs, buildUiAction);
            pm.Add(showFps, buildUiAction);
            pm.Add(gridCellSize, () => timeframeUI.ResizeGridUI());
            pm.Add(gridRowsData, buildUiAction);

            pm.Add(showGraph, buildUiAction);
            pm.Add(graphWidthAutoResize, buildUiAction);
            pm.Add(graphWidth, () => timeframeUI.ResizeGraphUI());
            pm.Add(graphTextureSize, () => timeframeUI.RecreateTexture());
            pm.Add(graphMsUpperLimit, () => timeframeUI.UpdateAllTextureColumns());
            pm.Add(graphColorAlternateTint, () => timeframeUI.RecreateTexture());
        }
    
        public override void OnInspectorGUI()
        {
            pm.Do();
        }  
    }
}
#endif
