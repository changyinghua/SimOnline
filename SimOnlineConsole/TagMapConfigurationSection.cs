using System.Configuration;

namespace com.acs.custom.config
{
    #region ConfigurationSection
    public class TagMapConfigurationSection: ConfigurationSection
    {
    	  // section 1
        [ConfigurationProperty("TagConfigurations", IsDefaultCollection = true)]
        public TagConfigurationCollection TagConfigurations
        {
            get { return (TagConfigurationCollection) base["TagConfigurations"]; }
        }
    	  // section 2
        [ConfigurationProperty("InputBlockMaps", IsDefaultCollection = true)]
        public InputBlockMapCollection InputBlockMaps
        {
            get { return (InputBlockMapCollection) base["InputBlockMaps"]; }
        }
    	  // section 3
        [ConfigurationProperty("InputStreamMaps", IsDefaultCollection = true)]
        public InputStreamMapCollection InputStreamMaps
        {
            get { return (InputStreamMapCollection)base["InputStreamMaps"]; }
        }
    	  // section 4
        [ConfigurationProperty("OutputBlockMaps", IsDefaultCollection = true)]
        public OutputBlockMapCollection OutputBlockMaps
        {
            get { return (OutputBlockMapCollection) base["OutputBlockMaps"]; }
        }
    	  // section 5
        [ConfigurationProperty("OutputStreamMaps", IsDefaultCollection = true)]
        public OutputStreamMapCollection OutputStreamMaps
        {
            get { return (OutputStreamMapCollection) base["OutputStreamMaps"]; }
        }

        // section 6
        [ConfigurationProperty("CheckAliveTags", IsDefaultCollection = true)]
        public CheckAliveTagCollection CheckAliveTags
        {
            get { return (CheckAliveTagCollection)base["CheckAliveTags"]; }
        }
    }
    #endregion
    
