using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;

namespace KozakSUS0
{


    class Program
    {
        //static Dictionary<string, int> dictNum = new Dictionary<string, int>();
        //readonly static List<Dictionary<string, int>> listDic = new List<Dictionary<string, int>>();

        readonly static List<double> listInfo = new List<double>();
        readonly static List<double> listGain = new List<double>();
        readonly static List<double> listSplitInfo = new List<double>();
        static List<double> listGainRatio2 = new List<double>();
        
        static int nrColumn = 0;
        static int maxNrRow = 0;
        //static double infoXTvalue;
       // static double infoT;


        static void Main()
        {
            string[,] tabData1;

           // string path = "D:/Dokumenty/Studia/mgr2/Systemy uczące się/breast-cancer.data";
            string path = "D:/Dokumenty/Studia/mgr2/Systemy uczące się/test2.txt";
            SizeTab(path);

            tabData1 = ReadFile(path);          
            //ShowTab(tabData1);
            //Console.WriteLine();

            BudowaDrzewa(tabData1);

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

        //Wyswietlanie tablicy
        static void ShowTab(string[,] tabXY)
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

        static int licznik = 0;
        static int actualColumn = 0;

        static int positionX = 0;
        static int positionY = 0;
     

        static void BudowaDrzewa(string[,] tabData)
        {
            //  licznik++;
            
           List<double> listaGains=InfoTList(CountSign(tabData), tabData);


          //  Console.WriteLine();
          //  Console.WriteLine("Tablica: ");
           // ShowTab(tabData);
           /// Console.WriteLine();


            int nrColumnMaxRatio = MaxGainRatio(listaGains);
            actualColumn = nrColumnMaxRatio + 1;
            //Console.WriteLine("actualColumn: "+actualColumn);
            List<string> listDiffrentSign = new List<string>();
            bool ifGainZero = false;

           //// Console.WriteLine("Gain List");
           // foreach (var item in listaGains)
           // {
           //     if (double.IsNaN(item) || item <= 0)
           //     {
           //         ifGainZero = true;
           //         //Console.Write("Tak jest NAN:   ");
           //     }
           //     else
           //     {
           //         ifGainZero = false;
           //     }
           //    // Console.WriteLine("gain: " + item);
           // }

          //  Console.WriteLine("Nr columny z max gainRatio: " + nrColumnMaxRatio);

            //dzielenie tabeli na poszczegolne grupy wzgeledum kolumny dzie jest maxGainRatio
            for (int i=0;i<tabData.GetLength(0);i++)
            {                
               if(listaGains.Count!=0)
                {
                    if (!listDiffrentSign.Contains(tabData[i, nrColumnMaxRatio]))
                    {
                        listDiffrentSign.Add(tabData[i, nrColumnMaxRatio]);
                    }
                }
                  
            }

            List<string> tempList = new List<string>();
            string[,] tabTemp;
            int nrLines;
            int nrcolumns = tabData.GetLength(1)-1;

            //przez każdy różny znak w kolumnie idzie
            foreach (var item in listDiffrentSign)
            {
                // if (licznik > 0)
                //  Console.Write("      ");

                Console.SetCursorPosition(positionX, positionY);
                Console.Write(item+"  -->  " );
                positionY++;
                
              

                nrLines = 0;
                //sprawdzane jest ile razy wystepuje dany item w danej kolumnie
                for (int i = 0; i < tabData.GetLength(0); i++)
                {
                    if (tabData[i, nrColumnMaxRatio] == item)
                       nrLines++;                    
                }
               
                tabTemp = new string[nrLines,nrcolumns];

                //tworzenie nowej tablicy
                int k = 0, l = 0;
                for (int i = 0; i < tabData.GetLength(0); i++)
                {
                    if (tabData[i, nrColumnMaxRatio] == item)
                    {                        
                        for (int j = 1; j < tabData.GetLength(1); j++)
                        {
                            tabTemp[k, l] = tabData[i, j];
                            l++;
                        }
                        l = 0;
                        k++;
                    }                            
                }                

                //wys gain-ow
                List<double> tempGains = InfoTList(CountSign(tabTemp), tabTemp);

                foreach (var gain in tempGains)
                {
                    if (double.IsNaN(gain) || gain <= 0)
                    {
                        ifGainZero = true;
                    }
                    else
                    {
                        positionX+=5;
                        ifGainZero = false;
                        break;
                    }
                  //  Console.WriteLine("gain2: "+gain);
                }

                string decision = "";
              
                   if(tabTemp.GetLength(1) - 1>0)
                    decision = tabTemp[0, tabTemp.GetLength(1)-1];
                   else               
                    Console.WriteLine(" Brak podział");
                   
                if (!ifGainZero)
                {
                    //++licznik;
                    BudowaDrzewa(tabTemp);
                   /// licznik = 0;
                    positionX = 0;
                    //Console.WriteLine(decision);
                }else
                {
                    //if (licznik > 0)
                        //Console.Write("      ");
                  //  ++actualColumn;
                    Console.WriteLine("  "+ decision);
                    positionY++;
                    //--actualColumn;
                   // Console.WriteLine();
                }
            }          
                
        }

        static void Calculating(string[,] tabxy)
        {
            List<double> listag=InfoTList(CountSign(tabxy), tabxy);
            MaxGainRatio(listag);

            foreach (var item in listag)
            {
                Console.WriteLine(item);
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
                     //Console.Write(tabsXY[j, i]+" ");
                    if (tabsXY[j, i] != null)
                    {
                        if (dictNum.ContainsKey(tabsXY[j, i]))
                        {
                            dictNum[tabsXY[j, i]]++;
                        }
                        else
                        {
                            dictNum[tabsXY[j, i]] = 1;
                        }
                    }
                }

                listDic.Add(dictNum);
                dictNum = new Dictionary<string, int>();
               // Console.WriteLine();
            }
            return listDic;
        }

        

