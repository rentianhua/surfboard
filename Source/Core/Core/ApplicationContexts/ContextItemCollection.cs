using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cedar.Core.ApplicationContexts
{
    /// <summary>
    /// Define the context item collection which is used for store the context items.
    /// </summary>
    [CollectionDataContract(Name = "Applicationcontext", Namespace = "http://www.smartac.co/")]
    public class ContextItemCollection : List<ContextItem>
    {
    }
}
