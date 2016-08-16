//<summary>
//  Title   : NullCommunicationLayerDescription
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    200x created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;

namespace CAS.Lib.CommonBus.CommunicationLayer.NULL
{
  /// <summary>
  /// Description class for the RS communication layer
  /// </summary>
  public class NullCommunicationLayerDescription: CommunicationLayer.CommunicationLayerDescription
  {
    private string m_settings = "no settigns yet";
    internal void SetSettings( string pTraceName )
    {
      m_settings = Title+","+ pTraceName;
    }
    #region ICommunicationLayerId Members
      /// <summary>
      /// Gets a title for the ICommunicationLayer provider.
      /// </summary>
    public override string Title
    {
      get { return "Simulator"; }
    }
      /// <summary>
    /// Gets a description for the ICommunicationLayer provider.
      /// </summary>
    public override string Description
    {
      get { return "Simulated implementation of the ICommunicationLayer"; }
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
  /// Identifier for the Null communication layer
  /// </summary>
  public class NullCommunicationLayerID: ICommunicationLayerId
  {
    #region private
    private const string m_Tag_LayerName = "LayerName";
    string m_LayerName = "Not set";
    private NullCommunicationLayerDescription m_NullCommunicationLayerDescription = new NullCommunicationLayerDescription();
    #endregion
    #region ICommunicationLayerId Members
    /// <summary>
    /// Gets a title for the ICommunicationLayer provider.
    /// </summary>
    public string Title { get { return m_NullCommunicationLayerDescription.Title; } }
    /// <summary>
    /// Gets the short text description for the communication layer.
    /// </summary>
    [DisplayName( "Communication Layer" )]
    [DescriptionAttribute( "The short text description for the communication layer." )]
    [TypeConverterAttribute( typeof( ExpandableObjectConverter ) )]
    public ICommunicationLayerDescription GetCommunicationLayerDescription { get { return m_NullCommunicationLayerDescription; } }
    /// <summary>
    /// Instantiate new object providing <see>ICommunicationLayer</see> functionality.
    /// </summary>
    /// <param name="pParent">Base class responsible for the resources management.</param>
    /// An object providing the <returns><see>ICommunicationLayer</see>functionality</returns>
      public ICommunicationLayer CreateCommunicationLayer( CommonBusControl pParent )
    {
      m_NullCommunicationLayerDescription.SetSettings( m_LayerName );
      return new NULL_to_Serial( m_LayerName, pParent );
    }
    /// <summary>
    /// Instantiate new object providing <see>ICommunicationLayer</see> functionality.
    /// </summary>
    /// <param name="param">list of parameters that are required to create a communication layer. 
    /// This list is specific for the communication layer:
    /// param[0] is string pTraceName, param[1] is CommonBusControl pParent </param>
    /// <returns> An object providing the <see>ICommunicationLayer</see>functionality</returns>
    public ICommunicationLayer CreateCommunicationLayer( object[] param )
    {
      return new NULL_to_Serial( param );
    }
    void ICommunicationLayerId.GetSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteElementString(m_Tag_LayerName, m_LayerName);
      m_NullCommunicationLayerDescription.SetSettings( m_LayerName );
    }
    void ICommunicationLayerId.SetSettings( System.Xml.XmlReader pSettings )
    {
      m_LayerName = pSettings.ReadElementString( m_Tag_LayerName );
      m_NullCommunicationLayerDescription.SetSettings( m_LayerName );
    }
    #endregion
    #region Settings
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
