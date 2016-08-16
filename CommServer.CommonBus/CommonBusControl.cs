//<summary>
//  Title   : Commonb bus component allowing to create all of the available communicationlayers
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol - 10-02-2007
//      created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;
using System.Diagnostics;

namespace CAS.Lib.CommonBus
{
  /// <summary>
  /// Base class responsible for all of resources management used by the component and providing tracing sources.
  /// </summary>
  public partial class CommonBusControl: Component, IContainer
  {
    #region private
    private TraceSource m_TraceSource = new TraceSource( "TraceNet_to_Serial", SourceLevels.Information );
    #endregion
    #region internal
    internal TraceSource GetTraceSource { get { return m_TraceSource; } }
    #endregion
    #region creators
    /// <summary>
    /// Componet creator
    /// </summary>
    public CommonBusControl()
    {
      components = new Container();
      InitializeComponent();
    }
    /// <summary>
    /// Componet creator
    /// </summary>
    /// <param name="container">The container.</param>
    public CommonBusControl( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion
    #region Creators of the ICommunicationLayer
    ///// <summary>
    ///// Creates ICommunicationLayer for UDP protocol
    ///// </summary>
    ///// <param name="localSockedNumber">The port number on which this driver should listen to, or 0 to specify any available port. 
    ///// </param>
    ///// <param name="port">Port description as string</param>
    ///// <param name="protocol">Protocol to be used by the provider.</param>
    ///// <returns>UDO provider for ICommunicationLayer</returns>
    //public ICommunicationLayer InitSerialPort( short localSockedNumber, ProtocolType protocol )
    //{
    //  ICommunicationLayer serialPort = new Net_to_Serial( localSockedNumber, protocol, "NET", this );
    //  return serialPort;
    //}
    ///// <summary>
    ///// Creates ICommunicationLayer for serial communication
    ///// </summary>
    ///// <param name="serialNum">Serail port number</param>
    ///// <param name="baudRate">The baud rate. </param>
    ///// <param name="parityBit">One of the Parity values.</param>
    ///// <param name="stopBits">One of the StopBits values. </param>
    ///// <returns>ICommunicationLayer</returns>
    //public ICommunicationLayer InitSerialPort( SerialPortSettings pSetting )
    //{
    //  ICommunicationLayer serialPort = new RS_to_Serial( pSetting, "NET", this );
    //  return serialPort;
    //}
    //// <summary>
    //// Creates ICommunicationLayer for communication
    //// </summary>
    //// <param name="st">serial type</param>
    //// <param name="parameters">parameters depend on serial type</param>
    //// <param name="port">out - description of created port</param>
    //// <returns></returns>
    //public ICommunicationLayer InitSerialPort( SerialType st, object[] parameters )
    //{
    //  switch ( st )
    //  {
    //    case SerialType.SerialNETTCP:
    //      return InitSerialPort( (short)parameters[ 0 ], System.Net.Sockets.ProtocolType.Tcp );
    //    case SerialType.SerialNETUDP:
    //      return InitSerialPort( (short)parameters[ 0 ], System.Net.Sockets.ProtocolType.Udp );
    //    case SerialType.SerialCOM:
    //      return InitSerialPort( (byte)parameters[ 0 ], (ushort)parameters[ 1 ], (Parity)parameters[ 2 ], (StopBits)parameters[ 3 ] );
    //    default:
    //      throw new NotSupportedException( "this serial port is not supported yet" );
    //  }
    //}
    //// <summary>
    //// Creates ICommunicationLayer for communication
    //// </summary>
    //// <param name="st">serial type</param>
    //// <param name="parameters">parameters depend on serial type</param>
    //// <param name="port">out - description of created port</param>
    //// <returns></returns>
    //public ICommunicationLayer InitSerialPort( string st, object[] parameters )
    //{
    //  foreach ( SerialType commlayer in Enum.GetValues( typeof( CAS.Lib.CommonBus.CommunicationLayer.SerialType ) ) )
    //  {
    //    if ( st == commlayer.ToString() )
    //      return InitSerialPort( commlayer, parameters);
    //  }
    //  throw new NotSupportedException( "this serial port is not supported yet" );
    //}
    #endregion
    #region IContainer Members
    /// <summary>
    /// Adds an <see cref="IComponent"/> item to the list.
    /// </summary>
    /// <param name="component">The <see cref="T:System.ComponentModel.IComponent"/> to add.</param>
    /// <param name="name">The unique, case-insensitive name to assign to the component.
    /// -or-
    /// null that leaves the component unnamed.</param>
    public void Add( IComponent component, string name )
    {
      components.Add( component, name );
    }
    /// <summary>
    /// Adds an <see cref="IComponent"/> item to the list.
    /// </summary>
    /// <param name="component">The <see cref="T:System.ComponentModel.IComponent"/> to add.</param>
    public void Add( IComponent component )
    {
      components.Add( component );
    }

    /// <summary>
    /// Gets all the components in the <see cref="T:System.ComponentModel.IContainer"/>.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// A collection of <see cref="T:System.ComponentModel.IComponent"/> objects that represents all the components in the <see cref="T:System.ComponentModel.IContainer"/>.
    /// </returns>
    public ComponentCollection Components
    {
      get { return components.Components; }
    }
    /// <summary>
    /// Removes an <see cref="IComponent"/> item from the list.
    /// </summary>
    /// <param name="component">The <see cref="T:System.ComponentModel.IComponent"/> to remove.</param>
    public void Remove( IComponent component )
    {
      components.Remove( component );
    }
    #endregion
  }
}
