//<summary>
//  Title   : Wrapper of the serial port handling API  provided by the operating system 
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
namespace CAS.Lib.CommonBus.CommunicationLayer.RS.Win32API
{
  using System;
  using System.Runtime.InteropServices;
  /// <summary>
  /// Wrapper of the serial port handling API  provided by the operating system 
  /// </summary>
  internal class RSWrapper
  {
    //Constants for errors:
    internal const UInt32 ERROR_FILE_NOT_FOUND = 2;
    internal const UInt32 ERROR_INVALID_NAME = 123;
    internal const UInt32 ERROR_ACCESS_DENIED = 5;
    internal const UInt32 ERROR_IO_PENDING = 997;

    //Constants for return value:
    internal const Int32 INVALID_HANDLE_VALUE = -1;

    //Constants for dwFlagsAndAttributes:
    internal const UInt32 FILE_FLAG_OVERLAPPED = 0x40000000;

    //Constants for dwCreationDisposition:
    internal const UInt32 OPEN_EXISTING = 3;

    //Constants for dwDesiredAccess:
    internal const UInt32 GENERIC_READ = 0x80000000;
    internal const UInt32 GENERIC_WRITE = 0x40000000;

    /// <summary>
    /// Parity settings
    /// </summary>
    public enum ParityBit
    {
      /// <summary>
      /// Characters do not have a parity bit.
      /// </summary>
      none = 0,
      /// <summary>
      /// If there are an odd number of 1s in the data bits, the parity bit is 1.
      /// </summary>
      odd = 1,
      /// <summary>
      /// If there are an even number of 1s in the data bits, the parity bit is 1.
      /// </summary>
      even = 2,
      /// <summary>
      /// The parity bit is always 1.
      /// </summary>
      mark = 3,
      /// <summary>
      /// The parity bit is always 0.
      /// </summary>
      space = 4
    };
    /// <summary>
    /// Stop bit settings
    /// </summary>
    public enum StopBits
    {
      /// <summary>
      /// Line is asserted for 1 bit duration at end of each character
      /// </summary>
      one = 0,
      /// <summary>
      /// Line is asserted for 1.5 bit duration at end of each character
      /// </summary>
      onePointFive = 1,
      /// <summary>
      /// Line is asserted for 2 bit duration at end of each character
      /// </summary>
      two = 2
    };
    /// <summary>
    /// Uses for RTS or DTR pins
    /// </summary>
    internal enum HSOutput
    {
      /// <summary>
      /// Pin is asserted when this station is able to receive data.
      /// </summary>
      handshake = 2,
      /// <summary>
      /// Pin is asserted when this station is transmitting data (RTS on NT, 2000 or XP only).
      /// </summary>
      gate = 3,
      /// <summary>
      /// Pin is asserted when this station is online (port is open).
      /// </summary>
      online = 1,
      /// <summary>
      /// Pin is never asserted.
      /// </summary>
      none = 0
    };
    /// <summary>
    /// Standard handshake methods
    /// </summary>
    public enum Handshake
    {
      /// <summary>
      /// No handshaking
      /// </summary>
      none,
      /// <summary>
      /// Software handshaking using Xon / Xoff
      /// </summary>
      XonXoff,
      /// <summary>
      /// Hardware handshaking using CTS / RTS
      /// </summary>
      CtsRts,
      /// <summary>
      /// Hardware handshaking using DSR / DTR
      /// </summary>
      DsrDtr
    }
    [StructLayout( LayoutKind.Sequential )]
    internal struct COMMTIMEOUTS
    {
      internal Int32 ReadIntervalTimeout;
      internal Int32 ReadTotalTimeoutMultiplier;
      internal Int32 ReadTotalTimeoutConstant;
      internal Int32 WriteTotalTimeoutMultiplier;
      internal Int32 WriteTotalTimeoutConstant;
    }
    [StructLayout( LayoutKind.Sequential )]
    internal struct DCB
    {
      internal Int32 DCBlength;
      internal Int32 BaudRate;
      internal Int32 PackedValues;
      internal Int16 wReserved;
      internal Int16 XonLim;
      internal Int16 XoffLim;
      internal Byte ByteSize;
      private Byte iParity;
      private Byte iStopBits;
      internal Byte XonChar;
      internal Byte XoffChar;
      internal Byte ErrorChar;
      internal Byte EofChar;
      internal Byte EvtChar;
      internal Int16 wReserved1;
      internal StopBits stopBits
      {
        set
        { iStopBits = (byte)value; }
        get
        { return (StopBits)iStopBits; }
      }
      internal ParityBit Parity
      {
        get
        { return (ParityBit)iParity; }
        set
        {
          iParity = (byte)value;
          if ( ( value == ParityBit.odd ) || ( value == ParityBit.even ) )
            PackedValues |= 0x0002;
          else
            PackedValues &= 0x0002;
        }
      }
      internal void init
        ( bool parity, bool outCTS, bool outDSR, int dtr, bool inDSR, bool txc, bool xOut, bool xIn, int rts )
      {
        DCBlength = 28;
        PackedValues = 0x8001;
        if ( parity )
          PackedValues |= 0x0002;
        if ( outCTS )
          PackedValues |= 0x0004;
        if ( outDSR )
          PackedValues |= 0x0008;
        PackedValues |= ( ( dtr & 0x0003 ) << 4 );
        if ( inDSR )
          PackedValues |= 0x0040;
        if ( txc )
          PackedValues |= 0x0080;
        if ( xOut )
          PackedValues |= 0x0100;
        if ( xIn )
          PackedValues |= 0x0200;
        PackedValues |= ( ( rts & 0x0003 ) << 12 );
      }
    }
    /// <summary>
    /// The GetLastError function retrieves the calling thread's last-error code value. The last-error code is maintained on 
    /// a per-thread basis. Multiple threads do not overwrite each other's last-error code.
    /// /// </summary>
    /// <returns>The return value is the calling thread's last-error code value. Functions set this value by calling the 
    /// SetLastError function. The Return Value section of each reference page notes the conditions under which the function 
    /// sets the last-error code.
    /// Windows Me/98/95:  Functions that are actually implemented in 16-bit code do not set the last-error code. You should 
    /// ignore the last-error code when you call these functions. They include window management functions, GDI functions, 
    /// and Multimedia functions. For functions that do set the last-error code, you should not rely on GetLastError returning 
    /// the same value under Windows Me/98/95 and Windows NT.
    /// </returns>
    [DllImport( "kernel32" )]
    public static extern UInt32 GetLastError();

