//<summary>
//  Title   : RSCommunicationLayerDescription
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http:/www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using CAS.Lib.CommonBus.Xml;
using System.Xml;

namespace CAS.Lib.CommonBus.CommunicationLayer.RS
{
  /// <summary>
  /// Description class for the RS communication layer
  /// </summary>
  public class RSCommunicationLayerDescription: CommunicationLayer.CommunicationLayerDescription
  {
    private string m_settings = "no settigns yet";
    internal void SetSettings(uint Comname ,SerialPortSettings.BAUD_RATE brate )
    {
      m_settings = Title + "," + "COM"+Comname.ToString()+":"+brate.ToString();
    }
    #region CommunicationLayerDescription overrides
    /// <summary>
    /// Gets a title for the ICommunicationLayer provider.
    /// </summary>
    public override string Title
    {
      get { return "RS"; }
    }
    /// <summary>
    /// Gets a description for the ICommunicationLayer provider.
    /// </summary>
    public override string Description
    {
      get { return "Implementation of the ICommunicationLayer using a serial port resource"; }
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
  /// Identifier for the RS communication layer
  /// </summary>
  public class RSCommunicationLayerID: SerialPortSettings, ICommunicationLayerId
  {
    #region private
    private const string m_Tag_Class = "RSCommunicationLayerID";
    private RSCommunicationLayerDescription m_RSCommunicationLayerDescription = new RSCommunicationLayerDescription();
    #endregion
    #region ICommunicationLayerId Members
    /// <summary>
    /// Gets a title for the ICommunicationLayer provider.
    /// </summary>
    public string Title { get { return m_RSCommunicationLayerDescription.Title; } }
    /// <summary>
    /// Gets the short text description for the communication layer.
    /// </summary>
    [DisplayName( "Communication Layer" )]
    [DescriptionAttribute( "The short text description for the communication layer." )]
    [TypeConverterAttribute( typeof( ExpandableObjectConverter ) )]
    public ICommunicationLayerDescription GetCommunicationLayerDescription { get { return m_RSCommunicationLayerDescription; } }
    void ICommunicationLayerId.GetSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_Tag_Class );
      //pSettings.WriteAttributeString( m_Tag_LayerName, m_LayerName );
      pSettings.WriteEndElement();
      base.GetSettings( pSettings );
      m_RSCommunicationLayerDescription.SetSettings( this.PortName, this.BaudRate );
    }
    void ICommunicationLayerId.SetSettings( System.Xml.XmlReader pSettings )
    {
      if ( !pSettings.IsStartElement( m_Tag_Class ) )
        throw new XmlException
          ( string.Format( "Expected element {0} not found at current position of the configuration file", m_Tag_Class ) );
      //m_LayerName = XmlHelper.ReadattributeString( pSettings, m_Tag_LayerName );
      pSettings.ReadStartElement( m_Tag_Class );
      base.SetSettings( pSettings );
      m_RSCommunicationLayerDescription.SetSettings( this.PortName, this.BaudRate );
    }
    #endregion
  }
}
