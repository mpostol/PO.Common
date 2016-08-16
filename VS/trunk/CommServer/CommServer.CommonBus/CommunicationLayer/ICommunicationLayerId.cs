//<summary>
//  Title   : ICommunicationLayerId
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
//  http://www.cas.eu
//</summary>

using System.ComponentModel;
using System.Xml;

namespace CAS.Lib.CommonBus.CommunicationLayer
{
  /// <summary>
  /// Allows to get all of the information identifying the data provider component by a client.
  /// </summary>
  public interface ICommunicationLayerId: ICommunicationLayerFactory
  {
    /// <summary>
    /// Gets the title for the <see cref="ICommunicationLayer"/> provider.
    /// </summary>
    /// <value>The title.</value>
    string Title { get;}
    /// <summary>
    /// Gets the short text description of the communication layer.
    /// </summary>
    /// <value>The communication layer description.</value>
    [DisplayName( "Communication Layer" )]
    [DescriptionAttribute( "The short text description of the communication layer." )]
    [TypeConverterAttribute( typeof( ExpandableObjectConverter ) )]
    ICommunicationLayerDescription GetCommunicationLayerDescription { get;}
    /// <summary>
    /// Returns as a string all the settings of the data provider used for operation.
    /// Client is only holder of the string and is responsible to return this string back
    /// using <see cref="SetSettings"/> before instantiating <see cref="CAS.Lib.CommonBus.ApplicationLayer.IApplicationLayerMaster"/>. All changes to the
    /// parameters have to be done using properties provided by the implementer of this interface.
    /// </summary>
    /// <param name="pSettings">The custom settings.</param>
    void GetSettings( XmlWriter pSettings );
    /// <summary>
    /// Sets all the settings of the data provider used for operation. This string should be returned from the <see cref="GetSettings"/>.
    /// </summary>
    /// <param name="pSettings">All the settings are needed to instantiate the data provider and underling <see>ICommunicationLayer</see></param>
    void SetSettings( XmlReader pSettings );
  }
}
