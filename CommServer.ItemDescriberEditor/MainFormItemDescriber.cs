//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using BaseStation.ItemDescriber;
using Opc.Da;
using System;
using System.Reflection;
using System.Windows.Forms;
using UAOOI.Windows.Forms;
using UAOOI.Windows.Forms.CodeProtectControls;
using CSVManagement = BaseStation.ItemDescriber.CSVManagement;

namespace CAS.CommServer.DA.ItemDescriberEditor
{

  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public partial class MainFormItemDescriber : Form
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="MainFormItemDescriber"/> class.
    /// </summary>
    public MainFormItemDescriber()
    {
      InitializeComponent();
      ClearAndInitialize();
    }

    #region private
    private ItemDecriberDataSet m_ItemDescriberDataSet;
    private void CloseForm()
    {
      Close();
    }
    private void ClearAndInitialize()
    {
      m_ItemDescriberDataSet.ItemProperty.Clear();
      m_ItemDescriberDataSet.Items.Clear();
      m_ItemDescriberDataSet.Property.Clear();
      foreach (PropertyDescription property in PropertyDescription.Enumerate())
      {
        ItemDecriberDataSet.PropertyRow row = m_ItemDescriberDataSet.Property.NewPropertyRow();
        row.Code = property.ID.Code;
        row.Name = property.Name;
        m_ItemDescriberDataSet.Property.AddPropertyRow(row);
      }
    }
    private void SaveXML()
    {
      SaveFileDialog saveXMLFileDialog = new SaveFileDialog
      {
        InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
      };
      switch (saveXMLFileDialog.ShowDialog())
      {
        case DialogResult.OK:
          XMLManagement myConfig = new XMLManagement();
          myConfig.writeXMLFile(m_ItemDescriberDataSet, saveXMLFileDialog.FileName);
          m_ItemDescriberDataSet.Items.AcceptChanges();
          m_ItemDescriberDataSet.ItemProperty.AcceptChanges();
          break;
        default:
          break;
      }
    }
    private void LoadXML()
    {
      if ((m_ItemDescriberDataSet.Items.GetChanges() != null) || (m_ItemDescriberDataSet.ItemProperty.GetChanges() != null))
      {
        if (MessageBox.Show(this, "Save current data?", "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          SaveXML();
      }
      OpenFileDialog openFileDialogXMLFile = new System.Windows.Forms.OpenFileDialog
      {
        InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
        Filter = "XML files (*.XML)|*.XML",
        DefaultExt = ".XML"
      };
      switch (openFileDialogXMLFile.ShowDialog())
      {
        case DialogResult.OK:
          m_ItemDescriberDataSet.ItemProperty.Clear();
          m_ItemDescriberDataSet.Items.Clear();
          m_ItemDescriberDataSet.Property.Clear();
          XMLManagement myConfig = new XMLManagement();
          myConfig.readXMLFile(m_ItemDescriberDataSet, openFileDialogXMLFile.FileName);
          //((Button)sender).Enabled = false;
          Text = "Item Describer: " + openFileDialogXMLFile.FileName;
          m_ItemDescriberDataSet.Items.AcceptChanges();
          m_ItemDescriberDataSet.ItemProperty.AcceptChanges();
          break;
        default:
          break;
      }
    }
    private void ExportCSV()
    {
      SaveFileDialog saveXMLFileDialog = new SaveFileDialog
      {
        InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
        Filter = "CSV files (*.CSV)|*.CSV",
        DefaultExt = ".CSV"
      };
      switch (saveXMLFileDialog.ShowDialog())
      {
        case System.Windows.Forms.DialogResult.OK:
          CSVManagement myConfig = new CSVManagement();
          myConfig.SaveCSV(m_ItemDescriberDataSet, saveXMLFileDialog.FileName);
          //itemDecriberDataSet1.Items.AcceptChanges();
          //itemDecriberDataSet1.ItemProperty.AcceptChanges();
          break;
        default:
          break;
      }
    }
    private void ImportCSV()
    {
      if ((m_ItemDescriberDataSet.Items.GetChanges() != null) || (m_ItemDescriberDataSet.ItemProperty.GetChanges() != null))
      {
        if (MessageBox.Show(this, "Save current data?", "Data changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          SaveXML();
      }
      OpenFileDialog openFileDialogXMLFile = new OpenFileDialog
      {
        InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
        Filter = "CSV files (*.CSV)|*.CSV",
        DefaultExt = ".CSV"
      };
      switch (openFileDialogXMLFile.ShowDialog())
      {
        case System.Windows.Forms.DialogResult.OK:
          CSVManagement _csvManagement = new CSVManagement();
          _csvManagement.LoadCSV(m_ItemDescriberDataSet, openFileDialogXMLFile.FileName);
          break;
        default:
          break;
      }
    }

    #region handlers
    private void menuItem10_Click(object sender, System.EventArgs e)
    {
      CloseForm();
    }
    private void menuItem3_Click(object sender, System.EventArgs e)
    {
      LoadXML();
    }
    private void menuItem7_Click(object sender, System.EventArgs e)
    {
      SaveXML();
    }
    private void menuItem2_Click(object sender, System.EventArgs e)
    {
      if (MessageBox.Show(this, "Clear all data grids???", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        ClearAndInitialize();
      }
    }
    private void menuItem9_Click(object sender, System.EventArgs e)
    {
      ExportCSV();
    }
    private void ShowAboutDialog()
    {
      string usr = "";
      Assembly cMyAss = Assembly.GetEntryAssembly();
      using (AboutForm cAboutForm = new AboutForm(null, usr, cMyAss))
      {
        cAboutForm.ShowDialog();
      }
    }
    private void menuItem12_Click(object sender, System.EventArgs e)
    {
      ShowAboutDialog();
    }
    private void menuItem6_Click(object sender, System.EventArgs e)
    {
      ImportCSV();
    }
    private void menuItem14_Click(object sender, EventArgs e)
    {
      string usr = "";
      Assembly cMyAss = Assembly.GetEntryAssembly();
      using (LicenseForm cAboutForm = new LicenseForm(null, usr, cMyAss))
      {
        using (Licenses cLicDial = new Licenses())
        {
          cAboutForm.SetAdditionalControl = cLicDial;
          cAboutForm.LicenceRequestMessageProvider = new LicenseForm.LicenceRequestMessageProviderDelegate(delegate () { return cLicDial.GetLicenseMessageRequest(); });
          cAboutForm.ShowDialog();
        }
      }
    }
    #endregion    

    #endregion

  }
}
