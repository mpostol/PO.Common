//<summary>
//  Title   : Helper class and interface that provides information about base directory
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2009, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.UA.SDK.ServerConfigurationBase
{
  /// <summary>
  /// Helper class and interface that provides information about base directory
  /// </summary>
  public class BaseDirectoryHelper
  {
    #region singleton
    static BaseDirectoryHelper()
    {
      BaseDirectoryHelper.Instance = new BaseDirectoryHelper();
    }
    /// <summary>
    /// Gets or sets the instance of BaseDirectoryHelper singleton.
    /// </summary>
    /// <value>The instance.</value>
    public static BaseDirectoryHelper Instance { get; private set; }
    #endregion singleton

    /// <summary>
    /// Gets the base directory.
    /// </summary>
    /// <returns>string with base directory</returns>
    public string GetBaseDirectory()
    {
      if ( myProvider != null )
        return myProvider.GetBaseDirectory();
      return string.Empty;
    }
    /// <summary>
    /// Sets the base directory provider.
    /// </summary>
    /// <param name="provider">The provider.</param>
    public void SetBaseDirectoryProvider( IBaseDirectoryProvider provider )
    {
      myProvider = provider;
    }

    //private
    private IBaseDirectoryProvider myProvider = null;
  }

}
