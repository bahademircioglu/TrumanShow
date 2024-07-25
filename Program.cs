using System;//STAThread için
using System.Collections.Generic;//list için
using System.IO;//file açma kapama
using System.Text;//text file yazma
using System.Linq;//query kullanımı için, from-in-where / foreach vs kullanımı sağlıyor


namespace algoritma
{
    
    class Program
    {

        public class Library //list obje birleşimiyle soruları ve textleri tutacağız
        {
            public int key { get; set; }
            public string sentence { get; set; }
        }

        static List<Library> _searchedList = new List<Library>();
        static List<Library> _blackList = new List<Library>();

        [STAThread]//standart single thread for winformapps, tek threadde çalıştırıyoruz, formlarda hata oluşturabiliyordu kullanılmaması.
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();//timer(stopwatch)
            System.Threading.Thread b = new System.Threading.Thread(blackListThreadStart);//stopword 
            b.SetApartmentState(System.Threading.ApartmentState.STA);
            b.Start();

            System.Threading.Thread t = new System.Threading.Thread(searchedThreadStart);//hikaye okuma
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            //Console.ReadLine();

            System.Threading.Thread q = new System.Threading.Thread(questionThreadStart);//soru texti
            q.SetApartmentState(System.Threading.ApartmentState.STA);
            q.Start();
            //Console.ReadLine();

            watch.Stop();
            Console.WriteLine("Time taken: {0}ms", watch.Elapsed.Milliseconds);
        }

        private static void blackListThreadStart()//stopwords
        {


            _blackList.Add(new Library
            {

                key = 0,
                sentence = "Who"

            });

            _blackList.Add(new Library
            {

                key = 0,
                sentence = "Where"

            });

            _blackList.Add(new Library
            {

                key = 0,
                sentence = "When"

            });

            _blackList.Add(new Library
            {

                key = 0,
                sentence = "Whose"

            });

            _blackList.Add(new Library
            {

                key = 0,
                sentence = "Why"

            });

            _blackList.Add(new Library
            {

                key = 0,
                sentence = "Which"

            });

            _blackList.Add(new Library
            {

                key = 0,
                sentence = "What"

            });

            _blackList.Add(new Library
            {

                key = 0,
                sentence = "did"

            });

        }

        private static void searchedThreadStart()
        {

            string dosya_yolu = @"C:\metin\the_truman_show_script.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);//dosyayı açıyoruz
            StreamReader sw = new StreamReader(fs, /*Encoding.GetEncoding("iso-8859-9"), türkçe karakter desteği içindi deneme textinde*/ false);
            //streamreader açtığımız text üzerinde işlem yapmamızı sağlıyor

            string row = "", _text = "";

            while ((row = sw.ReadLine()) != null)
            {
                _text += row;
            }
            char[] _char = { '.' };
            string[] _textArray = _text.Split(_char);//split stringi neye göre ayıracağımızı belirliyor. noktaya göre ayırıyoruz

            char _chr = Char.Parse(".");//nokta karşılaştırmasını char üzerinden yapacağımız için chara çeviriyoruz
            int _Id = 0;

            foreach (char i in _text)//her bir char karşılaştırması, nokta mı değil mi diye
            {
                if (i == _chr)
                    _Id++;
            }

            for (int i = 0; i < _Id; i++)
            {
                _searchedList.Add(new Library
                {
                    key = i,
                    sentence = _textArray[i].Trim()//trim whitespace character(boşluk tab vs) atlamaya yarıyor
                });
            }

            foreach (var item in _searchedList)
            {
               // Console.WriteLine("Key : " + item.key + " searched_Text : " + item.sentence);

            }
        }

        private static void questionThreadStart() //soruları okuma
        {

            List<Library> _questionList = new List<Library>();
            StringBuilder answers = new StringBuilder();//cevapları string yerine builderda tutup texte yazdırıyoruz çünkü çoklu string işlemlerinde daha hızlı.

            string dosya_yolu = @"C:\metin\questions.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs,/* Encoding.GetEncoding("iso-8859-9"), türkçe karakter desteği içindi*/ false);
            //basitçe dosya yolu üzerinden texti aç streamreader ile kullan
            string row = "", _text = "";

            while ((row = sw.ReadLine()) != null)
            {
                _text += row;
            }


            char[] _char = { '?' };
            string[] _textArray = _text.Split(_char);//split stringi neye göre ayıracağımızı belirliyor. soru işaretine göre ayırıyoruz

            char _chr = Char.Parse("?");//soru işareti karşılaştırmasını char üzerinden yapabiliyoruz
            int _Id = 0;

            foreach (char i in _text)//her bir charın soru işareti mi diye karşılaştırılması
            {
                if (i == _chr)
                    _Id++;
            }

            for (int i = 0; i < _Id; i++)
            {
                _questionList.Add(new Library
                {
                    key = i,
                    sentence = _textArray[i].Trim()//trim whitespace character(boşluk tab vs) atlamaya yarıyor
                });
            }


            List<Library> _qTextList = new List<Library>();
            foreach (var item in _questionList)
            {

               // Console.WriteLine("Key : " + item.key + " question_Text : " + item.sentence);
                string qText = item.sentence;
                string[] qParse = qText.Split(' ');//split stringi neye göre ayıracağımızı belirliyor. boşluğa göre ayırıyoruz
                foreach (var subitem in qParse)
                {
                    var searchText = subitem.Replace(",", "");

                    _qTextList.Add(new Library
                    {
                        key = item.key,
                        sentence = searchText
                    });
                   // Console.WriteLine("Key : " + item.key + " qText : " + searchText);
                }

            }

            int textCounter = 0;
            int soruCounter = _questionList.Count();

            foreach (var item in _searchedList)
            {
                for (int i = 0; i < soruCounter; i++)
                {

                    var result = _qTextList.Where(c => c.key == i).Where(p => !_blackList.Any(p2 => p2.sentence == p.sentence)).ToList();

                    foreach (var subitem in result)
                    {
                        if (subitem.sentence.Length >= 5) // Hassas arama için sayı düşürmek?
                        {
                            var searchText = subitem.sentence.Replace(",", "");//yazımı tekdüzeleştirmek, doğru okuma için
                            var p = item.sentence.Contains(searchText);
                            if (p)
                            {
                                textCounter++; //text için sayacımız, consoleda güzel ama txtde işe yaramaz

                                if (textCounter >= (result.Count() -2 )) // daha fazla sonuç için result.count daha büyük sayılara bölünebiliyor veya çıkarılabiliyor
                                {
                                    var soru = _questionList.FirstOrDefault(c => c.key == subitem.key);
                                  // Console.WriteLine("Given the input question : " + soru.sentence + "?");
                                  // Console.WriteLine("Your algorithm should output: " + item.sentence);
                                    textCounter = 0;
                                    answers.Append(item.sentence).Append("\r\n");//cevapları enterlayarak tutuyoruz
                                    break;
                                }
                            }
                        }
                    }
                   
                }
            }
            string path = @"c:\metin\answers.txt";
            string answerstext = answers.ToString();//builderı stringe çeviriyoruz
            File.WriteAllText(path, answerstext);//answers textine yazdırıyoruz.

            //   Console.ReadKey();//bir input daha bekletmek için, istersek silinebilir
        }

    }

}