    /// <summary>
    /// Opening Testing and Closing the Port Handle.
    /// </summary>
    [DllImport( "kernel32.dll", SetLastError = true )]
    internal static extern IntPtr CreateFile( String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode,
      IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes,
      IntPtr hTemplateFile );

    [DllImport( "kernel32.dll" )]
    internal static extern Boolean CloseHandle( IntPtr hObject );

    /// <summary>
    /// The GetHandleInformation function retrieves certain properties of an object handle.
    /// </summary>
    /// <param name="hObject">[in] Handle to an object whose information is to be retrieved. 
    /// You can specify a handle to one of the following types of objects: access token, event, file, file mapping, job, 
    /// mailslot, mutex, pipe, printer, process, registry key, semaphore, serial communication device, socket, thread, 
    /// or waitable timer.
    /// Windows Server 2003, Windows XP/2000:  This parameter can also be a handle to a console input buffer or a console 
    /// screen buffer.
    /// </param>
    /// <param name="lpdwFlags">[out] Pointer to a variable that receives a set of bit flags that specify properties of the 
    /// object handle. The following values are defined. 
    /// </param>
    /// <returns>If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport( "kernel32.dll" )]
    internal static extern Boolean GetHandleInformation( IntPtr hObject, out UInt32 lpdwFlags );
    /// <summary>
    /// Manipulating the communications settings. The GetCommState function retrieves the current control 
    /// settings for a specified communications device.
    /// </summary>
    [DllImport( "kernel32.dll" )]
    internal static extern Boolean GetCommState( IntPtr hFile, ref DCB lpDCB );
    [DllImport( "kernel32.dll" )]
    internal static extern Boolean GetCommTimeouts( IntPtr hFile, out COMMTIMEOUTS lpCommTimeouts );

