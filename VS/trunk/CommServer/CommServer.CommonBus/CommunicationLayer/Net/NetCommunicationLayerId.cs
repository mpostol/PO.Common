//<summary>
//  Title   : NetCommunicationLAyerDescription
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    2008-06-13: mzbrzezny: new function that comes from XML helper are used see: itr:[COM-801]
//    mpostol 2007 - 04 - created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary> 

using System.ComponentModel;
using System.Xml;
using CAS.Lib.CommonBus.Xml;

namespace CAS.Lib.CommonBus.CommunicationLayer.Net
{
  /// <summary>
  /// Description for the Net communication layer
  /// </summary>
  public class NetCommunicationLayerDescription: CommunicationLayer.CommunicationLayerDescription
  {
    private string m_settings = "no settigns yet";
    internal void SetSettings( short sockedNum, ProtocolType protocol, string pTraceName )
    {
      m_settings = Title + "," + pTraceName + "," + protocol.ToString() + ":" + sockedNum.ToString();
    }
    #region ICommunicationLayerId Members
      /// <summary>
      /// Gets a title for the ICommunicationLayer provider.
      /// </summary>
    public override string Title
    {
      get { return "UDP/TCP"; }
    }
      /// <summary>
    /// Gets a description for the ICommunicationLayer provider.
      /// </summary>
    public override string Description
    {
      get { return "Implementation of the ICommunicationLayer using the UDP or TCP protocols"; }
    }
    /// <summary>
    /// returns settings of the communication layer in human readable form
    /// </summary>
    public override string HumanReadableSettings
    {
      get { return m_settings; }
    }
    #endregion
  }
  /// <summary>
  /// this enum represents the type of transportation protocol
  /// </summary>
  public enum ProtocolType
  {
    /// <summary>
    /// TCP protocol type
    /// </summary>
    Tcp = System.Net.Sockets.ProtocolType.Tcp,
    /// <summary>
    /// UDP protocol type
    /// </summary>
    Udp = System.Net.Sockets.ProtocolType.Udp
  }
    /// <summary>
  /// Identifier for the Net communication layer
  /// </summary>
  public class NetCommunicationLayerID: ICommunicationLayerId
  {
    #region private
    private const string m_Tag_Class = "NetCommunicationLayerID";
    private const string m_Tag_ProtocolType = "ProtocolType";
    private const string m_Tag_LayerName = "LayerName";
    private const string m_Tag_SocketNumber = "SocketNumber";
    ProtocolType m_ProtocolType = ProtocolType.Udp;
    string m_LayerName = "Not set";
    short m_SocketNumber = -1;
    private NetCommunicationLayerDescription m_CommunicationLayerDescription= new NetCommunicationLayerDescription();
    #endregion
    #region ICommunicationLayerId Members
    /// <summary>
    /// Gets a title for the ICommunicationLayer provider.
    /// </summary>
    public string Title { get { return m_CommunicationLayerDescription.Title; ; } }
    /// <summary>
    /// Gets the short text description for the communication layer.
    /// </summary>
    [DisplayName( "Communication Layer" )]
    [DescriptionAttribute( "The short text description for the communication layer." )]
    [TypeConverterAttribute( typeof( ExpandableObjectConverter ) )]
    public ICommunicationLayerDescription GetCommunicationLayerDescription { get { return m_CommunicationLayerDescription; } }
    /// <summary>
    /// Instantiate new object providing <see>ICommunicationLayer</see> functionality.
    /// </summary>
    /// <param name="pParent">Base class responsible for the resources management.</param>
    /// An object providing the <returns><see>ICommunicationLayer</see>functionality</returns>
    public ICommunicationLayer CreateCommunicationLayer( CommonBusControl pParent )
    {
      m_CommunicationLayerDescription.SetSettings( m_SocketNumber, m_ProtocolType, m_LayerName );
      return new Net_to_Serial( m_SocketNumber, m_ProtocolType, m_LayerName, pParent );
    }
    /// <summary>
    /// Instantiate new object providing <see>ICommunicationLayer</see> functionality.
    /// </summary>
    /// <param name="param">list of parameters that are required to create a communication layer. 
    /// This list is specific for the communication layer:
    /// param[0] is  short sockedNum, param[1] is ProtocolType protocol, 
    /// param[2] is string pTraceName, param[3] is CommonBusControl pParent </param>
    /// <returns> An object providing the <see>ICommunicationLayer</see>functionality</returns>
    public ICommunicationLayer CreateCommunicationLayer( object[] param )
    {
      return new Net_to_Serial(param);
    }
    void ICommunicationLayerId.GetSettings( XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_Tag_Class );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_ProtocolType, (int)m_ProtocolType );
      pSettings.WriteAttributeString( m_Tag_LayerName, m_LayerName );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_SocketNumber, m_SocketNumber );
      pSettings.WriteEndElement();
      m_CommunicationLayerDescription.SetSettings( m_SocketNumber, m_ProtocolType, m_LayerName );
    }
    void ICommunicationLayerId.SetSettings( XmlReader pSettings )
    {
      if ( !pSettings.IsStartElement( m_Tag_Class ) )
        throw new XmlException
          ( string.Format( "Expected element {0} not found at current position of the configuration file", m_Tag_Class ) );
      m_ProtocolType = (ProtocolType)XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_ProtocolType );
      m_LayerName = XmlHelper.ReadattributeString( pSettings, m_Tag_LayerName );
      m_SocketNumber = (short)XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_SocketNumber );
      pSettings.ReadStartElement( m_Tag_Class );
      m_CommunicationLayerDescription.SetSettings( m_SocketNumber, m_ProtocolType, m_LayerName );
    }
    #endregion
    #region Settings
    /// <summary>
    /// Gets or sets the type of the protocol.
    /// </summary>
    /// <value>The type of the protocol.</value>
    public ProtocolType ProtocolType
    {
      get { return m_ProtocolType; }
      set { m_ProtocolType = value; }
    }
    /// <summary>
    /// Gets or sets the socket number.
    /// </summary>
    /// <value>The socket number.</value>
    public short SocketNumber
    {
      get { return m_SocketNumber; }
      set { m_SocketNumber = value; }
    }
    /// <summary>
    /// Gets or sets the name of the layer.
    /// </summary>
    /// <value>The name of the layer.</value>
    public string LayerName
    {
      get { return m_LayerName; }
      set { m_LayerName = value; }
    }
    #endregion
  }
}
