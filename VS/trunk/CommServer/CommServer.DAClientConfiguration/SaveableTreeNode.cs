//<summary>
//  Title   : Interface providing functionality to save configuration in the <see cref="OPCCliConfiguration"/>
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

using System;
using CAS.Lib.ControlLibrary;

namespace CAS.DataPorter.Configurator
{
  /// <summary>
  /// Interface providing functionality to save configuration in the <see cref="OPCCliConfiguration"/>
  /// </summary>
  public interface ISave: ITreeNodeInterface
  {
    /// <summary>
    /// Saves the current configuration in the <see cref="OPCCliConfiguration"/>.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="parentKey">The parent key.</param>
    void Save( OPCCliConfiguration configuration, long parentKey );
    /// <summary>
    /// Saves the specified configuration in the <see cref="OPCCliConfiguration"/>.
    /// </summary>
    /// <param name="configuration">The current configuration in the <see cref="OPCCliConfiguration"/>.</param>
    void Save( OPCCliConfiguration configuration );
  }
  /// <summary>
  /// The Advanced ISave interface
  /// </summary>
  public interface ISaveAdvanced
  {
    /// <summary>
    /// Gets the full name.
    /// </summary>
    /// <returns>full name of a node</returns>
    string GetFullName();
    /// <summary>
    /// Gets the last DataBAse identifier.
    /// </summary>
    /// <returns>the last database identifier</returns>
    long? GetLastDBIdentifier { get; set; }
    /// <summary>
    /// Occurs when this node is removed.
    /// </summary>
    event EventHandler<GenericEventArgs<ISaveAdvanced>> OnRemove;
  }
  /// <summary>
  /// Represents dedicated to the configuration management TreeNode
  /// </summary>
  public abstract class SaveableTreeNode<ObjectType, ParentType>: GenericTreeNode<ObjectType, ParentType>, ISave
    where ObjectType: class
    where ParentType: class, ISave
  {
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="text">The label <see cref="System.Windows.Forms.TreeNode.Text"/> of the new tree node.</param>
    /// <param name="obj">The object coupled with the node.</param>
    /// <param name="node">The node to add new object.</param>
    public SaveableTreeNode( string text, ObjectType obj, ParentType node )
      : base( text, obj, node )
    { }
    /// <summary>
    /// Initializes a new instance of the  class.
    /// </summary>
    /// <param name="text">The label <see cref="System.Windows.Forms.TreeNode.Text"/> of the new tree node.</param>
    /// <param name="obj">The object coupled with the node.</param>
    public SaveableTreeNode( string text, ObjectType obj )
      : base( text, obj )
    { }
    #endregion
    #region ISave Members
    /// <summary>
    /// Saves the current configuration in the <see cref="OPCCliConfiguration"/>.
    /// </summary>
    /// <param name="configuration">The current configuration.</param>
    /// <param name="parentKey">The parent key.</param>
    public virtual void Save( OPCCliConfiguration configuration, long parentKey )
    {
      System.Diagnostics.Debug.Assert( configuration != null, "Configuration data set cannot be null" );
      foreach ( ISave br in Nodes )
        br.Save( configuration, parentKey );
    }
    /// <summary>
    /// Saves the specified configuration in the <see cref="OPCCliConfiguration"/>.
    /// </summary>
    /// <param name="configuration">The current configuration in the <see cref="OPCCliConfiguration"/>.</param>
    public void Save( OPCCliConfiguration configuration )
    {
      Save( configuration, long.MinValue );
    }
    #endregion
  }
}
