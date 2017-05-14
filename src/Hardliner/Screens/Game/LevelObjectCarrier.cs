using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardliner.Screens.Game
{
    internal class LevelObjectCarrier : IEnumerable<LevelObject>
    {
        private readonly List<LevelObject> _opaqueObjects, _transparentObjects;

        internal IEnumerable<LevelObject> OpaqueObjects => _opaqueObjects;
        internal IEnumerable<LevelObject> TransparentObjects => _transparentObjects;

        public LevelObjectCarrier()
        {
            _opaqueObjects = new List<LevelObject>();
            _transparentObjects = new List<LevelObject>();
        }

        internal void AddRange(params LevelObject[] objs)
        {
            foreach (var obj in objs)
                Add(obj);
        }

        internal void Add(LevelObject obj)
        {
            if (obj.IsOpaque)
                _opaqueObjects.Add(obj);
            else
                _transparentObjects.Add(obj);
        }

        internal void ForEach(Action<LevelObject> method)
        {
            _opaqueObjects.ForEach(method);
            _transparentObjects.ForEach(method);
        }

        private LevelObject GetItem(int index)
        {
            if (index < _opaqueObjects.Count)
                return _opaqueObjects[index];
            else
                return _transparentObjects[index - _opaqueObjects.Count];
        }

        private void SetItem(int index, LevelObject obj)
        {
            if (index < _opaqueObjects.Count)
                _opaqueObjects[index] = obj;
            else
                _transparentObjects[index - _opaqueObjects.Count] = obj;
        }

        internal void RemoveAt(int index)
        {
            if (index < _opaqueObjects.Count)
                _opaqueObjects.RemoveAt(index);
            else
                _transparentObjects.RemoveAt(index - _opaqueObjects.Count);
        }
        
        internal void Sort()
        {
            _transparentObjects.Sort();
        }

        public IEnumerator<LevelObject> GetEnumerator()
        {
            var index = 0;
            while (index < _opaqueObjects.Count + _transparentObjects.Count)
            {
                yield return GetItem(index);
                index++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal LevelObject this[int index]
        {
            get
            {
                return GetItem(index);
            }
            set
            {
                SetItem(index, value);
            }
        }
    }
}
