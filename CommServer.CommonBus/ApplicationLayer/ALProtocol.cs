//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CommonBus.CommunicationLayer;
using UAOOI.ProcessObserver.RealTime.Management;

namespace CAS.Lib.CommonBus.ApplicationLayer
{

  /// <summary>
  /// Base implementation of the Application Layer Protocol
  /// </summary>
  /// <typeparam name="T_ALMessage">The type of the message that is used in communication through this protocol.</typeparam>
  public abstract class ALProtocol<T_ALMessage> where T_ALMessage : ProtocolALMessage
  {

    #region private
    private void InitObject(ICommunicationLayer cCommLayer, ProtocolParameters cProtParameters, IProtocolParent cStatistic)
    {
      myCommLayer = cCommLayer;
      myProtParameters = cProtParameters;
      myStatistic = cStatistic;
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
    protected internal abstract AL_ReadData_Result GetMessage(out T_ALMessage Rxmsg, T_ALMessage Txmsg);
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
    protected internal abstract AL_ReadData_Result TransmitMessage(T_ALMessage Txmsg);
    /// <summary>
    /// Gets the object responsible for providing the communication with the remote unit –service access point 
    /// </summary>
    public ICommunicationLayer GetICommunicationLayer => myCommLayer;
    #endregion

    #region public
    /// <summary>
    /// Gets the object containing the protocol parameters.
    /// </summary>
    public ProtocolParameters GetProtocolParameters => myProtParameters;
    /// <summary>
    /// Gets interface to statistical information about the communication performance.
    /// </summary>
    public IProtocolParent GetIProtocolParent => myStatistic;
    #endregion

    #region creators
    /// <summary>
    /// Creator of ALProtocol
    /// </summary>
    /// <param name="pCommLayer">Interface responsible for providing the communication</param>
    /// <param name="pProtParameters">Protocol parameters</param>
    /// <param name="pStatistic">Statistical information about the communication performance</param>
    protected ALProtocol(ICommunicationLayer pCommLayer, ProtocolParameters pProtParameters, IProtocolParent pStatistic)
    {
      InitObject(pCommLayer, pProtParameters, pStatistic);
    }
    #endregion

  }
}
