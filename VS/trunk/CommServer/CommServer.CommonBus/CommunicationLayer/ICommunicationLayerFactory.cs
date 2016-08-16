//<summary>
//  Title   : nterface while implemented responsible for instantiations of ICommunicationLayer
//  System  : Microsoft Visual C# .NET 
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

namespace CAS.Lib.CommonBus.CommunicationLayer
{
  /// <summary>
  /// Interface while implemented responsible for instantiations of of <see cref="ICommunicationLayer"/>
  /// </summary>
  public interface ICommunicationLayerFactory
  {
    /// <summary>
    /// Instantiate new object providing <see cref="ICommunicationLayer"/> functionality.
    /// </summary>
    /// <param name="cParent">Base class responsible for the resources management.</param>
    /// <returns>
    /// An object providing the <see cref="ICommunicationLayer"/> functionality
    /// </returns>
    ICommunicationLayer CreateCommunicationLayer( CommonBusControl cParent );
    /// <summary>
    /// Instantiate new object providing <see cref="ICommunicationLayer"/> functionality.
    /// </summary>
    /// <param name="parameter">list of parameters that are required to create a communication layer.
    /// This list is specific for the communication layer</param>
    /// <returns>
    /// An object providing the <see cref="ICommunicationLayer"/> functionality
    /// </returns>
    ICommunicationLayer CreateCommunicationLayer( object [] parameter );
  }
}