    [DllImport( "kernel32.dll" )]
    internal static extern Boolean BuildCommDCBAndTimeouts
      ( String lpDef, ref DCB lpDCB, ref COMMTIMEOUTS lpCommTimeouts );

    [DllImport( "kernel32.dll" )]
    internal static extern Boolean SetCommState( IntPtr hFile, [In] ref DCB lpDCB );
    /// <summary>
    /// The SetCommTimeouts function sets the time-out parameters for all read and write operations on a specified communications device.
    /// </summary>
    /// <param name="hFile">[in] Handle to the communications device. The CreateFile function returns this handle. </param>
    /// <param name="lpCommTimeouts">[in] Pointer to a COMMTIMEOUTS structure that contains the new time-out values.</param>
    /// <returns></returns>
    [DllImport( "kernel32.dll" )]
    internal static extern Boolean SetCommTimeouts( IntPtr hFile, [In] ref COMMTIMEOUTS lpCommTimeouts );

    /// <summary>
    /// The SetupComm function initializes the communications parameters for a specified communications device.
    /// </summary>
    /// <param name="hFile">[in] Handle to the communications device. The CreateFile function returns this handle.</param>
    /// <param name="dwInQueue">[in] Recommended size of the device's internal input buffer, in bytes.</param>
    /// <param name="dwOutQueue">[in] Recommended size of the device's internal output buffer, in bytes.
    /// </param>
    /// <returns>If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    /// <remarks>
    /// After a process uses the CreateFile function to open a handle to a communications device, but before doing any I/O with 
    /// the device, it can call SetupComm to set the communications parameters for the device. If it does not set them, 
    /// the device uses the default parameters when the first call to another communications function occurs.
    /// The dwInQueue and dwOutQueue parameters specify the recommended sizes for the internal buffers used by the driver for 
    /// the specified device. For example, YMODEM protocol packets are slightly larger than 1024 bytes. Therefore, a recommended 
    /// buffer size might be 1200 bytes for YMODEM communications. For Ethernet-based communications, a recommended buffer size 
    /// might be 1600 bytes, which is slightly larger than a single Ethernet frame.
    /// </remarks>
    [DllImport( "kernel32.dll" )]
    internal static extern Boolean SetupComm( IntPtr hFile, UInt32 dwInQueue, UInt32 dwOutQueue );
    /// <summary>
    /// Reading and writing.
    /// </summary>
    /// <summary>
    /// The WriteFile function writes data to a file and is designed for both synchronous and asynchronous operation. 
    /// The function starts writing data to the file at the position indicated by the file pointer. After the write operation 
    /// has been completed, the file pointer is adjusted by the number of bytes actually written, except when the file is opened 
    /// with FILE_FLAG_OVERLAPPED. If the file handle was created for overlapped input and output (I/O), the application must 
    /// adjust the position of the file pointer after the write operation is finished.
    /// This function is designed for both synchronous and asynchronous operation. The WriteFileEx function is designed 
    /// solely for asynchronous operation. It lets an application perform other processing during a file write operation.
    /// </summary>
    /// <param name="fFile">[in] Handle to the file. The file handle must have been created with the GENERIC_WRITE access right. 
    /// For more information, see File Security and Access Rights. 
    /// For asynchronous write operations, hFile can be any handle opened with the FILE_FLAG_OVERLAPPED flag by the CreateFile 
    /// function, or a socket handle returned by the socket or accept function.
    /// </param>
    /// <param name="lpBuffer">[in] Pointer to the buffer containing the data to be written to the file. 
    /// </param>
    /// <param name="nNumberOfBytesToWrite">[in] Number of bytes to be written to the file.
    /// A value of zero specifies a null write operation. The behavior of a null write operation depends on the underlying 
    /// file system. To truncate or extend a file, use the SetEndOfFile function.
    /// Named pipe write operations across a network are limited to 65,535 bytes.
    /// </param>
    /// <param name="lpNumberOfBytesWritten">[out] Pointer to the variable that receives the number of bytes written. 
    /// WriteFile sets this value to zero before doing any work or error checking. 
    /// If lpOverlapped is NULL, lpNumberOfBytesWritten cannot be NULL. If lpOverlapped is not NULL, lpNumberOfBytesWritten can 
    /// be NULL. If this is an overlapped write operation, you can get the number of bytes written by calling GetOverlappedResult. 
    /// If hFile is associated with an I/O completion port, you can get the number of bytes written by calling 
    /// GetQueuedCompletionStatus.
    /// If I/O completion ports are used and you are using a callback routine to free the memory allocated to the OVERLAPPED 
    /// structure pointed to by the lpOverlapped parameter, specify NULL as the value of this parameter to avoid a memory 
    /// corruption problem during the deallocation. This memory corruption problem will cause an invalid number of bytes to be 
    /// returned in this parameter.
    /// Windows Me/98/95:  This parameter cannot be NULL.
    /// </param>
    /// <param name="lpOverlapped">[in] Pointer to an OVERLAPPED structure. This structure is required if hFile was opened with 
    /// FILE_FLAG_OVERLAPPED.
    /// If hFile was opened with FILE_FLAG_OVERLAPPED, the lpOverlapped parameter must not be NULL. It must point to a valid 
    /// OVERLAPPED structure. If hFile was opened with FILE_FLAG_OVERLAPPED and lpOverlapped is NULL, the function can incorrectly 
    /// report that the write operation is complete.
    /// If hFile was opened with FILE_FLAG_OVERLAPPED and lpOverlapped is not NULL, the write operation starts at the offset specified 
    /// in the OVERLAPPED structure and WriteFile may return before the write operation has been completed. In this case, 
    /// WriteFile returns FALSE and the GetLastError function returns ERROR_IO_PENDING. This allows the calling process to 
    /// continue processing while the write operation is being completed. The event specified in the OVERLAPPED structure is set 
    /// to the signaled state upon completion of the write operation.
    /// If hFile was not opened with FILE_FLAG_OVERLAPPED and lpOverlapped is NULL, the write operation starts at the current file 
    /// position and WriteFile does not return until the operation has been completed.
    /// WriteFile resets the event specified by the hEvent member of the OVERLAPPED structure to a nonsignaled state when it 
    /// begins the I/O operation. Therefore, there is no need for the caller to do so.
    /// If hFile was not opened with FILE_FLAG_OVERLAPPED and lpOverlapped is not NULL, the write operation starts at the offset 
    /// specified in the OVERLAPPED structure and WriteFile does not return until the write operation has been completed.
    /// Windows Me/98/95:  For operations on files, disks, pipes, or mailslots, this parameter must be NULL; a pointer to 
    /// an OVERLAPPED structure causes the call to fail. However, the function supports overlapped I/O on serial and parallel 
    /// ports.
    /// </param>
    /// <returns>If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, 
    /// call GetLastError.
    /// </returns>
    [DllImport( "kernel32.dll", SetLastError = true )]
    internal static extern Boolean WriteFile
      ( IntPtr fFile, Byte[] lpBuffer, UInt32 nNumberOfBytesToWrite, out UInt32 lpNumberOfBytesWritten,
      IntPtr lpOverlapped );

