using System;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Metadata;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace RTLTMPro
{
    [Metadata(AllowedTypes = MetadataType.StringTable | MetadataType.StringTableEntry | MetadataType.Locale)] // Hint to the editor to only show this type for a Locale
    [Serializable]
    public class LanguageData : IMetadata
    {
        public TMP_FontAsset TMP_FontAsset;
    }

    [DefaultExecutionOrder(-90)]
    [ExecuteInEditMode]
    public class RTLTextMeshPro : TextMeshProUGUI
    {
        [SerializeField]
        private LocalizedString m_LocalizedStringReference = new LocalizedString();
        // ReSharper disable once InconsistentNaming
#if TMP_VERSION_2_1_0_OR_NEWER
        public override string text
#else
        public new string text
#endif
        {
            get { return base.text; }
            set
            {
                if (originalText == value)
                    return;

                originalText = value;

                UpdateText();
            }
        }

        public string OriginalText
        {
            get { return originalText; }
        }

        public bool PreserveNumbers
        {
            get { return preserveNumbers; }
            set
            {
                if (preserveNumbers == value)
                    return;

                preserveNumbers = value;
                havePropertiesChanged = true;
            }
        }

        public bool Farsi
        {
            get { return farsi; }
            set
            {
                if (farsi == value)
                    return;

                farsi = value;
                havePropertiesChanged = true;
            }
        }

        public bool FixTags
        {
            get { return fixTags; }
            set
            {
                if (fixTags == value)
                    return;

                fixTags = value;
                havePropertiesChanged = true;
            }
        }

        public bool ForceFix
        {
            get { return forceFix; }
            set
            {
                if (forceFix == value)
                    return;

                forceFix = value;
                havePropertiesChanged = true;
            }
        }

        public LocalizedString LocalizedString {
            get { return m_LocalizedStringReference; }
            set
            {
                m_LocalizedStringReference = value;
                originalText = m_LocalizedStringReference.GetLocalizedString();
                UpdateText();
            }
         }

        [SerializeField] protected bool preserveNumbers;

        [SerializeField] protected bool farsi = true;

        [SerializeField] [TextArea(3, 10)] protected string originalText;

        [SerializeField] protected bool fixTags = true;

        [SerializeField] protected bool forceFix;

        protected readonly FastStringBuilder finalText = new FastStringBuilder(RTLSupport.DefaultBufferSize);

        protected override void OnEnable()
        {
            base.OnEnable();
            m_LocalizedStringReference.StringChanged += UpdateString;
        }

        protected void Update()
        {
            if (havePropertiesChanged)
            {
                UpdateText();
            }
        }

        public void UpdateText()
        {
            if (originalText == null)
                originalText = "";

            if (ForceFix == false && TextUtils.IsRTLInput(originalText) == false)
            {
                isRightToLeftText = false;
                base.text = originalText;
            } else
            {
                isRightToLeftText = true;
                base.text = GetFixedText(originalText);
            }

            havePropertiesChanged = true;
        }

        private string GetFixedText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            finalText.Clear();
            RTLSupport.FixRTL(input, finalText, farsi, fixTags, preserveNumbers);
            finalText.Reverse();
            return finalText.ToString();
        }

        private void UpdateString(string value)
        {
            originalText = value;
            UpdateText();
        }

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        void OnValidate()
        {

#if UNITY_EDITOR
            if (LocalizedString.IsEmpty)
                return;
            string tmp = LocalizedString.GetLocalizedStringImmediateSafe();
            // This will let us make temporary changes to a serialized property.
            // When the Locale is changed back to None the changes will be reverted
            // back to the original value. This must be called before we make any changes.
            // Calling this in a player build will do nothing.
            // EditorPropertyDriver.RegisterProperty(this, originalText);
            UnityEditor.Undo.RegisterCompleteObjectUndo(this.gameObject, "Change localize Field");

            originalText = tmp;
            havePropertiesChanged = true;
            UpdateText();



#endif

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_LocalizedStringReference.StringChanged -= UpdateString;
        }
    }
}