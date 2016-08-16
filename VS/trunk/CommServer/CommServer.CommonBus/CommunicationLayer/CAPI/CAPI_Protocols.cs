//<summary>
//  Title   : CAPI_Protocols
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
namespace CAS.Lib.CommonBus.CommunicationLayer.CAPI.Protocols
{
  public interface IProto_Conf
  {
    bool Set_param( CAPI.CAPI_Message CAPImsg );
    bool def_conf
    {
      get;
      set;
    }
  }
  public interface IB1_Proto: IProto_Conf
  {
    B1_protocol B1
    {
      get;
    }
  }
  public interface IB2_Proto: IProto_Conf
  {
    B2_protocol B2
    {
      get;
    }
  }

  public interface IB3_Proto: IProto_Conf
  {
    B3_protocol B3
    {
      get;
    }
  }
  /// <summary>
  /// Configuration for B1 protocol: 64 kbit/s with HDLC framing (default)
  /// </summary>
  public class B1_64kbs_WITH_HDLC_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol._64kbs_WITH_HDLC;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  /// <summary>
  /// Configuration for B1 protocol:64 kbit/s bit-transparent operation with byte framing from the network
  /// </summary>
  public class B1_64kbs_TRANSPARENT_WITH_NETWORK_FRAMING_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol._64kbs_TRANSPARENT_WITH_NETWORK_FRAMING;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  /// <summary>
  /// Configuration for B1 protocol:V.110 asynchronous operation with start/stop byte framing
  /// </summary>
  public class B1_V110_ASYNCHRONOUS_WITH_START_STOP_BYTE_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( max_bit_rate );
        CAPImsg.WriteInt16( bits_per_char );
        CAPImsg.WriteInt16( (short)parity_type );
        CAPImsg.WriteInt16( (short)no_of_stop_bits );
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol.V110_ASYNCHRONOUS_WITH_START_STOP_BYTE;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public short max_bit_rate = 0;
    public short bits_per_char = 8;
    public parity parity_type = parity.NONE;
    public stop_bits no_of_stop_bits = stop_bits._1_STOP_BIT;
  }
  /// <summary>
  /// Configuration for B1 protocol: V.110 synchronous operation with HDLC framing
  /// </summary>
  public class B1_V110_SYNCHRONOUS_WITH_HDLC_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( max_bit_rate );
        for ( int i = 0; i < 6; i++ )
        {
          CAPImsg.WriteByte( 0 );
        }
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol.V110_SYNCHRONOUS_WITH_HDLC;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public short max_bit_rate = 56;
  }
  /// <summary>
  /// Configuration for B1 protocol: T.30 modem for Group 3 fax
  /// </summary>
  public class B1_T30_MODEM_FOR_GROUP_3_FAX_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( max_bit_rate );
        CAPImsg.WriteInt16( transmit_level_in_dB );
        for ( int i = 0; i < 4; i++ )
        {
          CAPImsg.WriteByte( 0 );
        }
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol.T30_MODEM_FOR_GROUP_3_FAX;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public short max_bit_rate = 0;
    public short transmit_level_in_dB = 0;
  }
  /// <summary>
  /// Configuration for B1 protocol: 64 kbit/s inverted with HDLC framing
  /// </summary>
  public class B1_64kbs_INVERTED_WITH_HDLC_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol._64kbs_INVERTED_WITH_HDLC;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  /// <summary>
  /// Configuration for B1 protocol: 56 kbit/s bit-transparent operation with byte framing from the network
  /// </summary>
  public class B1_56kbs_BIT_TRANSPARENT_WITH_NETWORK_FRAMING_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol._56kbs_BIT_TRANSPARENT_WITH_NETWORK_FRAMING;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  /// <summary>
  /// Configuration for B1 protocol: Modem with full negotiation
  /// </summary>
  public class B1_MODEM_WITH_FULL_NEGOTIATION_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( max_bit_rate );
        CAPImsg.WriteInt16( bits_per_char );
        CAPImsg.WriteInt16( (short)parity_type );
        CAPImsg.WriteInt16( (short)no_of_stop_bits );
        CAPImsg.WriteInt16( (short)options );
        CAPImsg.WriteInt16( (short)(byte)speed_neg_type );
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol.MODEM_WITH_FULL_NEGOTIATION;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public short max_bit_rate = 0;
    public short bits_per_char = 8;
    public parity parity_type = parity.NONE;
    public stop_bits no_of_stop_bits = stop_bits._1_STOP_BIT;
    public byte options = (byte)B1_options.LOUDSPEAKER_ON_DUR_DIALING_AND_NEGOTIATION | (byte)B1_options.LOUDSPEAKER_VOL_NORMAL_LOW;
    public speed_negotiation speed_neg_type = speed_negotiation.V8;
  }
  /// <summary>
  /// Configuration for B1 protocol: Modem asynchronous operation with start/stop byte framing
  /// </summary>
  public class B1_MODEM_ASYNCHRONOUS_WITH_START_STOP_BYTE_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( max_bit_rate );
        CAPImsg.WriteInt16( bits_per_char );
        CAPImsg.WriteInt16( (short)parity_type );
        CAPImsg.WriteInt16( (short)no_of_stop_bits );
        CAPImsg.WriteInt16( (short)options );
        CAPImsg.WriteInt16( (short)speed_neg_type );
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol.MODEM_ASYNCHRONOUS_WITH_START_STOP_BYTE;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public short max_bit_rate = 0;
    public short bits_per_char = 8;
    public parity parity_type = parity.NONE;
    public stop_bits no_of_stop_bits = stop_bits._1_STOP_BIT;
    public byte options = (byte)B1_options.LOUDSPEAKER_ON_DUR_DIALING_AND_NEGOTIATION | (byte)B1_options.LOUDSPEAKER_VOL_NORMAL_LOW;
    public speed_negotiation speed_neg_type = speed_negotiation.V8;
  }
  /// <summary>
  /// Configuration for B1 protocol: Modem synchronous operation with HDLC framing
  /// </summary>
  public class B1_MODEM_SYNCHRONOUS_WITH_HDLC_conf: IB1_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( max_bit_rate );
        for ( int i = 0; i < 6; i++ )
        {
          CAPImsg.WriteByte( 0 );
        }
        CAPImsg.WriteInt16( (short)options );
        CAPImsg.WriteInt16( (short)speed_neg_type );
        CAPImsg.End_block();
      }
      return true;
    }
    public B1_protocol B1
    {
      get
      {
        return B1_protocol.MODEM_SYNCHRONOUS_WITH_HDLC;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public short max_bit_rate = 0;
    public byte options = (byte)B1_options.LOUDSPEAKER_ON_DUR_DIALING_AND_NEGOTIATION | (byte)B1_options.LOUDSPEAKER_VOL_NORMAL_LOW;
    public speed_negotiation speed_neg_type = speed_negotiation.V8;
  }
  /// <summary>
  /// Configuration for B2 protocol: ISO 7776 (X.75 SLP) (default)
  /// </summary>
  public class B2_X75_SLP_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( link_add_A );
        CAPImsg.WriteByte( link_add_B );
        CAPImsg.WriteByte( (byte)mod_mode_type );
        CAPImsg.WriteByte( window_size );
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.X75_SLP;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public byte link_add_A = 0x03;
    public byte link_add_B = 0x01;
    public modulo_mode mod_mode_type = modulo_mode.NORMAL;
    public byte window_size = 7;
  }
  /// <summary>
  /// Configuration for B2 protocol: Transparent
  /// </summary>
  public class B2_TRANSPARENT_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.TRANSPARENT;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  /// <summary>
  /// Configuration for B2 protocol: SDLC
  /// </summary>
  public class B2_SDLC_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( link_add_A );
        CAPImsg.WriteByte( 0 );
        CAPImsg.WriteByte( (byte)mod_mode_type );
        CAPImsg.WriteByte( window_size );
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.SDLC;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public byte link_add_A = 0xC1;
    public modulo_mode mod_mode_type = modulo_mode.NORMAL;
    public byte window_size = 7;
  }
  /// <summary>
  /// Configuration for B2 protocol: LAPD in accordance with Q.921 for D channel X.25 (SAPI 16)
  /// </summary>
  public class B2_SAPI_16_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( add_A );
        CAPImsg.WriteByte( 0 );
        CAPImsg.WriteByte( (byte)mod_mode_type );
        CAPImsg.WriteByte( window_size );
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.SAPI_16;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public byte add_A = 1;
    public modulo_mode mod_mode_type = modulo_mode.EXTENDED;
    public byte window_size = 3;
  }
  /// <summary>
  /// Configuration for B2 protocol: T.30 for Group 3 fax
  /// </summary>
  public class B2_T30_FOR_GROUP_3_FAX_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.T30_FOR_GROUP_3_FAX;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  /// <summary>
  /// Configuration for B2 protocol: Point-to-Point Protocol (PPP)
  /// </summary>
  public class B2_PPP_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.PPP;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  /// <summary>
  /// Configuration for B2 protocol: Transparent (ignoring framing errors of B1 protocol)
  /// </summary>
  public class B2_TRANSPARENT_IGNORING_B1_ERRORS_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.TRANSPARENT_IGNORING_B1_ERRORS;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  /// <summary>
  /// Configuration for B2 protocol: Modem with full negotiation (e.g. V.42 bis, MNP 5)
  /// </summary>
  public class B2_MODEM_WITH_FULL_NEGOTIATION_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( (short)options );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.MODEM_WITH_FULL_NEGOTIATION;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public B2_options options = B2_options.NONE;
  }
  /// <summary>
  /// Configuration for B2 protocol: ISO 7776 (X.75 SLP) modified to support V.42 bis compression
  /// </summary>
  public class B2_X75_SLP_WITH_V42_BIS_COMPRESSION_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( add_A );
        CAPImsg.WriteByte( add_B );
        CAPImsg.WriteByte( (byte)mod_mode_type );
        CAPImsg.WriteByte( window_size );
        CAPImsg.WriteInt16( (short)direction );
        CAPImsg.WriteInt16( no_of_code_words );
        CAPImsg.WriteInt16( max_string_len );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.X75_SLP_WITH_V42_BIS_COMPRESSION;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public byte add_A = 0x03;
    public byte add_B = 0x01;
    public modulo_mode mod_mode_type = modulo_mode.NORMAL;
    public byte window_size = 7;
    public compression_direction direction = compression_direction.ALL;
    public short no_of_code_words = 2048;
    public short max_string_len = 250;
  }
  /// <summary>
  /// Configuration for B2 protocol: V.120 asynchronous mode
  /// </summary>
  public class B2_V120_ASYNCHRONOUS_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( add_A );
        CAPImsg.WriteByte( add_B );
        CAPImsg.WriteByte( (byte)mod_mode_type );
        CAPImsg.WriteByte( window_size );
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.V120_ASYNCHRONOUS;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public byte add_A = 0x00;
    public byte add_B = 0x01;
    public modulo_mode mod_mode_type = modulo_mode.EXTENDED;
    public byte window_size = 7;
  }
  /// <summary>
  /// Configuration for B2 protocol: V.120 asynchronous mode with V.42 bis compression
  /// </summary>
  public class B2_V120_ASYNCHRONOUS_WITH_V42_BIS_COMPRESSION_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( add_A );
        CAPImsg.WriteByte( add_B );
        CAPImsg.WriteByte( (byte)mod_mode_type );
        CAPImsg.WriteByte( window_size );
        CAPImsg.WriteInt16( (short)direction );
        CAPImsg.WriteInt16( no_of_code_words );
        CAPImsg.WriteInt16( max_string_len );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.V120_ASYNCHRONOUS_WITH_V42_BIS_COMPRESSION;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public byte add_A = 0x00;
    public byte add_B = 0x01;
    public modulo_mode mod_mode_type = modulo_mode.EXTENDED;
    public byte window_size = 7;
    public compression_direction direction = compression_direction.ALL;
    public short no_of_code_words = 2048;
    public short max_string_len = 250;
  }
  /// <summary>
  /// Configuration for B2 protocol: V.120 bit-transparent mode
  /// </summary>
  public class B2_V120_TRANSPARENT_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( add_A );
        CAPImsg.WriteByte( add_B );
        CAPImsg.WriteByte( 128 );
        CAPImsg.WriteByte( window_size );
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.V120_TRANSPARENT;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public byte add_A = 0x00;
    public byte add_B = 0x01;
    public modulo_mode mod_mode_type = modulo_mode.EXTENDED;
    public byte window_size = 7;
  }
  /// <summary>
  /// Configuration for B2 protocol: LAPD in accordance with Q.921 including free SAPI selection
  /// </summary>
  public class B2_LAPD_INC_FREE_SAPI_conf: IB2_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( add_A );
        CAPImsg.WriteByte( add_B );
        CAPImsg.WriteByte( (byte)mod_mode_type );
        CAPImsg.WriteByte( window_size );
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B2_protocol B2
    {
      get
      {
        return B2_protocol.LAPD_INC_FREE_SAPI;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public byte add_A = 1;
    public byte add_B = 0;
    public modulo_mode mod_mode_type = modulo_mode.EXTENDED;
    public byte window_size = 1;
  }
  /// <summary>
  /// Configuration for B3 protocol: Transparent (default)
  /// </summary>
  public class B3_TRANSPARENT_conf: IB3_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B3_protocol B3
    {
      get
      {
        return B3_protocol.TRANSPARENT;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  /// <summary>
  /// Configuration for B3 protocol: T.90NL with compatibility to T.70NL in accordance with T.90
  /// </summary>
  public class B3_T90NL_WITH_COMPAB_TO_T70NL_conf: IB3_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( LIC );
        CAPImsg.WriteInt16( HIC );
        CAPImsg.WriteInt16( LTC );
        CAPImsg.WriteInt16( HTC );
        CAPImsg.WriteInt16( LOC );
        CAPImsg.WriteInt16( HOC );
        CAPImsg.WriteInt16( (short)mod_mode_type );
        CAPImsg.WriteInt16( window_size );
        CAPImsg.End_block();
      }
      return true;
    }
    public B3_protocol B3
    {
      get
      {
        return B3_protocol.T90NL_WITH_COMPAB_TO_T70NL;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public short LIC = 0;
    public short HIC = 0;
    public short LTC = 1;
    public short HTC = 1;
    public short LOC = 0;
    public short HOC = 0;
    public modulo_mode mod_mode_type = modulo_mode.NORMAL;
    public short window_size = 2;
  }
  /// <summary>
  /// Configuration for B3 protocol: ISO 8208 (X.25 DTE-DTE)
  /// </summary>
  public class B3_X25_DTE_DTE_conf: IB3_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( LIC );
        CAPImsg.WriteInt16( HIC );
        CAPImsg.WriteInt16( LTC );
        CAPImsg.WriteInt16( HTC );
        CAPImsg.WriteInt16( LOC );
        CAPImsg.WriteInt16( HOC );
        CAPImsg.WriteInt16( (short)mod_mode_type );
        CAPImsg.WriteInt16( window_size );
        CAPImsg.End_block();
      }
      return true;
    }
    public B3_protocol B3
    {
      get
      {
        return B3_protocol.X25_DTE_DTE;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public short LIC = 0;
    public short HIC = 0;
    public short LTC = 1;
    public short HTC = 1;
    public short LOC = 0;
    public short HOC = 0;
    public modulo_mode mod_mode_type = modulo_mode.NORMAL;
    public short window_size = 2;
  }
  /// <summary>
  /// Configuration for B3 protocol: X.25 DCE
  /// </summary>
  public class B3_X25_DCE_conf: IB3_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteInt16( LIC );
        CAPImsg.WriteInt16( HIC );
        CAPImsg.WriteInt16( LTC );
        CAPImsg.WriteInt16( HTC );
        CAPImsg.WriteInt16( LOC );
        CAPImsg.WriteInt16( HOC );
        CAPImsg.WriteInt16( (short)mod_mode_type );
        CAPImsg.End_block();
      }
      return true;
    }
    public B3_protocol B3
    {
      get
      {
        return B3_protocol.X25_DCE;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
    public short LIC = 0;
    public short HIC = 0;
    public short LTC = 1;
    public short HTC = 1;
    public short LOC = 0;
    public short HOC = 0;
    public modulo_mode mod_mode_type = modulo_mode.NORMAL;
    public short window_size = 2;
  }
  //WMNI B3_T30_FOR_GROUP_3_FAX_conf
  /*  public class B3_T30_FOR_GROUP_3_FAX_conf:IB3_Proto
    {
    public bool Set_param(CAPI.CAPI_Message CAPImsg)
    {
      return false;
    }
    public B3_protocol B3
    {
      get
      {
      return B3_protocol.T30_FOR_GROUP_3_FAX;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
      return config;
      }
      set
      {
      config = value;
      }
    }
    }
    //WMNI B3_T30_FOR_GROUP_3_FAX_EXTENDED
    public class B3_T30_FOR_GROUP_3_FAX_EXTENDED:IB3_Proto
    {
    public bool Set_param(CAPI.CAPI_Message CAPImsg)
    {
      return false;
    }
    public B3_protocol B3
    {
      get
      {
      return B3_protocol.T30_FOR_GROUP_3_FAX_EXTENDED;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
      return config;
      }
      set
      {
      config = value;
      }
    }
    }*/
  /// <summary>
  /// Configuration for B3 protocol: Modem
  /// </summary>
  public class B3_MODEM_conf: IB3_Proto
  {
    public bool Set_param( CAPI.CAPI_Message CAPImsg )
    {
      if ( def_conf )
        CAPImsg.WriteByte( 0 );
      else
      {
        CAPImsg.Start_block();
        CAPImsg.WriteByte( 0 );
        CAPImsg.End_block();
      }
      return true;
    }
    public B3_protocol B3
    {
      get
      {
        return B3_protocol.MODEM;
      }
    }
    private bool config = true;
    public bool def_conf
    {
      get
      {
        return config;
      }
      set
      {
        config = value;
      }
    }
  }
  public enum B1_protocol: byte
  {
    _64kbs_WITH_HDLC = 0,
    _64kbs_TRANSPARENT_WITH_NETWORK_FRAMING = 1,
    V110_ASYNCHRONOUS_WITH_START_STOP_BYTE = 2,
    V110_SYNCHRONOUS_WITH_HDLC = 3,
    T30_MODEM_FOR_GROUP_3_FAX = 4,
    _64kbs_INVERTED_WITH_HDLC = 5,
    _56kbs_BIT_TRANSPARENT_WITH_NETWORK_FRAMING = 6,
    MODEM_WITH_FULL_NEGOTIATION = 7,
    MODEM_ASYNCHRONOUS_WITH_START_STOP_BYTE = 8,
    MODEM_SYNCHRONOUS_WITH_HDLC = 9
  }

  public enum B2_protocol: byte
  {
    X75_SLP = 0,
    TRANSPARENT = 1,
    SDLC = 2,
    SAPI_16 = 3,
    T30_FOR_GROUP_3_FAX = 4,
    PPP = 5,
    TRANSPARENT_IGNORING_B1_ERRORS = 6,
    MODEM_WITH_FULL_NEGOTIATION = 7,
    X75_SLP_WITH_V42_BIS_COMPRESSION = 8,
    V120_ASYNCHRONOUS = 9,
    V120_ASYNCHRONOUS_WITH_V42_BIS_COMPRESSION = 10,
    V120_TRANSPARENT = 11,
    LAPD_INC_FREE_SAPI = 12
  }

  public enum B3_protocol: byte
  {
    TRANSPARENT = 0,
    T90NL_WITH_COMPAB_TO_T70NL = 1,
    X25_DTE_DTE = 2,
    X25_DCE = 3,
    T30_FOR_GROUP_3_FAX = 4,
    T30_FOR_GROUP_3_FAX_EXTENDED = 5,
    MODEM = 7
  }

  public enum parity: byte
  {
    NONE = 0,
    ODD = 1,
    EVEN = 2
  }

  public enum stop_bits: byte
  {
    _1_STOP_BIT = 0,
    _2_STOP_BITS = 1
  }
  //WMNI kolejnoœæ bitów
  public enum B1_options: byte
  {
    LOUDSPEAKER_VOL_SILENT = 0,
    LOUDSPEAKER_VOL_NORMAL_LOW = 64,
    LOUDSPEAKER_VOL_NORMAL_HIGH = 128,
    LOUDSPEAKER_VOL_MAX = 162,
    LOUDSPEAKER_OFF = 0,
    LOUDSPEAKER_ON_DUR_DIALING_AND_NEGOTIATION = 16,
    LOUDSPEAKER_ALWAYS_ON = 32,
    GUARD_TONE_OFF = 0,
    GUARD_TONE_1800_Hz = 4,
    GUARD_TONE_550_Hz = 8,
    DISABLE_RING_TONE = 2,
    DISABLE_RETAIN = 1
  }

  public enum speed_negotiation: byte
  {
    NONE = 0,
    WITHIN_MODULATION_CLASS = 1,
    V100 = 2,
    V8 = 3
  }

  public enum modulo_mode: byte
  {
    NORMAL = 8,
    EXTENDED = 128,
  }

  public enum B2_options: byte
  {
    DISABLE_COMPRESION = 16,
    DISABLE_V42_NEGOTIATION = 8,
    DISABLE_TRANSPARENT_MODE = 4,
    DISABLE_MNP4_MNP5 = 2,
    DISABLE_V42_V42BIS = 1,
    NONE = 0,
  }

  public enum compression_direction: byte
  {
    ALL = 0,
    INCOMING = 1,
    OUTCOMING = 2
  }

  public enum resolution: byte
  {
    STANDARD = 0,
    HIGH = 1
  }

  public enum format: byte
  {
    SFF = 0,
    PLAIN_FAX_FORMAT = 1,
    PCX = 2,
    DCX = 3,
    TIFF = 4,
    ASCII = 5,
    EXTENDED_ANSI = 6,
    BINARY_FILE = 7
  }

}