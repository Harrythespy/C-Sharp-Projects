using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis; // for Analyser
using Lucene.Net.Documents; // for Document and Field
using Lucene.Net.Index; //for Index Writer
using Lucene.Net.Store; //for Directory

namespace EfficiencyTest
{
    class TestLuceneEfficiency
    {

        Lucene.Net.Store.Directory luceneIndexDirectory;
        Lucene.Net.Analysis.Analyzer analyzer;
        Lucene.Net.Index.IndexWriter writer;


        const Lucene.Net.Util.Version VERSION = Lucene.Net.Util.Version.LUCENE_30;
        const string TEXT_FN = "Text";

        public TestLuceneEfficiency()
        {
            luceneIndexDirectory = null;
            analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(VERSION);
            writer = null;

        }

        /// <summary>
        /// Creates the index at indexPath
        /// </summary>
        /// <param name="indexPath">Directory path to create the index</param>
        public void CreateIndex(string indexPath)
        {
            luceneIndexDirectory = Lucene.Net.Store.FSDirectory.Open(indexPath);
            IndexWriter.MaxFieldLength mfl = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);
            writer = new Lucene.Net.Index.IndexWriter(luceneIndexDirectory, analyzer,true, mfl);

        }


        /// <summary>
        /// Indexes the given text
        /// </summary>
        /// <param name="text">Text to index</param>
        public void IndexText(string text)
        {
        }


        /// <summary>
        /// Flushes buffer and closes the index
        /// </summary>
        public void CleanUpIndexer()
        {
            writer.Optimize();
            writer.Flush(true, true, true);
            writer.Dispose();
        }

        static void Main(string[] args)
        {

            TestLuceneEfficiency myLuceneApp = new TestLuceneEfficiency();

            // TODO: ADD PATHNAME
            string indexPath = @"C:\";

            myLuceneApp.CreateIndex(indexPath);

            System.Console.WriteLine("Adding Documents to Index");

            int numDocs = 1000000;
            string text = "A"; 

            for (int i = 0; i < numDocs; i++ )
            {
                myLuceneApp.IndexText(text);
            }

            System.Console.WriteLine("All documents added.");

            // clean up
            myLuceneApp.CleanUpIndexer();


            System.Console.ReadLine();
        }
    }
}
