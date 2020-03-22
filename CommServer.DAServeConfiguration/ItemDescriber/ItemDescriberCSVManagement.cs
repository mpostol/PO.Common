//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System.IO;

namespace UAOOI.ProcessObserver.Configuration.ItemDescriber
{
  /// <summary>
  /// Summary description for CSVManagement.
  /// </summary>
  public class CSVManagement
  {
    /// <summary>
    /// Saves the configuration in the CSV file.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <param name="filename">The filename.</param>
    public void SaveCSV(ItemDecriberDataSet config, string filename)
    {
      using (StreamWriter _sw = File.CreateText(filename))
      {
        _sw.Write("ItemID;ItemName;");
        foreach (ItemDecriberDataSet.PropertyRow _row in config.Property.Rows)
          _sw.Write(_row.Name + ";");
        _sw.WriteLine();
        foreach (ItemDecriberDataSet.ItemsRow _itemRow in config.Items.Rows)
        {
          _sw.Write(_itemRow.ItemID.ToString() + ";");
          _sw.Write(_itemRow.ItemName + ";");
          foreach (ItemDecriberDataSet.PropertyRow row in config.Property.Rows)
          {
            ItemDecriberDataSet.ItemPropertyRow[] itemProperties = _itemRow.GetItemPropertyRows();
            foreach (ItemDecriberDataSet.ItemPropertyRow _property in itemProperties)
              if (_property.PropertyCode.Equals(row.Code)) _sw.Write(_property.Value);
            _sw.Write(";");
          }
          _sw.WriteLine("ENDLINE;");
        }
      }
    }
    /// <summary>
    /// Loads the <see cref="ItemDecriberDataSet"/> form CSV file.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <param name="filename">The filename.</param>
    public void LoadCSV(ItemDecriberDataSet config, string filename)
    {
      StreamReader plik = new StreamReader(filename);//,System.Text.Encoding.Default);
      string _sourceText = plik.ReadToEnd();
      plik.Close();
      int pos = _sourceText.IndexOf("\r\n");
      _sourceText = _sourceText.Remove(0, pos + 2);
      _sourceText = _sourceText.Replace(";\r\n", ";");
      _sourceText = _sourceText.Replace("\r\n", ";");
      while (_sourceText.Length > 0)
      {
        pos = _sourceText.IndexOf(";");
        string itemId_str = _sourceText.Substring(0, pos);
        _sourceText = _sourceText.Remove(0, pos + 1);
        int itemID = System.Convert.ToInt32(itemId_str);
        pos = _sourceText.IndexOf(";");
        string item_name = _sourceText.Substring(0, pos);
        _sourceText = _sourceText.Remove(0, pos + 1);
        ItemDecriberDataSet.ItemsRow row = config.Items.NewItemsRow();
        row.ItemID = itemID;
        row.ItemName = item_name;
        config.Items.AddItemsRow(row);
        string wartosc = "";
        foreach (ItemDecriberDataSet.PropertyRow rowp in config.Property.Rows)
        {
          pos = _sourceText.IndexOf(";");
          wartosc = _sourceText.Substring(0, pos);
          _sourceText = _sourceText.Remove(0, pos + 1);
          if (wartosc != "")
          {
            ItemDecriberDataSet.ItemPropertyRow _row = config.ItemProperty.NewItemPropertyRow();
            _row.ItemID = itemID;
            _row.PropertyCode = rowp.Code;
            _row.Value = wartosc;
            config.ItemProperty.AddItemPropertyRow(_row);
          }
        }
        pos = _sourceText.IndexOf(";");
        wartosc = _sourceText.Substring(0, pos);
        _sourceText = _sourceText.Remove(0, pos + 1);
      }
    }
  }
}
