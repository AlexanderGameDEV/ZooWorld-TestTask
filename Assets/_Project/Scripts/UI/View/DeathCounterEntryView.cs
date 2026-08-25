using System.Text;
using TMPro;
using UnityEngine;
using ZooWorld.Common;

namespace ZooWorld.UI
{
    public sealed class DeathCounterEntryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;

        private readonly StringBuilder _builder = new();
        private string _prefix;

        public void Initialize(AnimalRole role)
        {
            _prefix = role + ": ";
        }

        public void SetCount(int count)
        {
            _builder.Clear();
            _builder.Append(_prefix);
            _builder.Append(count);
            _label.SetText(_builder);
        }
    }
}
