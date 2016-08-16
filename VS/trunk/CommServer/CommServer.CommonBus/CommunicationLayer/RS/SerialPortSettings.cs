//<summary>
//  Title   : SerialPortSettings
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    mpostol: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Xml;
using CAS.Lib.CommonBus.Xml;

namespace CAS.Lib.CommonBus.CommunicationLayer.RS
{
  /// <summary>
  /// Settings for the serial port (RS)
  /// </summary>
  public class SerialPortSettings: ICommunicationLayerFactory
  {
    #region private
    private const string m_Tag_Class = "SerialPortSettings";
    private const string m_Tag_BaudRate = "BaudRate";
    private const string m_Tag_DataBits = "DataBits";
    private const string m_Tag_Parity = "Parity";
    private const string m_Tag_COMNumber = "COMNumber";
    private const string m_Tag_StopBits = "StopBits";
    private int m_BaudRate = 9600;
    private int m_DataBits = 8;
    private Parity m_Parity = Parity.None;
    private byte m_COMNumber = 1;
    private StopBits m_StopBits = StopBits.One;
    #endregion
    #region Settings
    /// <summary>
    /// Baud Rate for serial port setting
    /// </summary>
    public enum BAUD_RATE: uint
    {
      /// <summary>
      /// Baud rate 110 bps
      /// </summary>
      BR____110 = 110,
      /// <summary>
      /// Baud rate 300 bps
      /// </summary>
      BR____300 = 300,
      /// <summary>
      /// Baud rate 600 bps
      /// </summary>
      BR____600 = 600,
      /// <summary>
      /// Baud rate 1200 bps
      /// </summary>
      BR___1200 = 1200,
      /// <summary>
      /// Baud rate 2400 bps
      /// </summary>
      BR___2400 = 2400,
      /// <summary>
      /// Baud rate 4800 bps
      /// </summary>
      BR___4800 = 4800,
      /// <summary>
      /// Baud rate 9600 bps
      /// </summary>
      BR___9600 = 9600,
      /// <summary>
      /// Baud rate 14400 bps
      /// </summary>
      BR__14400 = 14400,
      /// <summary>
      /// Baud rate 19200 bps
      /// </summary>
      BR__19200 = 19200,
      /// <summary>
      /// Baud rate 38400 bps
      /// </summary>
      BR__38400 = 38400,
      /// <summary>
      /// Baud rate 56000 bps
      /// </summary>
      BR__56000 = 56000,
      /// <summary>
      /// Baud rate 57600 bps
      /// </summary>
      BR__57600 = 57600,
      /// <summary>
      /// Baud rate 115200 bps
      /// </summary>
      BR_115200 = 115200,
      /// <summary>
      /// Baud rate 128000 bps
      /// </summary>
      BR_128000 = 128000,
      /// <summary>
      /// Baud rate 256000 bps
      /// </summary>
      BR_256000 = 256000
    }
    /// <summary>
    /// Gets or sets the serial baud rate.
    /// </summary>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// The baud rate specified is less than or equal to zero, or is greater than the maximum 
    /// allowable baud rate for the device.
    /// </exception>
    /// <exception cref="System.IO.IOException">
    /// The port is in an invalid state. - or - An attempt to set the state of the underlying 
    /// port failed. For example, the parameters passed from this System.IO.Ports.SerialPort
    /// object were invalid.
    /// </exception>
    [
    BrowsableAttribute( true ),
    CategoryAttribute( "Global Settings" ),
    DefaultValueAttribute( 9600 ),
    DescriptionAttribute( "Transmission speed [BaudRate = bits per second]" ),
    DisplayName ("Baud Rate")
    ]
    public BAUD_RATE BaudRate { get { return (BAUD_RATE)m_BaudRate; ;} set { m_BaudRate = (int)value; } }
    //
    // Summary:
    //     Gets or sets the standard length of data bits per byte.
    //
    // Returns:
    //     The data bits length.
    //
    // Exceptions:
    //   System.IO.IOException:
    //     The port is in an invalid state. - or -An attempt to set the state of the
    //     underlying port failed. For example, the parameters passed from this System.IO.Ports.SerialPort
    //     object were invalid.
    //
    //   System.ArgumentOutOfRangeException:
    //     The data bits value is less than 5 or more than 8.

