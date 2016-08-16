//<summary>
//  Title   : CAPI_Wrapper
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

obs³uga wszystkich funkcji

///////////////////////////////////////////////////////////////////////////////*/
namespace CAS.Lib.CommonBus.CommunicationLayer.CAPI
{
  using System;
  using System.Runtime.InteropServices;  /// <summary>
  /// Class that wraps all the CAPI functions and enumerators that concern them
  /// </summary>
  public abstract class CAPI_Wrapper
  {
    #region CAPI structures
    [StructLayout( LayoutKind.Sequential )]
    public class CAPI_Profile
    {
      public System.Int16 NumberOfControllers;
      public System.Int16 NumberOfSupBChannels;
      public System.Int32 GlobalOptions;
      public System.Int32 B1_prot_sup;
      public System.Int32 B2_prot_sup;
      public System.Int64 reserved0, reserved1, reserved2;
      public System.Int32 ManufactureInfo0;
      public System.Int32 ManufactureInfo1;
      public System.Int32 ManufactureInfo2;
      public System.Int32 ManufactureInfo3;
      public System.Int32 ManufactureInfo4;
      public override string ToString()
      {
        return "base(" + base.ToString() + "); NumberOfControllers=" + NumberOfControllers.ToString() +
          " NumberOfSupBChannels=" + NumberOfSupBChannels.ToString() + " GlobalOptions=" + GlobalOptions.ToString() +
          " B1_prot_sup=" + B1_prot_sup.ToString() + " B2_prot_sup=" + B2_prot_sup.ToString() +
          " ManufactureInfo=" + ManufactureInfo0.ToString() + "."
            + ManufactureInfo1.ToString() + "." + ManufactureInfo2.ToString() + "."
            + ManufactureInfo3.ToString() + "." + ManufactureInfo4.ToString() + ";";
      }

    }
    #endregion
    #region enums used with messages
    /// <summary>
    /// Commands and command extensions
    /// </summary>
    public enum comsub: ushort
    {
      // intenal events
      NO_ONE = 0,
      INTERNAL_WaitFConnInd = 1,

      //CAPI commands and subcommands
      ALERT_REQ = 0x8001,
      ALERT_CONF = 0x8101,

      LISTEN_REQ = 0x8005,
      LISTEN_CONF = 0x8105,

      CONNECT_REQ = 0x8002,
      CONNECT_CONF = 0x8102,
      CONNECT_IND = 0x8202,
      CONNECT_RESP = 0x8302,

      CONNECT_ACTIVE_IND = 0x8203,
      CONNECT_ACTIVE_RESP = 0x8303,

      CONNECT_B3_REQ = 0x8082,
      CONNECT_B3_CONF = 0x8182,
      CONNECT_B3_IND = 0x8282,
      CONNECT_B3_RESP = 0x8382,

      CONNECT_B3_ACTIVE_IND = 0x8283,
      CONNECT_B3_ACTIVE_RESP = 0x8383,

      DATA_B3_REQ = 0x8086,
      DATA_B3_CONF = 0x8186,
      DATA_B3_IND = 0x8286,
      DATA_B3_RESP = 0x8386,

      DISCONNECT_B3_REQ = 0x8084,
      DISCONNECT_B3_CONF = 0x8184,
      DISCONNECT_B3_IND = 0x8284,
      DISCONNECT_B3_RESP = 0x8384,

      DISCONNECT_REQ = 0x8004,
      DISCONNECT_CONF = 0x8104,
      DISCONNECT_IND = 0x8204,
      DISCONNECT_RESP = 0x8304,

      INFO_REQ = 0x8008,
      INFO_CONF = 0x8108,
      INFO_IND = 0x8208,
      INFO_RESP = 0x8308,

