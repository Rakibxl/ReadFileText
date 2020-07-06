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
        public class SamsungOrder

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

            public void OrderDetails()

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


                    // Here we can call SamsungOrderAdd to fill order item 

                }

                // Finally we can execute SamsungOrderJobSelect

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

            string fileName = @"C:\Temp\emails\20A1013502.txt";

            var skipLine = 0;

            List<SamsungOrderRow> List = new List<SamsungOrderRow>();

            SamsungOrderRow orderRow = new SamsungOrderRow();


            SamsungOrder samsungOrder = new SamsungOrder();

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

                        if (line.Trim().Contains("1 / 1"))
                        {
                            continue;
                        }



                        if (line.Trim().Contains("PO No"))
                        {


                            Match matchOrder = Regex.Match(line.Trim(), @"\d{10}", RegexOptions.IgnoreCase);

                            if (matchOrder.Success)
                            {
                                samsungOrder.OrderNumber = matchOrder.Value;
                            }

                            Match matchOrderDate = Regex.Match(line.Trim(), @"\d{4}[.]\d{2}[.]\d{2}", RegexOptions.IgnoreCase);

                            if (matchOrderDate.Success)
                            {
                                samsungOrder.OrderDate = matchOrderDate.Value.Replace(".", "-");
                            }

                            samsungOrder.ExternelReference = Path.GetFileNameWithoutExtension(fileName);
                        }

                        if (line.Trim().Contains("Sales Person"))
                        {
                            Match matchDistributor = Regex.Match(line.Trim(), @"\d{7}\s\w{5}", RegexOptions.IgnoreCase);

                            if (matchDistributor.Success)
                            {
                                samsungOrder.DistributorCode = matchDistributor.Value;
                            }


                        }

                        if (line.Trim().StartsWith("1"))
                        // if (Regex.IsMatch(line.Trim(), @"^\d")) // Check if start number is a number
                        {

                            string[] OrderItem = line.Trim().Split(' ');

                            orderRow.ArticleCode = OrderItem[1];
                            orderRow.Quantity = Convert.ToInt32(OrderItem[2]);
                            orderRow.Price = OrderItem[3];
                            orderRow.Amount = OrderItem[6];
                            orderRow.DeliveryDate = OrderItem[7].Replace(".", "-");

                            List.Add(orderRow);

                        }
                        if (line.Trim().StartsWith("2"))
                        //if (Regex.IsMatch(line.Trim(), @"^\d")) // // Check if start number is a number
                        {

                            try
                            {
                                orderRow = new SamsungOrderRow();

                                string[] OrderItem = line.Trim().Split(' ');

                                orderRow.ArticleCode = OrderItem[1];
                                orderRow.Quantity = Convert.ToInt32(OrderItem[2]);
                                orderRow.Price = OrderItem[3];
                                orderRow.Amount = OrderItem[6];
                                orderRow.DeliveryDate = OrderItem[7].Replace(".", "-");


                            }
                            catch (Exception ex)
                            {

                                Console.Write(ex.ToString());

                            }

                            List.Add(orderRow);

                        }

                        if (line.Trim().Contains("Remark"))
                        {
                            orderRow.getSetOrderRow = List;

                            samsungOrder.GetSetOrderRows = orderRow.getSetOrderRow;

                            samsungOrder.OrderDetails();


                            // reinitialize if there is two order in same txt

                            List = new List<SamsungOrderRow>();

                            orderRow = new SamsungOrderRow();

                            samsungOrder = new SamsungOrder();


                        }


                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }

                }
                sr.Close();

            }

        }
    }
}
