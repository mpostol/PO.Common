
//<summary>
//  Title   : C_ListenState
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

namespace CAS.Lib.CommonBus.CommunicationLayer.CAPI.Session
{
  using System;
  using Processes;
  /// <summary>
  /// Class that handles listaning state protocol machine
  /// </summary>
  public class C_ListenState
  {
    public byte CIP;
    public uint info;
    public bool start_listen;
    public enum Listen_Events
    {
      Listen_NONE,
      LISTEN_REQ,
      LISTEN_CONF_INFO_N0,
      LISTEN_CONF_INFO_0_CIP_0,
      LISTEN_CONF_INFO_O_CIP_N0
    };
    public static bool ListenActive = false;
    /// <summary>
    /// Listening protocol machine.
    /// Handles all events concerning listening states.
    /// </summary>
    /// <param name="LEventt"></param>
    public void SM_Listen(Listen_Events LEventt)
    {
      info = (uint) CAPI_Interface.TInterfaceError.GENERAL_ERROR;
      switch ( state )
      {
        case Listen_States.L0 :
          #region L0
        switch (LEventt)
        {
          case C_ListenState.Listen_Events.LISTEN_REQ:
            CAPI.CAPI_Message CAPImsg = new CAPI.CAPI_Message();
            Manager.Assert
              (
              CAPI_Interface.Send_msg_listen_req(ref CAPImsg, 1, start_listen, out info) 
              ); 
            state = Listen_States.L01;
            break;
        };
          break;
        case Listen_States.L01 :
        switch (LEventt)
        {
          case C_ListenState.Listen_Events.LISTEN_CONF_INFO_0_CIP_0 :
          case C_ListenState.Listen_Events.LISTEN_CONF_INFO_N0 :
            state = Listen_States.L0;
            break;
          case C_ListenState.Listen_Events.LISTEN_CONF_INFO_O_CIP_N0 :
            state = Listen_States.L1;
            break;
        };
          break;
          #endregion
        case Listen_States.L1 :
          #region L1
        switch (LEventt )
        {
          case C_ListenState.Listen_Events.LISTEN_REQ:
            CAPI.CAPI_Message CAPImsg = new CAPI.CAPI_Message();
            Manager.Assert
              (
              CAPI_Interface.Send_msg_listen_req(ref CAPImsg, 1, start_listen, out info)
              ); 
            state = Listen_States.L11;
            if (start_listen == true) CIP = 2;
            else CIP = 0;
            break;
        };
          break;
          #endregion
        case Listen_States.L11 :
          #region L11
        switch (LEventt)
        {
          case C_ListenState.Listen_Events.LISTEN_CONF_INFO_0_CIP_0 :
            state = Listen_States.L0;
            break;
          case C_ListenState.Listen_Events.LISTEN_CONF_INFO_N0 :
          case C_ListenState.Listen_Events.LISTEN_CONF_INFO_O_CIP_N0 :
            state = Listen_States.L1;
            break;
        };
          #endregion
          break;
      }
    }
    #region PRIVATE
    private enum Listen_States
    {
      L0,
      L01,
      L1,
      L11
    };
    private static Listen_States state =  Listen_States.L0;
    //private CAPI.CAPI_Message CAPImsg;
    #endregion
  }
}