//<summary>
//  Title   : ICommunicationLayerDescription
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Reflection;

namespace CAS.Lib.CommonBus.CommunicationLayer
{
  /// <summary>
  /// Allows to get by the client all information identifying the communication provider. 
  /// All information is read only from definition.
  /// </summary>
  public interface ICommunicationLayerDescription
  {
    /// <summary>Gets a title for the ICommunicationLayer provider.</summary>
    string Title { get;}
    /// <summary>Gets a full name of the ICommunicationLayer provider.</summary>
    string FullName { get; }
    /// <summary>Gets a company name for the ICommunicationLayer provider.</summary>
    string Company { get;}
    /// <summary>Gets a description for the ICommunicationLayer provider.</summary>
    string Description { get;}
    /// <summary>communication layer settings in human readable format</summary>
    string HumanReadableSettings { get;}
  }
  /// <summary>
  /// Basic implementation of <see>ICommunicationLayerDescription</see>
  /// </summary>
  public abstract class CommunicationLayerDescription: ICommunicationLayerDescription
  {
    #region ICommunicationLayerDescription Members
    /// <summary>
    /// Gets a title for the ICommunicationLayer provider.
    /// </summary>
    public abstract string Title { get;}
    /// <summary>
    /// Gets a full name of the ICommunicationLayer provider.
    /// </summary>
    public virtual string FullName
    {
      get { return Assembly.GetExecutingAssembly().FullName; }
    }
    /// <summary>
    /// Gets a company name for the ICommunicationLayer provider.
    /// </summary>
    public virtual string Company
    {
      get
      {
        object[] dsc = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( System.Reflection.AssemblyCompanyAttribute ), false );
        if ( dsc[ 0 ] == null )
          return "CAS";
        else
          return ( (System.Reflection.AssemblyCompanyAttribute)dsc[ 0 ] ).Company;
      }
    }
    /// <summary>
    /// Gets a description for the ICommunicationLayer provider.
    /// </summary>
    public abstract string Description { get;}
    /// <summary>
    /// Get settings in human readable form
    /// </summary>
    public abstract string HumanReadableSettings { get;}
    #endregion
    #region public
    /// <summary>
    /// Retrieves a string representation of the object.
    /// </summary>
    /// <returns>Description of the message:[station][address][myDataType][length]</returns>
    public override string ToString() { return FullName; }
    #endregion
  }
}
