using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace MyWebClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MyData.info();
            string defoult_uri = "http://localhost:51538/Service1.svc";
            string uri_inUse = defoult_uri;
            do
            {
                try
                {

                    //Console.WriteLine("Podaj format (xml lub json):");
                    //string format = Console.ReadLine();
                    string opcja = "";
                    Console.WriteLine("1 - wyświetl informacje o twórcach\n 'enter' - kontynuuj");
                    opcja = Console.ReadLine();
                    if (opcja == "1")
                    {
                        Console.WriteLine("\nTwórcy: Bartosz Walkowiak i Piotr Duliński\n");
                    }

                    Console.WriteLine("\nwybierz format:\n1 - xml\n2 - json");
                    string format_type = Console.ReadLine();
                    string format = "";
                    if (format_type == "1")
                        format = "xml";
                    if (format_type == "2")
                        format = "json";
                    if (format == "")
                        throw new Exception("wrong format");

                    //Console.WriteLine("Podaj metode (GET lub POST lub DELETE lub PUT):");
                    //string method = Console.ReadLine();
                    Console.WriteLine("\nWybierz metode:\n1 - GET\n2 - POST\n3 - DELETE\n4 - PUT");
                    string method_type = Console.ReadLine();
                    string method = "";
                    if (method_type == "1")
                        method = "GET";
                    if (method_type == "2")
                        method = "POST";
                    if (method_type == "3")
                        method = "DELETE";
                    if (method_type == "4")
                        method = "PUT";
                    if (method == "")
                        throw new Exception("wrong method");
                    string get_type = "";
                    if (method == "GET")
                    {
                        Console.WriteLine("\n1 - Wyświetl wszystkie\n2 - Wyświetl jeden");
                        get_type = Console.ReadLine();
                    }

                    //Console.WriteLine("Podaj URI):");
                    //string uri = Console.ReadLine();
                    string uri = "";
                    Console.WriteLine($"\n1 - Użyj aktualnie używanego URI {uri_inUse}\n2 - zmień obecnie używaneg uri\n3 - przywróć domyślne uri i go użyj {defoult_uri}");
                    string option = Console.ReadLine();
                    if(option == "1")
                    {
                        uri = uri_inUse;
                    }
                    if(option == "2")
                    {
                        Console.WriteLine("\nPodaj nowe URI");
                        uri_inUse = Console.ReadLine();
                        uri = uri_inUse;
                    }
                    if(option == "3")
                    {
                        uri_inUse = defoult_uri;
                        uri = uri_inUse;
                    }

                    if((method == "GET" && get_type == "1") || method == "POST")
                    {
                        if(format == "json")
                        {
                            uri += "/json/cars";
                        }
                        else
                        {
                            uri += "/cars";
                        }
                    }
                    string index = "";
                    if(method == "PUT" || method == "DELETE" || (method == "GET" && get_type == "2"))
                    {
                        Console.WriteLine("\npodaj indeks samochodu który cię interesuje");
                        index = Console.ReadLine();
                        if (format == "json")
                        {
                            uri += $"/json/cars/{index}";
                        }
                        else
                        {
                            uri += $"/cars/{index}";
                        }
                    }
                    Console.WriteLine(uri);
                    HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                    req.KeepAlive = false;
                    req.Method = method;
                    if (format == "xml") req.ContentType = "text/xml";
                    else if (format == "json") req.ContentType = "application/json";
                    else {
                        Console.WriteLine("Podałeś złe dane!");
                        return;
                    }
                    switch (method.ToUpper())
                    {
                        case "GET":
                            break;
                        case "DELETE":
                            break;
                        case "PUT":
                            //new Car { Id = 1, Name = "Corsa", Brand = "Opel", Price = 1000 }
                            Console.WriteLine("\nPodaj nowe dane dla auta: ");
                            Console.WriteLine("Nazwa: ");
                            string nowa_nazwa = Console.ReadLine();
                            Console.WriteLine("Marka: ");
                            string nowa_marka = Console.ReadLine();
                            Console.WriteLine("Cena: ");
                            string nowa_cena = Console.ReadLine();
                            //<Car xmlns="http://schemas.datacontract.org/2004/07/MyWebService2"><Id>5</Id><Name>Mondeo</Name><Brand>Ford</Brand><Price>4000</Price></Car>
                            string dane = "";
                            if (format == "xml")
                                dane = "<Car xmlns=\"http://schemas.datacontract.org/2004/07/MyWebService2\"><Id>" + index + "</Id><Name>" + nowa_nazwa + "</Name><Brand>" + nowa_marka + "</Brand><Price>" + nowa_cena + "</Price></Car>";
                            else
                                dane = "{ \"Id\":index,\"Name\":\"" + nowa_nazwa + "\",\"Brand\":\"" + nowa_marka + "\",\"Price\":" + nowa_cena + "}";
                            Console.WriteLine(dane);
                            byte[] bufor = Encoding.UTF8.GetBytes(dane);
                            req.ContentLength = bufor.Length;
                            Stream postData = req.GetRequestStream();
                            postData.Write(bufor, 0, bufor.Length);
                            postData.Close();
                            break;
                        case "POST":
                            Console.WriteLine("\nPodaj nowe dane dla auta: ");
                            Console.WriteLine("index: ");
                            index = Console.ReadLine();
                            Console.WriteLine("Nazwa: ");
                            string nazwa = Console.ReadLine();
                            Console.WriteLine("Marka: ");
                            string marka = Console.ReadLine();
                            Console.WriteLine("Cena: ");
                            string cena = Console.ReadLine();
                            if (format == "xml")
                                dane = "<Car xmlns=\"http://schemas.datacontract.org/2004/07/MyWebService2\"><Id>" + index + "</Id><Name>" + nazwa + "</Name><Brand>" + marka + "</Brand><Price>" + cena + "</Price></Car>";
                            else
                                dane = "{ \"Id\":" + index + ",\"Name\":\"" + nazwa + "\",\"Brand\":\"" + marka + "\",\"Price\":" + cena + "}";
                            Console.WriteLine(dane);
                            bufor = Encoding.UTF8.GetBytes(dane);
                            req.ContentLength = bufor.Length;
                            postData = req.GetRequestStream();
                            postData.Write(bufor, 0, bufor.Length);
                            postData.Close();
                            break;
                        default:
                            break;
                    }

                    HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                    Encoding enc = System.Text.Encoding.GetEncoding(1252);
                    StreamReader responseStream = new StreamReader(resp.GetResponseStream(), enc);
                    string responseString = responseStream.ReadToEnd();
                    responseStream.Close();
                    resp.Close();

                    if (format == "json")
                    {
                        JsonDocument doc = JsonDocument.Parse(responseString);
                        JsonElement root = doc.RootElement;
                        foreach(var car in root.EnumerateArray())
                        {
                            Console.WriteLine("\nid samochodu:" + car.GetProperty("Id"));
                            Console.WriteLine("nazwa samochodu:" + car.GetProperty("Name"));
                            Console.WriteLine("marka samochodu:" + car.GetProperty("Brand"));
                            Console.WriteLine("cena samochodu:" + car.GetProperty("Price"));
                        }
                    }
                    else
                    {
                        XDocument doc = XDocument.Parse(responseString);
                        Console.WriteLine(doc);
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
                Console.WriteLine();
                Console.WriteLine("Do you want to continue?");
            } while (Console.ReadLine().ToUpper() == "Y");

        }
    }
}
