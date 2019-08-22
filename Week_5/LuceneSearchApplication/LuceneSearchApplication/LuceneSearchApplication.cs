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

namespace LuceneApplication
{
    class LuceneApplication
    {
        Lucene.Net.Store.Directory luceneIndexDirectory;
        Lucene.Net.Analysis.Analyzer analyzer;
        Lucene.Net.Index.IndexWriter writer;
        Lucene.Net.Search.IndexSearcher searcher;
        Lucene.Net.QueryParsers.QueryParser parser;

        const Lucene.Net.Util.Version VERSION = Lucene.Net.Util.Version.LUCENE_30;
        const string TEXT_FN = "Text";

        public LuceneApplication()
        {
            luceneIndexDirectory = null; 
            analyzer = null;  
            writer = null; 
        }

        /// <summary>
        /// Creates the index at indexPath
        /// </summary>
        /// <param name="indexPath">Directory path to create the index</param>
        public void CreateIndex(string indexPath)
        {
            luceneIndexDirectory = Lucene.Net.Store.FSDirectory.Open(indexPath);
            analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(VERSION);
            IndexWriter.MaxFieldLength mfl = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);
            writer = new Lucene.Net.Index.IndexWriter(luceneIndexDirectory, analyzer,true, mfl);

        }

        /// <summary>
        /// Indexes the given text
        /// </summary>
        /// <param name="text">Text to index</param>
        public void IndexText(string text)
        {
            Lucene.Net.Documents.Field field = new Field(TEXT_FN, text, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
            Lucene.Net.Documents.Document doc = new Document();
            doc.Add(field);
            writer.AddDocument(doc);
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

        public void CreateSearcher()
        {
            searcher = new IndexSearcher(luceneIndexDirectory);
        }

        public void CreateParser()
        {
            parser = new QueryParser(VERSION, TEXT_FN, analyzer);
        }

        public void CleanUpSearcher()
        {
            searcher.Dispose();
        }

        public TopDocs SearchIndex(string query)
        {
            Console.WriteLine($"Searching for {query}...");
            // construct a lucene query
            Query qo = parser.Parse(query);

            // retrieve the results using the query
            TopDocs td = searcher.Search(qo, 100);
            // return the results
            return td;
        }

        public void DisplayResults(TopDocs td)
        {
            int rank = 0;
            foreach(ScoreDoc sd in td.ScoreDocs)
            {
                rank++;
                // retrieve the document from the "ScoreDoc" object
                Document doc = searcher.Doc(sd.Doc);

                string myfieldValue = doc.Get(TEXT_FN).ToString();
                
                Console.WriteLine($"Rank {rank} text  {myfieldValue}");
            }
            
        }

        static void Main(string[] args)
        {

            LuceneApplication myLuceneApp = new LuceneApplication();

            // TODO: ADD PATHNAME
            string indexPath = @"C:\Week_5";

            myLuceneApp.CreateIndex(indexPath);

            System.Console.WriteLine("Adding Documents to Index");

            List<string> l = new List<string>();
            l.Add("The magical world of oz");
            l.Add("The mad, mad, mad, mad world");
            l.Add("Possum magic");
            l.Add("Harry is a mad guy");

            foreach (string s in l)
            {
                System.Console.WriteLine("Adding " + s + "  to Index");
                myLuceneApp.IndexText(s);
            }

            System.Console.WriteLine("All documents added.");

            myLuceneApp.CreateSearcher();

            // clean up
            myLuceneApp.CleanUpIndexer();
            myLuceneApp.CreateParser();

            
            string text = "guy";
            TopDocs td = myLuceneApp.SearchIndex(text);
            Console.WriteLine($"Number of results is {td.TotalHits}");

            myLuceneApp.DisplayResults(td);

            myLuceneApp.CleanUpSearcher();
            
            System.Console.ReadLine();
 
        
        }
    }
}