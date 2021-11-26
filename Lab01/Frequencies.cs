using System;
using System.Collections.Generic;
using System.Text;

namespace Lab01
{
    class Frequencies
    {
        private Dictionary<string, int> _freqs;

        public Frequencies(string[] tags)
        {
            if (tags.Length > 0)
            {
                _freqs = new Dictionary<string, int>();
                foreach (var tag in tags)
                {
                    _freqs.Add(tag, 0);
                }
            }
            else
            {
                throw new Exception("Expected tags to be non-empty array");
            }
        }

        public void Increment(string tag)
        {
            if (_freqs.ContainsKey(tag))
            {
                _freqs[tag] += 1;
            }
            else
            {
                throw new Exception("Invalid tag");
            }
        }

        public Dictionary<string, int> Data
        {
            get => _freqs;
        }

        private bool Same(Frequencies frequencies)
        {
            if (_freqs.Keys.Count != frequencies.Data.Keys.Count)
            {
                return false;
            }

            foreach (var f in frequencies.Data)
            {
                if (!_freqs.ContainsKey(f.Key))
                {
                    return false;
                }
            }

            return true;
        }

        public void ReduceWith(Frequencies frequencies)
        {
            if (Same(frequencies))
            {
                foreach (var f in frequencies.Data)
                {
                    _freqs[f.Key] += f.Value;
                }
            }
            else
            {
                throw new Exception("Expected 'frequencies' to have same tags");
            }
        }
    }
}