    #region ConfigurationElement
    // section 1
    public class TagConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("tagname", IsKey = true, IsRequired = true)]
        public string tagname
        {
            get { return (string)this["tagname"]; }
            set { this["tagname"] = value; }
        }
    }
    
    // section 2
    public class InputBlockMapElement : ConfigurationElement
    {
        [ConfigurationProperty("blockvariablename", IsKey = true, IsRequired = true)]
        public string tagname
        {
            get { return (string)this["blockvariablename"]; }
            set { this["blockvariablename"] = value; }
        }
        [ConfigurationProperty("tagname", IsKey = false, IsRequired = true)]
        public string blockvariablename
        {
            get { return (string)this["tagname"]; }
            set { this["tagname"] = value; }
        }
    }
    
    // section 3
    public class InputStreamMapElement : ConfigurationElement
    {
        [ConfigurationProperty("streamname", IsKey = true, IsRequired = true)]
        public string streamname
        {
            get { return (string)this["streamname"]; }
            set { this["streamname"] = value; }
        }
        [ConfigurationProperty("tagname1", IsKey = false, IsRequired = true)]
        public string tagname1
        {
            get { return (string)this["tagname1"]; }
            set { this["tagname1"] = value; }
        }
        [ConfigurationProperty("property1", IsKey = false, IsRequired = true)]
        public string property1
        {
            get { return (string)this["property1"]; }
            set { this["property1"] = value; }
        }
        [ConfigurationProperty("tagname2", IsKey = false, IsRequired = true)]
        public string tagname2
        {
            get { return (string)this["tagname2"]; }
            set { this["tagname2"] = value; }
        }
        [ConfigurationProperty("property2", IsKey = false, IsRequired = true)]
        public string property2
        {
            get { return (string)this["property2"]; }
            set { this["property2"] = value; }
        }
        [ConfigurationProperty("balancemethod", IsKey = false, IsRequired = true)]
        public string balancemethod
        {
            get { return (string)this["balancemethod"]; }
            set { this["balancemethod"] = value; }
        }
    }

    // section 4
    public class OutputBlockMapElement : ConfigurationElement
    {
        [ConfigurationProperty("blockvariablename", IsKey = true, IsRequired = true)]
        public string blockvariablename
        {
            get { return (string)this["blockvariablename"]; }
            set { this["blockvariablename"] = value; }
        }
        [ConfigurationProperty("tagname", IsKey = false, IsRequired = true)]
        public string tagname
        {
            get { return (string)this["tagname"]; }
            set { this["tagname"] = value; }
        }
    }
    
    // section 5
    public class OutputStreamMapElement : ConfigurationElement
    {
        [ConfigurationProperty("property", IsKey = true, IsRequired = true)]
        public string property
        {
            get { return (string)this["property"]; }
            set { this["property"] = value; }
        }
        [ConfigurationProperty("tagname", IsKey = false, IsRequired = true)]
        public string tagname
        {
            get { return (string)this["tagname"]; }
            set { this["tagname"] = value; }
        }
    }

    // section 6
    public class CheckAliveTagElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        [ConfigurationProperty("tagname", IsKey = false, IsRequired = true)]
        public string tagname
        {
            get { return (string)this["tagname"]; }
            set { this["tagname"] = value; }
        }
    }
    
    #endregion
    
    #region ConfigurationElementCollection
    // section 1
    public class TagConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TagConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TagConfigurationElement)element).tagname;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "TagConfiguration"; }
        }

        public TagConfigurationElement this[int index]
        {
            get { return (TagConfigurationElement)BaseGet(index); }
            set
            {
               if (BaseGet(index) != null)
               {
                   BaseRemoveAt(index);
               }
               BaseAdd(index, value);
            }
         }

         new public TagConfigurationElement this[string id]
         {
             get{return (TagConfigurationElement)BaseGet(id);}
         }

         public bool ContainsKey(string key)
         {
             bool result = false;
             object[] keys = BaseGetAllKeys();
             foreach (object obj in keys)
             {
                 if ((string)obj == key)
                 {
                     result = true;
                     break;
                 }
             }
             return result;
         }
    }  
    
    // section 2
    public class InputBlockMapCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()      
        {
            return new InputBlockMapElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InputBlockMapElement)element).tagname;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "InputBlockMap"; }
        }

        public InputBlockMapElement this[int index]
        {
            get { return (InputBlockMapElement)BaseGet(index); }
            set
            {
               if (BaseGet(index) != null)
               {
                   BaseRemoveAt(index);
               }
               BaseAdd(index, value);
            }
         }

         new public InputBlockMapElement this[string id]
         {
             get{return (InputBlockMapElement)BaseGet(id);}
         }

         public bool ContainsKey(string key)
         {
             bool result = false;
             object[] keys = BaseGetAllKeys();
             foreach (object obj in keys)
             {
                 if ((string)obj == key)
                 {
                     result = true;
                     break;
                 }
             }
             return result;
         }
    }   
     
    // section 3
    public class InputStreamMapCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new InputStreamMapElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InputStreamMapElement)element).streamname;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "InputStreamMap"; }
        }

        public InputStreamMapElement this[int index]
        {
            get { return (InputStreamMapElement)BaseGet(index); }
            set
            {
               if (BaseGet(index) != null)
               {
                   BaseRemoveAt(index);
               }
               BaseAdd(index, value);
            }
         }

         new public InputStreamMapElement this[string id]
         {
             get{return (InputStreamMapElement)BaseGet(id);}
         }

         public bool ContainsKey(string key)
         {
             bool result = false;
             object[] keys = BaseGetAllKeys();
             foreach (object obj in keys)
             {
                 if ((string)obj == key)
                 {
                     result = true;
                     break;
                 }
             }
             return result;
         }
    }   
     
    // section 4
    public class OutputBlockMapCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new OutputBlockMapElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((OutputBlockMapElement)element).blockvariablename;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "OutputBlockMap"; }
        }

        public OutputBlockMapElement this[int index]
        {
            get { return (OutputBlockMapElement)BaseGet(index); }
            set
            {
               if (BaseGet(index) != null)
               {
                   BaseRemoveAt(index);
               }
               BaseAdd(index, value);
            }
         }

         new public OutputBlockMapElement this[string id]
         {
             get{return (OutputBlockMapElement)BaseGet(id);}
         }

         public bool ContainsKey(string key)
         {
             bool result = false;
             object[] keys = BaseGetAllKeys();
             foreach (object obj in keys)
             {
                 if ((string)obj == key)
                 {
                     result = true;
                     break;
                 }
             }
             return result;
         }
    }   
     
    // section 5
    public class OutputStreamMapCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new OutputStreamMapElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((OutputStreamMapElement)element).property;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "OutputStreamMap"; }
        }

        public OutputStreamMapElement this[int index]
        {
            get { return (OutputStreamMapElement)BaseGet(index); }
            set
            {
               if (BaseGet(index) != null)
               {
                   BaseRemoveAt(index);
               }
               BaseAdd(index, value);
            }
         }

         new public OutputStreamMapElement this[string id]
         {
             get{return (OutputStreamMapElement)BaseGet(id);}
         }

         public bool ContainsKey(string key)
         {
             bool result = false;
             object[] keys = BaseGetAllKeys();
             foreach (object obj in keys)
             {
                 if ((string)obj == key)
                 {
                     result = true;
                     break;
                 }
             }
             return result;
         }
    }

    // section 6
    public class CheckAliveTagCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new CheckAliveTagElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CheckAliveTagElement)element).name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "AliveTag"; }
        }

        public CheckAliveTagElement this[int index]
        {
            get { return (CheckAliveTagElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public CheckAliveTagElement this[string id]
        {
            get { return (CheckAliveTagElement)BaseGet(id); }
        }

        public bool ContainsKey(string key)
        {
            bool result = false;
            object[] keys = BaseGetAllKeys();
            foreach (object obj in keys)
            {
                if ((string)obj == key)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
    #endregion
}