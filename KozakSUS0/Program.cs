using System;
using System.IO;
using System.Collections.Generic;

namespace KozakSUS0
{


    class Program
    {
        //static Dictionary<string, int> dictNum = new Dictionary<string, int>();
        //readonly static List<Dictionary<string, int>> listDic = new List<Dictionary<string, int>>();

        readonly static List<double> listInfo = new List<double>();
        readonly static List<double> listGain = new List<double>();
        readonly static List<double> listSplitInfo = new List<double>();
        readonly static List<double> listGainRatio = new List<double>();
        
        static int nrColumn = 0;
        static int maxNrRow = 0;
        //static double infoXTvalue;
       // static double infoT;


        static void Main()
        {
            string[,] tabData;

            string path = "D:/Dokumenty/Studia/mgr2/Systemy uczące się/gielda.txt";
            SizeTab(path);

            tabData=ReadFile(path);
            Console.WriteLine();
            Show(tabData);
            Console.WriteLine();

            BudowaDrzewa(tabData);
           
            Console.ReadKey();
        }

        //Rozmiar tabeli przed wczytaniem
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


        //Wczytywanie pliku
        static string[,] ReadFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            string line;
           string[,] tabXY = new string[nrColumn,maxNrRow];
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
            return tabXY;
        }        

        //Wyswietlanie readFile
        static void Show(string[,] tabXY)
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


       

        static void BudowaDrzewa(string[,] tabData)
        {
            Console.WriteLine("Nr maxGainRatio: " + MaxGainRatio());
            List<string> listDiffrentSign = new List<string>();

            for(int i=0;i<tabData.GetLength(0);i++)
            {
                int nrColumn = MaxGainRatio();
               // Console.WriteLine(tabData[i,nrColumn].ToString());
                if(!listDiffrentSign.Contains(tabData[i, nrColumn]))
                {
                    listDiffrentSign.Add(tabData[i, nrColumn]);
                }
            }

            List<string> tempList = new List<string>();
            string[,] tabTemp= new string[10, 10];
            int nrLines = 0;

            foreach (var item in listDiffrentSign)
            {
                
                for (int i = 0; i < tabData.GetLength(0); i++)
                {
                    if (tabData[i, MaxGainRatio()] == item)
                    {
                        nrLines++;                        
                    }
                }
                tabTemp = new string[nrLines, tabData.GetLength(1)];

                for (int i = 0; i < tabData.GetLength(0); i++)
                {
                    if (tabData[i, MaxGainRatio()] == item)
                    {
                        
                        for (int j = 1; j < tabData.GetLength(1); j++)
                        {
                            tabTemp[i, j] = tabData[i, j];                            
                        }
                    }
                }

                if(MaxGainRatio()>0)
                    BudowaDrzewa(tabTemp);

                for(int i=0;i<tabTemp.GetLength(0);i++)
                {
                    for(int j=0;j<tabTemp.GetLength(1);j++)
                    {
                        Console.Write(tabTemp[i, j]+" ");
                    }
                   Console.WriteLine();
                }

            }
            
        }

        //liczy ilosc wystapien danego znaku w kolumnie
        static List<Dictionary<string, int>> CountSign(string[,] tabsXY)
        {
            Dictionary<string, int> dictNum = new Dictionary<string, int>();
            List<Dictionary<string, int>> listDic = new List<Dictionary<string, int>>();


            for (int i=0;i<tabsXY.GetLength(1)-1;i++)
            {
                for(int j=0;j<tabsXY.GetLength(0);j++)
                {
                   // Console.Write(tabsXY[j, i]+" ");
                  
                    if (dictNum.ContainsKey(tabsXY[j,i]))
                    {
                        dictNum[tabsXY[j, i]]++;
                    }
                    else
                    {
                        dictNum[tabsXY[j, i]] = 1;
                    }
                }

                listDic.Add(dictNum);
                dictNum = new Dictionary<string, int>();
                Console.WriteLine();
            }
            return listDic;
        }

        
        //liczenie InfoList(T)
        static void InfoTList(List<Dictionary<string, int>> listDictionary,string[,] tabXY)
        {
            int nrColum = 0;
            
            foreach (var dic in listDictionary)
            {
                SpliInfoT(dic,nrColum,tabXY);
                Console.WriteLine();
                nrColum++;
            }
        }

       
        //Obliczanie SplitInfo(T)
        static void SpliInfoT(Dictionary<string,int> dic,int nrColumn,string[,] tabXY)
        {
            int sumValue = 0;
            double logSum;

            foreach (var x in dic)
            {
                Console.Write(x.Key + ": " + x.Value + "  ");
                sumValue += x.Value;
            }
            Console.Write("   Suma: " + sumValue);

            

            logSum= Log2Sum(dic, sumValue);

            logSum *= -1;
            Console.WriteLine(" SplitInfo(T): " + logSum);
            
            CountInfo(dic,sumValue,nrColumn,logSum,tabXY);
        }       

