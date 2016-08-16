//<summary>
//  Title   : Protocol parameters for ApplicationLayer
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080917: mzbrzezny: CountTimeIntervals is marked virtual and can be overridden e.g. by modbus
//    2008-06-13: mzbrzezny: new function that comes from XML helper are used see: itr:[COM-801]
//    MPostol - created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.Xml;
using CAS.Lib.CommonBus.Xml;

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  /// <summary>
  /// Protocol parameters for ApplicationLayer
  /// </summary>
  [System.Serializable]
  public abstract class ProtocolParameters
  {
    #region PRIVATE
    #region XML identifiers
    /// <summary>
    /// XML name for the tag that represents ProtocolParameters class
    /// </summary>
    protected const string m_TagClass = "ProtocolParameters";
    /// <summary>
    /// XML name for the tag that represents ppNumberOfRetries field
    /// </summary>
    protected const string m_Tag_NumberOfRetries = "NumberOfRetries";
    /// <summary>
    /// XML name for the tag that represents ppTimeoutResponse field
    /// </summary>
    protected const string m_Tag_TimeoutResponse = "TimeoutResponse";
    /// <summary>
    /// XML name for the tag that represents ppInterfameGap field
    /// </summary>
    protected const string m_Tag_InterfameGap = "InterfameGap";
    #endregion
    /// <summary>
    /// Maximum number of retries this station will try.
    /// </summary>
    protected ushort ppNumberOfRetries;
    /// <summary>
    /// Maximum response time this station is willing to wait
    /// </summary>
    protected TimeSpan ppTimeoutResponse;
    /// <summary>
    /// Inter-frame gap – minimal time gap between two consecutive frames
    /// </summary>
    protected TimeSpan ppInterfameGap;
    /// <summary>
    /// Counts the timeout35 - the max time between 3.5 characters.
    /// </summary>
    /// <param name="timeout15">The timeout15 - the max time between 1.5 characters..</param>
    /// <returns>The timeout15 <see cref="TimeSpan"/></returns>
    protected static TimeSpan CountTimeout35( TimeSpan timeout15 )
    {
      return TimeSpan.FromMilliseconds( timeout15.TotalMilliseconds * 3.5 / 1.5 );
    }
    /// <summary>
    /// Counts the time intervals acording to Modbus rules.
    /// </summary>
    /// <param name="portSpeed">The port speed [baudrate].</param>
    /// <param name="t35">The time of 3.5 characters.</param>
    /// <param name="t15">The time od 1.5 characters.</param>
    protected virtual void CountTimeIntervals( uint portSpeed, out TimeSpan t35, out TimeSpan t15 )
    {
      if ( portSpeed > 19200 )
      {
        t15 = TimeSpan.FromMilliseconds( 0.750 );
        t35 = TimeSpan.FromMilliseconds( 1.750 );
      }
      else
      {
        double ticksPerCh = portSpeed / 11;
        double timeofonecharacter = 1000.0 / ticksPerCh;
        t35 = TimeSpan.FromMilliseconds( timeofonecharacter * 35 / 10 );
        t15 = TimeSpan.FromMilliseconds( timeofonecharacter * 15 / 10 );
      }
    }
    #endregion
    #region PUBLIC
    /// <summary>
    /// Gets and sets maximum number of retries this station will try. 
    /// </summary>
    [DescriptionAttribute( "Maximum number of retries this station will try" )]
    public ushort MaxNumberOfRetries
    {
      get { return ppNumberOfRetries; }
      set { ppNumberOfRetries = value; }
    }
    /// <summary>
    /// Maximum response time this station is willing to wait.
    /// </summary>
    /// <value>The response timeout <see cref="TimeSpan"/>.</value>
    [BrowsableAttribute( false )]
    public TimeSpan ResponseTimeOutSpan
    {
      get
      {
        return ppTimeoutResponse;
      }
    }
    /// <summary>
    /// Gets and sets maximum response time this station is willing to wait in ms.
    /// </summary>
    /// <value>The response time out.</value>
    /// <remarks>In ms.</remarks>
    [DescriptionAttribute( "Maximum response time this station is willing to wait in ms." )]
    public uint ResponseTimeOut
    {
      set
      {
        this.ppTimeoutResponse = TimeSpan.FromMilliseconds( value );
      }
      get
      {
        return Convert.ToUInt32( ppTimeoutResponse.TotalMilliseconds );
      }
    }
    /// <summary>
    /// Inter-frame gap – minimal time gap between two consecutive frames in internal ticks.
    /// </summary>
    /// <value>The interframe gap <see cref="TimeSpan"/>.</value>
    [BrowsableAttribute( false )]
    public TimeSpan InterframeGapSpan
    {
      get
      {
        return ppInterfameGap;
      }
    }
    /// <summary>
    /// Inter-frame gap – minimal time gap between two consecutive frames in ms
    /// </summary>
    /// <remarks>In ms.</remarks>
    [DescriptionAttribute( "Inter-frame gap – minimal time gap between two consecutive frames in [ms]. This time must elapse before any device send a new frame. (this is the time of the silence on the bus)." )]
    public virtual uint InterframeGap
    {
      set
      {
        ppInterfameGap = TimeSpan.FromMilliseconds( value );
      }
      get
      {
        return Convert.ToUInt32( ppInterfameGap.TotalMilliseconds );
      }
    }
    /// <summary>
    /// Set all parameters according to the baudrate.
    /// </summary>
    public abstract uint LineSpeed
    {
      set;
    }
    /// <summary>
    /// Gets the ProtocolParameters description as a string. 
    /// </summary>
    /// <returns>Description in form: [ResponseTimeOut][InterframeGap][Timeout35][Timeout15]</returns>
    public override string ToString()
    {
      return
        "\\Tout=" + ResponseTimeOut.ToString() + "\\Tint=" + InterframeGap.ToString();
    }
    /// <summary>
    /// this function  writes settings to xml stream
    /// </summary>
    /// <param name="pSettings">XmlWriter strea</param>
    public virtual void WriteSettings( System.Xml.XmlWriter pSettings )
    {
      pSettings.WriteStartElement( m_TagClass );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_InterfameGap, ppInterfameGap );
      XmlHelper.WriteStandardIntegerVale( pSettings, m_Tag_NumberOfRetries, ppNumberOfRetries );
      XmlHelper.WriteTimeInMicroseconds( pSettings, m_Tag_TimeoutResponse, ppTimeoutResponse );
      pSettings.WriteEndElement();
    }
    /// <summary>
    /// this function  reads settings from xml stream
    /// </summary>
    /// <param name="pSettings">XmlReader strea</param>
    public virtual void ReadSettings( System.Xml.XmlReader pSettings )
    {
      if ( !pSettings.IsStartElement( m_TagClass ) )
        throw new XmlException
          ( string.Format( "Expected element {0} not found at current position of the configuration file", m_TagClass ) );
      ppInterfameGap = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_InterfameGap );
      ppNumberOfRetries = (ushort)XmlHelper.ReadStandardIntegerValue( pSettings, m_Tag_NumberOfRetries );
      ppTimeoutResponse = XmlHelper.ReadTimeFromMicroseconds( pSettings, m_Tag_TimeoutResponse );
      pSettings.ReadStartElement( m_TagClass );
    }    /// <summary>
    /// Creator
    /// </summary>
    /// <param name="portSpeed">Baudrate of the communication line.</param>
    /// <param name="TimeoutResponse">Maximum response time this station is willing to wait.</param>
    /// <param name="NumberOfRetries">Maximum number of retries this station will try.</param>
    public ProtocolParameters( uint portSpeed, uint TimeoutResponse, ushort NumberOfRetries )
    {
      LineSpeed = portSpeed;
      ResponseTimeOut = TimeoutResponse;
      this.ppNumberOfRetries = NumberOfRetries;
    }
    #endregion
  } //ProtocolParameters
}
