using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public
    class PopupCollection : IPopupCollection
    {
        Dictionary<string, IPopupObject> collection;
        IParent target;

        public PopupCollection(IParent target)
        {
            this.target = target;
            collection = new Dictionary<string, IPopupObject>(10);
        }
        public int Count => collection.Count;

        public IPopupObject this[ string id] => collection[id];

        public bool Contains( IPopupObject popupObject)
        {
            return collection.ContainsKey(popupObject.ID);
        }

        public bool Contains( string popupObjectID)
        {
            return collection.ContainsKey(popupObjectID);
        }

        public void Add( IPopupObject popupObject)
        {
            if (collection.ContainsKey(popupObject.ID))
                return;
            collection.Add(popupObject.ID, popupObject);
            popupObject.Window = target;
        }

        public void Remove( IPopupObject popupObject)
        {
            popupObject.Window = null;
            collection.Remove(popupObject.ID);
        }

        public void Clear()
        {
            collection.Clear();
        }

        public IEnumerator<IPopupObject> GetEnumerator()
        {
            return collection.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.Values.GetEnumerator();
        }
    }
}
