//<summary>
//  Title   : Interface allowing to get access to Locale and Filter
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.DataPorter.Configurator
{
  /// <summary>
  /// Interface allowing to get access to Locale and Filter <see cref="global::Opc.Da.ResultFilter"/>
  /// </summary>
  public interface IOptions
  {
    /// <summary>
    /// Gets or sets the locale. The locale is used for any error 
    /// messages or results returned to the client.
    /// </summary>
    /// <value>The locale.</value>
    string Locale { set; get; }
    /// <summary>
    /// Gets or sets the filter <see cref="Opc.Da.ResultFilter"/>. Filters are
    /// applied by the server before returning item results.
    /// </summary>
    /// <value>The filter.</value>
    global::Opc.Da.ResultFilter Filter { get; set; }
  }
}

