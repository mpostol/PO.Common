//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CodeProtect;
using System;
using System.IO;

namespace UAOOI.ProcessObserver.Configuration.Settings
{
  /// <summary>
  /// Summary description for AppConfigManagement.
  /// </summary>
  [Obsolete("Use Settings and move the parameters to the application context.")]
  public class AppConfigManagement
  {
    /// <summary>
    /// Specifies the file, where the communication configuration was saved .
    /// </summary>
    public static readonly string filename;

    /// <summary>
    /// The value that is sent to client when status of the tag is bad.
    /// </summary>
    public static readonly float PLCNaN = 65535;

    /// <summary>
    /// The value indicates the number of frames when tag statistics are updated.
    /// </summary>
    public static readonly ushort MinAvgMax_Tag_management = 50;

    /// <summary>
    /// The value indicates the number of frames when group statistics are updated.
    /// </summary>
    public static readonly ushort MinAvgMax_Group_management = 50;

    /// <summary>
    /// The value indicates the number of frames when read group statistics are updated.
    /// </summary>
    public static readonly ushort MinAvgMax_Group_Read_management = 500;

    /// <summary>
    /// The value indicates the number of frames when transaction statistics are updated.
    /// </summary>
    public static readonly ushort MinAvgMax_Transaction_management = 50;

    /// <summary>
    /// The value indicates the number of frames when frame response statistics are updated.
    /// </summary>
    public static readonly ushort MinAvgMax_FrameResponse_management = 50;

    /// <summary>
    /// The value indicates the number of characters (bytes) in a communication channel when Character Gap statistics are updated
    /// </summary>
    public static readonly ushort MinAvgMax_CharacterGap_management = 500;

    /// <summary>
    /// The value indicates the waiting time of reconnection of the server (in seconds).
    /// </summary>
    public static readonly uint WaitForReconnectServerInSec = 50;

    /// <summary>
    /// The value indicates the waiting time of update for first group(in milliseconds).
    /// </summary>
    public static readonly int WaitForFirstGroupUpdateSendInMiliSec = 5000;

    /// <summary>
    /// The value indicates the  path channels inside the OPC diagnostic tag.
    /// </summary>
    public static string OPCPathChannels => "Internal/Channels/";

    /// <summary>
    /// The value indicates the  path stations inside the OPC diagnostic tag.
    /// </summary>
    public static string OPCPathStations => "Internal/Stations/";

    /// <summary>
    /// indicate whether to use local time or UTC time
    /// </summary>
    public static readonly bool UseLocalTime = false;

    /// <summary>
    /// indicate whether Timestamp is used t Check if Update  occurs
    /// </summary>
    public static readonly bool UseTimeStampToCheckForUpdate = false;

    #region private

    static AppConfigManagement()
    {
      PLCNaN = ApplicationConfiguration.GetAppSetting("PLCNaN", PLCNaN);
      MinAvgMax_Tag_management = ApplicationConfiguration.GetAppSetting("MinAvgMax_Tag_management", MinAvgMax_Tag_management);
      MinAvgMax_Group_management = ApplicationConfiguration.GetAppSetting("MinAvgMax_Group_management", MinAvgMax_Group_management);
      MinAvgMax_Transaction_management = ApplicationConfiguration.GetAppSetting("MinAvgMax_Transaction_management", MinAvgMax_Transaction_management);
      MinAvgMax_Group_Read_management = ApplicationConfiguration.GetAppSetting("MinAvgMax_Group_Read_management", MinAvgMax_Group_Read_management);
      MinAvgMax_FrameResponse_management = ApplicationConfiguration.GetAppSetting("MinAvgMax_FrameResponse_management", MinAvgMax_FrameResponse_management);
      MinAvgMax_CharacterGap_management = ApplicationConfiguration.GetAppSetting("MinAvgMax_CharacterGap_management", MinAvgMax_CharacterGap_management);
      WaitForReconnectServerInSec = ApplicationConfiguration.GetAppSetting("WaitForReconnectServerInSec", WaitForReconnectServerInSec);
      WaitForFirstGroupUpdateSendInMiliSec = ApplicationConfiguration.GetAppSetting("WaitForFirstGroupUpdateSendInMiliSec", WaitForFirstGroupUpdateSendInMiliSec);
      UseTimeStampToCheckForUpdate = ApplicationConfiguration.GetAppSetting("UseTimeStampToCheckForUpdate", UseTimeStampToCheckForUpdate);
      UseLocalTime = ApplicationConfiguration.GetAppSetting("UseLocalTime", UseLocalTime);
      filename = ApplicationConfiguration.GetAppSetting("configfile", "");
      FileInfo fi;
      if (filename == null || filename.Length < 3)
        fi = RelativeFilePathsCalculator.GetAbsolutePathToFileInApplicationDataFolder("\\DemoConfiguration.oses");
      else
        fi = RelativeFilePathsCalculator.GetAbsolutePathToFileInApplicationDataFolder(filename);
      filename = fi.FullName;
    } //static AppConfigManagement

    #endregion private
  }
}