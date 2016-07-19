//<summary>
//  Title   : List of a TreeNode
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  20080616: mzbrzezny: getfullname is allows to return the group name too
//  20080516: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Text;
using CAS.Lib.ControlLibrary;
using Opc;
using OpcDa = global::Opc.Da;

namespace CAS.DataPorter.Configurator
{
  /// <summary>
  /// List of a Items
  /// </summary>
  public abstract class ItemWrapper: ISaveAdvanced, IDisposable
  {
    #region private
    private OpcDa.ItemResult m_Item;
    /// <summary>
    /// Gets the name of the parent.
    /// </summary>
    /// <value>The name of the parent.</value>
    protected abstract string ParentName { get; }
    private void RaiseResuldChanged()
    {
      if ( ResuldChanged != null )
        ResuldChanged( this, new EventArgs() );
    }
    private class PrivateItem: Opc.Da.ItemResult
    {
      public PrivateItem( OPCCliConfiguration.ItemsRow row )
        : base( row.Item )
      {
        this.ResultID = Opc.ResultID.S_OK;
      }
    }
    #endregion
    #region static
    static private ISaveAdvancedList mTagTreeNodeList = new ISaveAdvancedList();
    /// <summary>
    /// Gets the list.
    /// </summary>
    /// <value>The list.</value>
    static public ISaveAdvancedList List
    {
      get
      {
        return mTagTreeNodeList;
      }
    }
    #endregion static
    #region public
    /// <summary>
    /// Occurs after changing <see cref="ResultID"/> of the item.
    /// </summary>
    public event EventHandler ResuldChanged;
    /// <summary>
    /// Saves the object children in the specified configuration.
    /// </summary>
    /// <param name="config">The config <see cref="OPCCliConfiguration"/>.</param>
    /// <param name="parentKey">The parent key.</param>
    /// <returns>current index</returns>
    public long Save( OPCCliConfiguration config, long parentKey )
    {
      GetLastDBIdentifier = config.Items.Save( m_Item, parentKey );
      System.Diagnostics.Debug.Assert( GetLastDBIdentifier.HasValue );
      return GetLastDBIdentifier.Value;
    }
    /// <summary>
    /// Gets a value indicating whether this <see cref="ItemWrapper"/> is active.
    /// </summary>
    /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
    public bool? Active
    {
      get
      {
        if ( m_Item.ActiveSpecified )
          return m_Item.Active;
        else
          return null;
      }
      set
      {
        if ( value.HasValue )
        {
          m_Item.Active = value.Value;
          m_Item.ActiveSpecified = true;
        }
        else
          m_Item.ActiveSpecified = false;
      }
    }
    /// <summary>
    /// Gets the associated item.
    /// </summary>
    /// <value>The item <see cref="OpcDa.Item"/>.</value>
    public OpcDa.Item GetItem { get { return new OpcDa.Item( m_Item ); } }
    /// <summary>
    /// Sets the current item result.
    /// </summary>
    /// <value>The item result <see cref="OpcDa.ItemResult"/>.</value>
    public OpcDa.ItemResult SetItemResult
    {
      set
      {
        m_Item = value;
        RaiseResuldChanged();
      }
    }
    /// <summary>
    /// Invalidates this instance.
    /// </summary>
    public void Invalidate()
    {
      m_Item.ResultID = ResultID.Da.E_INVALID_ITEM_NAME;
      RaiseResuldChanged();
    }
    /// <summary>
    /// Gets a value indicating whether this <see cref="ItemWrapper"/> has been successfully created.
    /// </summary>
    /// <value><c>true</c> if succeeded; otherwise, <c>false</c>.</value>
    public bool Succeeded { get { return m_Item.ResultID.Succeeded(); } }
    /// <summary>
    /// Gets the client handle.
    /// </summary>
    /// <value>The client handle <see cref="Guid"/>.</value>
    public Guid ClientHandle { get { return (Guid)m_Item.ClientHandle; } }
    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </returns>
    public override string ToString()
    {
      return GetFullName();
    }
    #endregion
    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemWrapper"/> class.
    /// </summary>
    /// <param name="row">The row representing the Item.</param>
    public ItemWrapper( OPCCliConfiguration.ItemsRow row )
      : this()
    {
      m_Item = new PrivateItem( row );
      GetLastDBIdentifier = row.ID;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemWrapper"/> class.
    /// </summary>
    /// <param name="item">The item.</param>
    public ItemWrapper( OpcDa.ItemResult item )
      : this()
    {
      m_Item = item;
      if ( m_Item.ResultID.Failed() )
        m_Item.ClientHandle = Guid.NewGuid();
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemWrapper"/> class.
    /// </summary>
    public ItemWrapper()
    {
      List.Add( this );
    }
    #endregion
    #region ISaveAdvanced Members
    /// <summary>
    /// Gets the full name.
    /// </summary>
    /// <returns>full name of a node</returns>
    public string GetFullName()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append( this.m_Item.ItemName );
      sb.Append( " (" );
      sb.Append( ParentName );
      sb.Append( ")" );
      return sb.ToString();
    }
    /// <summary>
    /// Gets the last DataBAse identifier.
    /// </summary>
    /// <returns>the last database identifier</returns>
    public long? GetLastDBIdentifier { get; set; }
    /// <summary>
    /// Occurs when this node is removed.
    /// </summary>
    public event EventHandler<GenericEventArgs<ISaveAdvanced>> OnRemove;
    #endregion
    #region IDisposable Members
    private bool disposed = false;
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <remarks>Do not make this method virtual. A derived class should not be able to override this method.</remarks>
    public void Dispose()
    {
      Dispose( true );
      // This object will be cleaned up by the Dispose method.
      // Therefore, you should call GC.SupressFinalize to
      // take this object off the finalization queue
      // and prevent finalization code for this object
      // from executing a second time.
      GC.SuppressFinalize( this );
    }
    /// <summary>
    /// Recursively searches the tree and free objects.
    /// </summary>
    /// <remarks>Dispose(bool disposing) executes in two distinct scenarios. If disposing 
    /// equals true, the method has been called directly or indirectly by a user's code. 
    /// Managed and unmanaged resources can be disposed. If disposing equals false, the 
    /// method has been called by the runtime from inside the finalizer and you should not 
    /// reference other objects. Only unmanaged resources can be disposed.
    /// </remarks>
    protected virtual void Dispose( bool disposing )
    {
      System.Diagnostics.Debug.Assert( !disposed );
      // Check to see if Dispose has already been called.
      if ( this.disposed )
        return;
      // If disposing equals true, dispose all managed and unmanaged resources.
      if ( disposing )
        List.Remove( this );
      if ( OnRemove != null )
        OnRemove( this, new GenericEventArgs<ISaveAdvanced>( this ) );
      // Call the appropriate methods to clean up unmanaged resources here.
      // If disposing is false, only the following code is executed.
      disposed = true;
    }
    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="GenericTreeNode&lt;ObjectType, ParentType&gt;"/> is reclaimed by garbage collection.
    /// </summary>
    /// <remarks> 
    /// Do not re-create Dispose clean-up code here. Calling Dispose(false) is optimal in terms 
    /// of readability and maintainability.
    /// </remarks>
    ~ItemWrapper()
    {
      Dispose( false );
    }
    #endregion
  }
}
