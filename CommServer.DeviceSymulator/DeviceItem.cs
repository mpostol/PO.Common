//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using Opc;
using Opc.Da;
using System;
using System.Collections;
using UAOOI.ProcessObserver.RealTime;

namespace CAS.Lib.DeviceSimulator
{
  /// <summary>
  /// A class representing a single data point in an underlying process replica. 
  /// </summary>
  [Serializable]
  public abstract class DeviceItem : I4UAServer
  {

    #region constructors
    /// <summary>
    /// Initializes the object with the specified item id.
    /// </summary>
    /// <param name="itemID">Item identifier</param>
    /// <param name="UTCTime">if set to <c>true</c> the time-stamp is expressed as the Coordinated Universal Time (UTC).</param>
    public DeviceItem(string itemID, bool UTCTime = true)
    {
      ItemID = itemID;
      if (UTCTime)
        m_getCurrentTime = () => DateTime.UtcNow;
      else
        m_getCurrentTime = () => DateTime.Now;
    }
    /// <summary>
    /// Initializes the object from a data value.
    /// </summary>
    /// <param name="itemID">Item identifier</param>
    /// <param name="value">Current value</param>
    /// <param name="UTCTime">if set to <c>true</c> the time-stamp is expressed as the Coordinated Universal Time (UTC).</param>
    public DeviceItem(string itemID, object value, bool UTCTime = true) : this(itemID, UTCTime)
    {
      if (value != null)
      {
        m_datatype = value.GetType();
        m_value = value;
        m_timestamp = m_getCurrentTime();
      }
    }
    /// <summary>
    /// Initializes the object from a data value.
    /// </summary>
    /// <param name="itemID">Item identifier</param>
    /// <param name="value">Current value</param>
    /// <param name="InitialQuality">Initial quality</param>
    /// <param name="AccessRights">Initial access rights</param>
    /// <param name="tagCanonicalType">Tag CAnonical Type</param>
    /// <param name="UTCTime">if set to <c>true</c> the time-stamp is expressed as the Coordinated Universal Time (UTC).</param>
    public DeviceItem(string itemID, object value, qualityBits InitialQuality, ItemAccessRights AccessRights, System.Type tagCanonicalType, bool UTCTime = true) : this(itemID, value, UTCTime)
    {
      if (value != null)
        m_quality = new Quality(InitialQuality);
      else
      {
        m_quality = new Quality(qualityBits.badWaitingForInitialData);
        m_datatype = tagCanonicalType;
      }
      switch (AccessRights)
      {
        case ItemAccessRights.ReadOnly:
          m_accessRights = accessRights.readable;
          break;
        case ItemAccessRights.ReadWrite:
          m_accessRights = accessRights.readWritable;
          break;
        case ItemAccessRights.WriteOnly:
          m_accessRights = accessRights.writable;
          break;
      }
    }
    #endregion

