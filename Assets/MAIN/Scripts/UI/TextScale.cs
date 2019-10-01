using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(Text))]
public class TextScale : MonoBehaviour
{
#if UNITY_EDITOR
    
    public class TextScaleEditor : Editor
    {
        private Text txt { get { return (target as Text); } }
        public float tailleFront = 10;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            int lenghtTxt = txt.text.Length;
            txt.rectTransform.sizeDelta= new Vector2(lenghtTxt * tailleFront, txt.rectTransform.sizeDelta.y);
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(txt);
        }
    }
#endif
}
