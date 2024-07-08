using System.Collections;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace UniPTG.Utility
{
    internal static class EditorUtil
    {
        internal static void OverrideSerializedArray(SerializedProperty fromProperty, SerializedProperty toProperty)
        {
            fromProperty.serializedObject.Update();
            toProperty.serializedObject.Update();

            if(!(fromProperty.isArray && toProperty.isArray))
            {
                Debug.LogError("Not Array Error");
                return;
            }

            for(int i = 0; i < fromProperty.arraySize; i++)
            {
                if(toProperty.arraySize <= i)
                {
                    toProperty.InsertArrayElementAtIndex(i);
                }
                toProperty.GetArrayElementAtIndex(i).objectReferenceValue = fromProperty.GetArrayElementAtIndex(i).objectReferenceValue;
            }

            //from‚ð’´‚¦‚é•”•ª‚ðíœ
            for(int i = toProperty.arraySize - 1; i >= fromProperty.arraySize; i--)
            {
                toProperty.DeleteArrayElementAtIndex(i);
            }

            toProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif