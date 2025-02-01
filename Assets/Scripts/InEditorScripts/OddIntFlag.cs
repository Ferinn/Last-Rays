//using UnityEditor;
//using UnityEngine;

//public class OddIntFlag : PropertyAttribute { }

//[CustomPropertyDrawer(typeof(OddIntFlag))]
//public class OddNumberDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.BeginProperty(position, label, property);

//        if (property.propertyType == SerializedPropertyType.Integer)
//        {
//            property.intValue = Mathf.Max(1, property.intValue | 1);
//        }

//        EditorGUI.PropertyField(position, property, label);

//        EditorGUI.EndProperty();
//    }
//}
