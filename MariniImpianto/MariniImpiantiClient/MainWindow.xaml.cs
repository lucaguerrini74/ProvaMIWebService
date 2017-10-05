using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MariniImpianti;
using System.Net;
using System.Xml;
using System.ComponentModel;


namespace MariniImpiantiClient
{

    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest timerHttpRequest = WebRequest.Create("http://localhost:8080/propertyvalue/IMPIANTO/description") as HttpWebRequest;

                XmlDocument timerXmlDoc = new XmlDocument();


                using (HttpWebResponse timerResp = timerHttpRequest.GetResponse() as HttpWebResponse)
                {
                    timerXmlDoc.Load(timerResp.GetResponseStream());
                    //XmlNode timerNode = timerXmlDoc.SelectSingleNode("GetPropertyValueResult");

                    //string s = timerNode.InnerText;
                    string s = timerXmlDoc.InnerText;

                    //Logger.InfoFormat("Nuova descrizione {0}", s);

                    MariniImpiantoTree.Instance.MariniImpianto.description = s;
                    //MariniImpiantoTree.InitializeFromXmlNode(node);

                    //Logger.InfoFormat("{0}", MariniImpiantoTree.Instance.SerializeObject("IMPIANTO"));
                }

                


            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);

                Console.Read();
                //return null;
            }// code goes here
        }



        public void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {

            Console.WriteLine("Sono in Main_Window.PropertyChangedEventHandler, il sender e' {0} e la proprieta' e' : {1}!!!!", (sender as MariniGenericObject).id, e.PropertyName);
            //methodToBeCalledWhenPropertyIsSet();

        }

        public MainWindow()
        {
            

            Logger.Info("***********************************");
            Logger.Info("--- CLIENT STARTED");
            Logger.Info("***********************************");

            

            try
            {
                HttpWebRequest request = WebRequest.Create("http://localhost:8080/xml/IMPIANTO") as HttpWebRequest;

                XmlDocument xmlDoc = new XmlDocument();

                
                using (HttpWebResponse resp = request.GetResponse() as HttpWebResponse)
                {
                    xmlDoc.Load(resp.GetResponseStream());
                    XmlNode node = xmlDoc.SelectSingleNode("MariniImpianto");
                    MariniImpiantoTree.InitializeFromXmlNode(node);

                    Logger.InfoFormat("{0}",MariniImpiantoTree.Instance.SerializeObject("IMPIANTO"));
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                Console.Read();
                //return null;
            }

            MariniImpianto mi = MariniImpiantoTree.Instance.MariniImpianto;
            this.DataContext = mi;



            InitializeComponent();

            tbXML.Text = MariniImpiantoTree.Instance.SerializeObject("IMPIANTO");


            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();

            

            foreach (MariniGenericObject mgo in MariniImpiantoTree.Instance.MariniImpiantoObjectsDictionary.Values)
            {
                mgo.PropertyChanged += PropertyChangedEventHandler;
            }

            

            



           
        }
    }
}