    /// <summary>
    /// Gets or sets the standard length of data bits per byte.
    /// </summary>
    /// <value>The data bits length.</value>
    /// <remarks>    Exceptions:
    ///   System.IO.IOException:
    ///     The port is in an invalid state. - or -An attempt to set the state of the
    ///     underlying port failed. For example, the parameters passed from this System.IO.Ports.SerialPort
    ///     object were invalid.
    ///
    ///   System.ArgumentOutOfRangeException:
    ///     The data bits value is less than 5 or more than 8.
    ///</remarks>
    [MonitoringDescription( "DataBits" )]
    [Browsable( true )]
    [DefaultValue( 8 )]
    public int DataBits { get { return m_DataBits; } set { m_DataBits = value; } }
    /// <summary>
    /// Summary:
    ///     Gets or sets the parity-checking protocol.
    ///
    /// Returns:
    ///     One of the System.IO.Ports.Parity values that represents the parity-checking
    ///     protocol. The default is None.
    ///
    /// Exceptions:
    ///   System.IO.IOException:
    ///     The port is in an invalid state. - or - An attempt to set the state of the
    ///     underlying port failed. For example, the parameters passed from this System.IO.Ports.SerialPort
    ///     object were invalid.
    ///
    ///   System.ArgumentOutOfRangeException:
    ///     The System.IO.Ports.SerialPort.Parity value passed is not a valid value in
    ///     the System.IO.Ports.Parity enumeration.
    /// </summary>
    [Browsable( true )]
    [MonitoringDescription( "Parity" )]
    public Parity Parity { get { return m_Parity; } set { m_Parity = value; } }
    /// <summary>
    /// Summary:
    ///     Gets or sets the port for communications, including but not limited to all
    ///     available COM ports.
    ///
    /// Returns:
    ///     The communications port. The default is COM1.
    ///
    /// Exceptions:
    ///   System.ArgumentNullException:
    ///     The System.IO.Ports.SerialPort.PortName property was set to null.
    ///
    ///   System.ArgumentException:
    ///     The System.IO.Ports.SerialPort.PortName property was set to a value with
    ///     a length of zero.-or-The System.IO.Ports.SerialPort.PortName property was
    ///     set to a value that starts with "\\".-or-The port name was not valid.
    ///
    ///   System.InvalidOperationException:
    ///     The specified port is open.
    /// </summary>
    [DefaultValue( 1 )]
    [Browsable( true )]
    [MonitoringDescription( "COM number" )]
    public byte PortName { get { return m_COMNumber; } set { m_COMNumber = value; } }
    /// <summary>
    /// Summary:
    ///     Gets or sets the standard number of stopbits per byte.
    ///
    /// Returns:
    ///     One of the System.IO.Ports.StopBits values.
    ///
    /// Exceptions:
    ///   System.ArgumentOutOfRangeException:
    ///     The System.IO.Ports.SerialPort.StopBits value is not one of the values from
    ///     the System.IO.Ports.StopBits enumeration.
    ///
    ///   System.IO.IOException:
    ///     The port is in an invalid state. - or - An attempt to set the state of the
    ///     underlying port failed. For example, the parameters passed from this System.IO.Ports.SerialPort
    ///     object were invalid.
    /// </summary>
    [MonitoringDescription( "StopBits" )]
    [Browsable( true )]
    public StopBits StopBits
    {
      get { return m_StopBits; }
      set { m_StopBits = value; }
    }
    #region Serial Settings

    ///// <summary>
    ///// Gets or sets serial numer
    ///// </summary>
    //[
    //BrowsableAttribute( true ),
    //CategoryAttribute( "Serial: Global Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "Defines Serial Number - must be unique" )
    //]
    //public short SerialNum
    //{
    //  get
    //  {
    //    return ( m_Parent.SerialNum );
    //  }
    //  set
    //  {
    //    m_Parent.SerialNum = value;
    //  }
    //}

    ///// <summary>
    ///// Gets or sets optional parity bit follows the data bits in the character frame
    ///// </summary>
    //[
    //BrowsableAttribute( true ),
    //CategoryAttribute( "Serial: Data Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "An optional parity bit follows the data bits in the character frame" )
    //]
    //public sbyte Parity
    //{
    //  get
    //  {
    //    return ( m_Parent2.Parity );
    //  }
    //  set
    //  {
    //    m_Parent2.Parity = value;
    //  }
    //}
    ///// <summary>
    ///// Gets or sets number of data bits are transmitted in one unit
    ///// </summary>
    //[
    //BrowsableAttribute( true ),
    //CategoryAttribute( "Serial: Data Settings" ),
    //DefaultValueAttribute( 8 ),
    //DescriptionAttribute( "Number of data bits are transmitted in one unit" )
    //]
    //public sbyte DataBits
    //{
    //  get
    //  {
    //    return ( m_Parent2.DataBits );
    //  }
    //  set
    //  {
    //    m_Parent2.DataBits = value;
    //  }
    //}
    ///// <summary>
    ///// Gets or setsumber of bits in the last part of a character frame
    ///// </summary>
    //[
    //BrowsableAttribute( true ),
    //CategoryAttribute( "Serial: Data Settings" ),
    //DefaultValueAttribute( 1 ),
    //DescriptionAttribute( "Number of bits in the last part of a character frame [1, 1.5, or 2 stop bits]" )
    //]
    //public sbyte StopBits
    //{
    //  get
    //  {
    //    return ( m_Parent2.StopBits );
    //  }
    //  set
    //  {
    //    m_Parent2.StopBits = value;
    //  }
    //}

