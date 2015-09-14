using System.Collections.Generic;
using System.Web;
using Microsoft.Practices.Unity.Utility;
using Cedar.Core.ApplicationContexts.Configuration;
using Cedar.Core.Configuration;

namespace Cedar.Core.ApplicationContexts
{
    /// <summary>
    /// The concrete <see cref="T:Cedar.Core.ApplicationContexts.ContextLocator" /> which use the <see cref="T:System.Web.SessionState.HttpSessionState" /> as the context storage.
    /// </summary>
    [ConfigurationElement(typeof(HttpSessionStateContextLocatorData))]
    public class HttpSessionStateContextLocator : ContextLocator
    {
        private const string SessionKeyOfContextItemKeys = "Cedar.ApplicationContexts.SessionKeyOfContextItemKeys";

        private IList<string> ContextItemKeys
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeyOfContextItemKeys] == null)
                {
                    HttpContext.Current.Session[SessionKeyOfContextItemKeys] = new List<string>();
                }
                return (IList<string>)HttpContext.Current.Session[SessionKeyOfContextItemKeys];
            }
        }

        /// <summary>
        /// define the backup locator
        /// </summary>
        public CallContextLocator CallContextLocator
        {
            get;
            private set;
        }

        /// <summary>
        /// get the session state's availabe
        /// </summary>
        public bool SessionStateAvailabe
        {
            get
            {
                return HttpContext.Current != null && null != HttpContext.Current.Session;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cedar.Core.ApplicationContexts.HttpSessionStateContextLocator" /> class.
        /// </summary>
        public HttpSessionStateContextLocator()
        {
            this.CallContextLocator = new CallContextLocator();
        }

        /// <summary>
        /// Get an existing context item by given key.
        /// </summary>
        /// <param name="key">The key of the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" /> to get.</param>
        /// <returns>
        /// The <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" /> object to get.
        /// </returns>
        public override ContextItem GetContextItem(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            if (this.SessionStateAvailabe)
            {
                return HttpContext.Current.Session[key] as ContextItem;
            }
            return this.CallContextLocator.GetContextItem(key);
        }

        /// <summary>
        /// Add a new context item or use the new context item to override the exiting one.
        /// </summary>
        /// <param name="contextItem">The new <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" /> to set.</param>
        protected override void SetContextItemCore(ContextItem contextItem)
        {
            Guard.ArgumentNotNull(contextItem, "contextItem");
            if (this.SessionStateAvailabe)
            {
                HttpContext.Current.Session[contextItem.Key] = contextItem;
                if (!this.ContextItemKeys.Contains(contextItem.Key))
                {
                    this.ContextItemKeys.Add(contextItem.Key);
                    return;
                }
            }
            else
            {
                this.CallContextLocator.SetContextItem(contextItem);
            }
        }

        /// <summary>
        /// Get all current context item collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:Cedar.Core.ApplicationContexts.ContextItemCollection" /> containg all of the current context items.
        /// </returns>
        public override ContextItemCollection GetCurrentContext()
        {
            if (this.SessionStateAvailabe)
            {
                ContextItemCollection contextItemCollection = new ContextItemCollection();
                foreach (string current in this.ContextItemKeys)
                {
                    contextItemCollection.Add(this.GetContextItem(current));
                }
                return contextItemCollection;
            }
            return this.CallContextLocator.GetCurrentContext();
        }

        /// <summary>
        /// Clear the current context item collection.
        /// </summary>
        public override void Clear()
        {
            if (this.SessionStateAvailabe)
            {
                foreach (string current in this.ContextItemKeys)
                {
                    HttpContext.Current.Session.Remove(current);
                }
                this.ContextItemKeys.Clear();
            }
            this.CallContextLocator.Clear();
        }

        /// <summary>
        /// Check if the context item of the given key exists.
        /// </summary>
        /// <param name="key">The key of the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" />.</param>
        /// <returns>
        /// true if the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" /> already exists; otherwise, false.
        /// </returns>
        public override bool ContextItemExits(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            if (this.SessionStateAvailabe)
            {
                return null != HttpContext.Current.Session[key];
            }
            return this.CallContextLocator.ContextItemExits(key);
        }
    }
}
