namespace CAS.Lib.CommonBus
{
  partial class CommonBusControl
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    /// <summary>
    /// Track whether Dispose has been called.
    /// </summary>
    private bool disposed = false;
    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposed )
        return;
      if ( disposing )
      {
        if ( components != null )
          components.Dispose();
        if ( m_TraceSource != null )
          m_TraceSource.Close();
      }
      disposed = true;
      base.Dispose( disposing );
    }
    #region Component Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.m_EeventLog = new System.Diagnostics.EventLog();
      ( (System.ComponentModel.ISupportInitialize)( this.m_EeventLog ) ).BeginInit();
      // 
      // m_EeventLog
      // 
      this.m_EeventLog.Log = "Application";
      this.m_EeventLog.Source = "CAS.CommonBus";
      ( (System.ComponentModel.ISupportInitialize)( this.m_EeventLog ) ).EndInit();
    }
    #endregion
    private System.Diagnostics.EventLog m_EeventLog;
  }
}
