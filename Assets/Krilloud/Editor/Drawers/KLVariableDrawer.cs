using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KrillAudio.Krilloud.Editor.Drawers
{
	[CustomPropertyDrawer(typeof(KLVariableAttribute))]
	public class KLVariableDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.String)
			{
				EditorGUI.BeginProperty(position, label, property);

				int currentIndex = KLEditorCore.AvailableVariablesString.ToList().IndexOf(property.stringValue);

				// The current variable is unavailable, show a custom enum with the current value
				if (currentIndex < 0)
				{
					GUI.backgroundColor = Color.red;
					GUIContent c = new GUIContent();
					c.text = property.stringValue;

					EditorGUI.BeginChangeCheck();
					currentIndex = EditorGUI.Popup(position, label.text, 0, GetVariablesAddingCustom(property.stringValue));
					if (EditorGUI.EndChangeCheck() && currentIndex > 0)
					{
						property.stringValue = KLEditorCore.AvailableVariablesString[currentIndex - 1];
					}
				}
				// The current variable is correct, show the variables loaded from JSON
				else
				{
					var lastColor = GUI.backgroundColor;
					GUI.backgroundColor = KLEditorUtils.GetVariableColor(KLEditorCore.AvailableVariables[currentIndex], lastColor);

					EditorGUI.BeginChangeCheck();
					currentIndex = EditorGUI.Popup(position, label.text, currentIndex, KLEditorCore.AvailableVariablesString);
					if (EditorGUI.EndChangeCheck())
					{
						property.stringValue = KLEditorCore.AvailableVariablesString[currentIndex];
					}

					GUI.backgroundColor = lastColor;
				}

				EditorGUI.EndProperty();
			}
			else
			{
				EditorGUI.BeginProperty(position, label, property);

				GUI.backgroundColor = Color.red;
				EditorGUI.LabelField(position, label.text, "Use [KLVariable] with strings.", EditorStyles.helpBox);

				EditorGUI.EndProperty();
			}
		}

		private string[] GetVariablesAddingCustom(string customValue)
		{
			if (string.IsNullOrEmpty(customValue)) customValue = "- (empty) -";

			string[] values = new string[KLEditorCore.AvailableVariablesString.Length + 1];
			values[0] = customValue;

			for (var i = 0; i < KLEditorCore.AvailableVariablesString.Length; i++)
			{
				values[i + 1] = KLEditorCore.AvailableVariablesString[i];
			}

			return values;
		}
	}
}