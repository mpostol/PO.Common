//<summary>
//  Title   : UMessage
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//
//    20080904: mbrzezny:  Writes functions returns true if operation has succeeded and false if not
//    2008-06-20: Mzbrzezny - GetManagedBuffer function is changed: now it returns the whole buffer 
//                            (from 0 offset to UserDataLength), previously the buffer 
//                            was returned from 0 to current offset.
//    2007-05-21 - Mzbrzezny - added tracing
//    MPOstol - 06-02-2007
//      GetManagedBuffer copies only bytes added to buffer instead of copping the whole buffer.
//    MPostol - 12-10-2003: 
//      uMUserDataLength was added and all methods rearranged accordingly
//      AddByte was added
//      reverse order for quantity larger than single byte is set by creator and cannot be chenged
//    MPostol - 04-11-2003
//      Assert ertrors cause system reboot 
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CAS.Lib.CommonBus.CommunicationLayer
{
  /// <summary>
  ///The Message class defines the base class for all protocol oriented message classes. 
  ///It support easy deposit and retrieval of primitive data types as binary values in the bytes stream. 
  ///To convert values marshaling and unmanaged memory is used.  
  /// </summary>
  public class UMessage: IDisposable
  {
    #region PRIVATE
    #region trace
    /// <summary>
    /// this name apears in the tracing messages
    /// </summary>
    protected string m_traceName = "TraceUMessage";
    private TraceSource m_TraceSource;
    #endregion
    private IntPtr uMBufforPtr;
    private readonly ushort uMBufforMaxLength = 0;
    private ushort uMBufforOffset = 0;
    private ushort uMUserDataLength = 0;
    private readonly bool uMBufforReverse;
    private System.Collections.Stack block_offset = new System.Collections.Stack();
    private void reverse( IntPtr ptr, ref ushort offset, ushort size )
    {
      byte lb, hb;
      ushort offH, offL;
      offL = offset;
      offset += size;
      offH = (ushort)( offset - 1 );
      while ( offL < offH )
      {
        hb = Marshal.ReadByte( ptr, offH );
        lb = Marshal.ReadByte( ptr, offL );
        Marshal.WriteByte( ptr, offH, lb );
        Marshal.WriteByte( ptr, offL, hb );
        offL += 1;
        offH -= 1;
      }
    }
    private bool TestIfOperationIsInsideBuffer( object val, string operation, int offsettobetested )
    {
      if ( uMBufforOffset + offsettobetested < uMUserDataLength )
        return true;
      string frm = this.GetType().FullName.ToString() + ": {0} tries to write outside buffor: byte: {1} message: {2}"; //, StackTrace: {3}";
      TraceEvent( TraceEventType.Warning, 80, String.Format( frm, operation, val, this.ToString() ) ); //, new System.Diagnostics.StackTrace( true ).ToString() ) );
      return false;
    }
    private static CAS.Lib.RTLib.Processes.Assertion myAssert =
      new CAS.Lib.RTLib.Processes.Assertion( "Assert error in BaseStation.UMessage", 300, false );
    #endregion
    #region PUBLIC
    /// <summary>
    /// Writes a trace event message to the trace listeners in the System.Diagnostics.TraceSource.Listeners collection 
    /// using the specified event type and event identifier.
    /// </summary>
    /// <param name="pEventType">
    /// One of the System.Diagnostics.TraceEventType values that specifies the event type of the trace data.
    /// </param>
    /// <param name="pId">A numeric identifier for the event.</param>
    /// <param name="pMessage">The trace message to write.</param>
    [Conditional( "TRACE" )]
    protected void TraceEvent( TraceEventType pEventType, int pId, string pMessage )
    {
      m_TraceSource.TraceEvent( pEventType, pId, m_traceName + ":" + pMessage );
    }
    /// <summary>
    /// Return information about this message as string.
    /// </summary>
    public string STRING
    {
      get
      {
        string a = "UMessage: " + "offset=" + this.offset.ToString() + " maxlen(userdata)=" + this.uMUserDataLength + "data:";
        for ( int i = 0; i < this.userDataLength; i++ )
        {
          a = a + " | " + this[ i ].ToString() + "(0x" + this[ i ].ToString( "X" ) + ")(pos:" + i.ToString() + ")";
          if ( offset == i )
            a += "current offset ";
        }
        return a;
      }
    }
    /// <summary>
    /// Retrieves a string representation of the object.
    /// </summary>
    /// <returns>Description of the message:[station][address][myDataType][length]</returns>
    public override string ToString()
    {
      return STRING;
    }
    /// <summary>
    /// true if buffer is not empty.
    /// </summary>
    public bool NotEmpty
    {
      get { return ( ( userDataLength > 0 ) && ( offset < userDataLength ) ); }
    }
    /// <summary>
    /// indexer allows random access to umanaged buffor
    /// </summary>
    public virtual byte this[ int index ]
    {
      get
      {
        myAssert.Assert( index < uMUserDataLength, 100 );
        return Marshal.ReadByte( uMBufforPtr, index );
      }
      set
      {
        myAssert.Assert( index < uMUserDataLength, 110 );
        Marshal.WriteByte( uMBufforPtr, index, value );
      }
    }
    /// <summary>
    /// Gets and sets offset to current byte 
    /// </summary>
    public virtual ushort offset
    {
      get
      {
        return uMBufforOffset;
      }
      set
      {
        if ( value <= uMUserDataLength )
          uMBufforOffset = value;
        else
          uMBufforOffset = uMUserDataLength;
      }
    }
    /// <summary>
    /// Gets pointer tu message in the umanaged memory
    /// </summary>
    public IntPtr uMessagePtr
    {
      get
      {
        return uMBufforPtr;
      }
    }
    /// <summary>
    /// Number of bytes deposited in the buffer. Position of the firs empty position in the buffer.
    /// </summary>
    public ushort userBuffLength
    {
      get { return uMBufforMaxLength; }
    }
    /// <summary>
    /// User data length
    /// </summary>
    /// <returns>length in bytes</returns>
    public ushort userDataLength
    {
      get
      {
        return uMUserDataLength;
      }
      set
      {
        if ( value <= uMBufforMaxLength )
          uMUserDataLength = value;
        else
          uMUserDataLength = uMBufforMaxLength;
      }
    }
    /// <summary>
    ///  Copies data from unmanaged memory pointer to UMessage starting 
    ///  at offset 0. After copping offset point on next free byte in UMesasage.
    /// </summary>
    /// <param name="source">The memory pointer to copy from.</param>
    /// <param name="length">The number of bytes to copy.</param>
    public void CopyToBuffor( IntPtr source, uint length )
    {
      if ( length > uMBufforMaxLength )
        length = uMBufforMaxLength;
      byte lastVal;
      uMBufforOffset = 0;
      for ( uMUserDataLength = 0; uMUserDataLength < length; uMUserDataLength++ )
      {
        lastVal = Marshal.ReadByte( source, uMUserDataLength );
        Marshal.WriteByte( uMBufforPtr, uMUserDataLength, lastVal );
      }
    }
    /// <summary>
    ///  Copies data from IDBuffer to unmanaged memory pointer starting 
    ///  at offset 0. Offste is ste to first empty byte.
    /// </summary>
    /// <param name="destination">The memory pointer to copy to.</param>
    public void CopyFromBuffor( IntPtr destination )
    {
      byte lastVal;
      for ( uMBufforOffset = 0; uMBufforOffset < uMUserDataLength; uMBufforOffset++ )
      {
        lastVal = Marshal.ReadByte( uMBufforPtr, uMBufforOffset );
        Marshal.WriteByte( destination, uMBufforOffset, lastVal );
      }
    }
    /// <summary>
    /// Writes a byte value into UMessage.
    /// </summary>
    /// <param name="val">The value to write.</param>
    /// <returns>true if success</returns>
    virtual public bool WriteByte( byte val )
    {
      if ( !TestIfOperationIsInsideBuffer( val, "WriteByte", 0 ) )
        return false;
      Marshal.WriteByte( uMBufforPtr, uMBufforOffset++, val );
      return true;
    }
    /// <summary>
    /// Reads a char value from UMessage.  
    /// </summary>
    /// <returns>byte that has been read</returns>
    virtual public byte ReadByte()
    {
      myAssert.Assert( uMBufforOffset < uMUserDataLength, 140 );
      return Marshal.ReadByte( uMBufforPtr, uMBufforOffset++ );
    }
    /// <summary>
    /// Convert char to byte and writes it into UMessage.
    /// </summary>
    /// <param name="val">Value to write.</param>
    virtual public bool WriteChar( char val )
    {
      if ( !TestIfOperationIsInsideBuffer( val, "WriteChar", 0 ) )
        return false;
      Marshal.WriteByte( uMBufforPtr, uMBufforOffset++, Convert.ToByte( val ) );
      return true;
    }
    /// <summary>
    /// Reads a char value from UMessage.
    /// </summary>
    /// <returns>Current character.</returns>
    virtual public char ReadChar()
    {
      myAssert.Assert( uMBufforOffset < uMUserDataLength, 160 );
      return (char)Marshal.ReadByte( uMBufforPtr, uMBufforOffset++ );
    }
    /// <summary>
    /// Writes a string into UMessage as string of ASCII char
    /// </summary>
    /// <param name="val">The value to write. </param> 
    virtual public bool WriteString( string val )
    {
      bool result = true;
      for ( int idx = 0; idx < val.Length; idx++ )
      {
        result = result && WriteByte( Convert.ToByte( val[ idx ] ) );
      }
      return result;
    }
    /// <summary>
    /// Reads a string from UMessage as string of ASCII char assuming that the first byte is the length of dtring.
    /// </summary>
    virtual public string ReadString()
    {
      string str = "";
      byte len = Marshal.ReadByte( uMBufforPtr, uMBufforOffset++ );
      for ( int idx = 0; idx < len; idx++ )
      {
        str += (char)ReadByte();
      }
      return str;
    }
    /// <summary>
    /// Reads a string of a specified length from UMessage as string of ASCII char 
    /// </summary>
    /// <param name="len">Length of the string</param>
    virtual public string ReadString( short len )
    {
      string str = "";
      for ( int idx = 0; idx < len; idx++ )
      {
        str += (char)ReadByte();
      }
      return str;
    }
    /// <summary>
    /// Writes a processor native sized pointer value into UMessage.  
    /// </summary>
    /// <param name="val">The value to write. </param>
    virtual public bool WriteIntPtr( IntPtr val )
    {
      if ( !TestIfOperationIsInsideBuffer( val, "WriteIntPtr", Marshal.SizeOf( val ) - 1 ) )
        return false;
      Marshal.WriteIntPtr( uMBufforPtr, uMBufforOffset, val );
      if ( uMBufforReverse )
        reverse( uMBufforPtr, ref uMBufforOffset, (ushort)Marshal.SizeOf( val ) );
      else
        uMBufforOffset += (ushort)Marshal.SizeOf( val );
      return true;
    }
    /// <summary>
    /// Reads a processor native sized pointer value from UMessage.
    /// </summary>
    virtual public IntPtr ReadIntPtr()
    {
      IntPtr retVal;
      if ( uMBufforReverse )
      {
        ushort startIds = uMBufforOffset;
        reverse( uMBufforPtr, ref startIds, (ushort)IntPtr.Size );
        retVal = Marshal.ReadIntPtr( uMBufforPtr, uMBufforOffset );
        reverse( uMBufforPtr, ref uMBufforOffset, (ushort)IntPtr.Size );
      }
      else
      {
        retVal = Marshal.ReadIntPtr( uMBufforPtr, uMBufforOffset );
        uMBufforOffset += (ushort)IntPtr.Size;
      }
      myAssert.Assert( uMBufforOffset < uMUserDataLength, 180 );
      return retVal;
    }
    /// <summary>
    /// Writes a 16-bit integer value into UMessage.
    /// </summary>
    /// <param name="val">Value to write.</param>
    virtual public bool WriteInt16( Int16 val )
    {
      if ( !TestIfOperationIsInsideBuffer( val, "WriteInt16", 1 ) )
        return false;
      Marshal.WriteInt16( uMBufforPtr, uMBufforOffset, val );
      if ( uMBufforReverse )
        reverse( uMBufforPtr, ref uMBufforOffset, 2 );
      else
        uMBufforOffset += 2;
      return true;
    }
    /// <summary>
    /// Reads a 16-bit integer value from UMessage.
    /// </summary>
    /// <returns></returns>
    virtual public short ReadInt16()
    {
      short retVal;
      if ( uMBufforReverse )
      {
        ushort startIds = uMBufforOffset;
        reverse( uMBufforPtr, ref startIds, 2 );
        retVal = Marshal.ReadInt16( uMBufforPtr, uMBufforOffset );
        reverse( uMBufforPtr, ref uMBufforOffset, 2 );
      }
      else
      {
        retVal = Marshal.ReadInt16( uMBufforPtr, uMBufforOffset );
        uMBufforOffset += 2;
      }
      myAssert.Assert( uMBufforOffset <= uMUserDataLength, 200 );
      return retVal;
    }
    /// <summary>
    /// Writes a 32-bit integer value into UMessage.
    /// </summary>
    /// <param name="val">The value to write. </param>
    virtual public bool WriteInt32( Int32 val )
    {
      if ( !TestIfOperationIsInsideBuffer( val, "WriteInt32", 3 ) )
        return false;
      Marshal.WriteInt32( uMBufforPtr, uMBufforOffset, val );
      if ( uMBufforReverse )
        reverse( uMBufforPtr, ref uMBufforOffset, 4 );
      else
        uMBufforOffset += 4;
      return true;
    }
    /// <summary>
    /// READ a 32-bit integer value from UMessage.
    /// </summary>
    virtual public int ReadInt32()
    {
      int retVal;
      if ( uMBufforReverse )
      {
        ushort startIds = uMBufforOffset;
        reverse( uMBufforPtr, ref startIds, 4 );
        retVal = Marshal.ReadInt32( uMBufforPtr, uMBufforOffset );
        reverse( uMBufforPtr, ref uMBufforOffset, 4 );
      }
      else
      {
        retVal = Marshal.ReadInt32( uMBufforPtr, uMBufforOffset );
        uMBufforOffset += 4;
      }
      myAssert.Assert( uMBufforOffset <= uMUserDataLength, 220 );
      return retVal;
    }
    /// <summary>
    /// Write a 64-bit integer value from UMessage.
    /// </summary>
    /// <param name="val">The value to write. </param>
    virtual public bool WriteInt64( Int64 val )
    {
      if ( !TestIfOperationIsInsideBuffer( val, "WriteInt64", 7 ) )
        return false;
      Marshal.WriteInt64( uMBufforPtr, uMBufforOffset, val );
      if ( uMBufforReverse )
        reverse( uMBufforPtr, ref uMBufforOffset, 8 );
      else
        uMBufforOffset += 8;
      return true;
    }
    /// <summary>
    /// Read a 64-bit integer value from UMessage.
    /// </summary>
    virtual public long ReadInt64()
    {
      uMBufforOffset += 8;
      return Marshal.ReadInt64( uMBufforPtr, uMBufforOffset - 8 );
    }
    /// <summary>
    /// Starts block operation. Leave the current byte as the location for length of the block calculated by the End_block operation. 
    /// </summary>
    public void Start_block()
    {
      block_offset.Push( offset );
      offset++;
    }
    /// <summary>
    /// Calculated the length of the block in bytes and writes it in the position marked by Start_block.
    /// </summary>
    public void End_block()
    {
      ushort off = (ushort)block_offset.Pop();
      Marshal.WriteByte( uMessagePtr, off, (byte)( offset - off - 1 ) );
    }
    /// <summary>
    /// Clears the message.
    /// </summary>
    public virtual void ResetContent()
    {
      uMUserDataLength = 0;
      uMBufforOffset = 0;
    }
    /// <summary>
    /// Copy unmanaged contenes to the array of bytes.
    /// </summary>
    /// <returns>Contenes to the message.</returns>
    public byte[] GetManagedBuffer()
    {
      byte[] managedBuffer = new byte[ this.userDataLength ];
      for ( int i = 0; i < this.userDataLength; i++ )
        managedBuffer[ i ] = this[ i ];
      return managedBuffer;
    }
    #endregion
    #region creator
    /// <summary>
    /// Message with buffer located in unmanaged memory
    /// </summary>
    /// <param name="length">Length of the data buffer</param>
    /// <param name="bigEndian">If true, numerical quantity larger then a single byte is stored in the buffer 
    /// with the most significant byte first. 
    /// true to use a ‘big-Endian’ representation for addresses and data items. 
    /// This means that when a numerical quantity larger than a
    /// single byte is transmitted, the most significant byte is sent first. So for example
    /// Register size value 16 - bits 0x1234 the first byte sent is 0x12 then 0x34 (fe. MODBUS, SBUS)
    /// </param>
    public UMessage( ushort length, bool bigEndian )
    {
      uMBufforMaxLength = length;
      uMBufforReverse = bigEndian;
      uMBufforPtr = Marshal.AllocCoTaskMem( uMBufforMaxLength );
      m_TraceSource = new TraceSource( m_traceName );
      TraceEvent( TraceEventType.Verbose, 468, "UMessageCreated" );
    }
    #endregion
    #region IDisposable Members
    private bool disposed = false;
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <remarks>
    /// Dispose(bool disposing) executes in two distinct scenarios. If disposing equals true, the method has been called directly
    /// or indirectly by a user's code. Managed and unmanaged resources can be disposed. If disposing equals false, 
    /// the method has been called by the runtime from inside the finalizer and you should not reference other objects. 
    /// Only unmanaged resources can be disposed.
    /// </remarks>
    /// <param name="disposing">
    /// If disposing equals true, the method has been called directly or indirectly by a user's code.
    /// </param>
    protected virtual void Dispose( bool disposing )
    {
      // Check to see if Dispose has already been called.
      if ( this.disposed )
        return;
      // Release unmanaged resources. If disposing is false, only the following code is executed.
      Marshal.FreeCoTaskMem( uMBufforPtr );
      disposed = true;
    }
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. 
    /// </summary>
    void IDisposable.Dispose()
    {
      Dispose( true );
      // Take yourself off the Finalization queue to prevent finalization code for this object from executing a second time.
      GC.SuppressFinalize( this );
    }
    /// <summary>
    /// Class destructor; Calling Dispose(false) is optimal in terms of readability and maintainability.
    /// </summary>
    ~UMessage() { Dispose( false ); }
    #endregion
  }
}
