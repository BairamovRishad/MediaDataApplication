using System;
using System.Collections.Generic;
using System.Linq;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using Microsoft.Ajax.Utilities;

namespace MediaDataApplication.AspNetMvcClient.Helpers {

    public sealed class SimpleMediaSearchEngine {
        public static IEnumerable<string> Search(string termString, MediaMetadata[] mediaMetadata) {
            var terms = termString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var searchResults = (from metadata in mediaMetadata 
                                 let fileName = metadata.FileName.ToLower() 
                                 let description = metadata.Description.IsNullOrWhiteSpace() ? String.Empty : metadata.Description.ToLower()
                                 let text = fileName + description 
                                 where terms.Any(term => text.Contains(term.ToLower()))
                                 select metadata.FileName).ToList();

            return searchResults;
        }

    }

}