//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using System.Xml;
using UAOOI.ProcessObserver.RealTime.Processes;

namespace CAS.Lib.CommonBus.Xml
{

  /// <summary>
  /// helper that helps to serialize and deserializing communication layer protocol 
  /// </summary>
  public static class XmlHelper
  {

    ///<summary>
    ///A method that is writing an attribute
    ///</summary>
    ///<param name="pSettings">XMLWriter stream</param>
    ///<param name="pName">name of attribute</param>
    ///<param name="pValue">value of attribute</param>
    private static void WriteAttributeIntOrLong(XmlWriter pSettings, string pName, long pValue)
    {
      pSettings.WriteStartAttribute(pName);
      pSettings.WriteValue(pValue);
      pSettings.WriteEndAttribute();
    }
    ///<summary>
    ///A method that is writing an attribute
    ///</summary>
    ///<remarks>because in .NET Framework there are only methods to save long value to the stream 
    /// <see cref="ulong"/> value is converted to string before writing to stream
    ///</remarks>
    ///<param name="pSettings">XMLWriter stream</param>
    ///<param name="pName">name of attribute</param>
    ///<param name="pValue">value of attribute</param>
    private static void WriteAttributeUIntOrULong(XmlWriter pSettings, string pName, ulong pValue)
    {
      pSettings.WriteStartAttribute(pName);
      pSettings.WriteValue(pValue.ToString());
      pSettings.WriteEndAttribute();
    }
    /// <summary>
    /// A method that is reading an "int" attribute
    /// </summary>
    /// <param name="pSettings">XmlReader stream</param>
    /// <param name="pName">name of the attribute</param>
    /// <returns>value of an attribute</returns>
    private static int ReadattributeInt(XmlReader pSettings, string pName)
    {
      pSettings.MoveToAttribute(pName);
      int val = XmlConvert.ToInt32(pSettings.Value);
      pSettings.MoveToElement();
      return val;
    }
    /// <summary>
    /// A method that is reading an "long" attribute
    /// </summary>
    /// <param name="pSettings">XmlReader stream</param>
    /// <param name="pName">name of the attribute</param>
    /// <returns>value of an attribute</returns>
    private static long ReadattributeLong(XmlReader pSettings, string pName)
    {
      pSettings.MoveToAttribute(pName);
      long val = XmlConvert.ToInt64(pSettings.Value);
      pSettings.MoveToElement();
      return val;
    }
    /// <summary>
    /// A method that is reading an <see cref="ulong"/> attribute
    /// </summary>
    /// <param name="pSettings">XmlReader stream</param>
    /// <param name="pName">name of the attribute</param>
    /// <returns>value of an attribute</returns>
    private static ulong ReadattributeULong(XmlReader pSettings, string pName)
    {
      pSettings.MoveToAttribute(pName);
      ulong val = XmlConvert.ToUInt64(pSettings.Value);
      pSettings.MoveToElement();
      return val;
    }

    #region public
    ///<summary>
    ///A method that is writing an attribute
    ///</summary>
    ///<param name="pSettings">XMLWriter stream</param>
    ///<param name="pName">name of attribute</param>
    ///<param name="pValue">value of attribute</param>
    public static void WriteAttributeBool(XmlWriter pSettings, string pName, bool pValue)
    {
      pSettings.WriteStartAttribute(pName);
      pSettings.WriteValue(pValue);
      pSettings.WriteEndAttribute();
    }
    /// <summary>
    /// A method that is reading an "long" attribute
    /// </summary>
    /// <param name="pSettings">XmlReader stream</param>
    /// <param name="pName">name of the attribute</param>
    /// <returns>value of an attribute</returns>
    public static bool ReadAttributeBool(XmlReader pSettings, string pName)
    {
      pSettings.MoveToAttribute(pName);
      bool val = XmlConvert.ToBoolean(pSettings.Value);
      pSettings.MoveToElement();
      return val;
    }
    /// <summary>
    /// Writes the standard integer vale.
    /// </summary>
    /// <param name="XmlWriterStreamOfSettings">The XML writer stream of settings.</param>
    /// <param name="NameOfAttribute">The name of attribute.</param>
    /// <param name="ValueToBeWritten">The value to be written.</param>
    public static void WriteStandardIntegerVale(XmlWriter XmlWriterStreamOfSettings, string NameOfAttribute, int ValueToBeWritten)
    {
      WriteAttributeIntOrLong(XmlWriterStreamOfSettings, NameOfAttribute, ValueToBeWritten);
    }
    /// <summary>
    /// Writes the time as microseconds.
    /// </summary>
    /// <param name="XmlWriterStreamOfSettings">The XML writer stream of settings.</param>
    /// <param name="NameOfAttribute">The name of attribute.</param>
    /// <param name="time">The time to be written.</param>
    public static void WriteTimeInMicroseconds(XmlWriter XmlWriterStreamOfSettings, string NameOfAttribute, TimeSpan time)
    {
      WriteAttributeUIntOrULong(XmlWriterStreamOfSettings, NameOfAttribute, Convert.ToUInt64(time.TotalMilliseconds * 1000));
    }
    /// <summary>
    /// A method that is reading a "string" attribute
    /// </summary>
    /// <param name="pSettings">XmlReader stream</param>
    /// <param name="pName">name of the attribute</param>
    /// <returns>value of an attribute</returns>
    public static string ReadattributeString(XmlReader pSettings, string pName)
    {
      pSettings.MoveToAttribute(pName);
      string val = pSettings.Value;
      pSettings.MoveToElement();
      return val;
    }
    /// <summary>
    /// Reads the standard integer value.
    /// </summary>
    /// <param name="XmlReaderStreamOfSettings">The XML reader stream of settings.</param>
    /// <param name="AttributeName">Name of the attribute.</param>
    /// <returns></returns>
    public static int ReadStandardIntegerValue(XmlReader XmlReaderStreamOfSettings, string AttributeName)
    {
      return ReadattributeInt(XmlReaderStreamOfSettings, AttributeName);
    }
    /// <summary>
    /// Reads the time from microseconds value.
    /// </summary>
    /// <param name="XmlReaderStreamOfSettings">The XML reader stream of settings.</param>
    /// <param name="AttributeName">Name of the attribute.</param>
    /// <returns>sThe time span <see cref="TimeSpan "/></returns>
    public static TimeSpan ReadTimeFromMicroseconds(XmlReader XmlReaderStreamOfSettings, string AttributeName)
    {
      return Timer.FromUSeconds(ReadattributeULong(XmlReaderStreamOfSettings, AttributeName));
    }
    #endregion

  }
}
