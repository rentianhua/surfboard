using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.Utility;
using Cedar.Core.Properties;

namespace Cedar.Core.ApplicationContexts
{
    /// <summary>
    /// Define the context item's extended property collection.
    /// </summary>   
    [CollectionDataContract(Namespace = "http://www.smartac.co/", ItemName = "Property", KeyName = "Key", ValueName = "Value", IsReference = true)]
    [Serializable]
    public class ExtendedPropertyCollection : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, object> innerDictionary = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the context item.
        /// </summary>
        /// <value>The context item.</value>
        public ContextItem ContextItem
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys
        {
            get
            {
                return this.innerDictionary.Keys;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public ICollection<object> Values
        {
            get
            {
                return this.innerDictionary.Values;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Object" /> with the specified key.
        /// </summary>
        /// <value>The value.</value>
        public object this[string key]
        {
            get
            {
                return this.innerDictionary.ContainsKey(key) ? this.innerDictionary[key] : null;
            }
            set
            {
                this.EnsureCanWrite();
                this.innerDictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                return this.innerDictionary.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ExtendedPropertyCollection class.
        /// </summary>
        public ExtendedPropertyCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ExtendedPropertyCollection class.
        /// </summary>
        /// <param name="contextItem">The context item.</param>
        public ExtendedPropertyCollection(ContextItem contextItem)
        {
            Guard.ArgumentNotNull(contextItem, "contextItem");
            this.ContextItem = contextItem;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, object value)
        {
            this.EnsureCanWrite();
            this.innerDictionary[key] = value;
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return this.innerDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            this.EnsureCanWrite();
            return this.innerDictionary.Remove(key);
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object value)
        {
            return this.innerDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<string, object> item)
        {
            this.EnsureCanWrite();
            this.innerDictionary.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.EnsureCanWrite();
            this.innerDictionary.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.innerDictionary.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            KeyValuePair<string, object>[] array2 = this.innerDictionary.ToArray<KeyValuePair<string, object>>();
            for (int i = arrayIndex; i < Math.Min(this.innerDictionary.Count, array.Length); i++)
            {
                array[i] = array2[i + arrayIndex];
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            this.EnsureCanWrite();
            return this.innerDictionary.Remove(item.Key);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.innerDictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerable enumerable = this.innerDictionary;
            return enumerable.GetEnumerator();
        }

        private void EnsureCanWrite()
        {
            if (this.ContextItem != null && this.ContextItem.ReadOnly)
            {
                throw new InvalidOperationException(ResourceUtility.Format(Resources.ExceptionCannotModifyReadonlyProperties, new object[0]));
            }
        }
    }
}
