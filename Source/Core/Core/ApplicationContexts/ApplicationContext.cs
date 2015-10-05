using System;
using System.Globalization;
using System.Threading;
using System.Web;
using Microsoft.Practices.Unity.Utility;
using Cedar.Core.ApplicationContexts.Configuration;
using Cedar.Core.IoC;
using Cedar.Core.Properties;

namespace Cedar.Core.ApplicationContexts
{
    /// <summary>
    /// supply the basic application context opertions
    /// </summary>
    public sealed class ApplicationContext
    {
        /// <summary>
        /// define the context header's namespace.
        /// </summary>
        public const string ContextHeaderNamespace = "http://www.Cedar.co/";

        /// <summary>
        /// define the context header's local name.
        /// </summary>
        public const string ContextHeaderLocalName = "Applicationcontext";

        /// <summary>
        /// The context http header name.
        /// </summary>
        public const string ContextHttpHeaderName = "Cedar.Core.Applicationcontext";
        private const string KeyofUserId = "Cedar.ApplicationContexts.UserId";
        private const string KeyofUserName = "Cedar.ApplicationContexts.UserName";
        private const string KeyofTransactionId = "Cedar.ApplicationContexts.TransactionId";
        private const string KeyofTimeZone = "Cedar.ApplicationContexts.TimeZone";
        private const string KeyofCulture = "Cedar.ApplicationContexts.Culture";
        private const string KeyofUICulture = "Cedar.ApplicationContexts.UICulture";
        private const string KeyofSessionId = "Cedar.ApplicationContexts.SessionId";
        private static ApplicationContext current;

        /// <summary>
        /// get the current application context
        /// </summary>
        public static ApplicationContext Current
        {
            get
            {
                if (ApplicationContext.current == null)
                {
                    lock (typeof(ApplicationContext))
                    {
                        if (ApplicationContext.current == null)
                        {
                            ApplicationContext.current = ApplicationContext.CreateApplicationContext();
                        }
                    }
                }
                return ApplicationContext.current;
            }
        }

        /// <summary>
        /// get or private set the context locator interface.
        /// </summary>
        public IContextLocator ContextLocator
        {
            get;
            private set;
        }

        /// <summary>
        /// get or private set the context attach behavior.
        /// </summary>
        public ContextAttachBehavior ContextAttachBehavior
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" /> with the specified key.
        /// </summary>
        /// <value>The <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" />.</value>
        public ContextItem this[string key]
        {
            get
            {
                return this.ContextLocator.GetContextItem(key);
            }
        }