    [DllImport( "kernel32.dll", SetLastError = true )]
    internal static extern Boolean WriteFile
      ( IntPtr fFile, IntPtr lpBuffer, UInt32 nNumberOfBytesToWrite, out UInt32 lpNumberOfBytesWritten,
      IntPtr lpOverlapped );

    [StructLayout( LayoutKind.Sequential )]
    internal struct OVERLAPPED
    {
      internal UIntPtr Internal;
      internal UIntPtr InternalHigh;
      internal UInt32 Offset;
      internal UInt32 OffsetHigh;
      internal IntPtr hEvent;
    }

    [DllImport( "kernel32.dll" )]
    internal static extern Boolean SetCommMask( IntPtr hFile, UInt32 dwEvtMask );

    // Constants for dwEvtMask:
    internal const UInt32 EV_RXCHAR = 0x0001;
    internal const UInt32 EV_RXFLAG = 0x0002;
    internal const UInt32 EV_TXEMPTY = 0x0004;
    internal const UInt32 EV_CTS = 0x0008;
    internal const UInt32 EV_DSR = 0x0010;
    internal const UInt32 EV_RLSD = 0x0020;
    internal const UInt32 EV_BREAK = 0x0040;
    internal const UInt32 EV_ERR = 0x0080;
    internal const UInt32 EV_RING = 0x0100;
    internal const UInt32 EV_PERR = 0x0200;
    internal const UInt32 EV_RX80FULL = 0x0400;
    internal const UInt32 EV_EVENT1 = 0x0800;
    internal const UInt32 EV_EVENT2 = 0x1000;

