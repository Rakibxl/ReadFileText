using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadFileText
{
    class Program
    {
     public  class  SamsungOrder

        {

            List<SamsungOrderRow> rowdata = new List<SamsungOrderRow>();

            public string OrderNumber { get; set; }

            public String OrderDate { get; set; }

            public string DistributorCode { get; set; }

            public string ExternelReference { get; set; }

 
            public List<SamsungOrderRow> GetSetOrderRows

            {

                set { rowdata = value; }

                get { return rowdata; }

            }

            public void Print()

            {

          
                Console.WriteLine(" OrderRows are:- ");

                foreach (SamsungOrderRow c in this.GetSetOrderRows)

                {
                    Console.WriteLine(" SamsungOrder's OrderNumber:- " + this.OrderNumber);
                    Console.WriteLine(" SamsungOrder's OrderDate:- " + this.OrderDate);
                    Console.WriteLine(" SamsungOrder's DistributorCode:- " + this.DistributorCode);

                    Console.WriteLine(" ArticleCode:->" + c.ArticleCode);
                    Console.WriteLine(" Price:->" + c.Price);
                    Console.WriteLine(" Quantity:->" + c.Quantity);
                    Console.WriteLine(" Amount:->" + c.Amount);
                    Console.WriteLine(" DeliveryDate:->" + c.DeliveryDate);
                    Console.WriteLine(" SamsungOrder's ExternalReference:- " + this.ExternelReference);

                    
                    Console.WriteLine(" Another Row");
                    Console.WriteLine("---------------------------------");

                    Console.WriteLine("Call SamsunOrder Add");

                    // Here we can call SamsungOrderAdd to fill order item 

                }

                    // Finally we can execute SamsungOrderJobSelect
                    // From development branch


            }

        }

        public class SamsungOrderRow

        {

            public List<SamsungOrderRow> getSetOrderRow { get; set; }

            public string ArticleCode { get; set; }
            public int Quantity { get; set; }
            public string Price { get; set; }
            public string Amount { get; set; }
            public string DeliveryDate { get; set; }

        }
        static void Main(string[] args)
        {

            string fileName = @"C:\Temp\emails\20A1035736.txt";

            var skipLine = 0;

            List<SamsungOrderRow> Lst = new List<SamsungOrderRow>();

            SamsungOrderRow orderRow = new SamsungOrderRow();

            //orderRow.ArticleCode = "F-SCBSMT36TM";
            //orderRow.Price = "69.02";
            //orderRow.Quantity = 1;
            //orderRow.Amount = "69.02";
            //orderRow.DeliveryDate = "2019.09.26".Replace(".","-");

            //Lst.Add(orderRow);



            //orderRow = new SamsungOrderRow();

            //orderRow.ArticleCode = "F-SCBSMT36TM";
            //orderRow.Price = "69.02";
            //orderRow.Quantity = 1;
            //orderRow.Amount = "69.02";
            //orderRow.DeliveryDate = "2019.09.26".Replace(".", "-");

            //Lst.Add(orderRow);

            //orderRow.getSetOrderRow = Lst;



            SamsungOrder m = new SamsungOrder();

            //m.OrderNumber = "5282196578";
            //m.OrderDate = "2019.09.25".Replace(".","-");
            //m.DistributorCode = "2061971 ESPRI";
            //m.ExternelReference = "19a1114551";


            //m.GetSetOrderRows = orderRow.getSetOrderRow;

            //m.Print();




            /// Now  try with dynamic data

            //- Open the text file
            using (StreamReader sr = new StreamReader(fileName))
            {
                                
                string line;    //- Holds the entire line

                //- Cycle thru the text file 1 line at a time pulling
                //- substrings into the variables initialized above
                while ((line = sr.ReadLine()) != null)
                {

                    //- Pulling substrings.  If there is a problem
                    //- with the start index and/or the length values
                    //- an exception is thrown
                    try
                    {


                        if (skipLine > 0)
                        {
                            skipLine -= 1;
                            continue;
                        }

                        if (line.Trim().StartsWith("Purchase"))
                        {
                            continue;
                        }

                        if (line.Trim().StartsWith("Supplier"))
                        {
                            skipLine = 3;
                            continue;
                        }


                        if (line.Trim().StartsWith("Payment"))
                        {
                            skipLine = 5;
                            continue;
                        }


                        if (line.Trim().Contains("PO No"))
                        {

                            //Regex regex = new Regex(@"\d{10}");

                            Match matchOrder = Regex.Match(line.Trim(), @"\d{10}",RegexOptions.IgnoreCase);

                            if (matchOrder.Success)
                            {
                                m.OrderNumber = matchOrder.Value;
                            }

                            Match matchOrderDate = Regex.Match(line.Trim(), @"\d{4}[.]\d{2}[.]\d{2}", RegexOptions.IgnoreCase);

                            if (matchOrderDate.Success)
                            {
                                m.OrderDate = matchOrderDate.Value.Replace(".", "-");
                            }

                            //m.OrderNumber = line.Substring(8, 10).Trim();
                            //m.OrderDate = line.Substring(26, 10).Trim().Replace(".", "-");
                        }

                        if (line.Trim().Contains("Sales Person"))
                        {
                            Match matchDistributor = Regex.Match(line.Trim(), @"\d{7}\s\w{5}", RegexOptions.IgnoreCase);

                            if (matchDistributor.Success)
                            {
                                m.DistributorCode = matchDistributor.Value;
                            }

                            //m.DistributorCode = line.Substring(15, 14).Trim();

                        }

                        if (line.Trim().StartsWith("1"))
                        {
                            orderRow.ArticleCode = line.Substring(2, 14).Trim();
                            orderRow.Quantity = Convert.ToInt32(line.Substring(16, 2).Trim());
                            orderRow.Price = line.Substring(18, 5).Trim();
                            orderRow.DeliveryDate = line.Substring(33, 10).Trim().Replace(".", "-");
                            orderRow.Amount = line.Substring(23, 2).Trim();


                            Lst.Add(orderRow);

                        }

                        if (line.Trim().StartsWith("2"))
                        {
                            orderRow = new SamsungOrderRow();

                            orderRow.ArticleCode = line.Substring(2, 14).Trim();
                            orderRow.Quantity = Convert.ToInt32(line.Substring(16, 2).Trim());
                            orderRow.Price = line.Substring(18, 5).Trim();
                            orderRow.DeliveryDate = line.Substring(33, 10).Trim().Replace(".", "-");
                            orderRow.Amount = line.Substring(23, 2).Trim();


                            Lst.Add(orderRow);

                        }

                        if (line.Trim().Contains("Remark"))
                        {
                            orderRow.getSetOrderRow = Lst;

                            m.GetSetOrderRows = orderRow.getSetOrderRow;

                            m.Print();

                        }


                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }

                }
                sr.Close();
               
            }








            Console.ReadLine();
        }
    }
}
