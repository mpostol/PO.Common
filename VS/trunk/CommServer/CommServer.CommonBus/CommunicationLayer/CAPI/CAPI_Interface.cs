
//<summary>
//  Title   : CAPI_Interface
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

/*///////////////////////////////////////////////////////////////////////////////

konfiguracja kana³ów w connect req
obs³uga wszystkich komunikatów INFO
uporz¹dkowaæ zwracanie konkretnych b³êdów : connect_b3_t90_active_resp; facility_req; facility_resp; info_req; info_resp;
                      manufacturer_req; manufacturer_resp; select_b3_protocol_req.

///////////////////////////////////////////////////////////////////////////////*/
namespace CAS.Lib.CommonBus.CommunicationLayer.CAPI
{
  using System;
  using System.Runtime.InteropServices;
  using CAPI.Protocols;  /// <summary>
  /// Class that handles all direct calls to CAPI functions
  /// </summary>
  public abstract class CAPI_Interface: CAPI_Wrapper
  {
    #region Private
    /// <summary>
    /// Appliction ID designated by CAPI
    /// </summary>
    private static uint app_ID = 0;
    /// <summary>
    /// Maximum number of logical connections that the application can maintain concurrently
    /// </summary>
    private static uint maxLogicalConnections = 4;
    /// <summary>
    /// Amount of memory required for message buffer
    /// </summary>
    private static uint MessageBufferSize = 1024 + ( 1024 * maxLogicalConnections );
    /// <summary>
    /// Maximum number of received data blocks that can be reported to the application for each logical connection
    /// </summary>
    private static uint maxBDataBlocks = 4;
    /// <summary>
    /// Maximum size of the application data block to be transmitted or received
    /// </summary>
    private static uint maxBDataLen = 300;
    /// <summary>
    /// True if application is registered to CAPI, otherwise false
    /// </summary>
    private static bool registered = false;
    #endregion
    #region send messages functions
    /// <summary>
    /// Sets all the parameters and sends DATA_B3_REQ message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="NCCI">Network Control Connection Identifier</param>
    /// <param name="dane">Data to send</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_data_b3_req( ref CAPI_Message CAPImsg, byte controller, byte PLCI, short NCCI, Processes.IDBuffer dane, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.DATA_B3_REQ;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = NCCI;
      CAPImsg.length = 22;
      CAPImsg.WriteIntPtr( dane.uMessagePtr );
      CAPImsg.WriteInt16( (short)dane.userDataLength );
      CAPImsg.WriteInt16( 0 );
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends DATA_B3_RESP message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="NCCI">Network Control Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_data_b3_resp( ref CAPI_Message CAPImsg, byte controller, byte PLCI, short NCCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.DATA_B3_RESP;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = NCCI;
      CAPImsg.length = 14;
      CAPImsg.WriteInt16( 0 );
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends CONNECT_REQ message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="tel">Called party number</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="CIP">Compatibilty Information Profile</param>
    /// <param name="B1">B1 protocol configuration</param>
    /// <param name="B2">B2 protocol configuration</param>
    /// <param name="B3">B3 protocol configuration</param>
    /// <param name="connect_ident">Any number to distinguish from connections</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_connect_req( ref CAPI_Message CAPImsg, string tel, byte controller, CIP_Value CIP, IB1_Proto B1, IB2_Proto B2, IB3_Proto B3, short connect_ident, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.CONNECT_REQ;
      CAPImsg.controller = controller;
      CAPImsg.number = connect_ident;
      CAPImsg.plci = 0;
      CAPImsg.ncci = 0;
      CAPImsg.WriteInt16( (short)CIP );
      CAPImsg.WriteTelNum( tel );
      CAPImsg.WriteByte( 0 );                                 //calling part no
      CAPImsg.WriteByte( 0 );                                 //called party subadd
      CAPImsg.WriteByte( 0 );                                 //calling party subadd
      CAPImsg.Start_block();
      CAPImsg.WriteInt16( (short)B1.B1 );
      CAPImsg.WriteInt16( (short)B2.B2 );
      CAPImsg.WriteInt16( (short)B3.B3 );
      B1.Set_param( CAPImsg );
      B2.Set_param( CAPImsg );
      B3.Set_param( CAPImsg );
      CAPImsg.End_block();
      //BC
      CAPImsg.WriteByte( 0 );
      //LLC
      CAPImsg.WriteByte( 0 );
      //HLC
      CAPImsg.WriteByte( 0 );
      CAPImsg.Start_block();
      CAPImsg.Start_block();								//B channel info
      CAPImsg.WriteInt16( 0 );
      CAPImsg.End_block();
      CAPImsg.WriteInt16( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.End_block();
      CAPImsg.length = (short)( ( CAPImsg.offset ) );
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends CONNECT_RESP message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="accept">Set true to accept connection, otherwise set false</param>
    /// <param name="B1">B1 protocol configuration</param>
    /// <param name="B2">B2 protocol configuration</param>
    /// <param name="B3">B3 protocol configuration</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_connect_resp( ref CAPI_Message CAPImsg, byte controller, byte PLCI, bool accept, IB1_Proto B1, IB2_Proto B2, IB3_Proto B3, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.CONNECT_RESP;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = 0;
      if ( accept )
        CAPImsg.WriteInt16( 0 );
      else
        CAPImsg.WriteInt16( 2 );
      ushort len_pos = CAPImsg.offset;
      CAPImsg.offset++;
      CAPImsg.WriteInt16( (short)B1.B1 );
      CAPImsg.WriteInt16( (short)B2.B2 );
      CAPImsg.WriteInt16( (short)B3.B3 );
      CAPImsg.WriteByte( 0 );                                 //b1 conf default
      CAPImsg.WriteByte( 0 );                                 //b2 conf default
      CAPImsg.WriteByte( 0 );                                 //b3 conf defautl
      ushort act_pos = CAPImsg.offset;
      CAPImsg.offset = len_pos;
      CAPImsg.WriteByte( (byte)( act_pos - len_pos ) );
      CAPImsg.offset = act_pos;
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.length = 28;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends CONNECT_B3_REQ message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_connect_b3_req( ref CAPI_Message CAPImsg, byte controller, byte PLCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.CONNECT_B3_REQ;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = 0;
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.WriteByte( 0 );
      CAPImsg.length = 13;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends CONNECT_B3_RESP message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="NCCI">Network Control Connection Identifier</param>
    /// <param name="accept">Set true to accept connection, otherwise set false</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_connect_b3_resp( ref CAPI_Message CAPImsg, byte controller, byte PLCI, short NCCI, bool accept, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.CONNECT_B3_RESP;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = NCCI;
      if ( accept )
        CAPImsg.WriteInt16( 0 );
      else
        CAPImsg.WriteInt16( 2 );
      CAPImsg.length = 14;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends DISCONNECT_B3_REQ message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="NCCI">Network Control Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_disconnect_b3_req( ref CAPI_Message CAPImsg, byte controller, byte PLCI, short NCCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.DISCONNECT_B3_REQ;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = NCCI;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends DISCONNECT_B3_RESP message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="NCCI">Network Control Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_disconnect_b3_resp( ref CAPI_Message CAPImsg, byte controller, byte PLCI, short NCCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.DISCONNECT_B3_RESP;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = NCCI;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends DISCONNECT_REQ message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_disconnect_req( ref CAPI_Message CAPImsg, byte controller, byte PLCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.DISCONNECT_REQ;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = 0;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends DISCONNECT_RESP message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_disconnect_resp( ref CAPI_Message CAPImsg, byte controller, byte PLCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.DISCONNECT_RESP;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = 0;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends RESET_B3_REQ message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="NCCI">Network Control Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_reset_b3_req( ref CAPI_Message CAPImsg, byte controller, byte PLCI, short NCCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.RESET_B3_REQ;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = NCCI;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends RESET_B3_RESP message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="NCCI">Network Control Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_reset_b3_resp( ref CAPI_Message CAPImsg, byte controller, byte PLCI, short NCCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.RESET_B3_RESP;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = NCCI;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends INFO_RESP message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_info_resp( ref CAPI_Message CAPImsg, byte controller, byte PLCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.INFO_RESP;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = 0;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends LISTEN_REQ message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="start_listen">True to start listening, false to stop signalling</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_listen_req( ref CAPI_Message CAPImsg, byte controller, bool start_listen, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.LISTEN_REQ;
      CAPImsg.controller = controller;
      CAPImsg.plci = 0;
      CAPImsg.ncci = 0;
      CAPImsg.WriteInt32( 0 );
      if ( start_listen )
        CAPImsg.WriteInt32( (int)CIP_Mask.UNRESTR_DIG_INFO );
      else
        CAPImsg.WriteInt32( (int)CIP_Mask.DISABLE_SIGNALING );
      CAPImsg.WriteInt32( 0 );
      CAPImsg.WriteInt16( 0 );
      CAPImsg.length = (short)( ( CAPImsg.offset ) );
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends ALERT_REQ message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_alert_req( ref CAPI_Message CAPImsg, byte controller, byte PLCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.ALERT_REQ;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = 0;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends ACTIVE_RESP message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_connect_active_resp( ref CAPI_Message CAPImsg, byte controller, byte PLCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.CONNECT_ACTIVE_RESP;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = 0;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    /// <summary>
    /// Sets all the parameters and sends ACTIVE_B3_RESP message to CAPI message queue
    /// </summary>
    /// <param name="CAPImsg">Message to send</param>
    /// <param name="controller">Number of controller</param>
    /// <param name="PLCI">Physical Link Connection Identifier</param>
    /// <param name="NCCI">Network Control Connection Identifier</param>
    /// <param name="info">Result code</param>
    /// <returns>True if success, otherwise false</returns>
    public static bool Send_msg_connect_b3_active_resp( ref CAPI_Message CAPImsg, byte controller, byte PLCI, short NCCI, out uint info )
    {
      CAPImsg.reset_offset();
      CAPImsg.appID = (short)app_ID;
      CAPImsg.sub_com = comsub.CONNECT_ACTIVE_RESP;
      CAPImsg.controller = controller;
      CAPImsg.plci = PLCI;
      CAPImsg.ncci = NCCI;
      CAPImsg.length = 12;
      TInterfaceError ret = CAPI_PUT_MESSAGE( app_ID, CAPImsg.uMessagePtr );
      info = (uint)ret;
      return ( ret == TInterfaceError.SUCCESS );
    }
    #endregion
    /// <summary>
    /// Retrieves the message from message queue
    /// </summary>
    /// <param name="CAPImsg">Retrieved message</param>
    /// <param name="comm">Command and subcommand of last message</param>
    /// <param name="info">Return code</param>
    public static void Get_msg( ref CAPI_Message CAPImsg, out comsub comm, out uint info )
    {
      comm = comsub.NO_ONE;
      IntPtr CAPImsgPtr = (IntPtr)null;
      info = (uint)CAPI_GET_MESSAGE( app_ID, ref CAPImsgPtr );
      if ( info != (ushort)TInterfaceError.SUCCESS )
        return;
      CAPImsg.CopyToBuffor( CAPImsgPtr, (uint)Marshal.ReadInt16( CAPImsgPtr ) );
      comm = CAPImsg.sub_com;
    }
    /// <summary>
    /// Registers application to CAPI and sets the application's ID
    /// </summary>
    /// <returns>True if success, otherwise false</returns>
    public static bool Register()
    {
      if ( CAPI_REGISTER( MessageBufferSize, maxLogicalConnections, maxBDataBlocks, maxBDataLen, ref app_ID ) == CAPI.CAPI_Interface.TInterfaceError.SUCCESS )
        registered = true;
      return registered;
    }
    /// <summary>
    /// Unregisters the application from CAPI
    /// </summary>
    /// <returns>True if success, otherwise false</returns>
    public static bool Unregister()
    {
      if ( CAPI_RELEASE( app_ID ) == CAPI.CAPI_Interface.TInterfaceError.SUCCESS )
        registered = false;
      return registered;
    }
    /// <summary>
    /// Returns true if application is registered, other returns false
    /// </summary>
    public static bool IsRegistered
    {
      get { return registered; }
    }
  }
}
