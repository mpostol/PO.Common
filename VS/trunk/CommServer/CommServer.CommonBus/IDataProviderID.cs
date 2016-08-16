//<summary>
//  Title   : Data provider identifying interface – provides general information about the plug-in and its functionality.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081003: mzbrzezny: AddressSpaceDescriptor implementation
//  20081003: mzbrzezny: ItemDefaultSettings implementation
//  Mpostol 21-04-2007: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml;
using CAS.Lib.CommonBus.ApplicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus
{
  /// <summary cref="IDataProviderID">
  /// Data provider identifying interface – provides general information about the plug-in and its functionality. 
  /// Allows to get and set by the client all parameters setting for the data provider component and after that 
  /// instantiate data provider.
  /// </summary>
  public interface IDataProviderID: IEnumerable<KeyValuePair<string, ICommunicationLayerId>>
  {
    /// <summary>Gets the title custom attribute for the plug-in assembly manifest.</summary>
    string Title { get; }
    /// <summary>
    /// Get all information identifying the communication provider.
    /// </summary>
    IDataProviderDescription GetDataProviderDescription { get; }
    /// <summary>
    /// Instantiate object providing  <see cref="IApplicationLayerMaster"/> - an object implementing 
    /// master (playing the role on the field network of the master station,) interfaces defined for the application layer. 
    /// </summary>
    /// <param name="pStatistic">Statistical information about the communication performance.</param>
    /// <param name="pParent"><seealso cref="CommonBusControl"/> - Base class responsible for all of resources management used 
    /// by the component and providing tracing sources.</param>
    /// <returns>Return an object implementing IApplicationLayerMaster.</returns>
    IApplicationLayerMaster GetApplicationLayerMaster( IProtocolParent pStatistic, CommonBusControl pParent );
    //IApplicationLayerPluginHelperSlave GetALHelperMAsterSlave { get;}
    //IApplicationLayerPluginHelperSniffer GetALHelperMAsterSniffer { get;}
    /// <summary>
    /// This function is responsible for returning the list of addressspaces in the data provider
    /// </summary>
    /// <returns>Hashtable with addressspaces</returns>
    IAddressSpaceDescriptor[] GetAvailiableAddressspaces();
    /// <summary>
    /// Gets the item default settings.
    /// </summary>
    /// <param name="AddressSpaceIdentifier">The address space identifier.</param>
    /// <param name="AddressInTheAddressSpace">The address in the address space.</param>
    /// <returns>default settings for the item</returns>
    IItemDefaultSettings GetItemDefaultSettings( short AddressSpaceIdentifier, ulong AddressInTheAddressSpace );
    /// <summary>
    /// Indexer allowing to get object providing <seealso cref="ICommunicationLayerId"/> from the collection using Title as an index.
    /// </summary>
    /// <param name="idx">Title of the object</param>
    /// <returns>Return an object implementing <see cref="IApplicationLayerMaster"/></returns>
    ICommunicationLayerId this[ string idx ] { get; }
    /// <summary>
    /// Returns as a string all the settings of the data provider used for operation. 
    /// Client is only holder of the string and is responsible to return this string back 
    /// using see <see cref="SetSettings"/> before instantiating <see cref="IApplicationLayerMaster"/> . All changes to the 
    /// parameters have to be done using provided by the implementer of this interface properties. 
    /// </summary>
    /// <returns>All the settings are needed to instantiate the data provider and underling <see>ICommunicationLayer</see>.</returns>
    string GetSettings();
    /// <summary>
    /// Returns as a string all the settings of the data provider used for operation. 
    /// Data is returned in Human readable way
    /// </summary>
    /// <returns>All the settings are needed in human readable format</returns>
    string GetSettingsHumanReadableFormat();
    /// <summary cref="SetSettings">
    /// Sets all the settings of the data provider used for operation. This string should be returned from <see cref="GetSettings"/>.
    /// </summary>
    /// <param name="pSettings">
    /// All the settings are needed to instantiate the data provider and underling <see>ICommunicationLayer</see>
    /// </param>
    void SetSettings( string pSettings );
    /// <summary>
    /// Get and set underlying <see cref="ICommunicationLayerId"/>. Information about selected communication layer 
    /// provider should be included in the setting string and used before instantiating the data <see cref="IApplicationLayerMaster"/>
    /// </summary>
    ICommunicationLayerId SelectedCommunicationLayer { get; set; }
  }
  /// <summary>
  /// Basic implementation of IDataProviderID.
  /// </summary>
  /// <remarks>
  /// Inheritor have to instantiate object providing  <see cref="IDataProviderID"/> - a helper interface allowing 
  /// in turn to configure and instantiate another objects implementing interfaces defined for the application layer.
  /// </remarks>
  public abstract class DataProviderID: IDataProviderID
  {
    #region private
    private const string m_Tag_TreeElement = "Settings";
    private const string m_Tag_CommunicationLayer = "CommunicationLayerId";
    private const string m_Tag_DataProviderSettings = "DataProviderSettings";
    private const string m_Tag_CmmLayerSettings = "CommunicationLayerSettings";
    private DataProviderDescription m_DataProviderDescription;
    private System.Collections.Generic.SortedList<string, ICommunicationLayerId> m_CommunicationLayerIdColl =
      new SortedList<string, ICommunicationLayerId>();
    private ICommunicationLayerId m_SelectedCommunicationLayerId = null;
    #endregion
    #region protected
    /// <summary>
    /// When overridden in a derived class, writes custom settings to the XML stream.
    /// </summary>
    /// <param name="pSettings"><see cref="XmlWriter"/> to save settings.</param>
    protected abstract void WriteSettings( XmlWriter pSettings );
    /// <summary>
    /// When overridden in a derived class, reads custom settings from the XML stream.
    /// </summary>
    /// <param name="pSettings">Custom settings in the <see cref="XmlReader"/> </param>
    protected abstract void ReadSettings( XmlReader pSettings );
    /// <summary>
    /// Instantiate new object providing <see cref="ICommunicationLayer"/> functionality.
    /// </summary>
    /// <param name="pParent">Base class responsible for the resources management.</param>
    /// <returns>
    /// An object providing the <see cref="ICommunicationLayer"/> functionality
    /// </returns>
    protected ICommunicationLayer CreateCommunicationLayer( CommonBusControl pParent )
    {
      return m_SelectedCommunicationLayerId.CreateCommunicationLayer( pParent );
    }
    /// <summary>
    /// Adds a communication  <see cref="ICommunicationLayerId"/> item to the list. 
    /// </summary>
    /// <param name="value">The <see cref="ICommunicationLayerId"/> object to add to the list.</param>
    /// <returns>The position into which the new element was inserted.</returns>
    protected void Add( ICommunicationLayerId value )
    {
      m_CommunicationLayerIdColl.Add( value.Title, value );
      if ( m_SelectedCommunicationLayerId == null )
        m_SelectedCommunicationLayerId = value;
    }
    /// <summary>
    /// Helper that converts members of an Enum to pairs name and corresponding value and fills up the Hashtable.
    /// </summary>
    /// <param name="AddressSpaceEnum">The address space enum.</param>
    /// <param name="StartEndPairs">The start end pairs. SortedList that represents Start and End Points for selected AddressSpace.
    /// The Key of the SortedList is linked to values in the Enum that is passed as AddressSpaceEnum</param>
    /// <returns>table of descriptors</returns>
    protected IAddressSpaceDescriptor[] GetAvailiableAddressspacesHelper( Type AddressSpaceEnum, SortedList<short, StartEndPair> StartEndPairs )
    {
      Array EnumValues = Enum.GetValues( AddressSpaceEnum );
      if ( StartEndPairs == null )
      {
        StartEndPairs = new SortedList<short, StartEndPair>();
        //default starts and end should be used 
        StartEndPair defaultStartEndPair = new StartEndPair( 0, long.MaxValue );
        for ( int idx = 0; idx < EnumValues.Length; idx++ )
        {
          StartEndPairs.Add( System.Convert.ToInt16( EnumValues.GetValue( idx ) ), defaultStartEndPair );
        }
      }
      IAddressSpaceDescriptor[] table = new IAddressSpaceDescriptor[ EnumValues.Length ];
      for ( int idx = 0; idx < EnumValues.Length; idx++ )
      {
        short currentidentifier = System.Convert.ToInt16( EnumValues.GetValue( idx ) );
        AddressSpaceDescriptor asd = new AddressSpaceDescriptor(
          Enum.GetName( AddressSpaceEnum, EnumValues.GetValue( idx ) ),
          currentidentifier, StartEndPairs[ currentidentifier ].StartAddress,
          StartEndPairs[ currentidentifier ].EndAddress );
        table[ idx ] = asd;
      }
      return table;
    }
    #endregion
    #region IDataProviderID Members
    /// <summary>
    /// Gets the title custom attribute for the plug-in assembly manifest.
    /// </summary>
    public string Title { get { return m_DataProviderDescription.Title; } }
    /// <summary>
    /// Get all information identifying the DatProvide component.
    /// </summary>
    [TypeConverterAttribute( typeof( ExpandableObjectConverter ) ), DisplayName( "Data Provider Description" )]
    public IDataProviderDescription GetDataProviderDescription
    {
      get { return m_DataProviderDescription; }
    }
    /// <summary>
    /// When overridden in a derived class, instantiates object providing  <see cref="IApplicationLayerMaster"/> - an object 
    /// implementing master side (playing the role on the field network of the master station,) interfaces defined for the 
    /// application layer. 
    /// </summary>
    /// <param name="pStatistic">Statistical information about the communication performance.</param>
    /// <param name="pParent"><seealso cref="CommonBusControl"/> - Base class responsible for all of resources management used 
    /// by the component and providing tracing sources.</param>
    /// <returns>Return an object implementing IApplicationLayerMaster.</returns>
    public abstract IApplicationLayerMaster GetApplicationLayerMaster
      ( IProtocolParent pStatistic, CommonBusControl pParent );
    /// <summary>
    /// This metchod is responsible for returning the list of addressspaces in the data provider.
    /// </summary>
    /// <returns>
    /// It returns collection <see cref="Hashtable"/> of pairs (Name, Identifier) each one representing 
    /// supported address space.
    /// </returns>
    public abstract IAddressSpaceDescriptor[] GetAvailiableAddressspaces();
    /// <summary>
    /// Gets the item default settings.
    /// </summary>
    /// <param name="AddressSpaceIdentifier">The address space identifier.</param>
    /// <param name="AddressInTheAddressSpace">The address in the address space.</param>
    /// <returns>default settings for the item</returns>
    public abstract IItemDefaultSettings GetItemDefaultSettings( short AddressSpaceIdentifier, ulong AddressInTheAddressSpace );    /// <summary>
    /// Indexer allowing to get object providing <seealso cref="ICommunicationLayerId"/> from the collection using Title as an index.
    /// </summary>
    /// <param name="idx">Title of the object</param>
    /// <returns>Return an object implementing <seealso cref="IApplicationLayerMaster"/></returns>
    public ICommunicationLayerId this[ string idx ] { get { return m_CommunicationLayerIdColl[ idx ]; } }
    /// <summary>
    /// this function creates an string XML representation of the dataprovider settings
    /// </summary>
    /// <returns>XML string representation of the dataprovider settings</returns>
    public virtual string GetSettings()
    {
      XmlWriterSettings m_WS = new XmlWriterSettings();
      m_WS.Indent = true;
      m_WS.IndentChars = "    ";
      m_WS.ConformanceLevel = ConformanceLevel.Document;
      StringWriter m_TW = new StringWriter();
      using ( XmlWriter m_XmlTW = XmlWriter.Create( m_TW, m_WS ) )
      {
        m_XmlTW.WriteStartDocument();
        m_XmlTW.WriteStartElement( m_Tag_TreeElement );
        m_XmlTW.WriteStartElement( m_Tag_DataProviderSettings );
        WriteSettings( m_XmlTW );
        m_XmlTW.WriteEndElement();
        m_XmlTW.WriteElementString( m_Tag_CommunicationLayer, m_SelectedCommunicationLayerId.GetCommunicationLayerDescription.Title );
        m_XmlTW.WriteStartElement( m_Tag_CmmLayerSettings );
        m_SelectedCommunicationLayerId.GetSettings( m_XmlTW );
        m_XmlTW.WriteEndElement();
        m_XmlTW.WriteEndElement();
        m_XmlTW.WriteEndDocument();
      }
      //m_XmlTW.Close();
      return m_TW.GetStringBuilder().ToString();
    }
    /// <summary>
    /// this function creates an string representation of the dataprovider settings
    /// </summary>
    /// <returns>human readable information about the dataprovider settings</returns>
    public virtual string GetSettingsHumanReadableFormat()
    {
      string Description = this.GetDataProviderDescription +
        "; Communication Layer: " + m_SelectedCommunicationLayerId.GetCommunicationLayerDescription.HumanReadableSettings;
      return Description;
    }
    /// <summary>
    /// This function is deserializing the data provider settings from the string
    /// </summary>
    /// <param name="pSettings">XML settings</param>
    public virtual void SetSettings( string pSettings )
    {
      if ( string.IsNullOrEmpty( pSettings ) )
        return;
      //remove white space from the beginning:
      pSettings = pSettings.TrimStart( '\r', ' ', '\n', '\t' );
      XmlReaderSettings m_XRS = new XmlReaderSettings();
      m_XRS.ConformanceLevel = ConformanceLevel.Document;
      m_XRS.IgnoreWhitespace = true;
      StringReader m_TR = new StringReader( pSettings );
      using ( XmlReader m_XmlTR = XmlReader.Create( m_TR, m_XRS ) )
      {
        m_XmlTR.Read();
        m_XmlTR.ReadStartElement( m_Tag_TreeElement );
        m_XmlTR.ReadStartElement( m_Tag_DataProviderSettings );
        ReadSettings( m_XmlTR );
        m_XmlTR.ReadEndElement();
        string cTitle = m_XmlTR.ReadElementString( m_Tag_CommunicationLayer );
        m_SelectedCommunicationLayerId = this[ cTitle ];
        if ( m_SelectedCommunicationLayerId == null )
          return;
        m_XmlTR.ReadStartElement( m_Tag_CmmLayerSettings );
        m_SelectedCommunicationLayerId.SetSettings( m_XmlTR );
        m_XmlTR.ReadEndElement();
        m_XmlTR.ReadEndElement();
      }
    }
    /// <summary>
    /// Get and set underlying <see cref="ICommunicationLayerId"/>. Information about selected communication layer 
    /// provider should be included in the setting string and used before instantiating the data <see cref="IApplicationLayerMaster"/>
    /// </summary>
    [Browsable( true )]
    [TypeConverterAttribute( typeof( ExpandableObjectConverter ) )]
    public ICommunicationLayerId SelectedCommunicationLayer
    {
      get { return m_SelectedCommunicationLayerId; }
      set
      {
        if ( this[ value.Title ] == null )
          throw new ArgumentException( "Selected object is not a member of this collection" );
        m_SelectedCommunicationLayerId = value;
      }
    }
    #endregion
    #region public
    /// <summary>
    /// Retrieves a string representation of the object.
    /// </summary>
    /// <returns>Description of the message:[station][address][myDataType][length]</returns>
    public override string ToString()
    {
      return m_DataProviderDescription.FullName;
    }
    #endregion
    #region IEnumerable<KeyValuePair<string,ICommunicationLayerId>> Members
    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<KeyValuePair<string, ICommunicationLayerId>> GetEnumerator()
    {
      return m_CommunicationLayerIdColl.GetEnumerator();
    }
    #endregion
    #region IEnumerable Members
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return m_CommunicationLayerIdColl.GetEnumerator();
    }
    #endregion
    #region constructor
    /// <summary>
    /// Basic implementation of the <see cref="IDataProviderID"/> interface.
    /// </summary>
    /// <remarks>
    /// Inheritor have to instantiate anobject providing  <see cref="IDataProviderID"/> - a helper interface allowing 
    /// in turn to configure and instantiate another objects implementing interfaces defined for the application layer.
    /// </remarks>
    public DataProviderID()
    {
      m_DataProviderDescription = new DataProviderDescription( Assembly.GetCallingAssembly() );
    }
    #endregion
  }
}
