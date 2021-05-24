using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
using System.Linq;
namespace Update
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "27.254.204.155";   // update me
            builder.UserID = "sa";              // update me
            builder.Password = "SimatSap@18";      // update me
            builder.InitialCatalog = "MMS";
           
            SqlConnectionStringBuilder builder2 = new SqlConnectionStringBuilder();
            builder2.DataSource = "27.254.204.149";   // update me
            builder2.UserID = "sa";              // update me
            builder2.Password = "SimatSap@18";      // update me
            builder2.InitialCatalog = "SLABEL";

            var itemPart = new List<data>();
            var Cus = new List<dataCus>();
            try
            {
                // Build connection string

                // Connect to SQL
                Console.WriteLine("Please Enter for Get PartNo");
                Console.WriteLine(">>>>>>> Geting PartNo ");
                using (SqlConnection con = new SqlConnection(builder.ConnectionString))
                {
                    con.Open();
                    //SqlCommand command = new SqlCommand("SELECT DISTINCT PartNo FROM Tooling_DieCut WHERE Customer_ID = ''", con);
                    SqlCommand command = new SqlCommand("SELECT DISTINCT PlatePart_No FROM Tooling_Plate WHERE Customer_ID = ''", con);

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            var x = new data();
                            var str = "";
                            str = reader["PlatePart_No"].ToString();
                            if(str.IndexOf("/") != -1)
                            {
                                Console.WriteLine("This OG / : " + str);
                                int n = str.IndexOf("/");
                                str = str.Substring(0,n);
                                Console.WriteLine("This : " + str);
                               
                            }else if (str.IndexOf("_") != -1)
                            {
                                Console.WriteLine("This OG _ : " + str);
                                int n = str.IndexOf("_");
                                str = str.Substring(0, n);
                                Console.WriteLine("This : " + str);
                                
                            }else if (str.IndexOf("(") != -1)
                            {
                                Console.WriteLine("This OG ( : " + str);
                                int n = str.IndexOf("(");
                                str = str.Substring(0, n);
                                Console.WriteLine("This : " + str);
                                
                            }
                            x.PartNo = str;
                            itemPart.Add(x);
                            //Console.WriteLine("This : "+data.PartNo);
                        }
                    }

                    Console.WriteLine("Get PartNo.................Successfully.");
                    Console.WriteLine("Please Enter for Get  Customer_ID and Customer_Name");
                    Console.ReadLine();
                    
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine(">>>>>>> Geting  Customer_ID and Customer_Name");
            foreach (var i in itemPart)
            {
                using (SqlConnection con = new SqlConnection(builder2.ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("SELECT  DISTINCT A.ItemCode,B.CardCode,B.CardName FROM OWOR AS A, " +
                        " OCRD AS B WHERE A.CardCode = B.CardCode AND A.ItemCode = '" + i.PartNo + "'", con);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Console.WriteLine("FirstColumn\tSecond Column\t");
                        while (reader.Read())
                        {
                            var y = new dataCus();
                            y.PartNo_cus = reader["ItemCode"].ToString();
                            y.Customer_ID = reader["CardCode"].ToString();
                            y.Customer_Name = reader["CardName"].ToString();
                            Cus.Add(y);
                            //Console.WriteLine(String.Format("{0} \t | {1}\t | {2}", reader[0],reader[1], reader[2]));
                        }
                       
                    }
                  
                }
            }
            Console.WriteLine("Get  Customer_ID and Customer_Name..............Successfully.");
            Console.WriteLine("Please Enter for Update");
            Console.ReadLine();
            foreach (var i in Cus)
            {
                using (SqlConnection con = new SqlConnection(builder.ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("UPDATE Tooling_Plate " +
                        " SET Customer_ID = '" + i.Customer_ID + "', Customer_Name = '" + i.Customer_Name + "' " +
                        " WHERE PlatePart_No LIKE '" + i.PartNo_cus + "%'", con);
                    command.ExecuteNonQuery();
                    // Console.WriteLine(i.PartNo_cus);
                }
            }

            Console.WriteLine("Update..............Successfully.");
            Console.ReadKey(true);
        }
    }
    
}
