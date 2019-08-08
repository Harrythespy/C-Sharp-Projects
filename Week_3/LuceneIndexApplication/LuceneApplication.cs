using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lucene.Net.Analysis; // for Analyser
using Lucene.Net.Documents; // for Socument
using Lucene.Net.Index; //for Index Writer
using Lucene.Net.Store; //for Directory

namespace LuceneApplication
{
    class LuceneApplication
    {
        Lucene.Net.Store.Directory luceneIndexDirectory;
        Lucene.Net.Analysis.Analyzer analyzer;
        Lucene.Net.Index.IndexWriter writer;
        Dictionary<string, int> invertedList = new Dictionary<string, int>();

        public static Lucene.Net.Util.Version VERSION = Lucene.Net.Util.Version.LUCENE_30;

        public LuceneApplication()
        {
            luceneIndexDirectory = null; // Is set in Create Index
            analyzer = null;  // Is set in CreateAnalyser
            writer = null; // Is set in CreateWriter
        }

        // Activity 7

        public void OpenIndex(string indexPath)
        {
            /* Make sure to pass a new directory that does not exist */
            if (System.IO.Directory.Exists(indexPath)) {
                Console.WriteLine("This directory already exists - Choose a directory that does not exist");
                Console.Write("Hit any key to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }

            luceneIndexDirectory = FSDirectory.Open(indexPath);

        }

        // Activity 8

        public void CreateAnalyser()
        {
            // TODO: Enter code to create the Lucene Analyser 
            
            analyzer = new SimpleAnalyzer();
            
        }

        public void CreateWriter()
        {
            
            IndexWriter.MaxFieldLength mfl = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);

            // TODO: Enter code to create the Lucene Writer 
            writer = new IndexWriter(luceneIndexDirectory, analyzer, true, mfl);

        }

        // Activity 9

        public void IndexText(string[] texts)
        {

            // TODO: Enter code to index text
            foreach (string text in texts)
            {
                Field field = new Field("Text", text, Field.Store.NO, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                Document document = new Document();
                document.Add(field);
                writer.AddDocument(document);
            }


        }


        public void CleanUp()
        {
            writer.Optimize();
            writer.Flush(true, true, true);
            writer.Dispose();
        }

        static void Main(string[] args)
        {
            string[] texts = { "The Daily Star", "The Daily Planet", "Daily News", "News of the Day", "New New York News" };


            System.Console.WriteLine("Hello Lucene.Net");
            

            LuceneApplication myLuceneApp = new LuceneApplication();
            string dir = @"H:\indexStores";
            myLuceneApp.OpenIndex(dir);
            myLuceneApp.CreateAnalyser();
            myLuceneApp.CreateWriter();
            myLuceneApp.IndexText(texts);
            myLuceneApp.CleanUp();


            
            System.Console.ReadLine();
        }
    }
}