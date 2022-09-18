using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.ReplayKit.Editor
{
	[CustomEditor(typeof(ReplayKitSettings))]
	public class ReplayKitSettingsInspector : SettingsObjectInspector
	{
        #region Fields

        private     SerializedProperty      m_isEnabledProperty;

        private     SerializedProperty      m_usesMicrophoneProperty;

        private     PropertyGroupInfo       m_iosPropertiesProperty;

        private     PropertyGroupInfo       m_androidPropertiesProperty;

        #endregion

        #region Base class methods

        protected override UnityPackageDefinition GetOwner()
        {
            return ReplayKitSettings.Package;
        }

		protected override InspectorDrawStyle GetDrawStyle()
        {
            return InspectorDrawStyle.Custom;
        }

        protected override ButtonInfo[] GetTopBarButtons()
        {
            return new ButtonInfo[]
            {
                new ButtonInfo(label: "Tutorials",      onClick: ReplayKitEditorUtility.OpenTutorialsPage),
                new ButtonInfo(label: "Discord",        onClick: ReplayKitEditorUtility.OpenSupportPage),
                new ButtonInfo(label: "Write Review",	onClick: ReplayKitEditorUtility.OpenProductPage),
                new ButtonInfo(label: "Subscribe",		onClick: ReplayKitEditorUtility.OpenSubscribePage),
            };
        }

		protected override void OnEnable()
        {
            var     iosPropertiesProperty       = serializedObject.FindProperty("m_iosProperties");
            var     androidPropertiesProperty   = serializedObject.FindProperty("m_androidProperties");

            // cache properties
            m_isEnabledProperty				    = serializedObject.FindProperty("m_isEnabled");
            m_usesMicrophoneProperty		    = serializedObject.FindProperty("m_usesMicrophone");
            m_iosPropertiesProperty             = new PropertyGroupInfo(reference: iosPropertiesProperty, displayName: iosPropertiesProperty.displayName);
            m_androidPropertiesProperty		    = new PropertyGroupInfo(reference: androidPropertiesProperty, displayName: androidPropertiesProperty.displayName);

			base.OnEnable();
		}

		protected override void DrawCustomInspector()
        {
            EditorGUILayout.PropertyField(m_isEnabledProperty);
            if (m_isEnabledProperty.boolValue)
            {
                EditorGUILayout.PropertyField(m_usesMicrophoneProperty);
                //DrawPropertyGroup(m_iosPropertiesProperty);
                DrawPropertyGroup(m_androidPropertiesProperty);
            }
        }

        protected override void DrawFooter()
        {
            base.DrawFooter();

            /*GUILayout.Space(5f);
            EditorLayoutUtility.Helpbox(
                title: "UPM Support",
                description: "You can install the package on UPM.",
                actionLabel: "Migrate To UPM",
                onClick: ReplayKitEditorUtility.MigratePackagesToUPM,
                style: CustomEditorStyles.GroupBackground);*/
        }

        #endregion
	}
}