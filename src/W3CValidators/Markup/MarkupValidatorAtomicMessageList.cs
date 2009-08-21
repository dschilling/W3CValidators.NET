// Copyright (c) 2009 Daniel A. Schilling

namespace W3CValidators.Markup
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;

    internal class MarkupValidatorAtomicMessageList : IList<MarkupValidatorAtomicMessage>
    {
        private readonly XmlHelper _helper;
        private readonly string _type;
        private readonly MarkupValidatorAtomicMessage[] _array;

        internal MarkupValidatorAtomicMessageList(XmlNode node, XmlNamespaceManager namespaceManager, string namespaceAlias, string type)
        {
            _helper = new XmlHelper(node, namespaceManager, namespaceAlias);

            _type = type;

            if (_helper.Node == null)
            {
                _array = new MarkupValidatorAtomicMessage[0];
                return;
            }

            var countStr = _helper[string.Concat(_type, "count")];
            int count;
            if (!int.TryParse(countStr, out count))
                count = 0;

            _array = new MarkupValidatorAtomicMessage[count];

            var messageNodes = _helper.Node.SelectNodes(string.Concat("child::", _helper.NamespaceAlias, ":", _type, "list/", _helper.NamespaceAlias, ":", _type), _helper.NamespaceManager);
            if (messageNodes == null)
                return;

            var i = 0;
            foreach (XmlNode messageNode in messageNodes)
            {
                this._array[i] = new MarkupValidatorAtomicMessage(messageNode, _helper.NamespaceManager, _helper.NamespaceAlias);
                i++;
            }
        }

        public IEnumerator<MarkupValidatorAtomicMessage> GetEnumerator()
        {
            return ((IEnumerable<MarkupValidatorAtomicMessage>)_array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(MarkupValidatorAtomicMessage item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(MarkupValidatorAtomicMessage item)
        {
            return this.IndexOf(item) >= 0;
        }

        public void CopyTo(MarkupValidatorAtomicMessage[] array, int arrayIndex)
        {
            _array.CopyTo(array, arrayIndex);
        }

        public bool Remove(MarkupValidatorAtomicMessage item)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return _array.Length; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public int IndexOf(MarkupValidatorAtomicMessage item)
        {
            return Array.IndexOf(_array, item);
        }

        public void Insert(int index, MarkupValidatorAtomicMessage item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public MarkupValidatorAtomicMessage this[int index]
        {
            get { return _array[index]; }
            set { throw new NotSupportedException(); }
        }
    }
}