    [DllImport( "kernel32.dll", SetLastError = true )]
    internal static extern Boolean WaitCommEvent( IntPtr hFile, IntPtr lpEvtMask, IntPtr lpOverlapped );

    [DllImport( "kernel32.dll" )]
    internal static extern Boolean CancelIo( IntPtr hFile );

    /// <summary>
    /// The ReadFile function reads data from a file, starting at the position indicated by the file pointer.
    /// After the read operation has been completed, the file pointer is adjusted by the number of bytes 
    /// actually read, unless the file handle is created with the overlapped attribute. If the file handle is
    /// created for overlapped input and output (I/O), the application must adjust the position of the file 
    /// pointer after the read operation.
    /// This function is designed for both synchronous and asynchronous operation. The ReadFileEx function 
    /// is designed solely for asynchronous operation. It lets an application perform other processing during 
    /// a file read operation.
    /// </summary>
    /// <param name="hFile">[in] Handle to the file to be read. The file handle must have been created with 
    /// the GENERIC_READ access right. For more information, see File Security and Access Rights. 
    /// For asynchronous read operations, hFile can be any handle opened with the FILE_FLAG_OVERLAPPED flag by 
    /// the CreateFile function, or a socket handle returned by the socket or accept function.
    /// Windows Me/98/95:  For asynchronous read operations, hFile can be a communications resource opened with 
    /// the FILE_FLAG_OVERLAPPED flag by CreateFile, or a socket handle returned by socket or accept. You cannot
    /// perform asynchronous read operations on mailslots, named pipes, or disk files. </param>
    /// <param name="lpBuffer">[out] Pointer to the buffer that receives the data read from the file. </param>
    /// <param name="nNumberOfBytesToRead">[in] Number of bytes to be read from the file.</param>
    /// <param name="nNumberOfBytesRead">[out] Pointer to the variable that receives the number of bytes read.
    /// ReadFile sets this value to zero before doing any work or error checking. If this parameter is zero when 
    /// ReadFile returns TRUE on a named pipe, the other end of the message-mode pipe called the WriteFile 
    /// function with nNumberOfBytesToWrite set to zero. 
    /// If lpOverlapped is NULL, lpNumberOfBytesRead cannot be NULL. If lpOverlapped is not NULL,
    /// lpNumberOfBytesRead can be NULL. If this is an overlapped read operation, you can get the number 
    /// of bytes read by calling GetOverlappedResult. If hFile is associated with an I/O completion port, 
    /// you can get the number of bytes read by calling GetQueuedCompletionStatus.
    /// If I/O completion ports are used and you are using a callback routine to free the memory allocated to 
    /// the OVERLAPPED structure pointed to by the lpOverlapped parameter, specify NULL as the value of this
    /// parameter to avoid a memory corruption problem during the deallocation. This memory corruption problem 
    /// will cause an invalid number of bytes to be returned in this parameter.
    /// Windows Me/98/95:  This parameter cannot be NULL.
    /// </param>
    /// <param name="lpOverlapped">[in] Pointer to an OVERLAPPED structure. This structure is required if hFile 
    /// was created with FILE_FLAG_OVERLAPPED. 
    /// If hFile was opened with FILE_FLAG_OVERLAPPED, the lpOverlapped parameter must not be NULL. It must 
    /// point to a valid OVERLAPPED structure. If hFile was created with FILE_FLAG_OVERLAPPED and lpOverlapped 
    /// is NULL, the function can incorrectly report that the read operation is complete.
    /// If hFile was opened with FILE_FLAG_OVERLAPPED and lpOverlapped is not NULL, the read operation starts at
    /// the offset specified in the OVERLAPPED structure and ReadFile may return before the read operation has
    /// been completed. In this case, ReadFile returns FALSE and the GetLastError function returns
    /// ERROR_IO_PENDING.
    /// This allows the calling process to continue while the read operation finishes. The event specified in
    /// the OVERLAPPED structure is set to the signaled state upon completion of the read operation.
    /// If hFile was not opened with FILE_FLAG_OVERLAPPED and lpOverlapped is NULL, the read operation starts 
    /// at the current file position and ReadFile does not return until the operation has been completed.
    /// If hFile is not opened with FILE_FLAG_OVERLAPPED and lpOverlapped is not NULL, the read operation starts 
    /// at the offset specified in the OVERLAPPED structure. ReadFile does not return until the read operation 
    /// has been completed.
    /// Windows 95/98/Me:  For operations on files, disks, pipes, or mailslots, this parameter must be NULL; 
    /// a pointer to an OVERLAPPED structure causes the call to fail. However, you can perform overlapped I/O on 
    /// serial and parallel ports.</param>
    /// <returns>The ReadFile function returns when one of the following conditions is met: a write operation
    /// ompletes on the write end of the pipe, the number of bytes requested has been read, or an error occurs.
    /// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. 
    /// To get extended error information, call GetLastError.
    /// If the return value is nonzero and the number of bytes read is zero, the file pointer was beyond the
    /// current end of the file at the time of the read operation. However, if the file was opened with 
    /// FILE_FLAG_OVERLAPPED and lpOverlapped is not NULL, the return value is zero and GetLastError returns
    /// RROR_HANDLE_EOF when the file pointer goes beyond the current end of file.</returns>
    [DllImport( "kernel32.dll", SetLastError = true )]
    internal static extern Boolean ReadFile
      (
      IntPtr hFile, [Out] Byte[] lpBuffer, UInt32 nNumberOfBytesToRead, out UInt32 nNumberOfBytesRead,
      IntPtr lpOverlapped
      );
    [DllImport( "kernel32.dll", SetLastError = true )]
    internal static extern Boolean ReadFile( IntPtr hFile, IntPtr lpBuffer, UInt32 nNumberOfBytesToRead,
      out UInt32 nNumberOfBytesRead, IntPtr lpOverlapped );

