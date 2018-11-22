using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDbExplorer
{
  public class Paths : INotifyPropertyChanged
  {
    static Paths()
    {
      if (!Directory.Exists(ApplicationDataFolder))
      {
        Directory.CreateDirectory(ApplicationDataFolder);
      }
    }

    private static readonly string ApplicationName = "LiteDbExplorer";
    public static string ProgramFolder => Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
    public static string ApplicationDataFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);

    public static string UninstallerPath => Path.Combine(ProgramFolder, "uninstall.exe");
    public static string RecentFilesPath => Path.Combine(ApplicationDataFolder, "recentfiles.txt");
    public static string SettingsFilePath => Path.Combine(ApplicationDataFolder, "settings.json");

    public static string TempPath
    {
      get
      {
        var path = Path.Combine(Path.GetTempPath(), ApplicationName);
        if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
        return path;
      }
    }

    private ObservableCollection<string> recentFiles;

    public ObservableCollection<string> RecentFiles
    {
      get
      {
        if (recentFiles == null)
        {
          if (File.Exists(RecentFilesPath))
          {
            recentFiles = new ObservableCollection<string>(File.ReadLines(RecentFilesPath));
          }
          else
          {
            recentFiles = new ObservableCollection<string>();
          }

          recentFiles.CollectionChanged += RecentFiles_CollectionChanged;
        }

        return recentFiles;
      }

      set
      {
        recentFiles = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RecentFiles"));
      }
    }

    private void RecentFiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      File.WriteAllText(RecentFilesPath, String.Join(Environment.NewLine, RecentFiles));
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}