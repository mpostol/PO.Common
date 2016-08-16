//<summary>
//  Title   : Allows to get by the client all information identifying the data provider component.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol - 08-05-2007: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CAS.Lib.CommonBus
{
  /// <summary>
  /// Allows to get by the client all information identifying the data provider component.
  /// </summary>
  public interface IDataProviderDescription
  {
    /// <summary>Gets the title custom attribute for the plug-in assembly manifest.</summary>
    string Title { get;}
    /// <summary>Gets the full name of the assembly.</summary>
    string FullName { get; }
    /// <summary>Gets a company name custom attribute for the plug-in assembly manifest.</summary>
    string Company { get;}
    /// <summary>Gets an assembly description custom attribute for the plug-in assembly manifest.
    /// Returns:
    ///     A string containing the company name.
    /// </summary>
    string Description { get;}
    /// <summary>
    /// Gets the major, minor, revision, and build numbers of the plug-in assembly.
    /// </summary>
    System.Version Version { get;}
    /// <summary>
    /// Data of last modification or release of the component.
    /// </summary>
    System.DateTime Date { get;}
    /// <summary>
    /// An unique identifier assigned to the plug-in assembly.
    /// </summary>
    System.Guid Identifier { get;}
    /// <summary>
    /// Gets a copyright custom attribute for the plug-in assembly manifest.
    /// </summary>
    string Copyright { get;}
    /// <summary>
    /// Gets a trademark custom attribute for the plug-in assembly manifest.
    /// </summary>
    string Trademark { get;}
  }
  internal class DataProviderDescription: IDataProviderDescription
  {
    #region private
    private Assembly m_Fingerprint;
    #endregion
    #region IDataProviderDescription Members
    public string Title
    {
      get
      {
        object[] dsc = m_Fingerprint.GetCustomAttributes( typeof( AssemblyTitleAttribute ), false );
        if ( dsc.Length == 0 )
          return "Data Provider Rapid Development Kit.";
        else
          return ( (AssemblyTitleAttribute)dsc[ 0 ] ).Title;
      }
    }
    public string FullName
    {
      get { return m_Fingerprint.GetName().FullName; }
    }
    public string Company
    {
      get
      {
        object[] dsc = m_Fingerprint.GetCustomAttributes( typeof( AssemblyCompanyAttribute ), false );
        if ( dsc.Length == 0 )
          return "CAS";
        else
          return ( (AssemblyCompanyAttribute)dsc[ 0 ] ).Company;
      }
    }
    public string Description
    {
      get
      {
        object[] dsc = m_Fingerprint.GetCustomAttributes( typeof( AssemblyDescriptionAttribute ), false );
        if ( dsc.Length == 0 )
          return "Data Provider ID";
        else
          return ( (AssemblyDescriptionAttribute)dsc[ 0 ] ).Description;
      }
    }
    public Version Version
    {
      get { return m_Fingerprint.GetName().Version; }
    }
    public DateTime Date
    {
      get
      {
        FileInfo fi = new FileInfo( m_Fingerprint.Location );
        return fi.LastWriteTime;
      }
    }
    public Guid Identifier
    {
      get
      {
        object[] dsc = m_Fingerprint.GetCustomAttributes( typeof( GuidAttribute ), false );
        if ( dsc.Length == 0 )
          return Guid.Empty;
        else
          return new Guid( ( (GuidAttribute)dsc[ 0 ] ).Value );
      }
    }
    public string Copyright
    {
      get
      {
        object[] dsc = m_Fingerprint.GetCustomAttributes( typeof( AssemblyCopyrightAttribute ), false );
        if ( dsc.Length == 0 )
          return "CAS Copyright ©  2007";
        else
          return ( (AssemblyCopyrightAttribute)dsc[ 0 ] ).Copyright;
      }
    }
    public string Trademark
    {
      get
      {
        object[] dsc = m_Fingerprint.GetCustomAttributes( typeof( AssemblyTrademarkAttribute ), false );
        if ( dsc.Length == 0 )
          return "CAS Copyright ©  2007";
        else
          return ( (AssemblyTrademarkAttribute)dsc[ 0 ] ).Trademark;
      }
    }
    public override string ToString() { return FullName; }
    #endregion
    #region creator
    internal DataProviderDescription( Assembly pAss )
    {
      m_Fingerprint = pAss;
    }
    #endregion
  }
}