      RESET_B3_REQ = 0x8087,
      RESET_B3_CONF = 0x8187,
      RESET_B3_IND = 0x8287,
      RESET_B3_RESP = 0x8387
    }
    /// <summary>
    /// Return codes
    /// </summary>
    public enum TInterfaceError: uint
    {
      SUCCESS = 0x0000,
      NCPI_NOT_SUPPORTED_BY_PROTOCOL = 0x0001,
      FLAGS_NOT_SUPPORTED_BY_PROTOCOL = 0x0002,
      ALERT_SENT_BY_ANOTHER_APPLICATION = 0x0003,
      TOO_MANY_APPLICATIONS = 0x1001,
      LOGICAL_BLOCK_TOO_SMALL = 0x1002,
      BUFFER_EXCEEDS_64KB = 0x1003,
      MESSAGE_BUFFER_TOO_SMALL = 0x1004,
      MAX_NO_OF_LOGICAL_CONNECTIONS_NOT_SUPPORTED = 0x1005,
      INTERNAL_BUSY_CONDITION = 0x1007,
      OS_RESOURCE_ERROR = 0x1008,
      CAPI_NOT_INSTALLED_REG = 0x1009,
      EXTERNAL_EQUIPMENT_NOT_SUPPORTED_REG = 0x100A,
      EXTERNAL_EQUIPMENT_ONLY_SUPPORTED_REG = 0x100B,
      ILLEGAL_APPLICATION_NO = 0x1101,
      ILLEGAL_COMSUB_OR_MESSAGE_TOO_SHORT = 0x1102,
      QUEUE_FULL = 0x1103,
      QUEUE_EMPTY = 0x1104,
      QUEUE_OVERFLOW = 0x1105,
      UNKNOWN_NOTIFICATION_PARAM = 0x1106,
      INTERNAL_BUSY_CONDITION_TRUE = 0x1107,
      CAPI_NOT_INSTALLED_MES = 0x1109,
      EXTERNAL_EQUIPMENT_NOT_SUPPORTED_MES = 0x110A,
      EXTERNAL_EQUIPMENT_ONLY_SUPPORTED_MES = 0x110B,
      MESSAGE_NOT_SUPPORTED_IN_CUR_STATE = 0x2001,
      ILLEGAL_CONTR_PLCI_NCCI = 0x2002,
      NO_PLCI_AVAILABLE = 0x2003,
      NO_NCCI_AVAILABLE = 0x2004,
      NO_LISTEN_RES_AVAILABLE = 0x2005,
      NO_FAX_RES_AVAILABLE = 0x2006,
      ILLEGAL_PARAM_CODING = 0x2007,
      NO_INTERCONNECTION_RES_AVAILABLE = 0x2008,
      B1_PROT_NOT_SUPPERTED = 0x3001,
      B2_PROT_NOT_SUPPERTED = 0x3002,
      B3_PROT_NOT_SUPPERTED = 0x3003,
      B1_PROT_PARAM_NOT_SUPPERTED = 0x3004,
      B2_PROT_PARAM_NOT_SUPPERTED = 0x3005,
      B3_PROT_PARAM_NOT_SUPPERTED = 0x3006,
      B_PROT_COMBINATION_NOT_SUPPORTED = 0x3007,
      NCPI_NOT_SUPPORTED = 0x3008,
      CIP_VALUE_UNKNOWN = 0x3009,
      FLAGS_NOT_SUPPORTED = 0x300A,
      FACILITY_NOT_SUPPORTED = 0x300B,
      DATA_LENGTH_NOT_SUPPORTED = 0x300C,
      RESET_PROC_NOT_SUPPORTED = 0x300D,
      TEI_ASSIGNMENT_FAILED = 0x300E,
      UNSUPPORTED_INTEROPERABILITY = 0x300F,
      REQUEST_NOT_ALLOWED_IN_THIS_STATE = 0x3010,
      FACILITY_FUNCTION_NOT_SUPPORTED = 0x3011,
      GENERAL_ERROR = 0xFFFF
    }
    /// <summary>
    /// Compatibilty Information Profile mask
    /// </summary>
    public enum CIP_Mask: int
    {
      DISABLE_SIGNALING = 0,
      ANY_MATCH = 0x1,
      SPEECH = 0x2,
      UNRESTR_DIG_INFO = 0x4,
      RESTR_DIG_INFO = 0x8,
      _3kHz_AUDIO = 0x10,
      _7kHz_AUDIO = 0x20,
      VIDEO = 0x40,
      PACKET_MODE = 0x80,
      _56kbs_RATE_ADAPT = 0x100,
      UNRESTR_DIG_INFO_WITH_TONES = 0x200,
      TELEPHONY = 0x10000,
      GROUP_2_3_FACSIMILE = 0x20000,
      GROPU_4_FACSIMILE = 0x40000,
      TELETEX_BASIC_AND_MIXED = 0x80000,
      TELETEX_BASIC_AND_PROCESSABLE = 0x100000,
      TELETEX_BASIC = 0x200000,
      INTERWORKIN_FOR_VIDEOTEX = 0x400000,
      TELEX = 0x800000,
      MESSAGE_HANDLING_SYSTEMS = 0x1000000,
      OSI_APPLICATION = 0x2000000,
      _7kHz_TELEPHONY = 0x4000000,
      VIDEO_TELEPHONY_FIRST_CON = 0x8000000,
      VIDEO_TELEPHONY_SECOND_CON = 10000000
    }
    /// <summary>
    /// Compatibilty Information Profile value
    /// </summary>
    public enum CIP_Value: int
    {
      NO_PROFILE = 0,
      SPEECH = 1,
      UNRESTR_DIG_INFO = 2,
      RESTR_DIG_INFO = 3,
      _3kHz_AUDIO = 4,
      _7kHz_AUDIO = 5,
      VIDEO = 6,
      PACKET_MODE = 7,
      _56kbs_RATE_ADAPT = 8,
      UNRESTR_DIG_INFO_WITH_TONES = 9,
      TELEPHONY = 16,
      GROUP_2_3_FACSIMILE = 17,
      GROPU_4_FACSIMILE = 18,
      TELETEX_BASIC_AND_MIXED = 19,
      TELETEX_BASIC_AND_PROCESSABLE = 20,
      TELETEX_BASIC = 21,
      INTERWORKIN_FOR_VIDEOTEX = 22,
      TELEX = 23,
      MESSAGE_HANDLING_SYSTEMS = 24,
      OSI_APPLICATION = 25,
      _7kHz_TELEPHONY = 26,
      VIDEO_TELEPHONY_FIRST_CON = 27,
      VIDEO_TELEPHONY_SECOND_CON = 28
    }
    #endregion
    #region CAPI2032 external definitions
    /// <summary>
    /// Registeres application to CAPI
    /// </summary>
    [DllImport( "capi2032.dll" )]
    public static extern TInterfaceError CAPI_REGISTER( uint MessageBufferSize, uint log, uint max, uint max2, ref uint app );
    /// <summary>
    /// Unregisters application from CAPI
    /// </summary>
    [DllImport( "capi2032.dll" )]
    public static extern TInterfaceError CAPI_RELEASE( uint appID );
    /// <summary>
    /// Puts CAPI message to the message queue
    /// </summary>
    [DllImport( "capi2032.dll" )]
    public static extern TInterfaceError CAPI_PUT_MESSAGE( uint appID, IntPtr CAPImsgptr );
    /// <summary>
    /// Retrieves CAPI message from the message queue
    /// </summary>
    [DllImport( "capi2032.dll" )]
    public static extern TInterfaceError CAPI_GET_MESSAGE( uint appID, ref IntPtr CAPImsgptr );
    /// <summary>
    /// Waits for an asynchronous event from CAPI
    /// </summary>
    [DllImport( "capi2032.dll" )]
    public static extern TInterfaceError CAPI_WAIT_FOR_SIGNAL( uint appID );

    /// <summary>
    /// The application uses this function to determine the capabilities of COMMON-ISDNAPI.  
    /// The application provides a 16:16 (segmented) protected-mode pointer to a buffer 
    /// of 64 bytes in szBuffer. COMMON-ISDN-API copies information about implemented 
    /// features, the number of controllers and supported protocols to this buffer. 
    /// CtrlNr contains the number of the controller (bits 0..6) for which this 
    /// information is requested.
    /// </summary>
    /// <param name="szBuffer">16:16 (segmented) protected-mode pointer to a buffer of 64 bytes</param>
    /// <param name="CtrlNr">Number of Controller. If 0, only the number of controllers installed is provided to the application.</param>
    /// <returns>Coded as described in parameter Info, class 0x11xx</returns>
    [DllImport( "capi2032.dll" )]
    public static extern TInterfaceError CAPI_GET_PROFILE( IntPtr szBuffer, uint CtrlNr );

    #endregion
  }
}
