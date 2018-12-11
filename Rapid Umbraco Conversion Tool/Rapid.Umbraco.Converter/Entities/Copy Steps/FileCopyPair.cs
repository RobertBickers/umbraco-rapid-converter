using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codetreehouse.RapidUmbracoConverter.Tools.Entities
{
    public class FileCopyPair
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryRoot"></param>
        /// <param name="source"></param>
        /// <param name="markupReference">The string that will be used in the replace statement as shown in the markup</param>
        /// <param name="destination"></param>
        public FileCopyPair(string directoryRoot, string source, string markupReference, string destination)
        {
            DirectoryRoot = directoryRoot;
            Source = source;
            MarkupReference = markupReference;

            Destination = destination;

            CombinedSource = CombinePathsForCopy(directoryRoot, source);
            CombinedDestination = CombinePathsForCopy(directoryRoot, destination);
        }

        public string Destination { get; internal set; }
        public string DirectoryRoot { get; private set; }
        public string Source { get; internal set; }


        public string CombinedSource { get; private set; }
        public string CombinedDestination { get; private set; }
        public string MarkupReference { get; private set; }

        public string CombinePathsForCopy(string directoryRoot, string specific)
        {
            string[] pathArray = directoryRoot.Replace("~", "").Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            string[] specificRoot = specific.Replace("~", "").Split(new String[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            string combinedPath = String.Empty;

            foreach (var item in pathArray)
            {
                Debug.WriteLine(combinedPath);
                combinedPath += item;
                combinedPath += "/";
            }

            foreach (var item in specificRoot)
            {
                Debug.WriteLine(combinedPath);
                combinedPath += item;
                combinedPath += "/";
            }
            Debug.WriteLine(combinedPath);

            return combinedPath;
        }


    }
}
