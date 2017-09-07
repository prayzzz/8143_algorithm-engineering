using System.IO;
using Microsoft.Win32;

namespace AE.AuditPlanning.Presentation.Common
{
    public static class FileDialogHelper
    {
        public static string OpenCsvFileDialog(string currentPath)
        {
            return OpenFileDialog(currentPath, ".csv", "Kommaliste (*.csv)|*.csv");
        }

        public static string OpenFileDialog(string currentPath, string defaultExt, string filter)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.CheckFileExists = true;
            fileDialog.DefaultExt = defaultExt;
            fileDialog.Filter = filter;

            if (File.Exists(currentPath))
            {
                fileDialog.InitialDirectory = Path.GetDirectoryName(currentPath);
                fileDialog.FileName = currentPath;
            }

            return fileDialog.ShowDialog() == true ? fileDialog.FileName : currentPath;
        }

        public static bool SaveFileDialog(string currentPath, string defaultExt, string filter, out string path)
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.DefaultExt = defaultExt;
            fileDialog.Filter = filter;

            var dir = Path.GetDirectoryName(currentPath);
            if (Directory.Exists(dir))
            {
                fileDialog.InitialDirectory = dir;
            }

            var v = fileDialog.ShowDialog().Value;
            path = fileDialog.FileName;
            return v;
        } 
    }
}