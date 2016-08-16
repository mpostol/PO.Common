//<summary>
//  Title   : C_NCCIState
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
nie wiem jak opisaæ calling monitor
--------||--------- TxDataRelease
--------||--------- TxDataClear

///////////////////////////////////////////////////////////////////////////////*/
namespace CAS.Lib.CommonBus.CommunicationLayer.CAPI.Session
{
  using System;
  using Processes;
  using CAPI.Protocols;  /// <summary>
  /// Class that handles logical connection states protocol machine
  /// </summary>
  internal abstract class C_NCCIState
  {
    #region PRIVATE	
    private enum NCCI_States
    {
      N0,
      N01,
      N1,
      N2,
      N3,
      N4,
      N5,
      N_ACT
    };
    private const int TxDateSendTO = 100;
    private class TxMessQueue
    {
      private ISesDBuffer buff_TxMess = null;
      private bool isConnected = false;
      private Condition buff_TxMessFree = new Condition();
      internal void redyToSend()
      {
        isConnected = true;
      }
      /// <summary>
      /// Adds data to the send data queue
      /// </summary>
      /// <param name="callingMonitor"></param>
      /// <param name="txMess">Data buffer to send</param>
      /// <returns>True if success, otherwise false</returns>
      internal bool TxDataSet(object callingMonitor, ISesDBuffer txMess)
      {
        bool res = true;
        while (buff_TxMess != null) res = buff_TxMessFree.Wait(callingMonitor, TxDateSendTO);
        res = res & isConnected;
        if (res) buff_TxMess = txMess;
        else
          txMess.ReturnEmptyEnvelope();
        return res;
      }
      /// <summary>
      /// 
      /// </summary>
      internal void TxDataRelease()
      {
        buff_TxMess.ReturnEmptyEnvelope();
        buff_TxMess = null;
        buff_TxMessFree.Notify(); 
      }
      /// <summary>
      /// 
      /// </summary>
      internal void TxDataClear()
      {
        isConnected = false;
        if (buff_TxMess != null) TxDataRelease();
      }
      /// <summary>
      /// Returns data buffer
      /// </summary>
      internal ISesDBuffer TxDataGet
      {
        get { return buff_TxMess; }
      }
    }
    private SesDBufferPool myDBufferPool = null;
    private Processes.Port buff_RxMess = new Processes.Port();
    private TxMessQueue    buff_TxMess = new TxMessQueue();
    private short NCCI = 0;
    private bool Disconnect_B3_Pending = false;
    private bool accept = true;
    private NCCI_States NCCIPCurrState = NCCI_States.N0;
    private Condition IsConnectedCon = new Condition();
    #endregion
    #region PUBLIC
    internal IB3_Proto B3_config = new Protocols.B3_X25_DTE_DTE_conf();
    internal uint info;
    internal byte PLCI = 0;
    internal void Signal_Disconnected()
    {
      IsConnectedCon.Notify();
    }
    internal SesDBufferPool userDBufferPool
    {
      set { myDBufferPool = value;} 
    }
    /// <summary>
    /// Clears the port and sets the starting conditions
    /// </summary>
    internal virtual void Release()
    {
      myDBufferPool = null;
      buff_RxMess.Clear();
      buff_RxMess.Open();
      buff_TxMess.TxDataClear();
      NCCI = 0;
      Disconnect_B3_Pending = false;
      accept = true;
      NCCIPCurrState = NCCI_States.N0;
      IsConnectedCon.NotifyAll();
      B3_config = null;
	  
    }
    /// <summary>
    /// Retrieves data from the data queue
    /// </summary>
    /// <param name="rxMess">Buffer for the data</param>
    /// <param name="timeout">Desired time to wait</param>
    /// <returns></returns>
    internal virtual bool RxDataGet(out ISesDBuffer rxMess, int timeout)
    {
      IEnvelope mess = null;
      bool ret = true;
      if ((buff_RxMess.Count == 0) & !IsConnected) ret = false;
      else buff_RxMess.WaitMsg(this, out mess, timeout);
      if (mess == null) ret = false;
      rxMess = (ISesDBuffer)mess;
      return ret;
    }
    /// <summary>
    /// Adds data to the send data queue
    /// </summary>
    /// <param name="txMess">Data buffer to send</param>
    /// <returns>True if success, otherwise false</returns>
    internal virtual bool TxDataSet(ISesDBuffer txMess)
    {
      return (buff_TxMess.TxDataSet(this, txMess) & IsConnected);
    }
    /// <summary>
    /// Used to determine if party is connected to remote host
    /// </summary>
    internal virtual bool IsConnected
    {
      get
      {
        return NCCIPCurrState==NCCI_States.N_ACT;
      }
    }
    /// <summary>
    /// Waits until parties are connected or until network disconnects the party
    /// </summary>
    /// <returns>True if parties are connected, otherwise false</returns>
    internal virtual bool IsConnectedWait()
    {
      if (! IsConnected ) IsConnectedCon.Wait(this);
      return IsConnected;
    }
    /// <summary>
    /// Waits until parties are connected, until network disconnects the party or until desired time elapses
    /// </summary>
    /// <param name="timeout">Desired time to wait</param>
    /// <returns>True if parties are connected, otherwise false</returns>
    internal virtual bool IsConnectedWait(int timeout)
    {
      if (! IsConnected ) IsConnectedCon.Wait(this, timeout);
      return IsConnected;
    }
    /// <summary>
    /// Used to check if parties are disconnected
    /// </summary>
    internal virtual bool IsDisconnected
    {
      get
      {
        return NCCIPCurrState==NCCI_States.N0;
      }
    }
    #endregion
    #region PROTECTED
    /// <summary>
    /// Logical connection protocol machine.
    /// Handles all events concerning logical connection states.
    /// </summary>
    /// <param name="CAPImsg">CAPI message to operate on</param>
    /// <param name="Protocol_Event">Last protocol event that occured during application's activity</param>
    protected void SM_NCCI(ref CAPI.CAPI_Message CAPImsg, ref CAPI_Wrapper.comsub Protocol_Event)
    {
      //zmienna zwi¹zana z obslug¹ odrzuconego DISCONNECT_B3_REQ
      NCCI_States previousState=NCCI_States.N_ACT;
		
      switch (NCCIPCurrState)
      {
        case NCCI_States.N0:
          #region N0
        switch (Protocol_Event)
        {
          case CAPI_Wrapper.comsub.CONNECT_B3_REQ:
            #region CONNECT_B3_REQ
            Manager.Assert(CAPI_Interface.Send_msg_connect_b3_req(ref CAPImsg, 1, PLCI, out info));
            NCCIPCurrState = NCCI_States.N01;
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            break;
            #endregion
          case CAPI_Wrapper.comsub.CONNECT_B3_IND:
            #region CONNECT_B3_IND
            NCCI = CAPImsg.ncci;
            accept = true;
            //MPNI tu trzeba zrobic sprawdzenie czy oczekujemy na konkretny protokol
            Disconnect_B3_Pending = false;
            NCCIPCurrState = NCCI_States.N1;
            Protocol_Event = CAPI_Wrapper.comsub.CONNECT_B3_RESP;
            break;
            #endregion
          default:
            Manager.Assert(false);
            break;
        };
          break;
          #endregion
        case NCCI_States.N01:
          #region N01
        switch (Protocol_Event)
        {
          case CAPI_Wrapper.comsub.CONNECT_B3_CONF:
            if (info != 0)
            {
              NCCIPCurrState = NCCI_States.N0;
            }
            else 
            {
              NCCIPCurrState = NCCI_States.N2;
              NCCI = CAPImsg.ncci;
            }
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            break;
          default:
            Manager.Assert(false);
            break;
        };
          break;
          #endregion
        case NCCI_States.N1:
          #region N1
        switch (Protocol_Event)
        {
          case CAPI_Wrapper.comsub.CONNECT_B3_RESP:
            #region CONNECT_B3_RESP
            Manager.Assert(
              CAPI_Interface.Send_msg_connect_b3_resp(
              ref CAPImsg, 1, PLCI, NCCI, accept, out info));
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            if (accept) NCCIPCurrState = NCCI_States.N2;
            else        NCCIPCurrState = NCCI_States.N4;
            break;
            #endregion
            #region N1_DISCONNECT
            // wspolne dla N-1, N-2, N-3, N-ACT
          case CAPI_Wrapper.comsub.DISCONNECT_B3_REQ:
            Manager.Assert(false);
            break;
          case CAPI_Wrapper.comsub.DISCONNECT_B3_IND:
            NCCIPCurrState = NCCI_States.N5;
            Protocol_Event = CAPI_Wrapper.comsub.DISCONNECT_B3_RESP;
            break;
            #endregion
          default:
            Manager.Assert(false);
            break;
        };
          break;
          #endregion
        case NCCI_States.N2:
          #region N2
        switch (Protocol_Event)
        {
          case CAPI_Wrapper.comsub.CONNECT_B3_ACTIVE_IND:
            Manager.Assert(CAPI_Interface.Send_msg_connect_b3_active_resp(ref CAPImsg, 1, PLCI, NCCI, out info));
            NCCIPCurrState = NCCI_States.N_ACT;
            buff_TxMess.redyToSend();
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
            IsConnectedCon.Notify();
            break;
            // wspolne dla N-1, N-2, N-3, N-ACT
          case CAPI_Wrapper.comsub.DISCONNECT_B3_REQ:
            Manager.Assert(false);
            break;
          case CAPI_Wrapper.comsub.DISCONNECT_B3_IND:
            NCCIPCurrState = NCCI_States.N5;
            Protocol_Event = CAPI_Wrapper.comsub.DISCONNECT_B3_RESP;
            break;
          default:
            Manager.Assert(false);
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            break;
        };
          break;
          #endregion
        case NCCI_States.N3:
          #region N3
        switch (Protocol_Event)
        {
          case CAPI_Wrapper.comsub.RESET_B3_IND:
            //MPNI - TU TRZEBA WYS£AÆ res
            NCCIPCurrState = NCCI_States.N_ACT;
            buff_TxMess.redyToSend();
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
            IsConnectedCon.Notify();
            break;
            #region N3_DISCONNECT
            // wspolne dla N-1, N-2, N-3, N-ACT
          case CAPI_Wrapper.comsub.DISCONNECT_B3_REQ:
            Manager.Assert(false);
            break;
          case CAPI_Wrapper.comsub.DISCONNECT_B3_IND:
            NCCIPCurrState = NCCI_States.N5;
            Protocol_Event = CAPI_Wrapper.comsub.DISCONNECT_B3_RESP;
            break;
            #endregion
          default:
            Manager.Assert(false);
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            break;
        };
          break;
          #endregion
        case NCCI_States.N4:
          #region N4
        switch (Protocol_Event)
        {
          case CAPI_Wrapper.comsub.DISCONNECT_B3_CONF:
            if (info !=0)
            {
              Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
              NCCIPCurrState = previousState;
            }
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
            break;
          case CAPI_Wrapper.comsub.DISCONNECT_B3_IND:
            NCCIPCurrState = NCCI_States.N5;
            Protocol_Event = CAPI_Wrapper.comsub.DISCONNECT_B3_RESP; 
            break;
          default:
            Manager.Assert(false);
            break;
        };
          break;
          #endregion
        case NCCI_States.N5:
          #region N5
        switch (Protocol_Event)
        {
          case CAPI_Wrapper.comsub.DISCONNECT_B3_RESP:
            Manager.Assert
              (
              CAPI_Interface.Send_msg_disconnect_b3_resp
              (ref CAPImsg, 1, PLCI, NCCI, out info)
              );
            NCCI = 0;
            NCCIPCurrState = NCCI_States.N0;
            if (Disconnect_B3_Pending) Protocol_Event = CAPI_Wrapper.comsub.DISCONNECT_REQ;
            else Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
            break;
          default:
            Manager.Assert(false);
            break;
        };
          break;
          #endregion
        case NCCI_States.N_ACT:
          #region N_ACT
        switch (Protocol_Event)
        {
          case CAPI_Wrapper.comsub.DATA_B3_REQ:
            Manager.Assert
              (
              CAPI_Interface.Send_msg_data_b3_req
              (
              ref CAPImsg, 1, PLCI, NCCI, buff_TxMess.TxDataGet, out info
              )
              );
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            break;
          case CAPI_Wrapper.comsub.DATA_B3_CONF:
            Manager.Assert(info == 0);
            buff_TxMess.TxDataRelease();
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            break;
          case CAPI_Wrapper.comsub.DATA_B3_RESP:
            Manager.Assert
              (
              CAPI_Interface.Send_msg_data_b3_resp(ref CAPImsg, 1, PLCI, NCCI, out info)
              );
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            break;
          case CAPI_Wrapper.comsub.DATA_B3_IND:
            ISesDBuffer newBuffor = myDBufferPool.GetEmptyISesDBuffer();
            CAPImsg.Get_Data(newBuffor);
            IEnvelope newIEBuffor = (IEnvelope) newBuffor;
            newBuffor = null;
            buff_RxMess.SendMsg(ref newIEBuffor);
            Protocol_Event = CAPI_Wrapper.comsub.DATA_B3_RESP;
            break;
          case CAPI_Wrapper.comsub.RESET_B3_REQ:
            //MPNI sprawdziæ co robi Reset - w stanie N3 nie s¹ przyjmowane dane
            //MPNI zaimplementowac funkcje reset
            Manager.Assert(CAPI_Interface.Send_msg_reset_b3_req(ref CAPImsg, 1, PLCI, NCCI, out info)
              );
            NCCIPCurrState = NCCI_States.N3;
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            break;
          case CAPI_Wrapper.comsub.RESET_B3_IND:
            //mpni - TU TRZEBA WYS£AÆ res
            NCCIPCurrState = NCCI_States.N_ACT;
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE; 
            break;
            #region DISCONNECT
            // wspolne dla N-1, N-2, N-3, N-ACT
          case CAPI_Wrapper.comsub.DISCONNECT_B3_REQ:
            Manager.Assert
              ( CAPI_Interface.Send_msg_disconnect_b3_req(ref CAPImsg, 1, PLCI, NCCI, out info) );
            Disconnect_B3_Pending = true;
            previousState = NCCI_States.N_ACT;
            NCCIPCurrState = NCCI_States.N4;
            Protocol_Event = CAPI_Wrapper.comsub.NO_ONE;
            buff_TxMess.TxDataClear();
            buff_RxMess.Close();
            break;
          case CAPI_Wrapper.comsub.DISCONNECT_B3_IND:
            NCCIPCurrState = NCCI_States.N5;
            Protocol_Event = CAPI_Wrapper.comsub.DISCONNECT_B3_RESP;
            buff_TxMess.TxDataClear();
            buff_RxMess.Close();
            break;
            #endregion
          default:
            Manager.Assert(false);
            break;
        }; //switch
          break;
          #endregion
      }; //switch (NCCIPCurrState)
    } //public void SM_NCCI
    #endregion
  } // class C_NCCIState  
} // namespace CAPI.Session
