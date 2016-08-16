//<summary>
//  Title   : CAPI_to_Serial
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol: 15-02-2007:
//      Dopasowalem do nowej definicji interfejsu ICommunicationLayer, gdzie nie ma ListenReq. 
//      ListenReq ma byc wywolywana w kreatorze zgodnie z konfiguracja - tego nie zaimplementowalem.
//    MPostol: 07-02-2007
//      Added but not implemented Dispose method according to Disposable interface. 
//    MZbrzezny 2007-01-31
//    usunalem wstepnie "port"
//    UWAGA - w przypadku koniecznosci wykorzystania capi
//    nalezy uporzadkowac sprawe portu
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
  using System.Runtime.InteropServices;
  /// <summary>
  /// Summary description for CAPI_to_Serial.
  /// </summary>
  public class CAPI_to_Serial: ICommunicationLayer
  {
    #region PUBLIC
    public class buff_pool: SesDBufferPool
    {
      protected override ISesDBuffer CreateISesDBuffer()
      {
        return new _MyMessage( this );
      }
    }
    public CAPI_to_Serial()
    {
      pool = new buff_pool();
      myMessage = (_MyMessage)pool.GetEmptyISesDBuffer();
    }
    #endregion
    #region ICommunicationLayer Members
    //MPTD Musi byc wywolywane w kreatorze - nie jest 
    private TResult ListenReq( bool state )
    {
      ret = sesja.ListenReq( state );
      switch ( ret )
      {
        case CAPI.CAPI_Session.TSessionError.Success:
          return TResult.Success;
        default:
          return TResult.ConnectReq_Failure;
      }
    }
    TConnectReqRes IConnectionManagement.ConnectReq( IAddress remoteAddress )
    {
      object port;// TODO:ten port zostal zimplementowany by dalo sie to skapilowac, ale nalezy usunac
      if ( !( add is _ISDN_Address ) )
        return TResult.Wrong_Address;
      _ISDN_Address isdn_add = (_ISDN_Address)add;
      if ( isdn_add.tel_num == "" )
        return TResult.Wrong_Address;
      ret = sesja.ConnectReq( out port, isdn_add.tel_num, B1, B2, B3, pool, 10000 );
      switch ( ret )
      {
        case CAPI.CAPI_Session.TSessionError.Success:
          return TResult.Success;
        default:
          return TResult.ConnectReq_Failure;
      }
    }
    TConnIndRes IConnectionManagement.ConnectInd( IAddress pRemoteAddress, int pTimeOutInMilliseconds )
    {
      object port;// TODO:ten port zostal zimplementowany by dalo sie to skapilowac, ale nalezy usunac
      ret = sesja.ConnectInd( out port, 7000, B1, B2, B3, pool );
      switch ( ret )
      {
        case CAPI.CAPI_Session.TSessionError.Success:
          return TConnIndRes.ConnectInd;
        default:
          return TConnIndRes.NoConnection;
      }
    }
    private TResult ClearData( object port )
    {
      ret = sesja.ClearData( port );
      switch ( ret )
      {
        case CAPI.CAPI_Session.TSessionError.Success:
          return TResult.Success;
        default:
          return TResult.DataInd_NoDat;
      }
    }
    //    public TResult GetChar(out byte lastChr, object port, bool GetResponse)
    //    {
    //      act_ofs = 0;
    //      ISesDBuffer ret_var;
    //      if (GetResponse) ret = sesja.DataInd(port, out ret_var, 2000);
    //      else ret = sesja.DataInd(port, out ret_var, 5000);
    //      myMessage = (_MyMessage)ret_var;
    //      switch (ret)
    //      {
    //        case CAPI.CAPI_Session.TSessionError.Success:
    //          lastChr = Marshal.ReadByte(myMessage.uMessagePtr, act_ofs++);
    //          return TResult.Success;
    //        default:
    //          lastChr = 0;
    //          return TResult.DataInd_NoDat;
    //      }
    //    } 
    /// <summary>
    /// Get the next character from the receiving stream.
    /// </summary>
    /// <param name="lastChr">The character or 0 if timeout</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the character.</param>
    /// <returns></returns>
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr, int millisecondsTimeout )
    {
      throw ( new NotImplementedException( "CAS.Lib.CommonBus.CommunicationLayer.CAPI.CAPI_to_Serial.GetChar(out byte, int)" ) );
    }
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr )
    {
      //      if (act_ofs > myMessage.userDataLength)
      //      {
      //        lastChr = 0;
      //        return TResult.DataInd_NoDat;
      //      }
      object port = null;// TODO:ten port zostal zimplementowany by dalo sie to skapilowac, ale nalezy usunac

      lastChr = 0;
      if ( myMessage.userDataLength == 0 )
      {
        ISesDBuffer ret_var;
        ret = sesja.DataInd( port, out ret_var, 2000 );
        myMessage = (_MyMessage)ret_var;
      }
      if ( myMessage.userDataLength > 0 )
      {
        lastChr = myMessage.ReadByte();
      }
      else
      {
        lastChr = 0;
        //MPTD dopasowalem do zmiany typu resultatu, ale bez sensu - trzeba zaimplementowaæ GetChar z timeout
        return  TGetCharRes.DisInd;
      }
      //lastChr = Marshal.ReadByte(myMessage.uMessagePtr, act_ofs++);
      //if (act_ofs == (myMessage.userDataLength)) myMessage.reset();
      //      if (myMessage.offset == (myMessage.userDataLength)) myMessage.reset();
      return TGetCharRes.Success;
    }
    //MPTD Wywalony z ICommunicationLayer
    //TResult ICommunicationLayer.PutChar( byte lastChr )
    //{
    //  if ( myMessage == null )
    //  {
    //    myMessage = (_MyMessage)pool.GetEmptyISesDBuffer();
    //  }
    //  if ( myMessage.userBuffLength > myMessage.userDataLength )
    //  {
    //    myMessage.WriteByte( lastChr );
    //    return TResult.Success;
    //  }
    //  else
    //    return TResult.BufferTooSmall;
    //}
    // MPTD Wywalony z ICommunicationLayer
    //TResult ICommunicationLayer.FrameEndSignal()
    //{
    //  object port = null;// TODO:ten port zostal zimplementowany by dalo sie to skapilowac, ale nalezy usunac
    //  // MZTC: Tak funkcja w przypadku RS'a wyglada zupelnie inaczej :       return BaseStation.TResult.Wrong_Address;
    //  ret = sesja.DataReq( port, myMessage );
    //  myMessage.reset();
    //  if ( ret == CAPI.CAPI_Session.TSessionError.Success )
    //    return TResult.Success;
    //  else
    //    return TResult.DisconnectReq_Failure;
    //}
    TResult ICommunicationLayer.FrameEndSignal( UMessage frame )
    {
      object port = null;// TODO:ten port zostal zimplementowany by dalo sie to skapilowac, ale nalezy usunac
      //MZTC: tak wyglada to w RSie
      //      BaseStation.TResult res = BaseStation.TResult.Success;
      //      bool wrok = Send(frame.uMessagePtr, frame.userDataLength);
      //      UInt32 lastError = CAPI_Wrapper..GetLastError();
      //      Processes.Manager.Assert(wrok);
      //      if ( ! wrok ) res = BaseStation.TResult.DisconnectReq_Failure;
      //      return res;
      ret = sesja.DataReq( port, (ISesDBuffer)frame );
      //frame.reset();
      if ( ret == CAPI.CAPI_Session.TSessionError.Success )
        return TResult.Success;
      else
        return TResult.DisconnectReq_Failure;
    }
    TResult ICommunicationLayer.CheckChar()
    {
      //if (act_ofs < myMessage.userDataLength) return TResult.Success;
      if ( myMessage.userDataLength > 0 )
        return TResult.Success;
      else
        return TResult.DataInd_NoDat;
    }
    void IConnectionManagement.DisReq()
    {
      object port = null;// TODO:ten port zostal zimplementowany by dalo sie to skapilowac, ale nalezy usunac
      ret = sesja.DisconnectReq( ref port, 10000 );
      switch ( ret )
      {
        case CAPI.CAPI_Session.TSessionError.Success:
          return TResult.Success;
        default:
          return TResult.DisconnectReq_Failure;
      }
    }
    //TDisIndRes ICommunicationLayer.DisconnectInd( int timeout )
    //{
    //  object port = null;// TODO:ten port zostal zimplementowany by dalo sie to skapilowac, ale nalezy usunac
    //  ret = sesja.DisconnectInd( port, timeout );
    //  switch ( ret )
    //  {
    //    case CAPI.CAPI_Session.TSessionError.Success:
    //      return TDisIndRes.NoConnection;
    //    default:
    //      return TDisIndRes.StillConnected;
    //  }
    //}
    bool ICommunicationLayer.Connected
    {
      get { throw new Exception( "The method or operation is not implemented." ); }
    }
    #endregion
    #region IDisposable
    /// <summary>
    /// dispose interface
    /// </summary>
    public void Dispose()
    {
      //MPTD must be implememnted
    }
    #endregion IDisposable
    #region PRIVATE
    private CAPI.CAPI_Session sesja = new CAPI.CAPI_Session();
    //private int act_ofs = 0; MZ
    CAPI.Protocols.B1_64kbs_WITH_HDLC_conf B1 = new CAPI.Protocols.B1_64kbs_WITH_HDLC_conf();
    CAPI.Protocols.B2_X75_SLP_conf B2 = new CAPI.Protocols.B2_X75_SLP_conf();
    CAPI.Protocols.B3_TRANSPARENT_conf B3 = new CAPI.Protocols.B3_TRANSPARENT_conf();
    CAPI.CAPI_Session.TSessionError ret = CAPI.CAPI_Session.TSessionError.NotConnected;
    buff_pool pool;
    _MyMessage myMessage;
    public class _MyMessage: UMessage, ISesDBuffer
    {
      // MZTD: to inPool dopisalem bo musi implementowac to po IENVELOPE
      bool inPoolFlag;
      bool Processes.IEnvelope.InPool
      {
        set { this.inPoolFlag = value; }
        get { return this.inPoolFlag; }
      }
      private SesDBufferPool pool = null;
      public void ReturnEmptyEnvelope()
      {
        ISesDBuffer var = (ISesDBuffer)this;
        this.pool.ReturnEmptyISesDBuffer( ref var );
      }
      public void reset()
      {
        offset = 0;
      }
      public _MyMessage( SesDBufferPool source )
        : base( 300, false )  //MZTC - tu dopisalem false - ale to wszystko jedno
      {
        pool = source;
      }
    }
    bool ICommunicationLayer.Flush()  // !*! MZ to dopisalem bo musi to impelemtowac
    {
      //myMess.ResetContent();
      //return FlushComm();  //MZ tak bylo w RS_to_serial
      return true;
    }
    private ISesDBuffer f( SesDBufferPool source )
    {
      return new _MyMessage( source );
    }
    #endregion
    #region IDisposable Members
    void IDisposable.Dispose()
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    #endregion
  }
  public class _ISDN_Address: IAddress
  {
    public string tel_num = "";
    object IAddress.address
    {
      get
      {
        return tel_num;
      }
      set
      {
        tel_num = (string)value;
      }
    }
  }
}
