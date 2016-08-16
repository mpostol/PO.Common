//<summary>
//  Title   : Collection of all plug-ins that are available in the executing folder.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol 21-04-2007: created.
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace CAS.Lib.CommonBus.Management
{
  /// <summary>
  /// Collection of all plug-ins that are available in the executing folder.
  /// </summary>
  public class PluginCollection: IEnumerable<KeyValuePair<Guid, IDataProviderID>>, IDisposable
  {
    private TraceSource m_TraceSource = new TraceSource( "NetworkConfig.ApplicationProtocol", SourceLevels.Off );
    private SortedDictionary<Guid, IDataProviderID> m_SortedDic = new SortedDictionary<Guid, IDataProviderID>();

    /// <summary>
    /// Sets the default ssettings.
    /// </summary>
    /// <param name="pSetting">The setting.</param>
    /// <param name="pGuid">The GUID.</param>
      public void SetDefaultSsetting( string pSetting, Guid pGuid )
    {
      IDataProviderID cDP = this[ pGuid ];
      if ( cDP == null )
        return;
      cDP.SetSettings( pSetting );
    }
     /// <summary>
     /// array of registers values
     /// </summary>
     /// <param name="idx"></param>
     /// <returns></returns>
    public IDataProviderID this[ Guid idx ] { get { return m_SortedDic[ idx ]; } }
    #region creator
    /// <summary>
    /// Creates a collection of all plug-ins that are available in the executing folder.
    /// </summary>
    /// <param name="pParent">Parent class <seealso cref="CommonBusControl"/></param>
    public PluginCollection( CommonBusControl pParent )
    {
      m_TraceSource = pParent.GetTraceSource;
      string ialphmFiltr = typeof( IDataProviderID ).ToString();
      //Przeszukujemy katalog w poszykiwaniu odpowiedniego pluginu
      string lPathToScann = Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location );
      foreach ( string fileOn in Directory.GetFiles( lPathToScann ) )
      {
        System.IO.FileInfo file = new System.IO.FileInfo( fileOn );
        //roszerzenie powinno byæ dll
        if ( file.Extension.ToLower().Equals( ".dll" ) && file.Name.ToLower().Contains( "plugin" ) )
        {
          m_TraceSource.TraceEvent
            ( TraceEventType.Verbose, 148, "NetworkConfiguration: Trying to find helper interface in file: {0}", fileOn );
          try
          {
            //Create a new assembly from the plugin we're reading
            System.Reflection.Assembly pluginAssembly = System.Reflection.Assembly.LoadFrom( fileOn );
            //Gets a type object of the interface we need the plugins to match
            foreach ( Type pluginType in pluginAssembly.GetExportedTypes() )
            {
              m_TraceSource.TraceEvent
                ( TraceEventType.Verbose, 158, "NetworkConfiguration: Checking if {0} implements the helper interface", pluginType.ToString() );
              if ( pluginType.IsPublic && !pluginType.IsAbstract && pluginType.GetInterface( ialphmFiltr ) != null ) //Only look at public types
              {
                m_TraceSource.TraceEvent
                  ( TraceEventType.Verbose, 162, "NetworkConfiguration: Trying to create helper." );
                IDataProviderID lHelper = (IDataProviderID)Activator.CreateInstance( pluginType );
                m_SortedDic.Add( lHelper.GetDataProviderDescription.Identifier, lHelper );
                m_TraceSource.TraceEvent
                  ( TraceEventType.Verbose, 158, "NetworkConfiguration: {0} has benn added to the plugins collection.", pluginType.ToString() );
              }
              else
                m_TraceSource.TraceEvent
                    ( TraceEventType.Verbose, 158, "NetworkConfiguration: {0} does not implement the helper interface", pluginType.ToString() );
            }//foreach (Type pluginType in pluginAssembly.GetTypes())
          }//try
          catch ( BadImageFormatException )//w przypadku biblioteki nie .NET - np WtOPCSvr.dll
          {
            m_TraceSource.TraceEvent
              ( TraceEventType.Warning, 211, "NetworkConfiguration: The file {0} image is invalid.", fileOn );
          }
          catch ( LicenseException ex )
          {
            m_TraceSource.TraceEvent
              ( TraceEventType.Warning, 217,
              "NetworkConfiguration: The component cannot be granted a license for the type: {0}.", ex.LicensedType.ToString() );
          }
          catch ( Exception ex )
          {
            m_TraceSource.TraceEvent
              ( TraceEventType.Warning, 223,
              "Some problem encountered while trying to connect a plugin in file {0}. An exception was thrown: {1}", fileOn, ex.Message );
          }
        }//if(file.Extension.Equals(".dll"))
      }//foreach(string fileOn in System.IO.Directory.GetFiles(Win32API.Application.Path))
    }
    #endregion
    #region IDisposable Members
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <remarks>
    /// Dispose(bool disposing) executes in two distinct scenarios. If disposing equals true, the method has been called directly
    /// or indirectly by a user's code. Managed and unmanaged resources can be disposed. If disposing equals false, 
    /// the method has been called by the runtime from inside the finalizer and you should not reference other objects. 
    /// Only unmanaged resources can be disposed.
    /// </remarks>
    public void Dispose()
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    #endregion
    #region IEnumerable Members
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      throw new Exception( "The method or operation is not implemented." );
    }
    #endregion
    #region IEnumerable<KeyValuePair<Guid,IDataProviderID>> Members
    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    IEnumerator<KeyValuePair<Guid, IDataProviderID>> IEnumerable<KeyValuePair<Guid, IDataProviderID>>.GetEnumerator()
    {
      return m_SortedDic.GetEnumerator();
    }
    #endregion
  }
}
