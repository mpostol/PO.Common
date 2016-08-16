//<summary>
//  Title   : Communication Layer Factory
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    2007-05-29 Mzbrzezny - created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Collections.Generic;
using CAS.Lib.CommonBus.CommunicationLayer.Net;

namespace CAS.Lib.CommonBus.CommunicationLayer
{
  /// <summary>
  /// Class Responsible for creation of any Communication Layer
  /// </summary>
  public class CommunicationLayerFactory
  {
    private static System.Collections.Generic.SortedList<string, ICommunicationLayerId> m_CommunicationLayerIdColl =
      new SortedList<string, ICommunicationLayerId>();
    private static void Add( ICommunicationLayerId value )
    {
      m_CommunicationLayerIdColl.Add( value.Title, value );
    }
    static CommunicationLayerFactory()
    {
      Add( new CAS.Lib.CommonBus.CommunicationLayer.Net.NetCommunicationLayerID() );
      Add( new CAS.Lib.CommonBus.CommunicationLayer.RS.RSCommunicationLayerID() );
      Add( new CAS.Lib.CommonBus.CommunicationLayer.NULL.NullCommunicationLayerID() );
    }
    /// <summary>
    /// This function should be use to retreive all communication layer names
    /// </summary>
    /// <returns>communication layer names as list of strings</returns>
    public static IList<string> GetCommunicationLayerNames()
    {
      return m_CommunicationLayerIdColl.Keys;
    }
    /// <summary>
    /// Instantiate new object providing <see>ICommunicationLayer</see> functionality.
    /// </summary>
    /// <param name="name">name of the communication layer</param>
    /// <param name="param">array of parameters</param>
    /// An object providing the <returns><see>ICommunicationLayer</see>functionality</returns>
    public static ICommunicationLayer CreateCommunicationLayer( string name, object[] param )
    {
      return m_CommunicationLayerIdColl[ name ].CreateCommunicationLayer( param );
    }
    #region communicationLayer specific creators
    /// <summary>
    /// Creates the net communication layer.
    /// </summary>
    /// <param name="sockedNum">The socked num.</param>
    /// <param name="protocol">The protocol.</param>
    /// <param name="pTraceName">Name of the trace.</param>
    /// <param name="pParent">The parent common bus control.</param>
    /// <returns>Communication layer</returns>
    public static ICommunicationLayer CreateNetCommunicationLayer( short sockedNum, ProtocolType protocol, string pTraceName, CommonBusControl pParent )
    {
        return new CAS.Lib.CommonBus.CommunicationLayer.Net.NetCommunicationLayerID().CreateCommunicationLayer(
          new object [] {sockedNum,protocol,pTraceName,pParent});
    }
    #endregion
  }
}