        //Liczenie ilosci wystapien poszczegolnych znakow
        static void CountInfo(Dictionary<string, int> dictionaryInfo,double sumValue,int nrColumn,double infoXTvalue,string[,] tabXY)
        {
            Dictionary<string, int> dictionary2;
            double logSum;
            double sumTotal = 0;

            foreach (var x in dictionaryInfo)
            {
                int quantitySign = 0;
                dictionary2 = new Dictionary<string, int>();

                for (int i = 0; i < tabXY.GetLength(0); i++)
                {

                    if (x.Key == tabXY[i, nrColumn])
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
                }

                logSum=Log2Sum(dictionary2, quantitySign);

                logSum *= -1 * ((double)quantitySign / sumValue);
                sumTotal += logSum;       

            }
            
            Console.WriteLine("X Info(x,T): " + sumTotal);
            double gain = InfoT(tabXY) - sumTotal;
            Console.WriteLine("X Gain: " + gain);
            Console.WriteLine("X SplitInfo: " +  infoXTvalue);
            double gainRatio= (InfoT(tabXY) - sumTotal)/ infoXTvalue;
            Console.WriteLine("X GainRatio: " + gainRatio );
            listInfo.Add(sumTotal);
            listGain.Add(gain);
            listSplitInfo.Add(infoXTvalue);
            listGainRatio.Add(gainRatio);
            Console.WriteLine();
        }


        static double Log2Sum(Dictionary<string, int> dictionary2,int quantitySign)
        {
            double logSum = 0;
            foreach (var item in dictionary2)
            {
                double tempx = (double)item.Value / quantitySign;
                logSum += tempx * Math.Log2(tempx);
            }

            return logSum;
        }


    //Obliczanie osatniego Decyzji INFO(T)
        static double InfoT(string[,] tabXY)
        {
            double infoTv;
            int quantitySign = 0;           
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();

            int length = tabXY.GetLength(0);
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
            
            infoTv = Log2Sum(dictionary2, quantitySign)*-1;


            return infoTv;
        }


        //Wyswietlanie wszytkich informacji
        static void ShowListAT(string[,] tabXY)
        {
            Console.WriteLine("Lista");
            Console.WriteLine("Info(T): "+InfoT(tabXY));
            Console.WriteLine();

            foreach(var item in listInfo)
            {
                Console.WriteLine("SpliInfo(T): "+item);                
            }

            Console.WriteLine();

            foreach(var item in listGain)
            {
                Console.WriteLine("Gain: " + item);
            }

            Console.WriteLine();

            foreach (var item in listSplitInfo)
            {
                Console.WriteLine("SplitInfo: " + item);
            }

            Console.WriteLine();

            foreach (var item in listGainRatio)
            {
                Console.WriteLine("GainRatio: " + item);
            }


            Console.WriteLine();
            //Console.WriteLine("Nr maxGainRatio: "+MaxGainRatio());
        }


        static int MaxGainRatio()
        {
            // double maxGainRatioValue = 0;
            int maxInt = 0;
            if (listGainRatio.Count > 0)
            {
               double maxGainR= listGainRatio[0];

                for (int i = 0; i < listGainRatio.Count; i++)
                {
                    if (listGainRatio[i] > maxGainR)
                    {
                        maxGainR= listGainRatio[i];
                        maxInt = i;
                    }
                }
            }
            return maxInt;
        }

    }
}
