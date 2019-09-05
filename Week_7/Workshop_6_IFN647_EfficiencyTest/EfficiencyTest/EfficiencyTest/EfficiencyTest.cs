using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficiencyTest
{
    class EfficiencyTest
    {
        public string[] stopWords = {"a", "an", "and", "are", "as", "at", "be", "but", "by","for", "if", "in", "into", "is", "it","no", "not", "of", "on", "or", "such","that", "the", "their", "then", "there", "these","they", "this", "to", "was", "will", "with"}; // for challange activity

        EfficiencyTest()
        {
            
        }

        /// <summary>
        /// Sends the thread to sleep and outputs the sleep time
        /// </summary>
        public void testSleepEfficiency(int millisleep){
            Console.WriteLine("Sending system to sleep...zzzz");
            DateTime start = System.DateTime.Now;
            System.Threading.Thread.Sleep(millisleep);
            DateTime end = System.DateTime.Now;
            Console.WriteLine("The sleep lasted for " + (end - start));
        }

        public void TestLinguisticProcessingEfficiency(string par)
        {
            DateTime start = System.DateTime.Now;
            string[] result = TokeniseString(par);
            string[] resultWithFilter = StopWordFilter(result);
            //System.Threading.Thread.Sleep();
            DateTime end = System.DateTime.Now;
            Console.WriteLine("time to tokenise: " + (end - start));
        }

        public string[] TokeniseString(string text)
        {
            char[] delimits = { ' ', '\n' };
            return text.ToLower().Split(delimits, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] StopWordFilter(string[] tokens)
        {
            List<string> filteredTokens = new List<string>();
            for (int i = 0; i < tokens.Count(); i++)
            {
                string token = tokens[i];
                if (!stopWords.Contains(token) && (token.Length > 2)) filteredTokens.Add(token);
            }
            return filteredTokens.ToArray<string>();
        }

        public void TestLinguisticProcessingEfficiency()
        {

        }


        static void Main(string[] args)
        {
            EfficiencyTest test = new EfficiencyTest();

            //test.testSleepEfficiency(5000);

            test.TestLinguisticProcessingEfficiency(AliceText.Text);
            Console.ReadLine();
        }



    }
}
