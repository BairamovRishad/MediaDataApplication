using System;

namespace MediaDataApplication.WcfService.BDO {

    public class FileChunkBDO {
        public string FileName { get; set; }
        public int Length { get; set; }
        public Int64 Offset { get; set; }
    }

}