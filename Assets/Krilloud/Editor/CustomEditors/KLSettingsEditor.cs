using KrillAudio.Krilloud.Definitions;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace KrillAudio.Krilloud.Editor
{
	using Editor = UnityEditor.Editor;

	[CustomEditor(typeof(KLSettings))]
	public sealed class KLSettingsEditor : Editor
	{
		private KLSettings m_target;
		private bool m_lastAutogencode;

		private List<SerialNumber> _serials = KrilloudBuildProcessor.serials;

		private void OnEnable()
		{
			m_target = target as KLSettings;

			KrilloudBuildProcessor.UpdateSerialsList();
		}

		public override void OnInspectorGUI()
		{
			KLEditorUtils.DrawKrillHeader();

			m_lastAutogencode = m_target.autogenerateCode;

			EditorGUI.BeginChangeCheck();

			base.OnInspectorGUI();

			if (EditorGUI.EndChangeCheck())
			{
				// Detech changes to autogenerate code!
				if (m_lastAutogencode != m_target.autogenerateCode)
				{
					RefreshCore();
				}

				// m_isDirty = m_isDirty || true;
			}

			EditorGUILayout.Space();

			DrawButtons();

			EditorGUILayout.Space();

			DrawSerialList();
		}

		private void DrawButtons()
		{
			EditorGUILayout.BeginHorizontal();

			//GUI.enabled = m_isDirty;
			//if (GUILayout.Button("Refresh"))
			//{
			//	RefreshCore();
			//}
			//GUI.enabled = true;

			GUI.enabled = CacheHasSomething();
			if (GUILayout.Button("Clear cache"))
			{
				ClearCache();
			}
			GUI.enabled = true;

			EditorGUILayout.EndHorizontal();
		}

		#region Serials

		private void DrawSerialList()
		{
			List<SerialNumber> serialList = ReadSerialsFromSource();

			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Serials", EditorStyles.boldLabel);

			EditorGUI.BeginChangeCheck();
			for (var i = 0; i < serialList.Count; i++)
			{
				var serial = serialList[i];
				if (DrawSerialField(serial))
				{
					EditorPrefs.DeleteKey(KrilloudBuildProcessor.prefs_key_name + serial.platform);
					serialList.RemoveAt(i);
					i--;
				}
			}

			if (GUILayout.Button("Add new serial"))
			{
				var menu = new GenericMenu();
				var enum_names = Enum.GetNames(typeof(SerialNumber.KrilloudSerialTargets));
				var enum_count = enum_names.Length;
				for (int x = 0; x < enum_count; x++)
				{
					menu.AddItem(new GUIContent(enum_names[x]), false, OnAddSerial, x);
				}
				menu.ShowAsContext();
			}

			if (EditorGUI.EndChangeCheck())
			{
				OnSerialsChanged(serialList);
			}

			EditorGUILayout.EndVertical();
		}

		private void OnAddSerial(object target)
		{
			int value = (int)target;
			SerialNumber sn = new SerialNumber();
			sn.platform = (SerialNumber.KrilloudSerialTargets)value;
			_serials.Add(sn);
		}

		private bool DrawSerialField(SerialNumber serialNumber)
		{
			bool shouldBeRemoved = false;

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			EditorGUILayout.LabelField("Platform " + serialNumber.platform);

			EditorGUILayout.BeginHorizontal();
			serialNumber.serial = EditorGUILayout.TextArea(serialNumber.serial);

			var prevColor = GUI.backgroundColor;
			GUI.backgroundColor = new Color(1f, 0.45f, 0.44f);

			if (GUILayout.Button("Remove", GUILayout.Width(60)))
			{
				if (EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to remove serial for " + serialNumber.platform,
					"Yes", "Nope!"))
				{
					shouldBeRemoved = true;
				}
			}

			GUI.backgroundColor = prevColor;

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();

			return shouldBeRemoved;
		}

		private List<SerialNumber> ReadSerialsFromSource()
		{
			return _serials;
		}

		private void OnSerialsChanged(List<SerialNumber> serialList)
		{
			for (int x = 0; x < serialList.Count; x++)
			{
				EditorPrefs.SetString(KrilloudBuildProcessor.prefs_key_name + serialList[x].platform, serialList[x].serial);
			}
		}

		#endregion

		#region Helpers

		private void RefreshCore()
		{
			KLEditorCore.RefreshCore();
			m_target.RemoveDuplicates();
		}

		#endregion Helpers

		#region Public Static API

		public static bool CacheHasSomething()
		{
			return
				KLSettings.Instance.cacheContract.tags.Count != KLEditorCore.MainContract.tags.Count ||
				KLSettings.Instance.cacheContract.variables.Count != KLEditorCore.MainContract.variables.Count;
		}

		public static void ClearCache()
		{
			KLSettings.Instance.cacheContract = new KLContractDefinition();
			KLEditorCore.RefreshCore();
		}

		#endregion Public Static API
	}
}