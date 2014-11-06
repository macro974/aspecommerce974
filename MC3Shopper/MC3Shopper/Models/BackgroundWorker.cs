using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;

namespace MC3Shopper.Models
{
    public class BackgroundWorker
    {
        public bool isOK = false;
        public DateTime DateInstancied = DateTime.Now;
        public List<lignedocument> Achats { get; set; }




        public BackgroundWorker()
        {

        }

        public void Start()
        {
            //BackgroundWorker.Ticker = new System.Timers.Timer();
            //BackgroundWorker.Ticker.Elapsed += new System.Timers.ElapsedEventHandler(Ticker_Elapsed);
            //BackgroundWorker.Ticker.Interval = 900000;
            //BackgroundWorker.Ticker.Enabled = true;
            this.DateInstancied = DateTime.Now;
            Ticker_Elapsed(null, null);
        }

        void Ticker_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Database maDB = new Database();
            GestionSys maSys = new GestionSys(maDB);
            //List<entetedocument> commandeAchat = maSys.recupererEnteteDocumentByType(12);
            //List<lignedocument> ligneCommandeAchat = maSys.recupererLigneDocumentByListe(commandeAchat);
            this.Achats = maSys.recupererLigneDocumentByType(12);
        }

        public static void SerializeMe(BackgroundWorker bkgdW, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BackgroundWorker));
            TextWriter textWriter = new StreamWriter(path + "bkgdW.xml");
            serializer.Serialize(textWriter, bkgdW);
            textWriter.Close();
        }

        public static BackgroundWorker DeserializeMe(string path)
        {
            BackgroundWorker movies = new BackgroundWorker();
            if (File.Exists(path + "bkgdW.xml"))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(BackgroundWorker));
                TextReader textReader = new StreamReader(path + "bkgdW.xml");

                movies = (BackgroundWorker)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            return movies;
        }
    }
}