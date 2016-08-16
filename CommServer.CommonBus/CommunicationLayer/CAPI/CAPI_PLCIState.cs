
//<summary>
//  Title   : C_PLCIState
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

obs³uga wszystkich komunikatów

///////////////////////////////////////////////////////////////////////////////*/
namespace CAS.Lib.CommonBus.CommunicationLayer.CAPI.Session
{
  using System;
  using Processes;
  using CAPI.Protocols;
  /// <summary>
  /// Class that handles physical connection states protocol machine
  /// </summary>
  internal abstract class C_PLCIState : CAPI.Session.C_NCCIState
  {
    #region PRIVATE
    //TYPE
    private enum PLCI_States
    {
      P0WaitingFConnIndication,
      P0,
      P01,
      P1,
      P2,
      P3,
      P4,
      P5,
      P6,
      P_ACT
    }
    //VAR
    private bool issueB3_req = false;
    private PLCI_States PLCIPCurrState = PLCI_States.P0;
    private Condition IsDisconnectedCon = new Condition();
    private string telNum ="    ";
    private CAPI.CAPI_Message lCAPImsg = new CAPI.CAPI_Message();
    private bool accept = false;
    //PROCEDURE
	/// <summary>
	/// Sets the proper state for the protocol machine to proceed passive disconnection
	/// </summary>
	/// <param name="Protocol_Event">Protocol event set to DISCONNECT_RESP on exit</param>
	/// <param name="PLCIPCurrState">Protocol machine state set to wait for disconnection from network</param>
    private void Prcd_DISCONNECT_IND(out CAPI_Wrapper.comsub Protocol_Event, out PLCI_States PLCIPCurrState)
    {
	  
      Protocol_Event = CAPI_Wrapper.comsub.DISCONNECT_RESP;

      PLCIPCurrState = PLCI_States.P6;
    } // Prcd_DISCONNECT_IND
	/// <summary>
	/// Start the physical disconnection procedure
	/// </summary>
	/// <param name="CAPImsg">CAPI message to send DISCONNECT_REQ</param>
	/// <param name="Protocol_Event">Protocol event set to NO_ONE on exit</param>
	/// <param name="PLCIPCurrState">Protocol machine state set to wait for disconnection network</param>
    private void Prcd_DISCONNECT_REQ
      (CAPI.CAPI_Message CAPImsg, out CAPI_Wrapper.comsub Protocol_Event, out PLCI_States PLCIPCurrState)
    {
      Manager.Assert(
        CAPI_Interface.Send_msg_disconnect_req(ref CAPImsg, 1, PLCI, out info)
        );
      Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
      PLCIPCurrState = PLCI_States.P5;    
    } // Prcd_DISCONNECT_REQ
	/// <summary>
	/// Respond to notification of physical connection from netwok
	/// </summary>
	/// <param name="CAPImsg">CAPI message to send CONNECT_ACTIVE_RESP</param>
	/// <param name="Protocol_Event">Protocol event set to CONNECT_B3_REQ on exit</param>
	/// <param name="PLCIPCurrState">Protocol machine state set to connection active on exit</param>
    private void Prcd_CONNECT_ACTIVE_IND
      (CAPI.CAPI_Message CAPImsg, out CAPI_Wrapper.comsub Protocol_Event, out PLCI_States PLCIPCurrState)
    {
      Manager.Assert
        (CAPI_Interface.Send_msg_connect_active_resp(ref CAPImsg, 1, PLCI, out info));
      if (issueB3_req) 
        Protocol_Event = CAPI_Wrapper.comsub.CONNECT_B3_REQ;
      else Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
      PLCIPCurrState = PLCI_States.P_ACT;
    }//Prcd_CONNECT_ACTIVE_IND
	/// <summary>
	/// Responds to notification of incoming call
	/// </summary>
	/// <param name="CAPImsg">CAPI message to send CONNECT_RESP</param>
	/// <param name="Protocol_Event">Protocol event set to NO_ONE on exit</param>
	/// <param name="PLCIPCurrState">Protocol machine state set to wait for connection (if accepted) or disconnection (if rejected)</param>
	/// <param name="ident"></param>
    private void Prcd_CONNECT_RESP
      (
      CAPI.CAPI_Message CAPImsg, 
      out CAPI_Wrapper.comsub Protocol_Event, 
      out PLCI_States PLCIPCurrState,
      short ident)
    {
      Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
      CAPImsg.number = ident;
      Manager.Assert(
        //MPNI implementacja konfiguracji protokolów
        CAPI_Interface.Send_msg_connect_resp
          (
          ref CAPImsg, 1, PLCI, accept, B1_config, 
          B2_config, B3_config, 
          out info
          )
        );  //Manager.Assert
      if (accept) PLCIPCurrState = PLCI_States.P4;
      else        PLCIPCurrState = PLCI_States.P5;
    } //Prcd_CONNECT_RESP
    #endregion
    #region PUBLIC
    internal IB1_Proto B1_config = new Protocols.B1_64kbs_WITH_HDLC_conf();
    internal IB2_Proto B2_config = new Protocols.B2_X75_SLP_conf();
	/// <summary>
	/// Clears the port and sets the starting conditions
	/// </summary>
    internal override void Release()
    {
      lock(this)
      {
        base.Release();
        issueB3_req = false;
        IsDisconnectedCon.NotifyAll();
		
        telNum ="    ";
        PLCIPCurrState = PLCI_States.P0;
		PLCI = 0;
        accept = false;
      }
    }
	/// <summary>
	/// Physical connection protocol machine.
	/// Handles all events concerning physical connection states.
	/// </summary>
	/// <param name="CAPImsg">CAPI message to operate on</param>
	/// <param name="Protocol_Event">Last protocol event that occured during application's activity</param>
    internal void SM_PLCI
      (CAPI.CAPI_Message CAPImsg, CAPI_Wrapper.comsub Protocol_Event)
    {
      lock (this)
      {
        while (Protocol_Event != CAPI_Wrapper.comsub.NO_ONE)
        {
          short packetNum = 0;
          info = (uint) CAPImsg.get_info;
          switch(PLCIPCurrState)
          {
            case PLCI_States.P0WaitingFConnIndication:
            #region P0WaitingFConnIndication
            switch (Protocol_Event)
            {
              case CAPI_Wrapper.comsub.CONNECT_IND:
                accept = true;
                PLCI = CAPImsg.plci;
                packetNum = CAPImsg.number;
                Protocol_Event = CAPI_Wrapper.comsub.CONNECT_RESP;
                PLCIPCurrState = PLCI_States.P2;
                break;
              default:
//                Manager.Assert(false);
                Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                break;
            };
              break;
            #endregion
            case PLCI_States.P0:
            #region P0
            switch (Protocol_Event)
            {
              case CAPI_Wrapper.comsub.INTERNAL_WaitFConnInd:
                PLCI = CAPImsg.plci;
                Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                PLCIPCurrState = PLCI_States.P0WaitingFConnIndication;
                break;
              case CAPI_Wrapper.comsub.CONNECT_IND:
                accept = false;
                PLCI = CAPImsg.plci;
                packetNum = CAPImsg.number;
                Protocol_Event = CAPI_Wrapper.comsub.CONNECT_RESP;
                PLCIPCurrState = PLCI_States.P2;
                break;
              case CAPI_Wrapper.comsub.CONNECT_REQ:
                Manager.Assert(
                  CAPI_Interface.Send_msg_connect_req
                  (
                  ref CAPImsg, 
                  portTelNumber, 
                  1, 
                  CAPI.CAPI_Wrapper.CIP_Value.UNRESTR_DIG_INFO, 
                  B1_config, 
                  B2_config, 
                  B3_config, 
                  (short)(PLCI+Byte.MaxValue),
                  out info)
                  );
                issueB3_req = true;
                Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                PLCIPCurrState  = PLCI_States.P01;
                break;
                // not supported 
                //              case CAPI_Wrapper.comsub.FACILITY_IND_STATE_PLUS:
                //                PLCIPCurrState = PLCI_States.P1;
                //                Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                //                break;
              default:
                Manager.Assert(false);
                Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                break;
            };
              break;
            #endregion
            case PLCI_States.P01:
            #region P01
            switch (Protocol_Event)
            {
              case CAPI_Wrapper.comsub.CONNECT_CONF:
                Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                if (info == 0)
                {
                  PLCIPCurrState = PLCI_States.P1;
                  PLCI = CAPImsg.plci;
                }
                else
                {
                  PLCIPCurrState = PLCI_States.P0;
                  IsDisconnectedCon.Notify();
                }
                break;
                //              case CAPI_Wrapper.comsub.FACILITY_IND_STATE_MINUS:
                //                //MPNI - do zrobienia nie wiem o co chodzi
                //                Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                //                PLCIPCurrState = PLCI_States.P5;
                //                break;
              default:
                Manager.Assert(false);
                break;
            };
              break;
              // PLCI_States.P01
            #endregion
            case PLCI_States.P1:
            #region P1
            switch (Protocol_Event)
            {
              case CAPI_Wrapper.comsub.CONNECT_ACTIVE_IND:
                Prcd_CONNECT_ACTIVE_IND(CAPImsg, out Protocol_Event, out PLCIPCurrState); 
                break;
                //MPNI - do zrobienia nie wiem o co chodzi
                //case CAPI_Wrapper.comsub.FACILITY_IND_STATE_MINUS:
                //  Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                //  PLCIPCurrState = PLCI_States.P5;
                //  break;
              case CAPI_Wrapper.comsub.DISCONNECT_IND:
                Prcd_DISCONNECT_IND(out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.DISCONNECT_REQ:
                Prcd_DISCONNECT_REQ(CAPImsg, out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.INFO_IND:
              case CAPI_Wrapper.comsub.INFO_REQ:
                break;
              default:
                Manager.Assert(false);
                break;
            };
              break;
              //PLCI_States.P1
            #endregion
            case PLCI_States.P2:
            #region P2
            switch (Protocol_Event)
            {
              case CAPI_Wrapper.comsub.ALERT_REQ:
                //MPNI nie zrobi³em z braku wiedzy i czasu
                Manager.Assert(false);
                break;
              case CAPI_Wrapper.comsub.CONNECT_RESP:
                Prcd_CONNECT_RESP(CAPImsg, out Protocol_Event, out PLCIPCurrState, packetNum);
                break;
                //MPNI not supported
                //case CAPI_Wrapper.comsub.FACILITY_IND_STATE_PLUS:
                //  Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                //  PLCIPCurrState = PLCI_States.P3;
                //  break;
                //case CAPI_Wrapper.comsub.FACILITY_IND_STATE_MINUS:
                //MPNI - do zrobienia nie wiem o co chodzi
                //Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                //PLCIPCurrState = PLCI_States.P5;
                //break;
              case CAPI_Wrapper.comsub.DISCONNECT_IND:
                Prcd_DISCONNECT_IND(out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.DISCONNECT_REQ:
                Prcd_DISCONNECT_REQ(CAPImsg, out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.INFO_IND:
              case CAPI_Wrapper.comsub.INFO_REQ:
                break;
              default:
                Manager.Assert(false);
                break;
            };
              break;
              // PLCI_States.P2
            #endregion
            case PLCI_States.P3:
            #region P3
            switch (Protocol_Event)
            {
              case CAPI_Wrapper.comsub.CONNECT_RESP:
                Prcd_CONNECT_RESP(CAPImsg, out Protocol_Event, out PLCIPCurrState, packetNum);
                break;
                //MPNI - do zrobienia nie wiem o co chodzi - zrobiæ metodê wspó³na
                //case CAPI_Wrapper.comsub.FACILITY_IND_STATE_MINUS:
                //  Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                //  PLCIPCurrState = PLCI_States.P5;
                //  break;
              case CAPI_Wrapper.comsub.DISCONNECT_IND:
                Prcd_DISCONNECT_IND(out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.DISCONNECT_REQ:
                Prcd_DISCONNECT_REQ(CAPImsg, out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.INFO_IND:
              case CAPI_Wrapper.comsub.INFO_REQ:
                break;
              default:
                Manager.Assert(false);
                break;
            };
              break;
              //PLCI_States.P3
            #endregion
            case PLCI_States.P4:
            #region p4
            switch (Protocol_Event)
            {
              case CAPI_Wrapper.comsub.CONNECT_ACTIVE_IND:
                Prcd_CONNECT_ACTIVE_IND(CAPImsg, out Protocol_Event, out PLCIPCurrState); 
                break;
                //MPNI - do zrobienia nie wiem o co chodzi
                //case CAPI_Wrapper.comsub.FACILITY_IND_STATE_MINUS:
                //Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                //PLCIPCurrState = PLCI_States.P5;
                //break;
              case CAPI_Wrapper.comsub.DISCONNECT_IND:
                Prcd_DISCONNECT_IND(out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.DISCONNECT_REQ:
                Prcd_DISCONNECT_REQ(CAPImsg, out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.INFO_IND:
              case CAPI_Wrapper.comsub.INFO_REQ:
                break;
              default:
                Manager.Assert(false);
                break;
            };
              break;
              // PLCI_States.P4
            #endregion
            case PLCI_States.P5:
            #region P5
            switch (Protocol_Event)
            {
              case CAPI_Wrapper.comsub.DISCONNECT_CONF:
                Manager.Assert( info == 0 );
                Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                break;
              case CAPI_Wrapper.comsub.DISCONNECT_IND:
                Protocol_Event = CAPI_Wrapper.comsub.DISCONNECT_RESP;
                PLCIPCurrState = PLCI_States.P6;
                break;
              case CAPI_Wrapper.comsub.INFO_IND:
                Manager.Assert(false);
                //MPNI tu coœ trzeba zrobiæ
                break;
                //MPNI - do zrobienia nie wiem o co chodzi
                //case CAPI_Wrapper.comsub.FACILITY_IND_STATE_MINUS:
                //Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                //PLCIPCurrState = PLCI_States.P5;
                //break;
              default:
                Manager.Assert(false);
                break;
            };
              break;
              // PLCI_States.P5
            #endregion
            case PLCI_States.P6:
            #region P6
            switch (Protocol_Event)
            {
              case CAPI_Wrapper.comsub.DISCONNECT_RESP:
                Manager.Assert(CAPI_Interface.Send_msg_disconnect_resp(ref CAPImsg, 1, PLCI, out info));
                Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
                PLCIPCurrState = PLCI_States.P0;
                IsDisconnectedCon.Notify();
                Signal_Disconnected();
                PLCI = 0;
//				NCCI = 0;
                break;
              default:
                Manager.Assert(false);
                break;
            };
              break;
              // PLCI_States.P6
            #endregion
            case PLCI_States.P_ACT:
            #region P_ACT
            switch (Protocol_Event)
            {
                //case CAPI_Wrapper.comsub.FACILITY_IND_STATE_MINUS:
                //MPNI - do zrobienia nie wiem o co chodzi
                //PLCIPCurrState = PLCI_States.P5;
                //break;
                //MPTI tu chyba najpierw trzeba roz³¹czyæ B3
              case CAPI_Wrapper.comsub.DISCONNECT_IND:
                issueB3_req = false;
                Prcd_DISCONNECT_IND(out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.DISCONNECT_REQ:
                Prcd_DISCONNECT_REQ(CAPImsg, out Protocol_Event, out PLCIPCurrState);
                break;
              case CAPI_Wrapper.comsub.INFO_IND:
              case CAPI_Wrapper.comsub.INFO_REQ:
                break;
              default:
                SM_NCCI(ref CAPImsg, ref Protocol_Event);
                break;
            } // switch
              break;
              // PLCI_States.P_ACT
              #endregion
          } // switch
        } //while
      }// lock
    }
    /// <summary>
	/// Physical connection protocol machine, which works on local instance of CAPI message.
	/// Handles all events concerning physical connection states.
	/// </summary>
	/// <param name="Protocol_Event">Last protocol event that occured during application's activity</param>
	internal void SM_PLCI(CAPI_Wrapper.comsub Protocol_Event)
    {
      SM_PLCI(this.lCAPImsg, Protocol_Event);
    } // SM_PLCI
	/// <summary>
	/// Used to determine if party is connected to remote host
	/// </summary>
    internal override bool IsConnected
    {
      get
      {
        lock(this)
        {
          return (base.IsConnected & (PLCIPCurrState==PLCI_States.P_ACT));
        }
      }
    }
	/// <summary>
	/// Waits until parties are connected or until network disconnects the party
	/// </summary>
	/// <returns>True if parties are connected, otherwise false</returns>
	internal override bool IsConnectedWait()
	{
	  lock(this)
	  {
		return base.IsConnectedWait();
	  }
	}
	/// <summary>
	/// Waits until parties are connected, until network disconnects the party or until desired time elapses
	/// </summary>
	/// <param name="timeout">Desired time to wait</param>
	/// <returns>True if parties are connected, otherwise false</returns>
	internal override bool IsConnectedWait(int timeout)
	{
	  lock(this)
	  {
		return base.IsConnectedWait(timeout);
	  }
	}
	/// <summary>
	/// Used to check if parties are disconnected
	/// </summary>
	internal override bool IsDisconnected 
    {
      get
      {
        lock(this)
        {
          return (base.IsDisconnected & 
            (
            (PLCIPCurrState==PLCI_States.P0) |
            (PLCIPCurrState==PLCI_States.P0WaitingFConnIndication) |
            (PLCIPCurrState==PLCI_States.P01)
            ));
        }
      }
    }
	/// <summary>
	/// Waits until parties disconnect or until desired time elapses
	/// </summary>
	/// <param name="timeout">Desired time to wait</param>
	/// <returns>True if connection has been closed, otherwise false</returns>
    internal bool IsDisconnectedWait(int timeout)
    {
      lock(this)
      {
        if ( ! IsDisconnected )
          IsDisconnectedCon.Wait(this, timeout);
        return PLCIPCurrState==PLCI_States.P0;
      }
    }
	/// <summary>
	/// Use to check if application is in state of accepting calls
	/// </summary>
    internal bool IsWaitingFConnIndication
    {
      get 
      {
        lock(this)
        {return PLCIPCurrState==PLCI_States.P0WaitingFConnIndication;}
      }
    }
	/// <summary>
	/// Called party number
	/// </summary>
    internal string portTelNumber
    {
      get
      {
        return telNum;
      }
      set
      {
        telNum = value;
      }
    }
	/// <summary>
	/// Retrieves the data from CAPI message
	/// </summary>
	/// <param name="rxMess">Buffer to get data to</param>
	/// <param name="timeout">Desired tim to wait</param>
	/// <returns>True if data received, otherwise false</returns>
    internal override bool RxDataGet(out ISesDBuffer rxMess, int timeout)
    {
      lock(this)
      {
        return base.RxDataGet(out rxMess, timeout);
      }
    }
	/// <summary>
	/// Adds data to queue
	/// </summary>
	/// <param name="txMess">Buffer with data to send</param>
	/// <returns>True if success, otherwise false</returns>
    internal override bool TxDataSet(ISesDBuffer txMess)
    {
      lock(this)
      {
        return base.TxDataSet(txMess);
      }
    }
    #endregion
  } // class C_PLCIState  
} // namespace CAPI.Session