        //liczenie InfoList(T)
        static List<double> InfoTList(List<Dictionary<string, int>> listDictionary,string[,] tabXY)
        {
            int nrColum = 0;
            List<double> listGain = new List<double>();

            foreach (var dic in listDictionary)
            {
                listGain.Add(SpliInfoT(dic, nrColum, tabXY));
                //Console.WriteLine(dic);
                nrColum++;
            }


            return listGain;
        }

       
        //Obliczanie SplitInfo(T)
        static double SpliInfoT(Dictionary<string,int> dic,int nrColumn,string[,] tabXY)
        {
            int sumValue = 0;
            double logSum;

            foreach (var x in dic)
            {
              // Console.Write(x.Key + ": " + x.Value + "  ");
                sumValue += x.Value;
            }
           // Console.Write("   Suma: " + sumValue);            

            logSum= Log2Sum(dic, sumValue);
            logSum *= -1;
          //  Console.WriteLine(" SplitInfo(T): " + logSum);
            
            double gainRatioValue= GainRatioList(dic,sumValue,nrColumn,logSum,tabXY);
            return gainRatioValue;
        }

        //Liczenie ilosci wystapien poszczegolnych znakow
        static double GainRatioList(Dictionary<string, int> dictionaryInfo,double sumValue,int nrColumn,double infoXTvalue,string[,] tabXY)
        {
            Dictionary<string, int> dictionary2;
            double logSum=0;
            double sumTotal = 0;

            foreach (var x in dictionaryInfo)
            {
                int quantitySign = 0;
                dictionary2 = new Dictionary<string, int>();

                for (int i = 0; i < tabXY.GetLength(0); i++)
                {

                    if (x.Key == tabXY[i, nrColumn] & tabXY[i, tabXY.GetLength(1) - 1]!=null)
                  // if ( tabXY[i, tabXY.GetLength(1) - 1]!=null)
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
                       // Console.WriteLine("quantitySign"+quantitySign);
                    }
                   // Console.WriteLine("quantitySign" + quantitySign);
                }

                logSum=Log2Sum(dictionary2, quantitySign);

                logSum *= -1 * ((double)quantitySign / sumValue);
                sumTotal += logSum;       

            }
            
           // Console.WriteLine("X Info(x,T): " + sumTotal);
            double gain = InfoT(tabXY) - sumTotal;
           // Console.WriteLine("X Gain: " + gain);
           // Console.WriteLine("X SplitInfo: " +  infoXTvalue);
            double gainRatio= (InfoT(tabXY) - sumTotal)/ infoXTvalue;
           // Console.WriteLine("X GainRatio: " + gainRatio );
            listInfo.Add(sumTotal);
            listGain.Add(gain);
            listSplitInfo.Add(infoXTvalue);
            listGainRatio2.Add(gainRatio);
            //Console.WriteLine();


           // MaxGainRatio(listGainRatio);

            return gainRatio;
        }


        static double Log2Sum(Dictionary<string, int> dictionary2,int quantitySign)
        {
            double logSum = 0;
            foreach (var item in dictionary2)
            {
                double tempx = (double)item.Value / quantitySign;
                
               // if (double.IsNaN(Math.Log2(tempx)))
                //    logSum = 0;
                //else
                    logSum += tempx * Math.Log2(tempx);

                //logSum += tempx * Math.Log2(tempx);

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
                if (tabXY[i, tabXY.GetLength(1) - 1] != null)
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
            
            infoTv = Log2Sum(dictionary2, quantitySign)*-1;


            return infoTv;
        }



        static int MaxGainRatio(List<double> listGainR)
        {
            int maxInt = 0;
            if (listGainR.Count > 0)
            {
                double maxGainR = listGainR[0];

                for (int i = 0; i < listGainR.Count; i++)
                {
                    if (listGainR[i] > maxGainR)
                    {
                        maxGainR = listGainR[i];
                        maxInt = i;
                    }
                }
            }

           // Console.WriteLine(maxInt);
            return maxInt;
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

            foreach (var item in listGainRatio2)
            {
                Console.WriteLine("GainRatio: " + item);
            }


            Console.WriteLine();
            //Console.WriteLine("Nr maxGainRatio: "+MaxGainRatio());
        }



    }
}
