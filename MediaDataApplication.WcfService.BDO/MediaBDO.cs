using System;
using System.IO;

namespace MediaDataApplication.WcfService.BDO {

    public class MediaBDO {
        public string MediaFileName { get; set; }

        public Int64 Length { get; set; }

        public Stream FileStream { get; set; }
    }

}