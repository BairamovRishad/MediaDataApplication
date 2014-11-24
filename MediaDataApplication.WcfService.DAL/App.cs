using System;
using System.IO;

namespace MediaDataApplication.WcfService.DAL {

    internal static class App {
        static App() {
            InitDataDirectory();
        }

        public static string DataDirectory { get; private set; }

        public static void InitDataDirectory() {
            try {
                var currentDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent;
                var solutionDirName = "MediaDataApplication".ToLower();
                while (!currentDir.FullName.ToLower().EndsWith(solutionDirName)) {
                    currentDir = currentDir.Parent;
                }
                var solutionDirPath = currentDir.FullName;
                DataDirectory = Path.Combine(solutionDirPath, @"MediaDataApplication.MediaDataStorage");
                AppDomain.CurrentDomain.SetData("DataDirectory", DataDirectory);
            }
            catch {
                throw new Exception("Can't find the database. Should be in '(Solution Folder)/MediaDataApplication.MediaDataStorage'");
            }
        }
    }

}