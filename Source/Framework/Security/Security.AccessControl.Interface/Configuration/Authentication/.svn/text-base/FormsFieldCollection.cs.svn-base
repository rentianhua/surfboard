using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace HiiP.Framework.Security.AccessControl.Interface.Configuration
{
    /// <summary>
    /// A configuration element collection of the field element used for security retrieval.
    /// </summary>
   [ConfigurationCollectionAttribute(typeof(FormsFieldElement),AddItemName = "field")]
   public class FormsFieldCollection:ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FormsFieldElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            FormsFieldElement formsFieldElement = element as FormsFieldElement;
            return (formsFieldElement==null)?string.Empty:formsFieldElement.Name;
        }
    }
}