//<summary>
//  Title   : OPCClient Configuration Schema
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    mzbrzezny 2004 - craeted
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;
using System.Net;
using Opc;
using Opc.Da;

namespace CAS.DataPorter.Configurator
{
  /// <summary>
  /// Client configuration data set.
  /// </summary>
  public partial class OPCCliConfiguration
  {
    partial class ConversionsDataTable
    {
    }
    partial class ConnectDataRow
    {
      /// <summary>
      /// Gets the connect data.
      /// </summary>
      /// <returns> connect data</returns>
      public ConnectData GetConnectData()
      {
        NetworkCredential credential = new NetworkCredential();
        ConnectData output = new ConnectData( credential );
        if ( !IsUserNameNull() )
          credential.UserName = UserName;
        if ( !IsPasswordNull() )
          credential.Password = Password;
        if ( !IsDomainNull() )
          credential.Domain = Domain;
        if ( !IsLicenseKeyNull() )
          output.LicenseKey = LicenseKey;
        return output;
      }
    }
    partial class ConnectDataDataTable
    {
      internal void Save( ConnectData credentials, long parentKey )
      {
        ConnectDataRow row = NewConnectDataRow();
        row.ID_Server = parentKey;
        if ( !string.IsNullOrEmpty( credentials.LicenseKey ) )
          row.LicenseKey = credentials.LicenseKey;
        if ( credentials.Credentials != null )
        {
          row.UserName = credentials.Credentials.UserName;
          row.Password = credentials.Credentials.Password;
          row.Domain = credentials.Credentials.Domain;
        }
      }
    }
    partial class ItemsDataTable
    {
      /// <summary>
      /// Saves the specified itm in the configuration data set.
      /// </summary>
      /// <param name="item">The item.</param>
      /// <param name="parentKey">The parent key.</param>
      /// <returns></returns>
      public long Save( Opc.Da.Item item, long parentKey )
      {
        ItemsRow row = NewItemsRow();
        //TODO assign rw.Async = ????
        //TODO assign rw.Conversion = ?????
        row.ID_Subscription = parentKey;
        row.RequestedType = item.ReqType;
        if ( item.MaxAgeSpecified )
          row.MaxAge = item.MaxAge;
        if ( item.ActiveSpecified )
          row.Active = item.Active;
        if ( item.DeadbandSpecified )
          row.Deadband = item.Deadband;
        if ( item.SamplingRateSpecified )
          row.SamplingRate = item.SamplingRate;
        if ( item.EnableBufferingSpecified )
          row.EnableBuffering = item.EnableBuffering;
        row.Name = item.ItemName;
        if ( !string.IsNullOrEmpty( item.ItemPath ) )
          row.ItemPath = item.ItemPath;
        this.AddItemsRow( row );
        return row.ID;
      }
    }
    partial class ItemsRow
    {
      /// <summary>
      /// Gets or sets the type of the requested.
      /// </summary>
      /// <value>The type of the requested.</value>
      public System.Type RequestedType
      {
        get { return IsRequestedTypeFullNameNull() ? null : System.Type.GetType( this.RequestedTypeFullName ); }
        set { if ( value != null ) this.RequestedTypeFullName = value.FullName; }
      }
      /// <summary>
      /// Gets the item form the configuration.
      /// </summary>
      /// <value>The item <see cref="Item"/>.</value>
      public Item Item
      {
        get
        {
          Opc.Da.Item ret = new Item
          {
            ClientHandle = Guid.NewGuid(),
            ItemName = Name,
            ActiveSpecified = !IsActiveNull(),
            DeadbandSpecified = !IsDeadbandNull(),
            EnableBufferingSpecified = !IsEnableBufferingNull(),
            ItemPath = IsItemPathNull() ? null : ItemPath,
            ReqType = RequestedType,
            SamplingRateSpecified = !IsSamplingRateNull(),
            MaxAgeSpecified = !IsMaxAgeNull()
          };
          if ( ret.ActiveSpecified )
            Active = Active;
          if ( ret.DeadbandSpecified )
            Deadband = Deadband;
          if ( ret.EnableBufferingSpecified )
            EnableBuffering = EnableBuffering;
          if ( ret.MaxAgeSpecified )
            MaxAge = MaxAge;
          if ( ret.SamplingRateSpecified )
            SamplingRate = SamplingRate;
          return ret;
        }
      }
    }
    partial class SubscriptionsDataTable
    {
      /// <summary>
      /// Saves the <see cref="Opc.Da.SubscriptionState"/> in the configuration data set..
      /// </summary>
      /// <param name="state">The state.</param>
      /// <param name="enabled">if set to <c>true</c> the <see cref="Subscription"/> is enabled.</param>
      /// <param name="asynchronous">if set to <c>true</c> [asynchronous].</param>
      /// <param name="parentKey">The parent key.</param>
      /// <returns></returns>
      public long Save( SubscriptionState state, bool enabled, bool asynchronous, long parentKey )
      {
        SubscriptionsRow row = this.NewSubscriptionsRow();
        row.Active = state.Active;
        row.Deadband = state.Deadband;
        row.ID_server = parentKey;
        row.KeepAliveRate = state.KeepAlive;
        if ( !string.IsNullOrEmpty( state.Locale ) )
          row.Locale = state.Locale;
        row.Name = state.Name;
        row.UpdateRate = state.UpdateRate;
        row.Enabled = enabled;
        row.Asynchronous = asynchronous;
        this.AddSubscriptionsRow( row );
        return row.ID;
      }
    }
    public partial class SubscriptionsRow
    {
      /// <summary>
      /// Gets the state of the <see cref="SubscriptionState"/>.
      /// </summary>
      /// <value>The state of a subscription.</value>
      public SubscriptionState CreateSubscriptionState
      {
        get
        {
          SubscriptionState state = new SubscriptionState()
          {
            Active = Active,
            Name = Name,
            ClientHandle = Guid.NewGuid(),
            UpdateRate = UpdateRate
          };
          if ( !IsDeadbandNull() )
            state.Deadband = Deadband;
          if ( !IsKeepAliveRateNull() )
            state.KeepAlive = KeepAliveRate;
          if ( !IsLocaleNull() )
            state.Locale = Locale;
          return state;
        }
      }
    }
    partial class ServersDataTable
    {
      /// <summary>
      /// Saves the <see cref="Opc.Da.Server"/> in the configuration data set.
      /// </summary>
      /// <param name="server">The server.</param>
      /// <param name="connectData">The connect data.</param>
      /// <param name="locale">The locale.</param>
      /// <param name="filter">The filter.</param>
      /// <returns></returns>
      public long Save( Opc.Da.Server server, ConnectData connectData, string locale, ResultFilter filter )
      {
        ServersRow rw = this.NewServersRow();
        rw.IsConnected = server.IsConnected;
        rw.Name = server.Name;
        rw.PreferedSpecyficationID = server.PreferedSpecyfication.ID;
        rw.PreferedSpecyficationDsc = server.PreferedSpecyfication.Description;
        rw.URL = server.Url.ToString();
        if ( !string.IsNullOrEmpty( locale ) )
          rw.Locale = locale;
        rw.Filter = (int)filter;
        this.AddServersRow( rw );
        if ( connectData != null )
          ( (OPCCliConfiguration)this.DataSet ).ConnectData.Save( connectData, rw.ID );
        return rw.ID;
      }
    }
    partial class ServersRow
    {
      /// <summary>
      /// Gets the options.
      /// </summary>
      /// <param name="options">The options.</param>
      public void GetOptions( IOptions options )
      {
        if ( !this.IsLocaleNull() )
          options.Locale = Locale;
        if ( !this.IsFilterNull() )
          options.Filter = (global::Opc.Da.ResultFilter)Filter;
      }
      /// <summary>
      /// Gets the prefered specification.
      /// </summary>
      /// <value>The prefered specification.</value>
      public global::Opc.Specification PreferedSpecification
      {
        get
        {
          global::Opc.Specification preferedspec = new global::Opc.Specification()
          {
            ID = this.PreferedSpecyficationID,
            Description = PreferedSpecyficationDsc
          };
          return preferedspec;
        }
      }
    }
    partial class TransactionsDataTable
    {
      /// <summary>
      /// Saves the specified name.
      /// </summary>
      /// <param name="Name">The name.</param>
      /// <param name="IDItemIn">The  item in ID.</param>
      /// <param name="Deadband">The deadband.</param>
      /// <param name="MinUpdateRate">The min update rate.</param>
      /// <param name="TransactionRate">The transaction rate.</param>
      /// <param name="Comment">The comment.</param>
      /// <param name="BadQualityValue">The bad quality value.</param>
      /// <param name="StopIfBadQuality">if set to <c>true</c> [stop if bad quality].</param>
      /// <returns></returns>
      public long Save( string Name, long? IDItemIn, int? Deadband,
        int? MinUpdateRate, int TransactionRate, string Comment, string BadQualityValue, bool StopIfBadQuality )
      {
        TransactionsRow rw = this.NewTransactionsRow();
        rw.Name = Name;
        if ( IDItemIn != null )
          rw.ID_itemIN = (long)IDItemIn;
        if ( Deadband != null )
          rw.Deadband = (int)Deadband;
        if ( MinUpdateRate != null )
          rw.MinUpdateRate = (int)MinUpdateRate;
        rw.TransactionRate = TransactionRate;
        rw.Comment = Comment;
        rw.BadQualityValue = BadQualityValue;
        rw.StopIfBadQuality = StopIfBadQuality;
        this.AddTransactionsRow( rw );
        return rw.ID;
      }
    }
    public partial class TransactionsRow
    {
      /// <summary>
      /// Gets identifier of the input item .
      /// </summary>
      /// <returns>identifier if any</returns>
      public long? InputItemIdentifier
      {
        get
        {
          if ( IsID_itemINNull() )
            return null;
          else
            return (long)ID_itemIN;
        }
      }
    }
    partial class OperationsDataTable
    {
      /// <summary>
      /// Saves the specified name.
      /// </summary>
      /// <param name="Name">The name.</param>
      /// <param name="IDTransaction">The  transaction ID.</param>
      /// <param name="Parameter">The parameter.</param>
      /// <param name="OperationType">Type of the operation.</param>
      /// <param name="IDItem">The  item ID.</param>
      /// <param name="Comment">The comment.</param>
      /// <returns></returns>
      public long Save( string Name, long IDTransaction, string Parameter,
        Guid OperationType, long? IDItem, string Comment )
      {
        if ( IDTransaction != long.MinValue )
        {
          OperationsRow rw = this.NewOperationsRow();
          rw.Name = Name;
          rw.ID_Transaction = IDTransaction;
          rw.Param = Parameter;
          if ( IDItem != null )
            rw.ID_Item = (long)IDItem;
          rw.OperationType = OperationType;
          rw.Comment = Comment;
          this.AddOperationsRow( rw );
          return rw.ID;
        }
        return long.MinValue;
      }
    }
    public partial class OperationsRow
    {
      /// <summary>
      /// Gets the identifier of the Input item.
      /// </summary>
      /// <returns>Identifier or null</returns>
      public long? InputItemIdentifier
      {
        get
        {
          if ( IsID_ItemNull() )
            return null;
          else
            return (long)ID_Item;
        }
      }
    }
    partial class OperationLinksDataTable
    {
      /// <summary>
      /// Saves the specified  operation.
      /// </summary>
      /// <param name="IDOperation">The operation ID.</param>
      /// <param name="InputNumber">The input number.</param>
      /// <param name="IDChildOperation">The  child operation ID.</param>
      /// <param name="ChildOutputNumber">The child output number.</param>
      public void Save( long IDOperation, int InputNumber, long IDChildOperation, int ChildOutputNumber )
      {
        OPCCliConfiguration.OperationLinksRow existrow =
          this.FindByID_OperationInput_number( IDOperation, InputNumber );
        if ( existrow != null )
        {
          Debug.Assert( IDChildOperation == existrow.IDChild_Operation,
            "This row already exist but contains other values" );
          Debug.Assert( ChildOutputNumber == existrow.ChildOutput_number,
            "This row already exist but contains other values" );
          return;
        }
        OperationLinksRow rw = this.NewOperationLinksRow();
        rw.ID_Operation = IDOperation;
        rw.Input_number = InputNumber;
        rw.IDChild_Operation = IDChildOperation;
        rw.ChildOutput_number = ChildOutputNumber;
        this.AddOperationLinksRow( rw );
      }
    }
  }
}
