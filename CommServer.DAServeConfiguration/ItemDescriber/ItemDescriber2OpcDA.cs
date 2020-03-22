//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using Opc.Da;

namespace UAOOI.ProcessObserver.Configuration.ItemDescriber
{
  /// <summary>
  /// Summary description for ItemDescriber2OpcDA.
  /// </summary>
  public static class ItemDescriber2OpcDA
  {
    /// <summary>
    /// static function that read item property collection for selected item
    /// </summary>
    /// <param name="ItemName">item name to be read</param>
    /// <param name="ds">data set with settings</param>
    /// <returns>collection of properties</returns>
    public static ItemPropertyCollection GetItemPropertiesCollection(string ItemName, ItemDecriberDataSet ds)
    {
      ItemPropertyCollection _ret = null;
      if (ds != null)
      {
        _ret = new ItemPropertyCollection();
        foreach (ItemDecriberDataSet.ItemsRow _row in ds.Items.Rows)
        {
          if (_row.ItemName == ItemName)
          {
            foreach (ItemDecriberDataSet.ItemPropertyRow row_property in _row.GetItemPropertyRows())
            {
              PropertyDescription prop_dsc = PropertyDescription.Find(new PropertyID(row_property.PropertyCode));
              ItemProperty itemprop = new ItemProperty
              {
                ID = prop_dsc.ID,
                Value = row_property.Value
              };
              _ret.Add(itemprop);
            }
            break;
          }
        }
      }
      return _ret;
    }
  }
}
