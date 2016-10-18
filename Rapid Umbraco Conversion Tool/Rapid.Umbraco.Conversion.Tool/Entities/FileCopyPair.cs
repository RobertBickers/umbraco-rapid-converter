using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codetreehouse.RapidUmbracoConverter.Tools.Entities
{
    public class FileCopyPair
    {
        public FileCopyPair(string directoryRoot, string source, string destination)
        {
            DirectoryRoot = directoryRoot;
            Source = source;
            Destination = destination;
        }

        public string Destination { get; internal set; }
        public string DirectoryRoot { get; private set; }
        public string Source { get; internal set; }


        public string CombinedSource
        {
            get
            {
                return (DirectoryRoot += Source).Replace("/~/", "/").Replace("/", "\\").Replace("\\~", "\\");
            }
        }

        public string CombinedDestination
        {
            get
            {
                return (DirectoryRoot += Destination).Replace("/~/", "/").Replace("/", "\\").Replace("\\~", "\\");

            }
        }

    }
}