    #region Public Members
    /// <summary>
    /// The unique item identifier.
    /// </summary>
    internal string ItemID { get; } = null;
    /// <summary>
    /// Function used for initialization the properties of the item
    /// </summary>
    /// <param name="PropertiesCollection">properties to add or write</param>
    protected void AddProperties(Opc.Da.ItemPropertyCollection PropertiesCollection)
    {
      //      if(PropertiesCollection.Count==0)
      //      {
      //#if DEBUG
      //        //tutaj chyba wyswietla wlasnie te co dodal
      //        System.Console.WriteLine(
      //          "|property for: "
      //          +this.ToString()+":" 
      //          +"  not defined");
      //#endif
      //      }
      foreach (ItemProperty itemProperty in PropertiesCollection)
      {
        ItemValue itemval = new ItemValue(ItemID)
        {
          Value = itemProperty.Value
        };
        if (!Write(itemProperty.ID, itemval, false).ResultID.Succeeded())
          m_properties.Add(itemProperty.ID, itemProperty.Value);
      }
    }
    /// <summary>
    /// Returns all available properties for the specified item.
    /// </summary>
    internal Opc.Da.ItemPropertyCollection GetAvailableProperties(bool returnValues)
    {
      ArrayList ids = new ArrayList
      {
        // add standard properties.
        Property.DATATYPE,
        Property.VALUE,
        Property.QUALITY,
        Property.TIMESTAMP,
        Property.ACCESSRIGHTS,
        Property.SCANRATE,
        Property.EUTYPE,
        Property.EUINFO
      };
      // add engineering limits for analog items.
      if (m_euType == euType.analog)
      {
        ids.Add(Property.HIGHEU);
        ids.Add(Property.LOWEU);
      }
      // add limits for date time and decimal items.
      if (m_datatype == typeof(DateTime) || m_datatype == typeof(decimal))
      {
        ids.Add(Property.MINIMUM_VALUE);
        ids.Add(Property.MAXIMUM_VALUE);
        ids.Add(Property.VALUE_PRECISION);
      }
      // add any additional properties.
      foreach (PropertyID id in m_properties.Keys)
        ids.Add(id);
      // fill in the property item ids and values.
      return GetAvailableProperties((PropertyID[])ids.ToArray(typeof(PropertyID)), returnValues);
    }
    /// <summary>
    /// Returns the specified properties for the specified item.
    /// </summary>
    internal Opc.Da.ItemPropertyCollection GetAvailableProperties(PropertyID[] propertyIDs, bool returnValues)
    {
      // initialize property collection.
      ItemPropertyCollection properties = new ItemPropertyCollection
      {
        ItemName = ItemID,
        ItemPath = null,
        ResultID = ResultID.S_OK,
        DiagnosticInfo = null
      };
      // fetch information for each requested property.
      foreach (PropertyID propertyID in propertyIDs)
      {
        ItemProperty property = new ItemProperty
        {
          ID = propertyID
        };
        // read the property value.
        if (returnValues)
        {
          ItemValueResult result = Read(propertyID);
          if (result.ResultID.Succeeded())
            property.Value = result.Value;
          property.ResultID = result.ResultID;
          property.DiagnosticInfo = result.DiagnosticInfo;
        }
        // just validate the property id.
        else
          property.ResultID = ValidatePropertyID(propertyID, accessRights.readWritable);
        // set status if one or more errors occur.
        if (property.ResultID.Failed())
          properties.ResultID = ResultID.S_FALSE;
        else
        {
          // set property description.
          PropertyDescription description = PropertyDescription.Find(propertyID);
          if (description != null)
          {
            property.Description = description.Name;
            property.DataType = description.Type;
          }
          // set property item id.
          if (propertyID.Code >= ENGINEERINGUINTS && propertyID.Code <= TIMEZONE)
          {
            property.ItemName = ItemID + ":" + propertyID.Code.ToString();
            property.ItemPath = null;
          }
        }
        // add to collection.
        properties.Add(property);
      }
      // return collection.
      return properties;
    }
    /// <summary>
    /// Reads the value of the specified item property.
    /// </summary>
    /// <remarks>It reads only from cache, it should be solved.</remarks>
    internal ItemValueResult Read(PropertyID propertyID)
    {
      //MPTD zaimplementowa� bezpo�rednie czytanie z prawdziwego device 
      // initialize value and validate property.
      ItemValueResult value = new ItemValueResult
      {
        ItemName = ItemID,
        ItemPath = null,
        ResultID = ValidatePropertyID(propertyID, accessRights.readable),
        DiagnosticInfo = null,
        Value = null,
        Quality = Quality.Bad,
        QualitySpecified = false,
        Timestamp = DateTime.MinValue,
        TimestampSpecified = false
      };
      if (value.ResultID.Failed())
        return value;
      // set default quality and time-stamp (overridden when returning the item value).
      value.Quality = Quality.Good;
      value.QualitySpecified = true;
      value.Timestamp = m_getCurrentTime();
      value.TimestampSpecified = true;
      // read the property value.
      switch (propertyID.Code)
      {
        case VALUE:
          {
            value.Value = Opc.Convert.Clone(m_value);
            value.Quality = m_quality;
            value.Timestamp = m_timestamp;
            break;
          }
        // standard properties.
        case DATATYPE: { value.Value = m_datatype; break; }
        case QUALITY: { value.Value = m_quality; break; }
        case TIMESTAMP: { value.Value = m_timestamp; break; }
        case ACCESSRIGHTS: { value.Value = m_accessRights; break; }
        case SCANRATE: { value.Value = m_scanRate; break; }
        case EUTYPE: { value.Value = m_euType; break; }
        case EUINFO: { value.Value = m_euInfo; break; }
        case HIGHEU: { value.Value = m_maxValue; break; }
        case LOWEU: { value.Value = m_minValue; break; }
        case MINIMUM_VALUE:
          {
            if (m_datatype == typeof(DateTime))
            {
              value.Value = DateTime.MinValue;
              break;
            }
            if (m_datatype == typeof(decimal))
            {
              value.Value = decimal.MinValue;
              break;
            }
            value.Value = null;
            break;
          }
        case MAXIMUM_VALUE:
          {
            if (m_datatype == typeof(DateTime))
            {
              value.Value = DateTime.MaxValue;
              break;
            }
            if (m_datatype == typeof(decimal))
            {
              value.Value = decimal.MaxValue;
              break;
            }
            value.Value = null;
            break;
          }
        case VALUE_PRECISION:
          {
            if (m_datatype == typeof(DateTime))
            {
              value.Value = 1 / (double)TimeSpan.TicksPerMillisecond;
              break;
            }
            if (m_datatype == typeof(decimal))
            {
              value.Value = 28;
              break;
            }
            value.Value = null;
            break;
          }
        // other defined properties.
        default:
          {
            if (!m_properties.Contains(propertyID))
            {
              value.ResultID = ResultID.Da.E_INVALID_PID;
              break;
            }
            value.Value = m_properties[propertyID];
            break;
          }
      }
      // read completed successfully.
      return value;
    }
    internal IdentifiedResult Write(PropertyID propertyID, Opc.Da.ItemValue value)
    {
      return Write(propertyID, value, true);
    }
    /// <summary>
    /// Writes the value of the specified item property.
    /// </summary>
    protected IdentifiedResult Write(PropertyID propertyID, ItemValue value, bool validate)
    {
      // initialize result and validate property.
      IdentifiedResult result = new IdentifiedResult
      {
        ItemName = ItemID,
        ItemPath = null
      };
      if (validate)
        result.ResultID = ValidatePropertyID(propertyID, accessRights.writable);
      else
        result.ResultID = ResultID.S_OK;
      result.DiagnosticInfo = null;
      //MP return if modification is not allowed
      //MPNI zg�osi� Rendy'iemu
      if (result.ResultID.Failed())
      {
        //nieudalo dodac sie property:
#if DEBUG
        //tutaj chyba wyswietla wlasnie te co dodal
        System.Console.WriteLine(
          "|problem with the property (Write fails) : "
          + ToString() + ":"
          + propertyID.ToString().Replace(Opc.Namespace.OPC_DATA_ACCESS, "")
          + "  ");
#endif
        return result;
      }
      // handle value writes.
      if (propertyID == Property.VALUE)
      {
        if (!UpdateRemote(value.Value))//MZTD: sprawdzic czy symulator bedzie dzialac (bylo przekazywane cale Opc.Da.ItemValue a teraz idzie tylko value z Opc.Da.ItemValue value)
        {
          result.ResultID = ResultID.E_TIMEDOUT;
          result.DiagnosticInfo = "communication error while writing to the device";
          return result;
        }
        lock (this)
        {
          m_value = value.Value;
          //MPTD sprawdzi� po co on to kopiuje
          // copy value.
          //m_value = Opc.Convert.Clone(value.Value);
          // update quality if specified.
          if (value.QualitySpecified)
            m_quality = value.Quality;
          // update time-stamp if specified.
          if (value.TimestampSpecified)
            m_timestamp = value.Timestamp;
        }
        // return results.
        return result;
      }
      // lookup property description.
      PropertyDescription property = PropertyDescription.Find(propertyID);
      if (property == null)
      {
        result.ResultID = ResultID.Da.E_INVALID_PID;
        return result;
      }
      if (!validate)
      {
        //mamy doczynienia z dodawaniem nie validowanych properties i trzeba przekonwertowac je do odpowiedniego typu
        switch (propertyID.Code)
        {
          // standard properties.
          case DATATYPE:
            value.Value = System.Type.GetType(System.Convert.ToString(value.Value));
            break;
          case QUALITY:
            value.Value = System.Convert.ToInt16(value.Value);
            break;
          case TIMESTAMP:
            value.Value = System.Convert.ToDateTime(value.Value);
            break;
          case ACCESSRIGHTS:
            value.Value = System.Convert.ToInt16(value.Value);
            break;
          case SCANRATE:
            value.Value = System.Convert.ToDouble(value.Value);
            break;
          case EUTYPE:
            if (System.Convert.ToString(value.Value).ToLower().Equals(euType.analog.ToString().ToLower()))
              value.Value = euType.analog;
            else if (System.Convert.ToString(value.Value).ToLower().Equals(euType.noEnum.ToString().ToLower()))
              value.Value = euType.noEnum;
            else if (System.Convert.ToString(value.Value).ToLower().Equals(euType.enumerated.ToString().ToLower()))
              value.Value = euType.enumerated;
            else
              value.Value = (int)0;
            break;
          case EUINFO:
            string[] temp_euinfo = new string[1];
            temp_euinfo[0] = System.Convert.ToString(value.Value);
            value.Value = temp_euinfo;
            break;
          case HIHI_LIMIT:
          case LOLO_LIMIT:
          case HI_LIMIT:
          case LO_LIMIT:
          case LOWEU:
          case HIGHEU:
            value.Value = System.Convert.ToDouble(value.Value);
            break;
          default: //do nothing
            break;
        }
      }
      // check data-type.
      if (!property.Type.IsInstanceOfType(value.Value))
      {
#if DEBUG
        //tutaj chyba wyswietla wlasnie te co dodal
        System.Console.WriteLine(
          "|problem with the property (Write fails) : "
          + ToString() + ":"
          + propertyID.ToString().Replace(Opc.Namespace.OPC_DATA_ACCESS, "")
          + "  ");
#endif
        result.ResultID = ResultID.Da.E_BADTYPE;
        return result;
      }
      // write non-value
      switch (propertyID.Code)
      {
        // standard properties.
        case DATATYPE: { m_datatype = (System.Type)value.Value; return result; }
        case QUALITY: { m_quality = (Quality)value.Value; return result; }
        case TIMESTAMP: { m_timestamp = (DateTime)value.Value; return result; }
        case ACCESSRIGHTS: { m_accessRights = (accessRights)value.Value; return result; }
        case SCANRATE: { m_scanRate = (float)value.Value; return result; }
        case EUTYPE: { m_euType = (euType)value.Value; return result; }
        case EUINFO: { m_euInfo = (string[])Opc.Convert.Clone(value.Value); return result; }
        case HIGHEU: { m_maxValue = (double)value.Value; return result; }
        case LOWEU: { m_minValue = (double)value.Value; return result; }
        // other defined properties.
        default:
          {
            if (!m_properties.Contains(propertyID))
            {
              result.ResultID = ResultID.Da.E_INVALID_PID;
              return result;
            }
            m_properties[propertyID] = Opc.Convert.Clone(value.Value);
            break;
          }
      }
      // write complete.
      return result;
    } //Write
    /// <summary>
    /// Set the quality according to specified bits
    /// </summary>
    /// <param name="quality"> Quality bits </param>
    public void MarkTagQuality(Opc.Da.qualityBits quality)
    {
      //      object OldTagValue=null;
      lock (this)
      {
        m_quality = new Quality(quality);
      }
      RaiseOnValueChanged();
    }
    /// <summary>
    /// Updates current value in the cache
    /// </summary>
    /// <param name="val">New value to be set in the cache</param>
    public virtual void UpdateTag(object val)
    {
      lock (this)
      {
        m_value = val;
        m_quality = Quality.Good;
        m_timestamp = m_getCurrentTime();
        //        return OPC.OPC_Wrapper.UpdateTag(Value, val,(short)Opc.Da.qualityBits.good);
      }
      RaiseOnValueChanged();
    }
    /// <summary>
    /// Get current value form the cache to be sent to the device
    /// </summary>
    /// <param name="Val">Value to be set</param>
    /// <returns>true if quality of the requested value is good  </returns>
    public virtual bool GetVal(ref object Val)
    {
      lock (this)
      {
        Val = m_value;
        return m_quality == Quality.Good;
      }
    }
    /// <summary>
    /// Send new value to a device
    /// </summary>
    /// <param name="data">new value to be set</param>
    /// <returns>true if success </returns>
    protected abstract bool UpdateRemote(object data);
    /// <summary>
    /// Read new value directly from device
    /// </summary>
    /// <param name="data">new received value</param>
    /// <returns>true if success</returns>
    protected abstract bool ReadRemote(out object data);
    /// <summary>
    /// Gets or sets the canonical type of the item
    /// </summary>
    public System.Type TagCanonicalType
    {
      get => m_datatype;
      protected set { lock (this) m_datatype = value; }
    }
    #endregion

