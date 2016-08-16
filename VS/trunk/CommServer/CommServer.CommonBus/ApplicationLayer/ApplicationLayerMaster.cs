//<summary>
//  Title   : Base implementation of the IApplicationLayerMaster interface
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20090123: mzbrzezny: some additional checking(if txframe!=null) and debugging (tracing) is added to TxGetResponse
//    20080612 : mzbrzezny: ApplicationLayerMaster is checking if !(IEnvelope)Rxmsg ).InPool before call ReturnEmptyEnvelope();
//    03-04-2007: MPostol created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.CommonBus.CommunicationLayer.Generic;
using CAS.Lib.RTLib.Management;
using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  /// <summary>
  /// Base implementation of IapplicationLayerMaster interface.
  /// </summary>
  /// <typeparam name="T_ALMessage">The type of the message that is used in communication through this protocol.</typeparam>
  public abstract class ApplicationLayerMaster<T_ALMessage>
    : ApplicationLayerCommon, IApplicationLayerMaster where T_ALMessage: ProtocolALMessage
  {
    #region private
    private SesDBufferPool<T_ALMessage> m_Pool;
    private ALProtocol<T_ALMessage> m_protocol;
    private IProtocolParent m_Statistic;
    private System.Diagnostics.Stopwatch InterfrarmeStopwatch = new System.Diagnostics.Stopwatch();
    /// <summary>
    /// Transits frame and gets response. Response is analyzed to much the sent frame.
    /// </summary>
    /// <param name="cTxmsg">Frame to be sent.</param>
    /// <returns>Result of the operation.</returns>
    /// <param name="cRetries"></param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    private AL_ReadData_Result TxGetResponse( T_ALMessage cTxmsg, byte cRetries )
    {
      T_ALMessage rxmsg;
      AL_ReadData_Result res = TxGetResponse( cTxmsg, out rxmsg, cRetries );
      if ( rxmsg != null )
        rxmsg.ReturnEmptyEnvelope();
      return res;
    }
    /// <summary>
    /// enum used by TxGetResponse function to indicate its state
    /// </summary>
    private enum TGRState { TxReq, RxRes, Retray };
    /// <summary>
    /// Transits frame and gets response. Response is analyzed to much the sent frame.
    /// </summary>
    /// <param name="Txmsg">Frame to be sent.</param>
    /// <param name="Rxmsg">Received frame</param>
    /// <param name="cRetries">Number of retries to get frame from remote unit.</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    private AL_ReadData_Result TxGetResponse( T_ALMessage Txmsg, out T_ALMessage Rxmsg, byte cRetries )
    {
      Rxmsg = null;
      TGRState currState = TGRState.TxReq;
      if ( Txmsg == null )
      {
        TraceEvent.Tracer.TraceInformation( 81, "ApplicationLayerMaster.TxGetResponse", "Transmitted message cannot be null." );
        return AL_ReadData_Result.ALRes_DatTransferErrr;
      }
      while ( true )
      {
        switch ( currState )
        {
          case TGRState.TxReq:
            Timer.WaitTimeout( m_protocol.GetProtocolParameters.InterframeGapSpan, InterfrarmeStopwatch );
            switch ( m_protocol.TransmitMessage( Txmsg ) )
            {
              case AL_ReadData_Result.ALRes_Success:
                currState = TGRState.RxRes;
                break;
              case AL_ReadData_Result.ALRes_DisInd:
                return AL_ReadData_Result.ALRes_DisInd;
              case AL_ReadData_Result.ALRes_DatTransferErrr:
                return AL_ReadData_Result.ALRes_DatTransferErrr;
            }
            break;
          case TGRState.RxRes:
            if ( Txmsg == null )
            {
              TraceEvent.Tracer.TraceInformation( 104, "ApplicationLayerMaster.TxGetResponse", "Before: m_protocol.GetMessage( out Rxmsg, Txmsg ): Transmitted message cannot be null." );
              return AL_ReadData_Result.ALRes_DatTransferErrr;
            }
            AL_ReadData_Result res = m_protocol.GetMessage( out Rxmsg, Txmsg );
            InterfrarmeStopwatch.Reset();
            InterfrarmeStopwatch.Start();
            switch ( res )
            {
              case AL_ReadData_Result.ALRes_Success:
                InterfrarmeStopwatch.Reset();
                InterfrarmeStopwatch.Start();
                ProtocolALMessage.CheckResponseResult lastCheckResult = Rxmsg.CheckResponseFrame( Txmsg );
                switch ( lastCheckResult )
                {
                  case ProtocolALMessage.CheckResponseResult.CR_OK:
                    m_Statistic.IncStRxFrameCounter();
                    return AL_ReadData_Result.ALRes_Success;
                  case ProtocolALMessage.CheckResponseResult.CR_SynchError:
                    m_Statistic.IncStRxSynchError();
                    break;
                  case ProtocolALMessage.CheckResponseResult.CR_Incomplete:
                    m_Statistic.IncStRxFragmentedCounter();
                    break;
                  case ProtocolALMessage.CheckResponseResult.CR_CRCError:
                    m_Statistic.IncStRxCRCErrorCounter();
                    break;
                  case ProtocolALMessage.CheckResponseResult.CR_Invalid:
                    m_Statistic.IncStRxInvalid();
                    break;
                  case ProtocolALMessage.CheckResponseResult.CR_NAK:
                    m_Statistic.IncStRxNAKCounter();
                    break;
                }
                if ( !( (IEnvelope)Rxmsg ).InPool )
                  Rxmsg.ReturnEmptyEnvelope();
                Rxmsg = null;
                currState = TGRState.Retray;
                break;
                //return AL_ReadData_Result.ALRes_DatTransferErrr;
              case AL_ReadData_Result.ALRes_DisInd:
                if ( Rxmsg != null  )
                {
                  if ( !( (IEnvelope)Rxmsg ).InPool )
                    Rxmsg.ReturnEmptyEnvelope();
                  Rxmsg = null;
                }
                return AL_ReadData_Result.ALRes_DisInd;
              default:
                currState = TGRState.Retray;
                break;
            }
            break;
          case TGRState.Retray:
            if ( Rxmsg != null )
            {
              if ( !( (IEnvelope)Rxmsg ).InPool )
                Rxmsg.ReturnEmptyEnvelope();
              Rxmsg = null;
            }
            if ( cRetries == 0 )
            {
              return AL_ReadData_Result.ALRes_DatTransferErrr;
            }
            cRetries--;
            currState = TGRState.TxReq;
            break;
        }
      }//while (true)
    }
    #endregion
    #region IApplicationLayerMaster Members
    /// <summary>
    /// Read Data
    /// </summary>
    /// <param name="block">Block description to be read</param>
    /// <param name="station">Address of the remote station connected to the common field bus. –1 if not applicable.
    /// </param>
    /// <param name="data">The frame with the requested data.</param>
    /// <param name="retries">Number of retries to get data.</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result IApplicationLayerMaster.ReadData
      ( IBlockDescription block, int station, out IReadValue data, byte retries )
    {
      data = null;
      if ( !m_protocol.GetICommunicationLayer.Connected )
        return AL_ReadData_Result.ALRes_DisInd;
      T_ALMessage request = m_Pool.GetEmptyISesDBuffer();
      T_ALMessage response; 
      request.PrepareRequest( station, block );
      AL_ReadData_Result res = TxGetResponse( request, out response, retries );
      if ( res == AL_ReadData_Result.ALRes_Success )
      {
        response.SetBlockDescription( station, block );
        data = (IReadValue)response;
      }
      else
        if (  response!= null && !( (IEnvelope)response ).InPool)
        {
          EventLogMonitor.WriteToEventLogInfo( "TxGetResponse has failed and  response != null  && !( (IEnvelope)response ).InPool", 195 );
          response.ReturnEmptyEnvelope();
        }
      request.ReturnEmptyEnvelope();
      m_protocol.GetIProtocolParent.RxDataBlock( res == AL_ReadData_Result.ALRes_Success );
      return res;
    }
    /// <summary>
    /// Gets a buffer from a pool and initiates it. After filling it up with the data can be send to the data provider remote 
    /// unit by the WriteData.
    /// </summary>
    /// <param name="block">Data description allowing to prepare appropriate header of the frame.</param>
    /// <param name="station">Address of the remote station connected to the common field bus. –1 if not applicable.
    /// </param>
    /// <returns>A buffer ready to be filled up with the data and write down to the destination – remote station.
    /// </returns>
    IWriteValue IApplicationLayerMaster.GetEmptyWriteDataBuffor( IBlockDescription block, int station )
    {
      T_ALMessage frame = m_Pool.GetEmptyISesDBuffer();
      frame.PrepareReqWriteValue( block, station );
      return frame;
    }
    /// <summary>
    /// Send values to the data provider
    /// </summary>
    /// <param name="data">
    /// Data to be send. Always null after return. Data buffer must be returned to the pool.
    /// </param>
    /// <param name="retries">Number of retries to wrtie data.</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result IApplicationLayerMaster.WriteData( ref IWriteValue data, byte retries )
    {
      if ( !m_protocol.GetICommunicationLayer.Connected )
        return AL_ReadData_Result.ALRes_DisInd;
      AL_ReadData_Result response = TxGetResponse( (T_ALMessage)data, retries );
      data.ReturnEmptyEnvelope();
      data = null; 
      m_protocol.GetIProtocolParent.TxDataBlock( response == AL_ReadData_Result.ALRes_Success );
      return response;
    }
    #endregion
    #region IDisposable
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <remarks>
    /// Dispose(bool disposing) executes in two distinct scenarios. If disposing equals true, the method has been called directly
    /// or indirectly by a user's code. Managed and unmanaged resources can be disposed. If disposing equals false, 
    /// the method has been called by the runtime from inside the finalizer and you should not reference other objects. 
    /// Only unmanaged resources can be disposed.
    /// </remarks>
    /// <param name="disposing">
    /// If disposing equals true, the method has been called directly or indirectly by a user's code.
    /// </param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing )
      {
        // Release managed resources.
      }
      // Release unmanaged resources.
      // Set large fields to null.
      // Call Dispose on your base class.
      base.Dispose( disposing );
    }
    #endregion
    #region creator
    /// <summary>
    /// ApplicationLayerMaster creator
    /// </summary>
    /// <param name="cProtocol">Protocol to be used to transfer data.</param>
    /// <param name="cPool">Empty frames pool to be used by the protocol.</param>
    public ApplicationLayerMaster( ALProtocol<T_ALMessage> cProtocol, SesDBufferPool<T_ALMessage> cPool )
      : base( cProtocol.GetICommunicationLayer )
    {
      this.m_protocol = cProtocol;
      this.m_Pool = cPool;
      this.m_Statistic = cProtocol.GetIProtocolParent;
      InterfrarmeStopwatch.Start();
    }
    #endregion
  }
}
