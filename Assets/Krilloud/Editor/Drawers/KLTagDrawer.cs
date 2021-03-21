using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KrillAudio.Krilloud.Editor.Drawers
{
	[CustomPropertyDrawer(typeof(KLTagAttribute))]
	public sealed class KLTagDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.String)
			{
				EditorGUI.BeginProperty(position, label, property);

				int currentIndex = -1;
				currentIndex = KLEditorCore.AvailableTagsString.ToList().IndexOf(property.stringValue);

				// The current tag is unavailable, show a custom enum with the current value
				if (currentIndex < 0)
				{
					GUI.backgroundColor = Color.red;
					GUIContent c = new GUIContent();
					c.text = property.stringValue;

					EditorGUI.BeginChangeCheck();
					currentIndex = EditorGUI.Popup(position, label.text, 0, GetTagsAddingCustom(property.stringValue));
					if (EditorGUI.EndChangeCheck() && currentIndex > 0)
					{
						property.stringValue = KLEditorCore.AvailableTagsString[currentIndex - 1];
					}
				}
				// The current tag is correct, show the tags loaded from JSON
				else
				{
					var lastColor = GUI.backgroundColor;
					GUI.backgroundColor = KLEditorUtils.GetTagColor(KLEditorCore.AvailableTags[currentIndex], lastColor);

					EditorGUI.BeginChangeCheck();
					currentIndex = EditorGUI.Popup(position, label.text, currentIndex, KLEditorCore.AvailableTagsString);
					if (EditorGUI.EndChangeCheck())
					{
						property.stringValue = KLEditorCore.AvailableTagsString[currentIndex];
					}

					GUI.backgroundColor = lastColor;
				}

				EditorGUI.EndProperty();
			}
			else
			{
				EditorGUI.BeginProperty(position, label, property);

				GUI.backgroundColor = Color.red;
				EditorGUI.LabelField(position, label.text, "Use [KLTag] with strings.", EditorStyles.helpBox);

				EditorGUI.EndProperty();
			}
		}

		private string[] GetTagsAddingCustom(string customValue)
		{
			if (string.IsNullOrEmpty(customValue)) customValue = "- (empty) -";

			string[] values = new string[KLEditorCore.AvailableTagsString.Length + 1];
			values[0] = customValue;

			for (var i = 0; i < KLEditorCore.AvailableTagsString.Length; i++)
			{
				values[i + 1] = KLEditorCore.AvailableTagsString[i];
			}

			return values;
		}
	}
}