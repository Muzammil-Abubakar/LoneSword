using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagMaskAttribute))]
public sealed class TagMaskDrawer : PropertyDrawer
{
    public override void OnGUI(
        Rect position,
        SerializedProperty property,
        GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(
                position,
                label.text,
                "TagMask requires a string[] property."
            );

            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

        if (tags == null || tags.Length == 0)
        {
            EditorGUI.LabelField(
                position,
                label.text,
                "No tags defined."
            );

            EditorGUI.EndProperty();
            return;
        }

        string[] selectedTags = ParseTags(property.stringValue);

        int mask = 0;

        for (int i = 0; i < tags.Length; i++)
        {
            if (Array.IndexOf(selectedTags, tags[i]) >= 0)
            {
                mask |= 1 << i;
            }
        }

        EditorGUI.BeginChangeCheck();

        int newMask = EditorGUI.MaskField(
            position,
            label,
            mask,
            tags
        );

        if (EditorGUI.EndChangeCheck())
        {
            property.stringValue = BuildTagString(tags, newMask);
        }

        EditorGUI.EndProperty();
    }

    private static string[] ParseTags(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Array.Empty<string>();
        }

        return value.Split('|');
    }

    private static string BuildTagString(
        string[] tags,
        int mask)
    {
        var selectedTags = new System.Collections.Generic.List<string>();

        for (int i = 0; i < tags.Length; i++)
        {
            if ((mask & (1 << i)) != 0)
            {
                selectedTags.Add(tags[i]);
            }
        }

        return string.Join("|", selectedTags);
    }
}