    [DllImport( "kernel32.dll" )]
    internal static extern Boolean TransmitCommChar( IntPtr hFile, Byte cChar );

    /// <summary>
    /// Control port functions.
    /// </summary>
    [DllImport( "kernel32.dll" )]
    internal static extern Boolean EscapeCommFunction( IntPtr hFile, UInt32 dwFunc );

    // Constants for dwFunc:
    internal const UInt32 SETXOFF = 1;
    internal const UInt32 SETXON = 2;
    internal const UInt32 SETRTS = 3;
    internal const UInt32 CLRRTS = 4;
    internal const UInt32 SETDTR = 5;
    internal const UInt32 CLRDTR = 6;
    internal const UInt32 RESETDEV = 7;
    internal const UInt32 SETBREAK = 8;
    internal const UInt32 CLRBREAK = 9;

    [DllImport( "kernel32.dll" )]
    internal static extern Boolean GetCommModemStatus( IntPtr hFile, out UInt32 lpModemStat );

    // Constants for lpModemStat:
    internal const UInt32 MS_CTS_ON = 0x0010;
    internal const UInt32 MS_DSR_ON = 0x0020;
    internal const UInt32 MS_RING_ON = 0x0040;
    internal const UInt32 MS_RLSD_ON = 0x0080;

    /// <summary>
    /// Status Functions.
    /// </summary>
    [DllImport( "kernel32.dll", SetLastError = true )]
    internal static extern Boolean GetOverlappedResult( IntPtr hFile, IntPtr lpOverlapped,
      out UInt32 nNumberOfBytesTransferred, Boolean bWait );

