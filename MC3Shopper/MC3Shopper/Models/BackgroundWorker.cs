using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Xml.Serialization;

namespace MC3Shopper.Models
{
    public class BackgroundWorker
    {
        public DateTime DateInstancied = DateTime.Now;
        public bool isOK = false;

        public List<lignedocument> Achats { get; set; }

        public void Start()
        {
            //BackgroundWorker.Ticker = new System.Timers.Timer();
            //BackgroundWorker.Ticker.Elapsed += new System.Timers.ElapsedEventHandler(Ticker_Elapsed);
            //BackgroundWorker.Ticker.Interval = 900000;
            //BackgroundWorker.Ticker.Enabled = true;
            DateInstancied = DateTime.Now;
            Ticker_Elapsed(null, null);
        }

        private void Ticker_Elapsed(object sender, ElapsedEventArgs e)
        {
            var maDB = new Database();
            var maSys = new GestionSys(maDB);
            //List<entetedocument> commandeAchat = maSys.recupererEnteteDocumentByType(12);
            //List<lignedocument> ligneCommandeAchat = maSys.recupererLigneDocumentByListe(commandeAchat);
            Achats = maSys.recupererLigneDocumentByType(12);
        }

        public static void SerializeMe(BackgroundWorker bkgdW, string path)
        {
            var serializer = new XmlSerializer(typeof(BackgroundWorker));
            TextWriter textWriter = new StreamWriter(path + "bkgdW.xml");
            serializer.Serialize(textWriter, bkgdW);
            textWriter.Close();
        }

        public static BackgroundWorker DeserializeMe(string path)
        {
            var movies = new BackgroundWorker();
            if (File.Exists(path + "bkgdW.xml"))
            {
                var deserializer = new XmlSerializer(typeof(BackgroundWorker));
                TextReader textReader = new StreamReader(path + "bkgdW.xml");

                movies = (BackgroundWorker)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            return movies;
        }
    }
}