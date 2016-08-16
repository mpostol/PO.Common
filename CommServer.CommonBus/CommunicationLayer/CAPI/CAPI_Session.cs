
//<summary>
//  Title   : CAPI_Session
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>
namespace CAS.Lib.CommonBus.CommunicationLayer.CAPI
{
  using System;
  using Processes;
  using CAPI.Session;
  using CAPI.Protocols;
  public class CAPI_Session
  {
    #region PUBLIC
    /// <summary>
    /// Return code
    /// </summary>
    public enum TSessionError
    {
      Success,
      Session_NotRegistered,
      Listen_ListenActive,
      ConnectReq_Failure,
      DataInd_NoDat,
      DisconnectInd_StillConnected,
      DisconnectReq_Failure,
      NotConnected
    }
    /// <summary>
    /// Starts or stops listening procedure
    /// </summary>
    /// <param name="start">Set true to start listening, othewise set false</param>
    /// <returns>Success if signalling was enabled, otherwise appropriate return code</returns>
    public TSessionError ListenReq( bool start )
    {
      if ( !CAPI_Interface.IsRegistered )
      {
        return TSessionError.Session_NotRegistered;
      };
      if ( C_ListenState.ListenActive )
      {
        return TSessionError.Listen_ListenActive;
      };
      ListenState.start_listen = start;
      ListenState.SM_Listen( C_ListenState.Listen_Events.LISTEN_REQ );
      return TSessionError.Success;
    }
    /// <summary>
    /// Requests for connection
    /// </summary>
    /// <param name="portID">Class that represents the logical connection</param>
    /// <param name="telNum">Called party number</param>
    /// <param name="B1_conf">B1 protocol configuration</param>
    /// <param name="B2_conf">B2 protocol configuration</param>
    /// <param name="B3_conf">B3 protocol configuration</param>
    /// <param name="userDBufferPool">Buffer pool for data</param>
    /// <param name="timeout">timeout</param>
    /// <returns>Success if parties are connected, otherwise appropriate error code</returns>
    public TSessionError ConnectReq
      (
      out object portID,
      string telNum,
      IB1_Proto B1_conf,
      IB2_Proto B2_conf,
      IB3_Proto B3_conf,
      SesDBufferPool userDBufferPool,
      int timeout
      )
    {
      portID = null;
      if ( !CAPI_Interface.IsRegistered )
        return TSessionError.Session_NotRegistered;
      C_PLCIState port = Connections.NewPLCIPort();
      portID = port;
      if ( port == null )
        return TSessionError.ConnectReq_Failure;
      port.portTelNumber = telNum;
      port.B1_config = B1_conf;
      port.B2_config = B2_conf;
      port.B3_config = B3_conf;
      port.userDBufferPool = userDBufferPool;
      port.SM_PLCI( CAPI_Wrapper.comsub.CONNECT_REQ );
      if ( port.IsConnectedWait( timeout ) )
        return TSessionError.Success;
      else
        //  port.Release();
        Connections.ClosePLCIPort( ref port );
      return TSessionError.ConnectReq_Failure;
    }
    /// <summary>
    /// Waits for incoming connection
    /// </summary>
    /// <param name="portID">Class that represents the logical connection</param>
    /// <param name="timeout">Desired time to wait</param>
    /// <param name="B1_conf">B1 protocol configuration</param>
    /// <param name="B2_conf">B2 protocol configuration</param>
    /// <param name="B3_conf">B3 protocol configuration</param>
    /// <param name="userDBufferPool">Buffer pool for data</param>
    /// <returns>Success if parties are connected, otherwise appropriate error code</returns>
    public TSessionError ConnectInd
      ( out object portID, int timeout, IB1_Proto B1_conf, IB2_Proto B2_conf, IB3_Proto B3_conf, SesDBufferPool userDBufferPool )
    {
      portID = null;
      if ( !CAPI_Interface.IsRegistered )
        return TSessionError.Session_NotRegistered;
      //MPNI to jest zle - trzeba szukaæ czy ind nie pojawi³o siê wczeœniej
      C_PLCIState port = Connections.NewPLCIPort();
      portID = port;
      if ( port == null )
        return TSessionError.ConnectReq_Failure;
      port.B1_config = B1_conf;
      port.B2_config = B2_conf;
      port.B3_config = B3_conf;
      port.userDBufferPool = userDBufferPool;
      port.SM_PLCI( CAPI_Wrapper.comsub.INTERNAL_WaitFConnInd );
      if ( port.IsConnectedWait( timeout ) )
        return TSessionError.Success;
      else
      {
        //port.Release();
        Connections.ClosePLCIPort( ref port );
        return TSessionError.ConnectReq_Failure;
      }
    }
    public TSessionError DataReq( object portID, ISesDBuffer tXmsg )
    {
      if ( !CAPI_Interface.IsRegistered )
        return TSessionError.Session_NotRegistered;
      if ( portID == null )
        return TSessionError.NotConnected;
      C_PLCIState port = (C_PLCIState)portID;
      if ( !port.IsConnected )
        return TSessionError.NotConnected;
      if ( port.TxDataSet( tXmsg ) )
        port.SM_PLCI( CAPI_Wrapper.comsub.DATA_B3_REQ );
      else
        return TSessionError.NotConnected;
      return TSessionError.Success;
    }
    /// <summary>
    /// Waits for incomming data
    /// </summary>
    /// <param name="portID">Class that represents the logical connection</param>
    /// <param name="rXMsg">Buffer for received data</param>
    /// <param name="timeout">Desired time to wait</param>
    /// <returns>Success if data was received, otherwise appropriate error code</returns>
    public TSessionError DataInd
      ( object portID, out ISesDBuffer rXMsg, int timeout )
    {
      rXMsg = null;
      if ( !CAPI_Interface.IsRegistered )
        return TSessionError.Session_NotRegistered;
      if ( portID == null )
        return TSessionError.NotConnected;
      ;
      C_PLCIState port = (C_PLCIState)portID;
      bool isData = port.RxDataGet( out rXMsg, timeout );
      if ( isData )
        return TSessionError.Success;
      else
      {
        if ( !port.IsConnected )
          return TSessionError.NotConnected;
        else
          return TSessionError.DataInd_NoDat;
      }
    }
    public TSessionError ClearData( object portID )
    {
      ISesDBuffer rXMsg;
      if ( !CAPI_Interface.IsRegistered )
        return TSessionError.Session_NotRegistered;
      if ( portID == null )
        return TSessionError.NotConnected;
      ;
      C_PLCIState port = (C_PLCIState)portID;
      while ( port.RxDataGet( out rXMsg, 10 ) )
      {
        rXMsg = null;
      }
      return TSessionError.Success;
    }
    /// <summary>
    /// Disconnects both parties and clears the connection
    /// </summary>
    /// <param name="portID">Class that represents the logical connection</param>
    /// <param name="timeout">Desired time to wait</param>
    /// <returns>Success if parties are disconnected, otherwise appropriate error code</returns>
    public TSessionError DisconnectReq( ref object portID, int timeout )
    {
      if ( !CAPI_Interface.IsRegistered )
        return TSessionError.Session_NotRegistered;
      if ( portID == null )
        return TSessionError.NotConnected;
      ;
      C_PLCIState port = (C_PLCIState)portID;
      if ( !port.IsConnected )
        return TSessionError.NotConnected;
      port.SM_PLCI( CAPI_Wrapper.comsub.DISCONNECT_B3_REQ );
      if ( port.IsDisconnectedWait( timeout ) )
      {
        Connections.ClosePLCIPort( ref port );
        //port.Release();
        return TSessionError.Success;
      }
      else
        return TSessionError.DisconnectInd_StillConnected;
    }
    /// <summary>
    /// Cleans up port after connection was closed
    /// </summary>
    /// <param name="portID">Class that represents the logical connection</param>
    /// <param name="timeout">Desired time to wait</param>
    /// <returns>Success if port was cleared, otherwise appropriate error code</returns>
    public TSessionError DisconnectInd( object portID, int timeout )
    {
      if ( !CAPI_Interface.IsRegistered )
        return TSessionError.Session_NotRegistered;
      if ( portID == null )
        return TSessionError.NotConnected;
      ;
      C_PLCIState port = (C_PLCIState)portID;
      if ( port.IsDisconnectedWait( timeout ) )
      {
        Connections.ClosePLCIPort( ref port );
        //  port.Release();
        return TSessionError.Success;
      }
      else
        return TSessionError.DisconnectInd_StillConnected;
    }
    /// <summary>
    /// Stops the protocol machine and unregisters application from CAPI
    /// </summary>
    public void Close()
    {
      T_ReceiverWork = false;
      T_Receiver.Join( 10000 );
      CAPI_Interface.Unregister();
    }
    public CAPI_Session()
    {
      CAPI_Interface.Register();
      ListenState = new C_ListenState();
      T_Receiver = Processes.Manager.StartProcess( new System.Threading.ThreadStart( Receiver ) );
    }
    ~CAPI_Session()
    {
      T_ReceiverWork = false;
      T_Receiver.Join( 10000 );
      CAPI_Interface.Unregister();
    }
    #endregion
    #region PRIVATE
    /// <summary>
    /// Variable that stops the receiver thread
    /// </summary>
    private bool T_ReceiverWork = false;
    private System.Threading.Thread T_Receiver;
    private uint info;
    private C_ListenState ListenState;
    private enum TStateM
    {
      SM_no,
      SM_listen,
      SM_plci,
    }
    /// <summary>
    /// Main thread that handles protocol machines
    /// </summary>
    private void Receiver()
    {
      T_ReceiverWork = true;
      TStateM callSM = TStateM.SM_no;
      C_ListenState.Listen_Events Listen_LastEv;
      CAPI_Wrapper.comsub lastFrameCmd;
      CAPI.CAPI_Message CAPImsg = new CAPI.CAPI_Message();
      C_PLCIState port;
      while ( T_ReceiverWork )
      {
        Listen_LastEv = C_ListenState.Listen_Events.Listen_NONE;
        while ( true )
        {
          if ( !T_ReceiverWork )
            return;
          CAPI_Interface.Get_msg( ref CAPImsg, out lastFrameCmd, out info );
          if ( lastFrameCmd == CAPI_Interface.comsub.NO_ONE )
            Processes.Timer.Wait( Processes.Timer.TInOneSecond / 20 );
          else
            break;
        };
        port = null;
        switch ( lastFrameCmd )
        {
          case CAPI_Interface.comsub.LISTEN_CONF:
            callSM = TStateM.SM_listen;
            if ( info == 0 )
              if ( ListenState.start_listen == false )
                Listen_LastEv = C_ListenState.Listen_Events.LISTEN_CONF_INFO_0_CIP_0;
              else
                Listen_LastEv = C_ListenState.Listen_Events.LISTEN_CONF_INFO_O_CIP_N0;
            else
              Listen_LastEv = C_ListenState.Listen_Events.LISTEN_CONF_INFO_N0;
            callSM = TStateM.SM_listen;
            break;
          case CAPI_Interface.comsub.ALERT_CONF:
            // nie uwzglêdniony w SM
            callSM = TStateM.SM_no;
            break;
          case CAPI_Interface.comsub.CONNECT_IND:
            port = Connections.FindWaitingFConnIndPort();
            callSM = TStateM.SM_plci;
            break;
          case CAPI_Interface.comsub.CONNECT_CONF:
            port = Connections.FindPortNum( CAPImsg.number );
            callSM = TStateM.SM_plci;
            break;
          case CAPI_Interface.comsub.CONNECT_ACTIVE_IND:
          case CAPI_Interface.comsub.CONNECT_B3_IND:
          case CAPI_Interface.comsub.CONNECT_B3_ACTIVE_IND:
          case CAPI_Interface.comsub.CONNECT_B3_CONF:
          case CAPI_Interface.comsub.DATA_B3_IND:
          case CAPI_Interface.comsub.DATA_B3_CONF:
          case CAPI_Interface.comsub.DISCONNECT_B3_IND:
          case CAPI_Interface.comsub.DISCONNECT_B3_CONF:
          case CAPI_Interface.comsub.RESET_B3_CONF://MPNI event nie obs³ugiwany przez SM
          case CAPI_Interface.comsub.RESET_B3_IND:
          case CAPI_Interface.comsub.DISCONNECT_IND:
          case CAPI_Interface.comsub.DISCONNECT_CONF:
          case CAPI_Interface.comsub.INFO_IND:
          case CAPI_Interface.comsub.INFO_CONF:   //MPNI  event nie obs³ugiwany przez SM
            port = Connections.FindPLCIPort( (byte)CAPImsg.plci );
            callSM = TStateM.SM_plci;
            break;
          default:
            Manager.Assert( false );
            break;
        } //do switch (CAPI......
        switch ( callSM )
        {
          case TStateM.SM_no:
            break;
          case TStateM.SM_listen:
            ListenState.SM_Listen( Listen_LastEv );
            break;
          case TStateM.SM_plci:
            port.SM_PLCI( CAPImsg, lastFrameCmd );
            break;
        }
      }
    }
    private CAPI_PLCIPortManager Connections = new CAPI_PLCIPortManager();
    #endregion
  }
}
