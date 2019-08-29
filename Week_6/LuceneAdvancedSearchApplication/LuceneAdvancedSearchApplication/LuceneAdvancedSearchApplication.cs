using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis; // for Analyser
using Lucene.Net.Documents; // for Document and Field
using Lucene.Net.Index; //for Index Writer
using Lucene.Net.Store; //for Directory
using Lucene.Net.Search; // for IndexSearcher
using Lucene.Net.QueryParsers;  // for QueryParser
using Lucene.Net.Analysis.Snowball; // for snowball analyser 

namespace LuceneAdvancedSearchApplication
{
    class LuceneAdvancedSearchApplication
    {
        Lucene.Net.Store.Directory luceneIndexDirectory;
        Lucene.Net.Analysis.Analyzer analyzer;
        Lucene.Net.Index.IndexWriter writer;
        IndexSearcher searcher;
        QueryParser parser;

        DefaultSimilarity newSimilarity = new NewSimilarity();

        const Lucene.Net.Util.Version VERSION = Lucene.Net.Util.Version.LUCENE_30;
        const string TEXT_FN = "Text";

        public LuceneAdvancedSearchApplication()
        {
            luceneIndexDirectory = null;
            writer = null;
            analyzer = new SnowballAnalyzer(VERSION, "English");
            //analyzer = new Lucene.Net.Analysis.WhitespaceAnalyzer();
            //analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(VERSION);
            //analyzer = new Lucene.Net.Analysis.StopAnalyzer(VERSION);
            parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, TEXT_FN, analyzer);
            
        }
        /// <summary>
        /// Creates the index at a given path
        /// </summary>
        /// <param name="indexPath">The pathname to create the index</param>
        public void CreateIndex(string indexPath)
        {
            luceneIndexDirectory = Lucene.Net.Store.FSDirectory.Open(indexPath);
            IndexWriter.MaxFieldLength mfl = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);
            writer = new Lucene.Net.Index.IndexWriter(luceneIndexDirectory, analyzer, true, mfl);
            writer.SetSimilarity(newSimilarity);
        }

        /// <summary>
        /// Indexes a given string into the index
        /// </summary>
        /// <param name="text">The text to index</param>
        public void IndexText(string text)
        {

            Lucene.Net.Documents.Field field = new Field(TEXT_FN, text, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES);
            Lucene.Net.Documents.Document doc = new Document();
            doc.Add(field);
            writer.AddDocument(doc);
        }

        /// <summary>
        /// Flushes the buffer and closes the index
        /// </summary>
        public void CleanUpIndexer()
        {
            writer.Optimize();
            writer.Flush(true, true, true);
            writer.Dispose();
        }


        /// <summary>
        /// Creates the searcher object
        /// </summary>
        public void CreateSearcher()
        {
            searcher = new IndexSearcher(luceneIndexDirectory);
            searcher.Similarity = newSimilarity;
        }

        /// <summary>
        /// Searches the index for the querytext
        /// </summary>
        /// <param name="querytext">The text to search the index</param>
        public void SearchText(string querytext)
        {

            System.Console.WriteLine("Searching for " + querytext);
            querytext = querytext.ToLower();
            Query query = parser.Parse(querytext);

            TopDocs results = searcher.Search(query, 100);
            System.Console.WriteLine("Number of results is " + results.TotalHits);
             int rank = 0;
            foreach (ScoreDoc scoreDoc in results.ScoreDocs)
            {
                rank++;
                Lucene.Net.Documents.Document doc = searcher.Doc(scoreDoc.Doc);
                string myFieldValue = doc.Get(TEXT_FN).ToString();
                Explanation ex = searcher.Explain(query, scoreDoc.Doc);
                Console.WriteLine("Rank " + rank + " Score " + scoreDoc.Score + " text " + myFieldValue);
                Console.WriteLine(ex.ToString());
            }


        }

        /// <summary>
        /// Closes the index after searching
        /// </summary>
        public void CleanUpSearcher()
        {
            searcher.Dispose();
        }


        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello Lucene.Net");

            LuceneAdvancedSearchApplication myLuceneApp = new LuceneAdvancedSearchApplication();

            // source collection
            List<string> l = new List<string>();
            l.Add("The magical world of oz");
            l.Add("The mad, mad, mad, mad world");
            l.Add("Possum magic");
            l.Add("Mad isn't bad");
            l.Add("Mad's greatest hits");
            

            // Index code
            string indexPath = @"C:\Week_6\";
            myLuceneApp.CreateIndex(indexPath);
            System.Console.WriteLine("Adding Documents to Index");
            int docID = 0;
            foreach (string s in l)
            {

                System.Console.WriteLine("Adding doc " + docID + ". " + s + "  to Index");
                myLuceneApp.IndexText(s);
                docID++;
            }
            System.Console.WriteLine("All documents added.");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine();
            myLuceneApp.CleanUpIndexer();

            // Searching Code
            myLuceneApp.CreateSearcher();

            string[] queries = { "mad", "world", "mad^2 world", "\"mad world\"", "\"mad world\" mad world" };
            int countQuery = 0;
            foreach (string query in queries)
            {
                countQuery++;
                Console.Write($"Query {countQuery} ");
                myLuceneApp.SearchText(query);
                Console.WriteLine();
            }
            

            myLuceneApp.CleanUpSearcher();


            Console.ReadLine();
        }
    }
}
