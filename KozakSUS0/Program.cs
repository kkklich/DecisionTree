using System;
using System.IO;
using System.Collections.Generic;

namespace KozakSUS0
{


    class Program
    {


        static Dictionary<string, int> dictNum = new Dictionary<string, int>();
        static List<Dictionary<string, int>> listDic = new List<Dictionary<string, int>>();

        static List<List<string>> listXY = new List<List<string>>();
        static List<double> listInfoAT = new List<double>();
        static string[,] tabXY;
        static int nrColumn = 0;
        static int maxNrRow = 0;
        static double infoTValue;

        static void Main(string[] args)
        {

            string path = "D:/Dokumenty/Studia/mgr2/Systemy uczące się/gielda.txt";
            SizeTab(path);

            ReadFile(path);

            Console.WriteLine();

            Show();
            Console.WriteLine();
            Count();
            Console.WriteLine();



            showListAT();

            Logaritm();
            Console.WriteLine();

            showListAT();

            Console.WriteLine();


            //CountInfo();
            infoT();

            Console.ReadKey();
        }

        //Wczytywanie pliku
        static void ReadFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            string line;
            tabXY = new string[nrColumn,maxNrRow];
            int counter = 0;
            

            while ((line = reader.ReadLine()) != null)
            {
                string[] tab = line.Split(',');

                for (int i = 0; i < tab.Length; i++)
                {
                    tabXY[counter, i] = tab[i];                   
                }

                counter++;         
            }
        }

        //Rozmiar tabeli
        static void SizeTab(string path)
        {
            StreamReader reader = new StreamReader(path);
            string line;       

            while ((line = reader.ReadLine()) != null)
            {                
                nrColumn++;
                string[] tab = line.Split(',');
                if (tab.Length > maxNrRow)
                    maxNrRow = tab.Length;
            }
        }

        //Wyswietlaanie
        static void Show()
        {
            for(int i=0;i<tabXY.GetLength(0);i++)
            {
                for(int j=0;j<tabXY.GetLength(1);j++)
                {
                    Console.Write(tabXY[i, j]+" ");
                }
                Console.WriteLine();
            }
        }

        //liczy ilosc wystapien danego znaku w kolumnie
        static void Count()
        {

            for(int i=0;i<tabXY.GetLength(1);i++)
            {
                for(int j=0;j<tabXY.GetLength(0);j++)
                {
                    Console.Write(tabXY[j, i]+" ");

                    if (dictNum.ContainsKey(tabXY[j,i]))
                    {
                        dictNum[tabXY[j, i]]++;
                    }
                    else
                    {
                        dictNum[tabXY[j, i]] = 1;
                    }
                }

                foreach (var itemDick in dictNum)
                {
                    Console.Write(itemDick);                 
                }

                listDic.Add(dictNum);
                dictNum = new Dictionary<string, int>();
                Console.WriteLine();
            }
        }

        //Obliczanie logarytmow
        static void Logaritm()
        {            

            InfoTList(listDic);
            

        }


        //liczenie InfoList(T)
        static void InfoTList(List<Dictionary<string, int>> listDictionary)
        {
            int nrColum = 0;

            foreach (var dic in listDictionary)
            {
                SpliInfoT(dic,nrColum);
                Console.WriteLine();
                nrColum++;
            }
        }

       
        //Obliczanie info(T)
        static void SpliInfoT(Dictionary<string,int> dic,int nrColumn)
        {
            double sumValue = 0;
            double LogSum = 0;

            foreach (var x in dic)
            {
                Console.Write(x.Key + ": " + x.Value + "  ");
                sumValue += x.Value;
            }
            Console.Write("   Suma: " + sumValue);

            foreach (var x in dic)
            {
                double fraction = x.Value / sumValue;
                LogSum += fraction * Math.Log2(fraction);
            }
            LogSum *= -1;
            Console.WriteLine(" SplitInfo(T): " + LogSum);
            infoTValue = LogSum;
            CountInfo(dic,sumValue,nrColumn);
        }



        //Logarytm o podstawie 2
        static double log2(double number)
        {
            double logNum = Math.Log2(number)*number;
            return logNum;
        }

        //Liczenie ilosci wystapien poszczegolnych znakow
        static void CountInfo(Dictionary<string, int> dictionaryInfo,double sumValue,int nrColumn)
        {
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
            double logSum = 0;
            double sumTotal = 0;

            foreach (var x in dictionaryInfo)
            {
                //Console.Write(x.Key + "  ");
                int quantitySign = 0;
                dictionary2 = new Dictionary<string, int>();

                for (int i = 0; i < tabXY.GetLength(0); i++)
                {

                    if (x.Key == tabXY[i, nrColumn])
                    {
                        //Console.Write(tabXY[i, tabXY.GetLength(1) - 1]);

                        if (dictionary2.ContainsKey(tabXY[i, tabXY.GetLength(1) - 1]))
                        {
                            dictionary2[tabXY[i, tabXY.GetLength(1) - 1]]++;
                        }
                        else
                        {
                            dictionary2[tabXY[i, tabXY.GetLength(1) - 1]] = 1;
                        }

                        quantitySign++;
                    }
                }

                logSum = 0;
                foreach (var item in dictionary2)
                {
                    double tempx = (double)item.Value / quantitySign;
                    logSum += tempx * Math.Log2(tempx);
                  //  Console.Write(" k: " + item.Key + " v:" + item.Value + "  ");

                }
                logSum *= -1 * ((double)quantitySign / sumValue);
                sumTotal += logSum;

                //Console.Write("ilosc wystapien: " + quantitySign + "  logSum: " + logSum+" logTotal: "+sumTotal);
               // Console.WriteLine(" logTotal: "+sumTotal);         

            }
            
            Console.WriteLine(" Info(x,T): " + sumTotal);
            Console.WriteLine(" Gain: " + (infoT()- sumTotal));
            Console.WriteLine(" SplitInfo: " +  infoTValue);
            Console.WriteLine(" GainRatio: " + ((infoT() - sumTotal)/infoTValue) );
            listInfoAT.Add(sumTotal);
            Console.WriteLine();
        }


        //Obliczanie osatniego INFO(T)
        static double infoT()
        {
           // Console.WriteLine("ostatnia");

            double logSum = 0;
            int quantitySign = 0;
            int length = tabXY.GetLength(0);
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();

            for (int i = 0; i < length ; i++)
            { 
                    if (dictionary2.ContainsKey(tabXY[i, tabXY.GetLength(1) - 1]))
                    {
                        dictionary2[tabXY[i, tabXY.GetLength(1) - 1]]++;
                    }
                    else
                    {
                        dictionary2[tabXY[i, tabXY.GetLength(1) - 1]] = 1;
                    }

                    quantitySign++;
            }
            
            foreach(var item in dictionary2)
            {
                double tempx = (double)item.Value / quantitySign;
                logSum += tempx * Math.Log2(tempx);            
            }
            logSum *= -1;

           // Console.WriteLine(logSum);
            return logSum;

        }


        //Wyswietlanie wszytkich info
        static void showListAT()
        {
            Console.WriteLine("Lista");
            Console.WriteLine("Info(T): "+infoTValue);

            foreach(var item in listInfoAT)
            {
                Console.WriteLine("SpliInfo(T): "+item+" Gain: "+(infoTValue-item));
                
            }
        }

    }
}
