//<summary>
//  Title   : Class to save and restore address space data base to/from external dictionary file.
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
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using CAS.DataPorter.Configurator.Properties;
using CAS.Lib.RTLib.Utils;

namespace CAS.DataPorter.Configurator
{
  /// <summary>
  /// Class to save and restore address space data base to/from external dictionary file. c:\VS2008\Projects\OPCViewerTrunk\PR24-Biblioteka\DataPorterConfigLib\ConfigurationManagement.cs
  /// </summary>
  public partial class ConfigurationManagement: Component
  {

    #region private
    private bool saveInProgress = false;
    private bool m_Empty = true;
    private void UpdateCurrentDirectoryInConfigurationFile( FileDialog fileDialog )
    {
      try
      {
        Settings.Default.InitialDirectory = fileDialog.InitialDirectory;
        if ( !string.IsNullOrEmpty( fileDialog.FileName ) )
        {
          FileInfo fi = new FileInfo( fileDialog.FileName );
          if ( fi.Exists )
          {
            Settings.Default.InitialDirectory = fi.DirectoryName;
            m_OpenFileDialog.FileName = fi.FullName;
            m_SaveFileDialog.FileName = fi.FullName;
          }
        }
        m_OpenFileDialog.InitialDirectory = Settings.Default.InitialDirectory;
        m_SaveFileDialog.InitialDirectory = Settings.Default.InitialDirectory;
      }
      catch { }
    }
    private void SaveConfigurationFile()
    {
      try
      {
        Settings.Default.Save();
      }
      catch { }
    }
    #endregion
    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationManagement"/> class.
    /// </summary>
    public ConfigurationManagement()
    {
      InitializeComponent();
      m_TSMI_Open.Click += new EventHandler( OnOpen_Click );
      m_TSMI_Save.Click += new EventHandler( OnSave_Click );
      m_TSMI_SaveAs.Click += new EventHandler( OnSaveAs_Click );
      m_TSMI_New.Click += new EventHandler( m_TSMI_New_Click );
      if ( string.IsNullOrEmpty( Settings.Default.InitialDirectory ) )
        Settings.Default.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments );
      m_OpenFileDialog.InitialDirectory = Settings.Default.InitialDirectory;
      m_SaveFileDialog.InitialDirectory = Settings.Default.InitialDirectory;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationManagement"/> class.
    /// </summary>
    /// <param name="container">The container.</param>
    public ConfigurationManagement( IContainer container )
      : this()
    {
      container.Add( this );
    }
    #endregion
    #region public
    /// <summary>
    /// enum that provides additional informations from ConfigurationManagement function calls
    /// </summary>
    public enum AdditionalResultInfo
    {
      /// <summary>
      /// function has completed successfully
      /// </summary>
      OK,
      /// <summary>
      /// the cancel was pressed during function call
      /// </summary>
      Cancel,
      /// <summary>
      /// the exception has occured during function call
      /// </summary>
      Exception
    }
    /// <summary>
    /// Gets the configuartion.
    /// </summary>
    /// <value>The configuartion <see cref="OPCCliConfiguration"/>.</value>
    public OPCCliConfiguration Configuartion { get { return m_Configuration; } }
    /// <summary>
    /// Gets the menu.
    /// </summary>
    /// <value>The menu <see cref="ContextMenuStrip"/>.</value>
    public ToolStripItem[] Menu
    {
      get
      {
        ToolStripItem[] ret = new ToolStripItem[ 4 ];
        ret[ 0 ] = new ToolStripMenuItem
          ( m_TSMI_New.Text, m_TSMI_New.Image, new EventHandler( m_TSMI_New_Click ), "Configuration New" ) { ToolTipText = m_TSMI_New.ToolTipText };
        ret[ 1 ] = new ToolStripMenuItem
          ( m_TSMI_Open.Text, m_TSMI_Open.Image, new EventHandler( OnOpen_Click ), "Configuration Open" ) { ToolTipText = m_TSMI_Open.ToolTipText };
        ret[ 2 ] = new ToolStripMenuItem
          ( m_TSMI_Save.Text, m_TSMI_Save.Image, new EventHandler( OnSave_Click ), "Configuration Save" ) { ToolTipText = m_TSMI_Save.ToolTipText };
        ret[ 3 ] = new ToolStripMenuItem
          ( m_TSMI_SaveAs.Text, m_TSMI_SaveAs.Image, new EventHandler( OnSaveAs_Click ), "Configuration Save As" ) { ToolTipText = m_TSMI_SaveAs.ToolTipText };
        return ret;
      }
    }
    /// <summary>
    /// Specialized Event Argument <see cref="EventArgs"/>sent as parameter to events
    /// </summary>
    public class ConfigurationEventArg: EventArgs
    {
      /// <summary>
      /// Gets or sets the configuration.
      /// </summary>
      /// <value>The configuration.</value>
      public OPCCliConfiguration Configuration { get; private set; }
      /// <summary>
      /// Initializes a new instance of the <see cref="ConfigurationEventArg"/> class.
      /// </summary>
      /// <param name="config">The config.</param>
      public ConfigurationEventArg( OPCCliConfiguration config )
      {
        Configuration = config;
      }
    }
    /// <summary>
    /// Occurs when configuration has been chnged.
    /// </summary>
    public event EventHandler<ConfigurationEventArg> ConfigurationChnged;
    /// <summary>
    /// Occurs before configuration saving.
    /// </summary>
    public event EventHandler<ConfigurationEventArg> ConfigurationSaving;
    /// <summary>
    /// Gets or sets the default name of the file.
    /// </summary>
    /// <value>The default name of the file.</value>
    public string DefaultFileName
    {
      set
      {
        m_OpenFileDialog.FileName = value;
        m_Empty = true;
      }
      get { return m_OpenFileDialog.FileName; }
    }
    /// <summary>
    /// Read the address space data set from an external dictionary file. 
    /// </summary>
    /// <returns>it opens the file dialog to choose the file</returns>
    public bool Open()
    {
      return Open( string.Empty );
    }
    /// <summary>
    /// Read the address space data set from an external dictionary file. 
    /// </summary>
    /// <returns>it opens the file dialog to choose the file</returns>
    public bool Open( out AdditionalResultInfo AdditionalResult)
    {
      return Open( string.Empty,out AdditionalResult );
    }   /// <summary>
    /// Opens the specified file name.
    /// Read the address space data set from an external dictionary file.
    /// </summary>
    /// <param name="FileName">Name of the file.</param>
    /// <returns>if FileName parameter is empty, it opens the file dialog to choose the file</returns>
    public bool Open( string FileName )
    {
      AdditionalResultInfo AdditionalResult;
      return Open( FileName, out AdditionalResult );
    }
    /// <summary>
    /// Opens the specified file name.
    /// Read the address space data set from an external dictionary file.
    /// </summary>
    /// <param name="FileName">Name of the file.</param>
    /// <param name="AdditionalResult">The additional result.</param>
    /// <returns>
    /// if FileName parameter is empty, it opens the file dialog to choose the file
    /// </returns>
    public bool Open( string FileName, out AdditionalResultInfo AdditionalResult )
    {
      m_OpenFileDialog.InitialDirectory = CAS.Lib.CodeProtect.InstallContextNames.ApplicationDataPath;
      if ( string.IsNullOrEmpty( FileName ) )
      {
        if ( m_OpenFileDialog.ShowDialog() != DialogResult.OK )
        {
          AdditionalResult = AdditionalResultInfo.Cancel;
          return false;
        }
      }
      else
      {
        m_OpenFileDialog.FileName = FileName;
      }
      Cursor myPreviousCursor = Cursor.Current;
      try
      {

        Cursor.Current = Cursors.WaitCursor;
        Application.UseWaitCursor = true;
        ReadConfiguration( m_OpenFileDialog.FileName );
        m_Empty = false;
        UpdateCurrentDirectoryInConfigurationFile( m_OpenFileDialog );
        AdditionalResult = AdditionalResultInfo.OK;
        return true;
      }
      catch ( Exception ex )
      {
        MessageBox.Show
          ( ex.Message + Environment.NewLine + m_OpenFileDialog.FileName, 
          Properties.Resources.SessionFileOpenError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
        m_OpenFileDialog.FileName = "";
        AdditionalResult = AdditionalResultInfo.Exception;
        return false;
      }
      finally
      {
        Application.UseWaitCursor = false;
        Cursor.Current = myPreviousCursor;
      }
    }
    /// <summary>
    /// Reads the configuration.
    /// </summary>
    /// <param name="fileName">The fully qualified name of the file, or the relative file name.</param>
    /// <exception cref="FileNotFoundException">The exception that is thrown when an attempt to access a file 
    /// that does not exist on disk fails.
    /// </exception>
    public void ReadConfiguration( string fileName )
    {
      FileInfo info = RelativeFilePathsCalculator.GetAbsolutePathToFileInApplicationDataFolder( fileName );
      if ( !info.Exists )
        throw new FileNotFoundException( info.FullName );
      m_Configuration.Clear();
      m_Configuration.EnforceConstraints = false;
      m_Configuration.ReadXml( info.FullName, System.Data.XmlReadMode.IgnoreSchema );
      m_Configuration.EnforceConstraints = true;
      m_SaveFileDialog.FileName = m_OpenFileDialog.FileName = info.FullName;
      RaiseConfigurationChnged();
    }
    /// <summary>
    /// Save the address space data set in an external dictionary file. 
    /// </summary>
    /// <param name="prompt">If set to <c>true</c> show prompt to enter a file name.</param>
    /// <returns></returns>
    public bool Save( bool prompt )
    {
       if (saveInProgress)
         return false;
       try
       {
         saveInProgress = true;
         m_Configuration.Clear();
         if ( ConfigurationSaving != null )
           ConfigurationSaving( this, new ConfigurationEventArg( m_Configuration ) );
         prompt = m_Empty || prompt;
         if ( prompt )
         {
           m_SaveFileDialog.InitialDirectory = CAS.Lib.CodeProtect.InstallContextNames.ApplicationDataPath;
           m_SaveFileDialog.FileName = string.IsNullOrEmpty( DefaultFileName ) ? "OPCViewerDictionary" : DefaultFileName;
           if ( m_SaveFileDialog.ShowDialog() != DialogResult.OK )
             return false;
           m_Empty = false;
         }
         Cursor myPreviousCursor = Cursor.Current;
         try
         {
           Cursor.Current = Cursors.WaitCursor;
           Application.UseWaitCursor = true;
           m_Configuration.WriteXml( m_SaveFileDialog.FileName, System.Data.XmlWriteMode.IgnoreSchema );
         }
         catch ( Exception ex )
         {
           MessageBox.Show
             ( ex.Message, Properties.Resources.SessionFileSaveError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
         }
         finally
         {
           Application.UseWaitCursor = false;
           Cursor.Current = myPreviousCursor;
           UpdateCurrentDirectoryInConfigurationFile( m_SaveFileDialog );
         }
       }
       finally
       {
         saveInProgress = false;
       }

      return true;
    }
    /// <summary>
    /// Gets the open file dialog.
    /// </summary>
    /// <value>The open file dialog.</value>
    public OpenFileDialog OpenFileDialog { get { return m_OpenFileDialog; } }
    #region private menu handlers
    private void m_TSMI_New_Click( object sender, EventArgs e )
    {
      m_Configuration.Clear();
      m_Empty = true;
      RaiseConfigurationChnged();
    }
    private void RaiseConfigurationChnged()
    {
      if ( ConfigurationChnged != null )
        ConfigurationChnged( this, new ConfigurationEventArg( m_Configuration ) );
    }
    /// <summary>
    /// Called when SaveAs was clicked].
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void OnSaveAs_Click( object sender, EventArgs e )
    {
      Save( true );
    }
    /// <summary>
    /// Called when Save was clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void OnSave_Click( object sender, EventArgs e )
    {
      Save( false );
    }
    /// <summary>
    /// Called when Open was clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void OnOpen_Click( object sender, EventArgs e )
    {
      Open();
    }
    #endregion
    #endregion //public

  }
}