    //#region Flow Control
    ///*Following attributes shall be browsable only in software/hardware flow control CASE*/
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "Transmit CTS control" )
    //]
    //public bool TxFlowCTS
    //{
    //  get { return ( m_Parent2.TxFlowCTS ); }
    //  set { m_Parent2.TxFlowCTS = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "Transmit DSR control" )
    //]
    //public bool TxFlowDSR
    //{
    //  get { return ( m_Parent2.TxFlowDSR ); }
    //  set { m_Parent2.TxFlowDSR = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "Transmit ??????" )
    //]
    //public bool TxFlowX
    //{
    //  get { return ( m_Parent2.TxFlowX ); }
    //  set { m_Parent2.TxFlowX = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public bool TxWhenRxXoff
    //{
    //  get { return ( m_Parent2.TxWhenRxXoff ); }
    //  set { m_Parent2.TxWhenRxXoff = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public bool RxGateDSR
    //{
    //  get { return ( m_Parent2.RxGateDSR ); }
    //  set { m_Parent2.RxGateDSR = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "Receive ??????" )
    //]
    //public bool RxFlowX
    //{
    //  get { return ( m_Parent2.RxFlowX ); }
    //  set { m_Parent2.RxFlowX = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public sbyte UseRTS
    //{
    //  get { return ( m_Parent2.UseRTS ); }
    //  set { m_Parent2.UseRTS = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public sbyte UseDTR
    //{
    //  get { return ( m_Parent2.UseDTR ); }
    //  set { m_Parent2.UseDTR = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public sbyte XonChar
    //{
    //  get { return ( m_Parent2.XonChar ); }
    //  set { m_Parent2.XonChar = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public sbyte XoffChar
    //{
    //  get { return ( m_Parent2.XoffChar ); }
    //  set { m_Parent2.XoffChar = value; }
    //}

    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public ulong rxHighWater
    //{
    //  get { return ( m_Parent2.rxHighWater ); }
    //  set { m_Parent2.rxHighWater = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public ulong rxLowWater
    //{
    //  get { return ( m_Parent2.rxLowWater ); }
    //  set { m_Parent2.rxLowWater = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public ulong sendTimeoutMultiplier
    //{
    //  get { return ( m_Parent2.sendTimeoutMultiplier ); }
    //  set { m_Parent2.sendTimeoutMultiplier = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public ulong sendTimeoutConstant
    //{
    //  get { return ( m_Parent2.sendTimeoutConstant ); }
    //  set { m_Parent2.sendTimeoutConstant = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public ulong rxQueue
    //{
    //  get { return ( m_Parent2.rxQueue ); }
    //  set { m_Parent2.rxQueue = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public ulong txQueue
    //{
    //  get { return ( m_Parent2.txQueue ); }
    //  set { m_Parent2.txQueue = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public bool autoReopen
    //{
    //  get { return ( m_Parent2.autoReopen ); }
    //  set { m_Parent2.autoReopen = value; }
    //}
    //[
    //BrowsableAttribute( false ),
    //CategoryAttribute( "Flow Control Settings" ),
    //DefaultValueAttribute( 0 ),
    //DescriptionAttribute( "??????" )
    //]
    //public bool checkAllSends
    //{
    //  get { return ( m_Parent2.checkAllSends ); }
    //  set { m_Parent2.checkAllSends = value; }
    //}
    ////end of Flow Control
    //#endregion //Flow Control
    #endregion
    #endregion
    #region ICommunicationLayerFactory Members
      ICommunicationLayer ICommunicationLayerFactory.CreateCommunicationLayer( CommonBusControl cParent )
      {
        return new RS_to_Serial( this, cParent );
      }
      ICommunicationLayer ICommunicationLayerFactory.CreateCommunicationLayer( object [] param )
      {
        return new RS_to_Serial( param );
      }
    #endregion
    #region protected
    /// <summary>
    /// Returns as a string all the settings of the data provider used for operation. 
    /// Client is only holder of the string and is responsible to return this string back 
    /// using see<see cref="SetSettings"/> before instantiating IApplicationLayerMaster. All changes to the 
    /// parameters have to be done using provided by the implementer of this interface properties. 
    /// </summary>
    /// <returns>All the settings are needed to instantiate the data provider and underling <see>ICommunicationLayer</see>.</returns>
    /// <param name="pSettings"></param>
    protected virtual void GetSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_Tag_Class );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_BaudRate, m_BaudRate );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_COMNumber, m_COMNumber );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_DataBits, (int)m_DataBits );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_Parity, (int)m_Parity );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_StopBits, (int)m_StopBits );
      pSettings.WriteEndElement();
    }
    /// <summary cref="SetSettings">
    /// Sets all the settings of the data provider used for operation. This string should be returned from <see cref="GetSettings"/>.
    /// </summary>
    /// <param name="pSettings">
    /// All the settings are needed to instantiate the data provider and underling <see>ICommunicationLayer</see></param>
    protected virtual void SetSettings( System.Xml.XmlReader pSettings )
    {
      if ( !pSettings.IsStartElement( m_Tag_Class ) )
        throw new XmlException
          ( string.Format( "Expected element {0} not found at current position of the configuration file", m_Tag_Class ) );
      m_BaudRate = XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_BaudRate );
      m_COMNumber = (byte)XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_COMNumber );
      m_DataBits = XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_DataBits );
      m_Parity = (Parity)XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_Parity );
      m_StopBits = (StopBits)XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_StopBits );
      pSettings.ReadStartElement( m_Tag_Class );
    }
    #endregion
  }
}
