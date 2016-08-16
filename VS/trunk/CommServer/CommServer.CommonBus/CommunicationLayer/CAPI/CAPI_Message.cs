
//<summary>
//  Title   : CAPI_Message
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
  using CAPI.Protocols;
  using System.Runtime.InteropServices;
  using System;

  public class CAPI_Message: UMessage
  {
    public CAPI_Message()
      : base( 212, false )//MZTC: dopisalem false bo musialem cos dopisac, a tu tak naprawde wszystko jedno co bedzie
    {
      offset = 12;
    }
    public CAPI_Message( ushort len )
      : base( len, false )//MZTC: dopisalem false bo musialem cos dopisac, a tu tak naprawde wszystko jedno co bedzie
    {
      offset = 12;
    }
    /// <summary>
    /// Total length of CAPI message including header
    /// </summary>
    public short length
    {
      set
      {
        Marshal.WriteInt16( uMessagePtr, 0, value );
      }
      get
      {
        return Marshal.ReadInt16( uMessagePtr, 0 );
      }
    }
    /// <summary>
    /// Unique identification number assigned by CAPI
    /// </summary>
    public short appID
    {
      set
      {
        Marshal.WriteInt16( uMessagePtr, 2, value );
      }
      get
      {
        return Marshal.ReadInt16( uMessagePtr, 2 );
      }
    }
    /// <summary>
    /// Command and command extension
    /// </summary>
    public CAPI_Wrapper.comsub sub_com
    {
      set
      {
        Marshal.WriteInt16( uMessagePtr, 4, (short)value );
      }
      get
      {
        return (CAPI_Wrapper.comsub)Marshal.ReadInt16( uMessagePtr, 4 );
      }
    }
    /// <summary>
    /// Message number
    /// </summary>
    public short number
    {
      set
      {
        Marshal.WriteInt16( uMessagePtr, 6, value );
      }
      get
      {
        return Marshal.ReadInt16( uMessagePtr, 6 );
      }
    }
    /// <summary>
    /// Number of controller
    /// </summary>
    public byte controller
    {
      set
      {
        Marshal.WriteByte( uMessagePtr, 8, value );
      }
      get
      {
        return Marshal.ReadByte( uMessagePtr, 8 );
      }
    }
    /// <summary>
    /// Physical Link Connection Identifier
    /// </summary>
    public byte plci
    {
      set
      {
        Marshal.WriteByte( uMessagePtr, 9, value );
      }
      get
      {
        return Marshal.ReadByte( uMessagePtr, 9 );
      }
    }
    /// <summary>
    /// Network Control Connection Identifier
    /// </summary>
    public short ncci
    {
      set
      {
        Marshal.WriteInt16( uMessagePtr, 10, value );
      }
      get
      {
        return Marshal.ReadInt16( uMessagePtr, 10 );
      }
    }
    /// <summary>
    /// Copies the data from CAPI buffer to IDBuffer
    /// </summary>
    /// <param name="bufor">Buffer to copy data to</param>
    public void Get_Data( Processes.IDBuffer bufor )
    {
      offset = 12;
      bufor.CopyToBuffor( ReadIntPtr(), (uint)ReadInt16() );
    }
    /// <summary>
    /// Writes the called number according with ETS 300 102-1/Q.931
    /// </summary>
    /// <param name="val">Called party number</param>
    public void WriteTelNum( string val )
    {
      WriteByte( (byte)( val.Length + 1 ) );
      WriteByte( 0x81 );
      for ( int idx = 0; idx < val.Length; idx++ )
      {
        WriteByte( Convert.ToByte( val[ idx ] ) );
      }
    }
    /// <summary>
    /// Retrieves the calling number
    /// </summary>
    /// <returns>Calling party number</returns>
    public string GetCallingNum()
    {
      if ( sub_com != CAPI_Wrapper.comsub.CONNECT_REQ )
        return "";
      ReadInt16();
      byte len = ReadByte();
      ReadByte();
      ReadString( (short)( len - 1 ) );
      len = ReadByte();
      ReadByte();
      return ReadString( (short)( len - 1 ) );
    }
    /// <summary>
    /// Pointer to transmitted or received data
    /// </summary>
    public IntPtr userDataPtr
    {
      get
      {
        return Marshal.ReadIntPtr( uMessagePtr, 12 );
      }
      set
      {
        Marshal.WriteIntPtr( uMessagePtr, 12, value );
      }
    }
    /// <summary>
    /// Data total length
    /// </summary>
    new public short userDataLength
    {
      get
      {
        return Marshal.ReadInt16( uMessagePtr, 16 );
      }
      set
      {
        Marshal.WriteInt16( uMessagePtr, 16, value );
      }
    }
    /// <summary>
    /// Return code
    /// </summary>
    public CAPI_Wrapper.TInterfaceError get_info
    {
      get
      {
        switch ( sub_com )
        {
          case CAPI_Wrapper.comsub.DATA_B3_CONF:
            return (CAPI_Wrapper.TInterfaceError)Marshal.ReadInt16( uMessagePtr, 14 );
          default:
            return (CAPI_Wrapper.TInterfaceError)Marshal.ReadInt16( uMessagePtr, 12 );
        }
      }
    }
    /// <summary>
    /// Resets message's offset
    /// </summary>
    public void reset_offset()
    {
      offset = 12;
    }
  }
}