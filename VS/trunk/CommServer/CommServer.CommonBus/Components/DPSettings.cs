//<summary>
//  Title   : DP Setting : this UserControl alow user to set settings for the DPSettings
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

using System;
using System.Windows.Forms;

namespace CAS.Lib.CommonBus.Components
{
  /// <summary>
  /// this UserControl alow user to set settings for the DPSettings
  /// </summary>
  public partial class DPSettings: UserControl
  {
    #region creator
      /// <summary>
      /// default creator of the DPSettings
      /// </summary>
    public DPSettings()
    {
      InitializeComponent();
    }
    #endregion
    #region public
      /// <summary>
      /// sets the source object for the property window
      /// </summary>
      /// <param name="pDPID"></param>
    public void SetObjects( IDataProviderID pDPID )
    {
      cn_PropertyGridApp.SelectedObject = pDPID;
      cn_PropertyGridComm.SelectedObject = pDPID.SelectedCommunicationLayer;
    }
    #endregion
    #region events handlers
    private void cn_ToolStripButtonApp_Click( object sender, EventArgs e )
    {
      cn_SplitContainer.Panel1Collapsed = !cn_ToolStripButtonApp.Checked;
      cn_ToolStripButtonComm.Checked = !cn_SplitContainer.Panel2Collapsed;
    }
    private void cn_ToolStripButtonComm_Click( object sender, EventArgs e )
    {
      cn_SplitContainer.Panel2Collapsed = !cn_ToolStripButtonComm.Checked;
      cn_ToolStripButtonApp.Checked = !cn_SplitContainer.Panel1Collapsed;
    }
    #endregion
  }
}
