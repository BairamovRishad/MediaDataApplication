using System;
using System.IO;
using MediaDataApplication.WcfService.DAL.Helpers;

namespace MediaDataApplication.WcfService.DAL.Helpers {

    internal abstract class UserFilesManager {
        private static readonly string _dataDirectory = App.DataDirectory + "\\media";
        private readonly string userName;

        protected UserFilesManager(string userName) {
            this.userName = userName;
        }

        public void CopyTo(string fileName, Stream file, out Int64 fileLength) {
            var fileAbsolutePath = this.GetFileAbsolutePath(fileName);

            string fileDirAbsolutePath = Directory.GetParent(fileAbsolutePath).FullName;
            if (!Directory.Exists(fileDirAbsolutePath)) {
                Directory.CreateDirectory(fileDirAbsolutePath);
            }

            using (
                var destinationStream = new FileStream(fileAbsolutePath, FileMode.Append, FileAccess.Write, FileShare.None)) {
                file.CopyTo(destinationStream);
                fileLength = destinationStream.Length;
            }
        }

        public void Delete(string fileName) {
            string fileAbsolutePath = this.GetFileAbsolutePath(fileName);

            if (!File.Exists(fileAbsolutePath)) {
                throw new FileNotFoundException("File not found: ", fileName);
            }

            File.Delete(fileAbsolutePath);
        }

        protected abstract string GetRelativePath(string fileName);

        public Stream OpenRead(string fileName) {
            var fileAbsolutePath = this.GetFileAbsolutePath(fileName);

            if (!File.Exists(fileAbsolutePath)) {
                throw new FileNotFoundException("File not found: ", fileName);
            }

            Stream result = new MemoryStream();

            using (var stream = new FileStream(fileAbsolutePath, FileMode.Open, FileAccess.Read)) {
                stream.CopyTo(result);
                result.Seek(0, SeekOrigin.Begin);
            }

            return result;
        }

        public Stream OpenRead(string fileName, Int64 offset, int length) {
            var fileAbsolutePath = this.GetFileAbsolutePath(fileName);

            if (!File.Exists(fileAbsolutePath)) {
                throw new FileNotFoundException("File not found: ", fileName);
            }

            Stream result = new MemoryStream();

            using (var stream = new FileStream(fileAbsolutePath, FileMode.Open, FileAccess.Read)) {
                var buffer = new byte[length];
                stream.Position = offset;
                int bytesRead = stream.Read(buffer, 0, length);
                result.Write(buffer, 0, bytesRead);
                result.Seek(0, SeekOrigin.Begin);
            }

            return result;
        }

        public void Rename(string previousFileName, string newFileName) {
            string previousFileAbsolutePath = this.GetFileAbsolutePath(previousFileName);
            string newFileAbsolutePath = this.GetFileAbsolutePath(newFileName);

            File.Move(previousFileAbsolutePath, newFileAbsolutePath);
        }

        #region Private Helpers

        private string GetFileAbsolutePath(string fileName) {
            string fileRelativePath = this.GetRelativePath(fileName);

            string fileAbsolutePath = Path.Combine(_dataDirectory, this.userName, fileRelativePath);

            return fileAbsolutePath;
        }

        #endregion
    }

}

internal class UserMediaThumbFilesManager : UserFilesManager {
    private const string THUMBS_DIRECTORY = "_thumbs";

    public UserMediaThumbFilesManager(string userName) : base(userName) { }

    protected override string GetRelativePath(string fileName) {
        return Path.Combine(THUMBS_DIRECTORY, fileName);
    }
}

internal class UserMediaFilesManager : UserFilesManager {
    public UserMediaFilesManager(string userName) : base(userName) { }

    protected override string GetRelativePath(string fileName) {
        return fileName;
    }
}