    #region override object
    /// <summary>
    /// ToString implementation
    /// </summary>
    /// <returns>text that describe this item</returns>
    public override string ToString()
    {
      return "ItemInDevice:" + ItemID + " ";
    }
    #endregion

    #region Private Members
    /// <summary>
    /// number id for the data type property
    /// </summary>
    private const int DATATYPE = 1;
    /// <summary>
    /// number id for the value property
    /// </summary>
    private const int VALUE = 2;
    /// <summary>
    /// number id for the quality property
    /// </summary>
    protected const int QUALITY = 3;
    /// <summary>
    /// number id for the timestamps property
    /// </summary>
    protected const int TIMESTAMP = 4;
    /// <summary>
    /// number id for the access rights property
    /// </summary>
    protected const int ACCESSRIGHTS = 5;
    /// <summary>
    /// number id for the scan rate property
    /// </summary>
    protected const int SCANRATE = 6;
    /// <summary>
    /// number id for the Eng. unit type property
    /// </summary>
    protected const int EUTYPE = 7;
    /// <summary>
    /// number id for the eng unit information property
    /// </summary>
    protected const int EUINFO = 8;
    /// <summary>
    /// number id for the engineering units property
    /// </summary>
    private const int ENGINEERINGUINTS = 100;
    /// <summary>
    /// number id for the high eng. unit property
    /// </summary>
    protected const int HIGHEU = 102;
    /// <summary>
    /// number id for the low eng. unit property
    /// </summary>
    protected const int LOWEU = 103;
    /// <summary>
    /// number id for the time zone property
    /// </summary>
    protected const int TIMEZONE = 108;
    private const int MINIMUM_VALUE = 109;
    private const int MAXIMUM_VALUE = 110;
    private const int VALUE_PRECISION = 111;
    private const int HIHI_LIMIT = 307;
    private const int HI_LIMIT = 308;
    private const int LO_LIMIT = 309;
    private const int LOLO_LIMIT = 310;
    private readonly Func<DateTime> m_getCurrentTime = () => DateTime.UtcNow;
    /// <summary>
    /// the data type of the item
    /// </summary>
    private System.Type m_datatype = null;
    /// <summary>
    /// the value of the item
    /// </summary>
    private object m_value = null;
    /// <summary>
    /// the quality of the item
    /// </summary>
    private Quality m_quality = Quality.Good;
    /// <summary>
    /// the time stamp of the item
    /// </summary>
    private DateTime m_timestamp = DateTime.MinValue;
    /// <summary>
    /// the access rights of the item
    /// </summary>
    private accessRights m_accessRights = accessRights.readWritable;
    /// <summary>
    /// the scan rate of the item
    /// </summary>
    private float m_scanRate = 0;
    /// <summary>
    /// the eng. unit type of the item
    /// </summary>
    private euType m_euType = euType.noEnum;
    /// <summary>
    /// Gets or sets the eng. unit type of the item
    /// </summary>
    /// <value>The type of the eu.</value>
    protected euType EuType
    {
      get => m_euType;
      set
      {
        lock (this)
          m_euType = value;
      }
    }
    /// <summary>
    /// the eng. unit information of the item
    /// </summary>
    private string[] m_euInfo = null;
    /// <summary>
    /// the Maximal value of the item
    /// </summary>
    private double m_maxValue = double.MaxValue;
    /// <summary>
    /// the minimal value of the item
    /// </summary>
    private double m_minValue = double.MinValue;
    /// <summary>
    /// properties table for this item
    /// </summary>
    private Hashtable m_properties = new Hashtable();
    /// <summary>
    /// Checks if the specified property is valid for the specifed access type.
    /// </summary>
    private ResultID ValidatePropertyID(PropertyID propertyID, accessRights accessRequired)
    {
      switch (propertyID.Code)
      {
        // check access rights for value properties.
        case VALUE:
        case QUALITY:
        case TIMESTAMP:
          {
            if (m_accessRights != accessRights.readWritable && m_accessRights != accessRequired)
            {
              switch (accessRequired)
              {
                case accessRights.readable: { return ResultID.Da.E_WRITEONLY; }
                case accessRights.writable: { return ResultID.Da.E_READONLY; }
              }
            }
            break;
          }
        // no checks required for intrinsic properties.
        case DATATYPE:
        case ACCESSRIGHTS:
        case SCANRATE:
        case EUTYPE:
        case EUINFO:
          break;
        // eu limits only valid for analog items.
        case HIGHEU:
        case LOWEU:
          if (m_euType != euType.analog)
            return ResultID.Da.E_INVALID_PID;
          break;
        // data type limits properties are always read only.
        case MINIMUM_VALUE:
        case MAXIMUM_VALUE:
        case VALUE_PRECISION:
          {
            if (accessRequired == accessRights.writable)
              return ResultID.Da.E_READONLY;
            break;
          }
        // lookup any addition property.
        default:
          {
            if (!m_properties.Contains(propertyID))
              return ResultID.Da.E_INVALID_PID;
            break;
          }
      }
      // property is valid.
      return ResultID.S_OK;
    }
    /// <summary>
    /// Updates the ValueQualityTimestamp. (thread safe with lock this)
    /// </summary>
    /// <param name="val">The val.</param>
    /// <param name="quality">The quality.</param>
    /// <param name="timestamp">The time-stamp.</param>
    protected void UpdateVQT(object val, Quality quality, DateTime timestamp)
    {
      lock (this)
      {
        m_value = val;
        m_quality = quality;
        m_timestamp = timestamp;
      }
      RaiseOnValueChanged();
    }
    private void RaiseOnValueChanged()
    {
      OnValueChanged?.Invoke(this, new ItemValueArgs(Read(Property.VALUE)));
    }
    /// <summary>
    /// this value is used by I4UAServer to temporarily store the value to be written by the Flush function
    /// </summary>
    private ItemValue m_ValueToWrite = null;
    #endregion

    #region I4UAServer Members
    /// <summary>
    /// Occurs when value of the process data is changed.
    /// </summary>
    public event EventHandler<ItemValueArgs> OnValueChanged;
    System.Type I4UAServer.ItemCanonicalType => m_datatype;
    ItemValue I4UAServer.LastKnownValue => Read(Property.VALUE);
    object I4UAServer.ValueToWrite
    {
      set => m_ValueToWrite = new Opc.Da.ItemValue(ItemID)
      {
        Value = value,
        QualitySpecified = true,
        Quality = Quality.Good,
        Timestamp = DateTime.Now,
        TimestampSpecified = true
      };
    }
    bool I4UAServer.Flush()
    {
      bool result = false;
      if (m_ValueToWrite != null)
      {
        IdentifiedResult writeresult = Write(Property.VALUE, m_ValueToWrite, true);
        if (writeresult.ResultID == ResultID.S_OK)
          result = true;
      }
      return result;
    }
    #endregion

  }
}
