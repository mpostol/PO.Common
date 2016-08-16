//<summary>
//  Title   : Base implementation of the Application Layer Protocol
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol 03-04-2007: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.RTLib.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer
{

  /// <summary>
  /// Base implementation of the Application Layer Protocol
  /// </summary>
  /// <typeparam name="T_ALMessage">The type of the message that is used in communication through this protocol.</typeparam>
  public abstract class ALProtocol<T_ALMessage> where T_ALMessage: ProtocolALMessage
  {
    #region private
    private void InitObject
      ( ICommunicationLayer cCommLayer, ProtocolParameters cProtParameters, IProtocolParent cStatistic )
    {
      this.myCommLayer = cCommLayer;
      this.myProtParameters = cProtParameters;
      this.myStatistic = cStatistic;
    }
    private IProtocolParent myStatistic;
    private ProtocolParameters myProtParameters;
    private ICommunicationLayer myCommLayer;
    #endregion
    #region protected
    /// <summary>
    /// This function gets message from the remote unit.
    /// </summary>
    /// <param name="Rxmsg">Received message</param>
    /// <param name="Txmsg">Transmited message, information about this frmae could be necessary to properly init received frame.
    /// </param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    internal protected abstract AL_ReadData_Result GetMessage( out T_ALMessage Rxmsg, T_ALMessage Txmsg );
    /// <summary>
    /// Transmit message to the remote unit.
    /// </summary>
    /// <param name="Txmsg">Message to be transmitted</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    internal protected abstract AL_ReadData_Result TransmitMessage( T_ALMessage Txmsg );
    /// <summary>
    /// Gets the object responsible for providing the communication with the remote unit –service access point 
    /// </summary>
    public ICommunicationLayer GetICommunicationLayer { get { return myCommLayer; } }
    #endregion
    #region public
    /// <summary>
    /// Gets the object containing the protocol parameters.
    /// </summary>
    public ProtocolParameters GetProtocolParameters { get { return myProtParameters; } }
    /// <summary>
    /// Gets interface to statistical information about the communication performance.
    /// </summary>
    public IProtocolParent GetIProtocolParent { get { return myStatistic; } }
    #endregion
    #region creators
    /// <summary>
    /// Creator of ALProtocol
    /// </summary>
    /// <param name="pCommLayer">Interface responsible for providing the communication</param>
    /// <param name="pProtParameters">Protocol parameters</param>
    /// <param name="pStatistic">Statistical information about the communication performance</param>
    protected ALProtocol( ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters, IProtocolParent pStatistic )
    {
      InitObject( pCommLayer, pProtParameters, pStatistic );
    }
    #endregion
  }
}