    // The ClearCommError function retrieves information about a communications error and reports the current
    // status of a communications device. The function is called when a communications error occurs, and it 
    // clears the device's error flag to enable additional input and output (I/O) operations.
    [DllImport( "kernel32.dll" )]
    internal static extern Boolean ClearCommError( IntPtr hFile, out UInt32 lpErrors, IntPtr lpStat );
    [DllImport( "kernel32.dll" )]
    internal static extern Boolean ClearCommError( IntPtr hFile, out UInt32 lpErrors, out COMSTAT cs );

    //Constants for lpErrors:
    internal const UInt32 CE_RXOVER = 0x0001;
    internal const UInt32 CE_OVERRUN = 0x0002;
    internal const UInt32 CE_RXPARITY = 0x0004;
    internal const UInt32 CE_FRAME = 0x0008;
    internal const UInt32 CE_BREAK = 0x0010;
    internal const UInt32 CE_TXFULL = 0x0100;
    internal const UInt32 CE_PTO = 0x0200;
    internal const UInt32 CE_IOE = 0x0400;
    internal const UInt32 CE_DNS = 0x0800;
    internal const UInt32 CE_OOP = 0x1000;
    internal const UInt32 CE_MODE = 0x8000;

    [StructLayout( LayoutKind.Sequential )]
    internal struct COMSTAT
    {
      internal const uint fCtsHold = 0x1;
      internal const uint fDsrHold = 0x2;
      internal const uint fRlsdHold = 0x4;
      internal const uint fXoffHold = 0x8;
      internal const uint fXoffSent = 0x10;
      internal const uint fEof = 0x20;
      internal const uint fTxim = 0x40;
      internal UInt32 Flags;
      internal UInt32 cbInQue;
      internal UInt32 cbOutQue;
    }
    [DllImport( "kernel32.dll" )]
    internal static extern Boolean GetCommProperties( IntPtr hFile, out COMMPROP cp );
    [StructLayout( LayoutKind.Sequential )]
    internal struct COMMPROP
    {
      internal UInt16 wPacketLength;
      internal UInt16 wPacketVersion;
      internal UInt32 dwServiceMask;
      internal UInt32 dwReserved1;
      internal UInt32 dwMaxTxQueue;
      internal UInt32 dwMaxRxQueue;
      internal UInt32 dwMaxBaud;
      internal UInt32 dwProvSubType;
      internal UInt32 dwProvCapabilities;
      internal UInt32 dwSettableParams;
      internal UInt32 dwSettableBaud;
      internal UInt16 wSettableData;
      internal UInt16 wSettableStopParity;
      internal UInt32 dwCurrentTxQueue;
      internal UInt32 dwCurrentRxQueue;
      internal UInt32 dwProvSpec1;
      internal UInt32 dwProvSpec2;
      internal Byte wcProvChar;
    }
    /// <summary>The baud rate. </summary>
    public enum BAUD_RATE: uint
    {
      /// <summary>baudRate = 110</summary>
      CBR____110 = 110,
      /// <summary>baudRate = 300</summary>
      CBR____300 = 300,
      /// <summary>baudRate = 600</summary>
      CBR____600 = 600,
      /// <summary>baudRate = 1200</summary>
      CBR___1200 = 1200,
      /// <summary>baudRate = 2400</summary>
      CBR___2400 = 2400,
      /// <summary>baudRate = 4800</summary>
      CBR___4800 = 4800,
      /// <summary>baudRate = 9600</summary>
      CBR___9600 = 9600,
      /// <summary>baudRate = 14400</summary>
      CBR__14400 = 14400,
      /// <summary>baudRate = 19200</summary>
      CBR__19200 = 19200,
      /// <summary>baudRate = 38400</summary>
      CBR__38400 = 38400,
      /// <summary>baudRate = 56000</summary>
      CBR__56000 = 56000,
      /// <summary>baudRate = 57600</summary>
      CBR__57600 = 57600,
      /// <summary>baudRate = 115200</summary>
      CBR_115200 = 115200,
      /// <summary>baudRate = 128000</summary>
      CBR_128000 = 128000,
      /// <summary>baudRate = 256000</summary>
      CBR_256000 = 256000
    }
  }
}//Win32API.RS