        /// <summary>
        /// Get or set the Id of the current user.
        /// </summary>
        /// <value>The the Id of the current user.</value>
        public string UserId
        {
            get
            {
                var value = this.GetValue<string>(KeyofUserId);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofUserId, string.Empty);
                    return string.Empty;
                }
                return this.GetValue<string>(KeyofUserId);
            }
            set
            {
                this.SetContext(KeyofUserId, value);
            }
        }

        /// <summary>
        /// Gets the user id context item.
        /// </summary>
        /// <value>The user id context item.</value>
        public ContextItem UserIdContextItem
        {
            get
            {
                string value = this.GetValue<string>(KeyofUserId);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofUserId, string.Empty);
                }
                return this[KeyofUserId];
            }
        }

        /// <summary>
        /// Get or set the Id of the current session.
        /// </summary>
        /// <value>The the Id of the current session.</value>
        public string SessionId
        {
            get
            {
                string value = this.GetValue<string>(KeyofSessionId);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofSessionId, string.Empty);
                    return string.Empty;
                }
                return this.GetValue<string>(KeyofSessionId);
            }
            set
            {
                this.SetContext(KeyofSessionId, value);
            }
        }

        /// <summary>
        /// Gets the session id context item.
        /// </summary>
        /// <value>The session id context item.</value>
        public ContextItem SessionIdContextItem
        {
            get
            {
                string value = this.GetValue<string>(KeyofSessionId);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofSessionId, string.Empty);
                }
                return this[KeyofSessionId];
            }
        }

        /// <summary>
        /// Get or set the name of the current user.
        /// </summary>
        /// <value>The name of the current user.</value>
        public string UserName
        {
            get
            {
                string value = this.GetValue<string>(KeyofUserName);
                if (string.IsNullOrEmpty(value))
                {
                    string value2 = string.Empty;
                    if (HttpContext.Current != null && HttpContext.Current.User != null)
                    {
                        value2 = HttpContext.Current.User.Identity.Name;
                    }
                    if (string.IsNullOrEmpty(value2) && Thread.CurrentPrincipal != null)
                    {
                        value2 = Thread.CurrentPrincipal.Identity.Name;
                    }
                    this.SetContext(KeyofUserName, value2);
                }
                return this.GetValue<string>(KeyofUserName);
            }
            set
            {
                this.SetContext(KeyofUserName, value);
            }
        }

        /// <summary>
        /// Gets the user name context item.
        /// </summary>
        /// <value>The user name context item.</value>
        public ContextItem UserNameContextItem
        {
            get
            {
                string value = this.GetValue<string>(KeyofUserName);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofUserName, string.Empty);
                }
                return this[KeyofUserName];
            }
        }

        /// <summary>
        /// Gets or sets the id of the current ambient transaction.
        /// </summary>
        /// <value>The transaction id.</value>
        public string TransactionId
        {
            get
            {
                string value = this.GetValue<string>(KeyofTransactionId);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofTransactionId, string.Empty);
                    return string.Empty;
                }
                return this.GetValue<string>(KeyofTransactionId);
            }
            set
            {
                this.SetContext(KeyofTransactionId, value);
            }
        }

        /// <summary>
        /// Gets the transaction id context item.
        /// </summary>
        /// <value>The transaction id context item.</value>
        public ContextItem TransactionIdContextItem
        {
            get
            {
                string value = this.GetValue<string>(KeyofTransactionId);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofTransactionId, string.Empty);
                }
                return this[KeyofTransactionId];
            }
        }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public TimeZoneInfo TimeZone
        {
            get
            {
                string value = this.GetValue<string>(KeyofTimeZone);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofTimeZone, TimeZoneInfo.Local.ToSerializedString());
                    return TimeZoneInfo.Local;
                }
                return TimeZoneInfo.FromSerializedString(value);
            }
            set
            {
                Guard.ArgumentNotNull(value, "value");
                this.SetContext(KeyofTimeZone, value.ToSerializedString());
            }
        }

        /// <summary>
        /// Gets the time zone context item.
        /// </summary>
        /// <value>The time zone context item.</value>
        public ContextItem TimeZoneContextItem
        {
            get
            {
                this.GetValue<string>(KeyofTimeZone);
                if (string.IsNullOrEmpty(KeyofTimeZone))
                {
                    this.SetContext(KeyofTimeZone, TimeZoneInfo.Local);
                }
                return this[KeyofTimeZone];
            }
        }

        /// <summary>
        /// Gets or sets the current culture.
        /// </summary>
        /// <value>The current culture.</value>
        public CultureInfo Culture
        {
            get
            {
                string value = this.GetValue<string>(KeyofCulture);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofCulture, CultureInfo.CurrentCulture.Name);
                    return CultureInfo.CurrentCulture;
                }
                return new CultureInfo(this.GetValue<string>(KeyofCulture));
            }
            set
            {
                Guard.ArgumentNotNull(value, "value");
                this.SetContext(KeyofCulture, value.Name);
            }
        }

        /// <summary>
        /// Gets the culture context item.
        /// </summary>
        /// <value>The culture context item.</value>
        public ContextItem CultureContextItem
        {
            get
            {
                this.GetValue<string>(KeyofCulture);
                if (string.IsNullOrEmpty(KeyofCulture))
                {
                    this.SetContext(KeyofCulture, CultureInfo.CurrentCulture.Name);
                }
                return this[KeyofCulture];
            }
        }

        /// <summary>
        /// Gets or sets the current UI culture.
        /// </summary>
        /// <value>The current UI culture.</value>
        public CultureInfo UICulture
        {
            get
            {
                string value = this.GetValue<string>(KeyofUICulture);
                if (string.IsNullOrEmpty(value))
                {
                    this.SetContext(KeyofUICulture, CultureInfo.CurrentUICulture.Name);
                    return CultureInfo.CurrentUICulture;
                }
                return new CultureInfo(this.GetValue<string>(KeyofUICulture));
            }
            set
            {
                Guard.ArgumentNotNull(value, "value");
                this.SetContext(KeyofUICulture, value.Name);
            }
        }

        /// <summary>
        /// Gets the UI culture context item.
        /// </summary>
        /// <value>The UI culture context item.</value>
        public ContextItem UICultureContextItem
        {
            get
            {
                this.GetValue<string>(KeyofUICulture);
                if (string.IsNullOrEmpty(KeyofUICulture))
                {
                    this.SetContext(KeyofUICulture, CultureInfo.CurrentUICulture.Name);
                }
                return this[KeyofUICulture];
            }
        }

        private static ApplicationContext CreateApplicationContext()
        {
            ApplicationContextSettings applicationContextSettings;
            if (!ConfigManager.TryGetConfigurationSection<ApplicationContextSettings>(out applicationContextSettings))
            {
                return new ApplicationContext(new CallContextLocator(), ContextAttachBehavior.Clear);
            }
            IContextLocator service = ServiceLocatorFactory.GetServiceLocator(null).GetService<IContextLocator>(applicationContextSettings.DefaultContextLocator);
            ContextAttachBehavior contextAttachBehavior = applicationContextSettings.ContextAttachBehavior;
            return new ApplicationContext(service, contextAttachBehavior);
        }

        private ApplicationContext(IContextLocator contextLocator, ContextAttachBehavior contextAttachBehavior)
        {
            Guard.ArgumentNotNull(contextLocator, "contextLocator");
            this.ContextLocator = contextLocator;
            this.ContextAttachBehavior = contextAttachBehavior;
        }

        /// <summary>
        /// Gets the value of the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" /> with the specified key.
        /// </summary>
        /// <typeparam name="TValue">The type of the value of <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" /></typeparam>
        /// <param name="key">The key of the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" /> to get.</param>
        /// <returns>The value of the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" /> to get. </returns>
        public TValue GetValue<TValue>(string key)
        {
            Guard.ArgumentNotNull(key, "key");
            ContextItem contextItem = this.ContextLocator.GetContextItem(key);
            if (contextItem != null)
            {
                return (TValue)((object)contextItem.Value);
            }
            return default(TValue);
        }

        /// <summary>
        /// Sets the context.
        /// </summary>
        /// <param name="key">The key of the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" />.</param>
        /// <param name="value">The value of the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" />.</param>
        /// <exception cref="T:System.InvalidOperationException"></exception>
        public void SetContext(string key, object value)
        {
            Guard.ArgumentNotNull(key, "key");
            ContextItem contextItem = this.ContextLocator.GetContextItem(key);
            if (contextItem != null && contextItem.ReadOnly)
            {
                throw new InvalidOperationException(ResourceUtility.Format(Resources.ExceptionCannotModifyReadonlyValue, new object[0]));
            }
            if (contextItem != null)
            {
                contextItem.Value = value;
                return;
            }
            contextItem = new ContextItem(key, value, false);
            this.ContextLocator.SetContextItem(contextItem);
        }

        /// <summary>
        /// Sets the context.
        /// </summary>
        /// <param name="key">The key of the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" />.</param>
        /// <param name="value">The value of the <see cref="T:Cedar.Core.ApplicationContexts.ContextItem" />.</param>
        /// <param name="isLocal">if set to <c>true</c> [is local].</param>
        /// <exception cref="T:System.InvalidOperationException"></exception>
        public void SetContext(string key, object value, bool isLocal)
        {
            Guard.ArgumentNotNull(key, "key");
            ContextItem contextItem = this.ContextLocator.GetContextItem(key);
            if (contextItem != null && contextItem.ReadOnly)
            {
                throw new InvalidOperationException(ResourceUtility.Format(Resources.ExceptionCannotModifyReadonlyValue, new object[0]));
            }
            if (contextItem != null && contextItem.IsLocal == isLocal)
            {
                contextItem.Value = value;
                return;
            }
            contextItem = new ContextItem(key, value, isLocal);
            this.ContextLocator.SetContextItem(contextItem);
        }
    }
}
