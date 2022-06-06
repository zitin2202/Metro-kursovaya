using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
namespace Метро2
{
    public partial class Form1 : Form
    {
        SQLiteConnection SqlConn;
        SQLiteCommand SqlCmd;
        string sqlFimeName = "database.sqlite";
        string[] doroga;
        int len_doroga;
        int timeCount = 0;
        bool aktivno = false;
        bool point1 = false;
        bool point2 = false;
        string start;
        string finish;
        Graphics g;
        Label[] masLabel = new Label[50];
        int countLabel;
        PictureBox[] marshrut = new PictureBox[2];
        Dictionary<string, Brush> ColorsLine = new Dictionary<string, Brush>() 
            {
                {"синяя",Brushes.Blue},
                {"красная",Brushes.Red},
                {"фиолетовая",Brushes.Purple},
                {"зеленая",Brushes.Green},
                {"оранжевая",Brushes.Orange},
            };
        Dictionary<string, string[]> stations = new Dictionary<string, string[]>()
        {
            {"Kupchino", new string[]{"Купчино","синяя"}},
            {"Zvyozdnaya", new string[]{"Звездная","синяя"}},
            {"Moskovskaya", new string[]{ "Московская", "синяя"}},
            {"Park_Pobedy", new string[]{ "Парк победы", "синяя"}},
            {"Elektrosila", new string[]{ "Электросила", "синяя"}},
            {"Moskovskie_Vorota", new string[]{ "Московские ворота", "синяя"}},
            {"Frunzenskaya", new string[]{ "Фрунзенская", "синяя"}},              
            {"Gorkovskaya", new string[]{ "Горьковская", "синяя"}},
            {"Petrogradskaya", new string[]{ "Петроградская", "синяя"}},
            {"Chornaya_Rechka", new string[]{ "Черная речка", "синяя"}},
            {"Pionerskaya", new string[]{ "Пионерская", "синяя"}},
            {"Udelnaya", new string[]{ "Удельная", "синяя"}},
            {"Ozerki", new string[]{ "Озерки", "синяя"}},
            {"Prospekt_Prosvescheniya", new string[]{ "Проспект просвещения", "синяя"}},
            {"Parnas", new string[]{ "Парнас","синяя"}},
            {"Prospekt_Veteranov", new string[]{ "Проспект Ветеранов","красная"}},
            {"Leninsky_Prospekt", new string[]{ "Ленинский проспект","красная"}},
            {"Avtovo", new string[]{ "Автово", "красная"}},
            {"Kirovsky_Zavod", new string[]{ "Кировский завод", "красная"}},
            {"Narvskaya", new string[]{ "Нарвская", "красная"}},
            {"Baltiyskaya", new string[]{ "Балтийская", "красная"}},
            {"Chernyshevskaya", new string[]{ "Чернышевская", "красная"}},
            {"Ploshchad_Lenina", new string[]{ "Площадь Ленин", "красная"}},
            {"Vyborgskaya", new string[]{ "Выборская", "красная"}},
            {"Lesnaya", new string[]{ "Лесная", "красная"}},
            {"Ploshchad_Muzhestva", new string[]{ "Площадь Мужества", "красная"}},
            {"Politekhnicheskaya", new string[]{ "Политихническая", "красная"}},
            {"Akademicheskaya", new string[]{ "Академическая", "красная"}},
            {"Grazhdansky_Prospekt", new string[]{ "Гражднаский проспект", "красная"}},
            {"Devyatkino", new string[]{ "Девяткино","красная"}},
            {"Shushary", new string[]{ "Шушары","фиолетовая"}},
            {"Dunayskaya", new string[]{ "Дунаская","фиолетовая"}},
            {"Prospekt_Slavy", new string[]{ "Проспект славы", "фиолетовая"}},
            {"Mezhdunarodnaya", new string[]{ "Международная", "фиолетовая"}},
            {"Bukharestskaya", new string[]{ "Бухаретская", "фиолетовая"}},
            {"Volkovskaya", new string[]{ "Волковская", "фиолетовая"}},
            {"Obvodny_Kanal", new string[]{ "Обводный канал", "фиолетовая"}},
            {"Admiralteyskaya", new string[]{ "Адмииралтейская", "фиолетовая"}},
            {"Sportivnaya", new string[]{ "Спортивная", "фиолетовая"}},
            {"Chkalovskaya", new string[]{ "Чкаловская", "фиолетовая"}},
            {"Krestovsky_Ostrov", new string[]{ "Крестовский остров", "фиолетовая"}},
            {"Staraya_Derevnya", new string[]{ "Старая деревня", "фиолетовая"}},
            {"Komendantsky_Prospekt", new string[]{ "Комендантский проспект", "фиолетовая"}},
            {"Rybatskoe", new string[]{ "Рыбацкое","зеленая"}},
            {"Obukhovo", new string[]{ "Обухово", "зеленая"}},
            {"Proletarskaya", new string[]{ "Пролетарская", "зеленая"}},
            {"Lomonosovskaya", new string[]{ "Ломоносовская", "зеленая"}},
            {"Yelizarovskaya", new string[]{ "Елизаровская", "зеленая"}},
            {"Vasileostrovskaya", new string[]{ "Василиостровская", "зеленая"}},
            {"Primorskaya", new string[]{ "Приморская", "зеленая"}},
            {"Zenit", new string[]{ "Зенит", "зеленая"}},
            {"Begovaya", new string[]{"Беговая","зеленая"}},
            {"Ulitsa_Dybenko", new string[]{ "Улица Дыбенко","оранжевая"}},
            {"Prospekt_Bolshevikov", new string[]{ "Проспект Большевиков", "оранжевая"}},
            {"Ladozhskaya", new string[]{ "Ладожская", "оранжевая"}},
            {"Novocherkasskaya", new string[]{ "Новочеркасская", "оранжевая"}},
            {"Ligovsky_Prospekt", new string[]{ "Лиговский проспект", "оранжевая"}},

            {"Nevsky_ProspektII", new string[]{ "Невский проспект", "синяя"}},
            {"Gostiny_DvorII", new string[]{ "Гостиный двор", "зеленая"}},
            {"Ploschad_VosstaniaII", new string[]{ "Площадь Восстания", "красная"}},
            {"MayakovskayaII", new string[]{ "Маяковская", "зеленая" }},
            {"Ploshchad_Alexandra_Nevskogo1II", new string[]{ "Площадь Александра Невского 1", "зеленая"}},
            {"Ploshchad_Alexandra_Nevskogo2II", new string[]{ "Площадь Александра Невского 2", "оранжевая"}},
            {"VladimirskayaII", new string[]{ "Владимирская", "красная" }},
            {"DostoyevskayaII", new string[]{ "Достоевская", "оранжевая" }},
            {"PushkinskayaII", new string[]{ "Пушкинская", "красная" }},
            {"ZvenigorodskayaII", new string[]{ "Звенигородская", "фиолетовая" }},
            {"Tekhnologichesky_Institut1II", new string[]{ "Технологически институт 1", "красная" }},
            {"Tekhnologichesky_Institut2II", new string[]{ "Технологически институт 2", "синяя"}},
            {"Sennaya_PloshchadII", new string[]{ "Сенная площадь", "синяя"}},
            {"SadovayaII", new string[]{ "Садовая", "фиолетовая" }},
            {"SpasskayaII", new string[]{ "Спасская", "оранжевая",}},


        };
        Dictionary<string, int> TimeWay = new Dictionary<string, int>()
        {
            {"Kupchino__Zvyozdnaya",3},
            {"Zvyozdnaya__Moskovskaya",4},
            {"Moskovskaya__Park_Pobedy",3},
            {"Park_Pobedy__Elektrosila",2},
            {"Elektrosila__Moskovskie_Vorota",2},
            {"Moskovskie_Vorota__Frunzenskaya",2},
            {"Frunzenskaya__Tekhnologichesky_Institut2II",2},
            {"Nevsky_ProspektII__Gorkovskaya",4},
            {"Gorkovskaya__Petrogradskaya",2},
            {"Petrogradskaya__Chornaya_Rechka",4},
            {"Chornaya_Rechka__Pionerskaya",3},
            {"Pionerskaya__Udelnaya",3},
            {"Udelnaya__Ozerki",3},
            {"Ozerki__Prospekt_Prosvescheniya",2},
            {"Prospekt_Prosvescheniya__Parnas",3},
            {"Prospekt_Veteranov__Leninsky_Prospekt",2},
            {"Leninsky_Prospekt__Avtovo",3},
            {"Avtovo__Kirovsky_Zavod",2},
            {"Kirovsky_Zavod__Narvskaya",4},
            {"Narvskaya__Baltiyskaya",3},
            {"Baltiyskaya__Tekhnologichesky_Institut1II",3},
            {"Ploschad_VosstaniaII__Chernyshevskaya",2},
            {"Chernyshevskaya__Ploshchad_Lenina",3},
            {"Ploshchad_Lenina__Vyborgskaya",2},
            {"Vyborgskaya__Lesnaya",3},
            {"Lesnaya__Ploshchad_Muzhestva",3},
            {"Ploshchad_Muzhestva__Politekhnicheskaya",3},
            {"Politekhnicheskaya__Akademicheskaya",2},
            {"Akademicheskaya__Grazhdansky_Prospekt",6},
            {"Grazhdansky_Prospekt__Devyatkino",3},
            {"Shushary__Dunayskaya",4},
            {"Dunayskaya__Prospekt_Slavy",3},
            {"Prospekt_Slavy__Mezhdunarodnaya",2},
            {"Mezhdunarodnaya__Bukharestskaya",3},
            {"Bukharestskaya__Volkovskaya",3},
            {"Volkovskaya__Obvodny_Kanal",3},
            {"Obvodny_Kanal__ZvenigorodskayaII",3},
            {"SadovayaII__Admiralteyskaya",4},
            {"Admiralteyskaya__Sportivnaya01",3},
            {"Sportivnaya__Chkalovskaya",2},
            {"Chkalovskaya__Krestovsky_Ostrov",4},
            {"Krestovsky_Ostrov__Staraya_Derevnya",3},
            {"Staraya_Derevnya__Komendantsky_Prospekt",3},
            {"Rybatskoe__Obukhovo",4},
            {"Obukhovo__Proletarskaya",3},
            {"Proletarskaya__Lomonosovskaya",3},
            {"Lomonosovskaya__Yelizarovskaya",3},
            {"Yelizarovskaya__Ploshchad_Alexandra_Nevskogo1II",5},
            {"Gostiny_DvorII__Vasileostrovskaya",5},
            {"Vasileostrovskaya__Primorskaya",4},
            {"Primorskaya_Zenit",4},
            {"Zenit__Begovaya",4},
            {"Ulitsa_Dybenko__Prospekt_Bolshevikov",2},
            {"Prospekt_Bolshevikov__Ladozhskaya",3},
            {"Ladozhskaya__Novocherkasskaya",3},
            {"Novocherkasskaya__Ploshchad_Alexandra_Nevskogo2II",4},
            {"Ploshchad_Alexandra_Nevskogo2II__Ligovsky_Prospekt",3},
            {"Ligovsky_Prospekt__DostoyevskayaII",4},
            {"!Nevsky_ProspektII__Gostiny_DvorII",2},
            {"Sennaya_PloshchadII__Nevsky_ProspektII",2},
            {"MayakovskayaII__Gostiny_DvorII",3},
            {"!Ploschad_VosstaniaII__MayakovskayaII",2},
            {"VladimirskayaII__Ploschad_VosstaniaII",4},
            {"Ploshchad_Alexandra_Nevskogo1II__MayakovskayaII",3},
            {"!Ploshchad_Alexandra_Nevskogo1II__Ploshchad_Alexandra_Nevskogo2II",2},
            {"!VladimirskayaII__DostoyevskayaII",2},
            {"PushkinskayaII__VladimirskayaII",2},
            {"SpasskayaII__DostoyevskayaII",3},
            {"!PushkinskayaII__ZvenigorodskayaII",2},
            {"Tekhnologichesky_Institut1II__PushkinskayaII",5},
            {"SadovayaII__ZvenigorodskayaII",2},
            {"!Tekhnologichesky_Institut1II__Tekhnologichesky_Institut2II",1},
            {"Tekhnologichesky_Institut2II__Sennaya_PloshchadII",3},
            {"!Sennaya_PloshchadII__SadovayaII",3},
            {"!Sennaya_PloshchadII__SpasskayaII",3},
            {"!SadovayaII__SpasskayaII",3},
        };
        Dictionary<string, string[]> sosedi = new Dictionary<string, string[]>()
        {
            {"Kupchino", new string[]{"Zvyozdnaya"}},
            {"Zvyozdnaya", new string[]{ "Kupchino","Moskovskaya"}},
            {"Moskovskaya", new string[]{ "Zvyozdnaya", "Park_Pobedy"}},
            {"Park_Pobedy", new string[]{ "Moskovskaya", "Elektrosila"}},
            {"Elektrosila", new string[]{ "Park_Pobedy", "Moskovskie_Vorota"}},
            {"Moskovskie_Vorota", new string[]{ "Elektrosila", "Frunzenskaya"}},
            {"Frunzenskaya", new string[]{ "Moskovskie_Vorota", "Tekhnologichesky_Institut2II"}},
            {"Gorkovskaya", new string[]{ "Nevsky_ProspektII", "Petrogradskaya"}},
            {"Petrogradskaya", new string[]{ "Gorkovskaya", "Chornaya_Rechka"}},
            {"Chornaya_Rechka", new string[]{ "Petrogradskaya", "Pionerskaya"}},
            {"Pionerskaya", new string[]{ "Chornaya_Rechka", "Udelnaya"}},
            {"Udelnaya", new string[]{ "Pionerskaya", "Ozerki"}},
            {"Ozerki", new string[]{ "Udelnaya", "Prospekt_Prosvescheniya"}},
            {"Prospekt_Prosvescheniya", new string[]{ "Ozerki", "Parnas"}},
            {"Parnas", new string[]{ "Prospekt_Prosvescheniya"}},
            {"Prospekt_Veteranov", new string[]{ "Leninsky_Prospekt"}},
            {"Leninsky_Prospekt", new string[]{ "Prospekt_Veteranov","Avtovo"}},
            {"Avtovo", new string[]{ "Leninsky_Prospekt", "Kirovsky_Zavod"}},
            {"Kirovsky_Zavod", new string[]{ "Avtovo", "Narvskaya"}},
            {"Narvskaya", new string[]{ "Kirovsky_Zavod", "Baltiyskaya"}},
            {"Baltiyskaya", new string[]{ "Narvskaya", "Tekhnologichesky_Institut1II"}},
            {"Chernyshevskaya", new string[]{ "Ploschad_VosstaniaII", "Ploshchad_Lenina"}},
            {"Ploshchad_Lenina", new string[]{ "Chernyshevskaya", "Vyborgskaya"}},
            {"Vyborgskaya", new string[]{ "Ploshchad_Lenina", "Lesnaya"}},
            {"Lesnaya", new string[]{ "Vyborgskaya", "Ploshchad_Muzhestva"}},
            {"Ploshchad_Muzhestva", new string[]{ "Lesnaya", "Politekhnicheskaya"}},
            {"Politekhnicheskaya", new string[]{ "Ploshchad_Muzhestva", "Akademicheskaya"}},
            {"Akademicheskaya", new string[]{ "Politekhnicheskaya", "Grazhdansky_Prospekt"}},
            {"Grazhdansky_Prospekt", new string[]{ "Akademicheskaya", "Devyatkino"}},
            {"Devyatkino", new string[]{ "Grazhdansky_Prospekt"}},
            {"Shushary", new string[]{ "Dunayskaya"}},
            {"Dunayskaya", new string[]{ "Shushary","Prospekt_Slavy"}},
            {"Prospekt_Slavy", new string[]{ "Dunayskaya", "Mezhdunarodnaya"}},
            {"Mezhdunarodnaya", new string[]{ "Prospekt_Slavy", "Bukharestskaya"}},
            {"Bukharestskaya", new string[]{ "Mezhdunarodnaya", "Volkovskaya"}},
            {"Volkovskaya", new string[]{ "Bukharestskaya", "Obvodny_Kanal"}},
            {"Obvodny_Kanal", new string[]{ "Volkovskaya", "ZvenigorodskayaII"}},
            {"Admiralteyskaya", new string[]{ "SadovayaII", "Sportivnaya"}},
            {"Sportivnaya", new string[]{ "Admiralteyskaya", "Chkalovskaya"}},
            {"Chkalovskaya", new string[]{ "Sportivnaya", "Krestovsky_Ostrov"}},
            {"Krestovsky_Ostrov", new string[]{ "Chkalovskaya", "Staraya_Derevnya"}},
            {"Staraya_Derevnya", new string[]{ "Krestovsky_Ostrov", "Komendantsky_Prospekt"}},
            {"Komendantsky_Prospekt", new string[]{ "Staraya_Derevnya"}},
            {"Rybatskoe", new string[]{ "Obukhovo"}},
            {"Obukhovo", new string[]{ "Rybatskoe", "Proletarskaya"}},
            {"Proletarskaya", new string[]{ "Obukhovo", "Lomonosovskaya"}},
            {"Lomonosovskaya", new string[]{ "Proletarskaya", "Yelizarovskaya"}},
            {"Yelizarovskaya", new string[]{ "Lomonosovskaya", "Ploshchad_Alexandra_Nevskogo1II"}},
            {"Vasileostrovskaya", new string[]{ "Gostiny_DvorII", "Primorskaya"}},
            {"Primorskaya", new string[]{ "Vasileostrovskaya", "Zenit"}},
            {"Zenit", new string[]{ "Primorskaya", "Begovaya"}},
            {"Begovaya", new string[]{"Zenit"}},
            {"Ulitsa_Dybenko", new string[]{ "Prospekt_Bolshevikov"}},
            {"Prospekt_Bolshevikov", new string[]{ "Ulitsa_Dybenko", "Ladozhskaya"}},
            {"Ladozhskaya", new string[]{ "Prospekt_Bolshevikov", "Novocherkasskaya"}},
            {"Novocherkasskaya", new string[]{ "Ladozhskaya", "Ploshchad_Alexandra_Nevskogo2II"}},
            {"Ligovsky_Prospekt", new string[]{ "Ploshchad_Alexandra_Nevskogo2II", "DostoyevskayaII"}},

            {"Nevsky_ProspektII", new string[]{ "Gostiny_DvorII", "Gorkovskaya", "Sennaya_PloshchadII"}},
            {"Gostiny_DvorII", new string[]{ "Nevsky_ProspektII", "MayakovskayaII", "Vasileostrovskaya"}},
            {"Ploschad_VosstaniaII", new string[]{ "MayakovskayaII", "Chernyshevskaya", "VladimirskayaII"}},
            {"MayakovskayaII", new string[]{ "Ploschad_VosstaniaII", "Ploshchad_Alexandra_Nevskogo1II", "Gostiny_DvorII", }},
            {"Ploshchad_Alexandra_Nevskogo1II", new string[]{ "Ploshchad_Alexandra_Nevskogo2II", "Yelizarovskaya", "MayakovskayaII", }},
            {"Ploshchad_Alexandra_Nevskogo2II", new string[]{ "Ploshchad_Alexandra_Nevskogo1II", "Novocherkasskaya", "Ligovsky_Prospekt", }},
            {"VladimirskayaII", new string[]{ "DostoyevskayaII", "PushkinskayaII", "Ploschad_VosstaniaII", }},
            {"DostoyevskayaII", new string[]{ "VladimirskayaII", "Ligovsky_Prospekt", "SpasskayaII", }},
            {"PushkinskayaII", new string[]{ "ZvenigorodskayaII", "Tekhnologichesky_Institut1II", "VladimirskayaII", }},
            {"ZvenigorodskayaII", new string[]{ "PushkinskayaII", "Obvodny_Kanal", "SadovayaII", }},
            {"Tekhnologichesky_Institut1II", new string[]{ "Tekhnologichesky_Institut2II", "Baltiyskaya", "PushkinskayaII", }},
            {"Tekhnologichesky_Institut2II", new string[]{ "Tekhnologichesky_Institut1II", "Frunzenskaya", "Sennaya_PloshchadII", }},
            {"Sennaya_PloshchadII", new string[]{ "SadovayaII", "SpasskayaII", "Tekhnologichesky_Institut2II", "Nevsky_ProspektII", }},
            {"SadovayaII", new string[]{ "Sennaya_PloshchadII", "SpasskayaII", "ZvenigorodskayaII", "Admiralteyskaya", }},
            {"SpasskayaII", new string[]{ "Sennaya_PloshchadII", "SadovayaII", "DostoyevskayaII",}},


        };
        Dictionary<string, string[]> puti = new Dictionary<string, string[]>()
        {
            {"Kupchino", new string[]{"Kupchino__Zvyozdnaya"}},
            {"Zvyozdnaya", new string[]{ "Kupchino__Zvyozdnaya", "Zvyozdnaya__Moskovskaya"}},
            {"Moskovskaya", new string[]{ "Zvyozdnaya__Moskovskaya", "Moskovskaya__Park_Pobedy"}},
            {"Park_Pobedy", new string[]{ "Moskovskaya__Park_Pobedy", "Park_Pobedy__Elektrosila"}},
            {"Elektrosila", new string[]{ "Park_Pobedy__Elektrosila", "Elektrosila__Moskovskie_Vorota"}},
            {"Moskovskie_Vorota", new string[]{ "Elektrosila__Moskovskie_Vorota", "Moskovskie_Vorota__Frunzenskaya"}},
            {"Frunzenskaya", new string[]{ "Moskovskie_Vorota__Frunzenskaya", "Frunzenskaya__Tekhnologichesky_Institut2II"}},
            {"Gorkovskaya", new string[]{ "Nevsky_ProspektII__Gorkovskaya", "Gorkovskaya__Petrogradskaya"}},
            {"Petrogradskaya", new string[]{ "Gorkovskaya__Petrogradskaya", "Petrogradskaya__Chornaya_Rechka"}},
            {"Chornaya_Rechka", new string[]{ "Petrogradskaya__Chornaya_Rechka", "Chornaya_Rechka__Pionerskaya"}},
            {"Pionerskaya", new string[]{ "Chornaya_Rechka__Pionerskaya", "Pionerskaya__Udelnaya"}},
            {"Udelnaya", new string[]{ "Pionerskaya__Udelnaya", "Udelnaya__Ozerki"}},
            {"Ozerki", new string[]{ "Udelnaya__Ozerki", "Ozerki__Prospekt_Prosvescheniya"}},
            {"Prospekt_Prosvescheniya", new string[]{ "Ozerki__Prospekt_Prosvescheniya", "Prospekt_Prosvescheniya__Parnas"}},
            {"Parnas", new string[]{ "Prospekt_Prosvescheniya__Parnas"}},
            {"Prospekt_Veteranov", new string[]{ "Prospekt_Veteranov__Leninsky_Prospekt"}},
            {"Leninsky_Prospekt", new string[]{ "Prospekt_Veteranov__Leninsky_Prospekt", "Leninsky_Prospekt__Avtovo"}},
            {"Avtovo", new string[]{ "Leninsky_Prospekt__Avtovo", "Avtovo__Kirovsky_Zavod"}},
            {"Kirovsky_Zavod", new string[]{ "Avtovo__Kirovsky_Zavod", "Kirovsky_Zavod__Narvskaya"}},
            {"Narvskaya", new string[]{ "Kirovsky_Zavod__Narvskaya", "Narvskaya__Baltiyskaya"}},
            {"Baltiyskaya", new string[]{ "Narvskaya__Baltiyskaya", "Baltiyskaya__Tekhnologichesky_Institut1II"}},
            {"Chernyshevskaya", new string[]{ "Ploschad_VosstaniaII__Chernyshevskaya", "Chernyshevskaya__Ploshchad_Lenina"}},
            {"Ploshchad_Lenina", new string[]{ "Chernyshevskaya__Ploshchad_Lenina", "Ploshchad_Lenina__Vyborgskaya"}},
            {"Vyborgskaya", new string[]{ "Ploshchad_Lenina__Vyborgskaya", "Vyborgskaya__Lesnaya"}},
            {"Lesnaya", new string[]{ "Vyborgskaya__Lesnaya", "Lesnaya__Ploshchad_Muzhestva"}},
            {"Ploshchad_Muzhestva", new string[]{ "Lesnaya__Ploshchad_Muzhestva", "Ploshchad_Muzhestva__Politekhnicheskaya"}},
            {"Politekhnicheskaya", new string[]{ "Ploshchad_Muzhestva__Politekhnicheskaya", "Politekhnicheskaya__Akademicheskaya"}},
            {"Akademicheskaya", new string[]{ "Politekhnicheskaya__Akademicheskaya", "Akademicheskaya__Grazhdansky_Prospekt"}},
            {"Grazhdansky_Prospekt", new string[]{ "Akademicheskaya__Grazhdansky_Prospekt", "Grazhdansky_Prospekt__Devyatkino"}},
            {"Devyatkino", new string[]{ "Grazhdansky_Prospekt__Devyatkino"}},
            {"Shushary", new string[]{ "Shushary__Dunayskaya"}},
            {"Dunayskaya", new string[]{ "Shushary__Dunayskaya", "Dunayskaya__Prospekt_Slavy"}},
            {"Prospekt_Slavy", new string[]{ "Dunayskaya__Prospekt_Slavy", "Prospekt_Slavy__Mezhdunarodnaya"}},
            {"Mezhdunarodnaya", new string[]{ "Prospekt_Slavy__Mezhdunarodnaya", "Mezhdunarodnaya__Bukharestskaya"}},
            {"Bukharestskaya", new string[]{ "Mezhdunarodnaya__Bukharestskaya", "Bukharestskaya__Volkovskaya"}},
            {"Volkovskaya", new string[]{ "Bukharestskaya__Volkovskaya", "Volkovskaya__Obvodny_Kanal"}},
            {"Obvodny_Kanal", new string[]{ "Volkovskaya__Obvodny_Kanal", "Obvodny_Kanal__ZvenigorodskayaII"}},
            {"Admiralteyskaya", new string[]{ "SadovayaII__Admiralteyskaya", "Admiralteyskaya__Sportivnaya01"}},
            {"Sportivnaya", new string[]{ "Admiralteyskaya__Sportivnaya01","Sportivnaya__Chkalovskaya"}},
            {"Chkalovskaya", new string[]{ "Sportivnaya__Chkalovskaya", "Chkalovskaya__Krestovsky_Ostrov"}},
            {"Krestovsky_Ostrov", new string[]{ "Chkalovskaya__Krestovsky_Ostrov", "Krestovsky_Ostrov__Staraya_Derevnya"}},
            {"Staraya_Derevnya", new string[]{ "Krestovsky_Ostrov__Staraya_Derevnya", "Staraya_Derevnya__Komendantsky_Prospekt"}},
            {"Komendantsky_Prospekt", new string[]{ "Staraya_Derevnya__Komendantsky_Prospekt"}},
            {"Rybatskoe", new string[]{ "Rybatskoe__Obukhovo"}},
            {"Obukhovo", new string[]{ "Rybatskoe__Obukhovo","Obukhovo__Proletarskaya"}},
            {"Proletarskaya", new string[]{ "Obukhovo__Proletarskaya", "Proletarskaya__Lomonosovskaya"}},
            {"Lomonosovskaya", new string[]{ "Proletarskaya__Lomonosovskaya", "Lomonosovskaya__Yelizarovskaya"}},
            {"Yelizarovskaya", new string[]{ "Lomonosovskaya__Yelizarovskaya", "Yelizarovskaya__Ploshchad_Alexandra_Nevskogo1II"}},
            {"Vasileostrovskaya", new string[]{ "Gostiny_DvorII__Vasileostrovskaya", "Vasileostrovskaya__Primorskaya"}},
            {"Primorskaya", new string[]{ "Vasileostrovskaya__Primorskaya", "Primorskaya_Zenit"}},
            {"Zenit", new string[]{ "Primorskaya_Zenit", "Zenit__Begovaya"}},
            {"Begovaya", new string[]{"Zenit__Begovaya"}},
            {"Ulitsa_Dybenko", new string[]{ "Ulitsa_Dybenko__Prospekt_Bolshevikov"}},
            {"Prospekt_Bolshevikov", new string[]{ "Ulitsa_Dybenko__Prospekt_Bolshevikov", "Prospekt_Bolshevikov__Ladozhskaya"}},
            {"Ladozhskaya", new string[]{ "Prospekt_Bolshevikov__Ladozhskaya", "Ladozhskaya__Novocherkasskaya"}},
            {"Novocherkasskaya", new string[]{ "Ladozhskaya__Novocherkasskaya", "Novocherkasskaya__Ploshchad_Alexandra_Nevskogo2II"}},
            {"Ligovsky_Prospekt", new string[]{ "Ploshchad_Alexandra_Nevskogo2II__Ligovsky_Prospekt", "Ligovsky_Prospekt__DostoyevskayaII"}},


            {"Nevsky_ProspektII", new string[]{ "!Nevsky_ProspektII__Gostiny_DvorII", "Nevsky_ProspektII__Gorkovskaya", "Sennaya_PloshchadII__Nevsky_ProspektII"}},
            {"Gostiny_DvorII", new string[]{ "!Nevsky_ProspektII__Gostiny_DvorII", "MayakovskayaII__Gostiny_DvorII", "Gostiny_DvorII__Vasileostrovskaya"}},
            {"Ploschad_VosstaniaII", new string[]{ "!Ploschad_VosstaniaII__MayakovskayaII", "Ploschad_VosstaniaII__Chernyshevskaya", "VladimirskayaII__Ploschad_VosstaniaII"}},
            {"MayakovskayaII", new string[]{ "!Ploschad_VosstaniaII__MayakovskayaII", "Ploshchad_Alexandra_Nevskogo1II__MayakovskayaII", "MayakovskayaII__Gostiny_DvorII", }},
            {"Ploshchad_Alexandra_Nevskogo1II", new string[]{ "!Ploshchad_Alexandra_Nevskogo1II__Ploshchad_Alexandra_Nevskogo2II", "Yelizarovskaya__Ploshchad_Alexandra_Nevskogo1II", "Ploshchad_Alexandra_Nevskogo1II__MayakovskayaII", }},
            {"Ploshchad_Alexandra_Nevskogo2II", new string[]{ "!Ploshchad_Alexandra_Nevskogo1II__Ploshchad_Alexandra_Nevskogo2II", "Novocherkasskaya__Ploshchad_Alexandra_Nevskogo2II", "Ploshchad_Alexandra_Nevskogo2II__Ligovsky_Prospekt", }},
            {"VladimirskayaII", new string[]{ "!VladimirskayaII__DostoyevskayaII", "PushkinskayaII__VladimirskayaII", "VladimirskayaII__Ploschad_VosstaniaII", }},
            {"DostoyevskayaII", new string[]{ "!VladimirskayaII__DostoyevskayaII", "Ligovsky_Prospekt__DostoyevskayaII", "SpasskayaII__DostoyevskayaII", }},
            {"PushkinskayaII", new string[]{ "!PushkinskayaII__ZvenigorodskayaII", "Tekhnologichesky_Institut1II__PushkinskayaII", "PushkinskayaII__VladimirskayaII", }},
            {"ZvenigorodskayaII", new string[]{ "!PushkinskayaII__ZvenigorodskayaII", "Obvodny_Kanal__ZvenigorodskayaII", "SadovayaII__ZvenigorodskayaII", }},
            {"Tekhnologichesky_Institut1II", new string[]{ "!Tekhnologichesky_Institut1II__Tekhnologichesky_Institut2II", "Baltiyskaya__Tekhnologichesky_Institut1II", "Tekhnologichesky_Institut1II__PushkinskayaII", }},
            {"Tekhnologichesky_Institut2II", new string[]{ "!Tekhnologichesky_Institut1II__Tekhnologichesky_Institut2II", "Frunzenskaya__Tekhnologichesky_Institut2II", "Tekhnologichesky_Institut2II__Sennaya_PloshchadII", }},
            {"Sennaya_PloshchadII", new string[]{ "!Sennaya_PloshchadII__SadovayaII", "!Sennaya_PloshchadII__SpasskayaII", "Tekhnologichesky_Institut2II__Sennaya_PloshchadII", "Sennaya_PloshchadII__Nevsky_ProspektII", }},
            {"SadovayaII", new string[]{ "!Sennaya_PloshchadII__SadovayaII", "!SadovayaII__SpasskayaII", "SadovayaII__ZvenigorodskayaII", "SadovayaII__Admiralteyskaya", }},
            {"SpasskayaII", new string[]{ "!Sennaya_PloshchadII__SpasskayaII", "!SadovayaII__SpasskayaII", "SpasskayaII__DostoyevskayaII", }},

        };


        Dictionary<string, PictureBox[]> picts = new Dictionary<string, PictureBox[]>();


        public Form1()
        {
            InitializeComponent();
        }


        public void Form1_Load(object sender, EventArgs e)
        {

            foreach (var pict in Controls.OfType<PictureBox>())
            {
                PictureBox Copy = Clone(pict);
                Copy.Name = pict.Name + "_copy";
                Copy.Visible = false;
                picts.Add(pict.Name, new PictureBox[] { pict, Copy });

                if (sosedi.ContainsKey(pict.Name))
                {

                    pict.Click += new EventHandler(clickingStation);
                    Copy.Click += new EventHandler(clickingStation);

                }




            }







        }

        public Dictionary<string, string[]> navigation(string start, string finish)
        {
            Dictionary<string, string[]> visid = new Dictionary<string, string[]>()
            {
                {start, new string[]{null} },
            };

            string station;
            Queue<string> queue_stations = new Queue<string>();
            for (int i = 0; i < sosedi[start].Length; i++)
            {
                queue_stations.Enqueue(sosedi[start][i]);
                string[] value = new string[] { start, puti[start][i] };
                visid.Add(sosedi[start][i], value);
            }
            while (queue_stations.Count > 0)
            {
                station = queue_stations.Dequeue();
                if (station == finish)
                {

                    break;
                }

                for (int i = 0; i < sosedi[station].Length; i++)
                    if (!visid.ContainsKey(sosedi[station][i]))
                    {
                        queue_stations.Enqueue(sosedi[station][i]);


                        
                        string[] value = new string[] { station, puti[station][i] };
                        visid.Add(sosedi[station][i], value);
                    }


            }

            VisidStation(visid,finish);
            return visid;
        }

        public void coloringPath(Dictionary<string, string[]> prohod, string finsih, string start)
        {
            doroga = new string[100];
            len_doroga = 0;
            string name;
            string[] stations = prohod[finsih];           
            PictureBox picture;
            timeCount = 0;
            while (true)
            {
                timeCount += TimeWay[stations[1]];
                
                if (stations[1].IndexOf("!")==-1)
                {
                    name = picts[stations[1]][0].Name;                    
                    picture = picts[stations[1]][1];
                    doroga[len_doroga] = name;                    
                    len_doroga += 1;                  
                    picts[stations[1]][0].Visible = false;
                    picture.Visible = true;
                    objectСoloring(picture, Color.Aqua);

                    if (stations[1].IndexOf("01")>-1)
                    {
                        string st = stations[1].Replace("01", "02");
                        name = picts[st][0].Name;
                        picture = picts[st][1];
                        doroga[len_doroga] = name;
                        len_doroga += 1;
                        picts[st][0].Visible = false;
                        picture.Visible = true;
                        objectСoloring(picture, Color.Aqua);
                    }
                }
                if (stations[0] == start)
                    break;

                name = picts[stations[0]][0].Name;
                picture = picts[stations[0]][1];
                doroga[len_doroga] = name;
                len_doroga += 1;
                picts[stations[0]][0].Visible = false;
                picture.Visible = true;
                objectСoloring(picture, Color.Aqua);
                stations = prohod[stations[0]];
            }
            aktivno = true;

            LabelTime.Text = ConvertTime(timeCount);

        }

        public void objectСoloring(PictureBox picture, Color ColorElement)
        {
            Bitmap bit = new Bitmap(picture.Image);

            for (int x = 0; x < bit.Width; x++)
            {
                for (int y = 0; y < bit.Height; y++)
                {
                    if (bit.GetPixel(x, y).Name != "0")
                    {
                        bit.SetPixel(x, y, ColorElement);
                    }



                }
            }
            picture.Image = bit;
        }
        public void clickingStation(object sender, EventArgs e)
        {
            if (aktivno)
            {
                foreach (string el in doroga)
                {
                    if (el is null)
                        break;

                    picts[el][1].Visible = false;
                    picts[el][0].Visible = true;

                }
                PictureBox picturee = (PictureBox)sender;
                if (picturee.Name.IndexOf("copy") == -1 || marshrut[0].Name == picturee.Name || marshrut[1].Name == picturee.Name)             
                    aktivno = false;
            }

            PictureBox picture = (PictureBox)sender;
            Color pointColor = Color.Green;
            if (aktivno)
            {
                picture = picts[picture.Name.Substring(0, picture.Name.Length - 5)][0];
                aktivno = false;
            }

            if (point1 && !point2)
            {

                pointColor = Color.Red;
                if (picture.Name.IndexOf("copy") > -1)
                {
                    picture.Visible = false;
                    point1 = false;
                    string nameOriginal = picture.Name.Substring(0, picture.Name.Length - 5);
                    picts[nameOriginal][0].Visible = true;

                    return;


                }
                else
                {

                    point2 = true;
                    marshrut[1] = picts[picture.Name][1];
                }

            }

            else if (point2)
            {

                if (picture.Name.IndexOf("copy") > -1)
                {
                    if (picture.Name == marshrut[0].Name)
                    {
                        marshrut[0] = marshrut[1];
                        objectСoloring(marshrut[0], Color.Green);
                    }



                    point2 = false;
                    picture.Visible = false;
                    string nameOriginal = picture.Name.Substring(0, picture.Name.Length - 5);
                    picts[nameOriginal][0].Visible = true;

                    return;



                }
                else
                {
                    marshrut[0].Visible = false;
                    string nameOriginal = marshrut[0].Name.Substring(0, marshrut[0].Name.Length - 5);
                    picts[nameOriginal][0].Visible = true;
                    marshrut[0] = marshrut[1];
                    objectСoloring(marshrut[0], Color.Green);
                    marshrut[1] = picts[picture.Name][1];
                    pointColor = Color.Red;

                }


            }

            else
            {
                point1 = true;
                marshrut[0] = picts[picture.Name][1];
            }




            picture.Visible = false;
            picts[picture.Name][1].Visible = true;

            objectСoloring(picts[picture.Name][1], pointColor);
            try
            {
                if (point1 && point2)
            {

                start = marshrut[0].Name.Substring(0, marshrut[0].Name.Length - 5);
                finish = marshrut[1].Name.Substring(0, marshrut[1].Name.Length - 5);
                coloringPath(navigation(start, finish), finish, start);
                



            }

         
                start = stations[marshrut[0].Name.Substring(0, marshrut[0].Name.Length - 5)][0];
                finish = stations[marshrut[1].Name.Substring(0, marshrut[1].Name.Length - 5)][0];

            }
            catch
            {

            }

           
        }

            public static T Clone<T>(T controlToClone) where T : Control
            {
                T instance = Activator.CreateInstance<T>();
                Type control = controlToClone.GetType();
                PropertyInfo[] info = control.GetProperties();
                object p = control.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, controlToClone, null);
                foreach (PropertyInfo pi in info)
                {
                    if ((pi.CanWrite) && !(pi.Name == "WindowTarget") && !(pi.Name == "Capture"))
                    {
                        pi.SetValue(instance, pi.GetValue(controlToClone, null), null);
                    }
                }
                return instance;
            }


      



        private void MarshrutLine(string[] naborStation, string finish,int countSt)
        {

            g = CreateGraphics();
            g.Clear(SystemColors.Control);
            int x = 50;
            int y = 80;
            int len = this.Width/naborStation.Length/3;
            int interval = 10;
            int range = 30;
            string station;
            for (int i = 0; i < countLabel; i++)
                this.Controls.Remove(masLabel[i]);

            countLabel = 0;
            for(int i=0;i<naborStation.Length;i++)
            {
                station = naborStation[i];
                Brush Colr = ColorsLine[stations[station][1]];
                g.FillEllipse(Colr, x, y - 5, 30, 30);
                Label stName = new Label();
                masLabel[countLabel] = stName;
                stName.Text = stations[station][0];
                stName.Location = new Point(x-10, y + 30);
                stName.AutoSize = false;
                stName.Size = new Size(73, 50);
                this.Controls.Add(stName);
                countLabel += 1;
                if (i == naborStation.Length - 1)
                    break;
                x += range + interval;
                string strA = String.Format("!{0}__{1}", naborStation[i], naborStation[i + 1]);
                string strB = String.Format("!{0}__{1}", naborStation[i + 1], naborStation[i]);
                if (TimeWay.ContainsKey(strA) || TimeWay.ContainsKey(strB))
                {
                    int ln = 5;
                    for (int l = 0; l <= len+len/5; l +=ln*3)
                    {                       
                        g.FillRectangle(Brushes.Black, x, y, ln, 20);
                        x+=ln*2;
                    }
                }
                else
                {
                   
                    g.FillRectangle(Colr, x, y, len, 20);
                    x = x + len + interval;

                }
                
                


            }


        }

        public void VisidStation(Dictionary<string, string[]> visid, string finish)
        {
            string[] wayStation = new string[50];
            int countStation=-1;

            string st = finish;
            while(st!=null)
            {
                countStation += 1;
                wayStation[countStation] = st;
                st = visid[st][0];
                
            }

            MarshrutLine(ReverseMass(wayStation, countStation), finish, countStation);

      
        }

        public string[] ReverseMass(string[] mas, int count)
        {
            string[] newMass = new string[count+1];
            for (int i = count; i >= 0; i--)
            {
                newMass[count - i] = mas[i];

            }

            return newMass;
        }


        public string ConvertTime(int minuts)
        {
            int hour = 0;
            string strTime;
            while (minuts >= 60)
            {
                minuts -= 60;
                hour += 1;
            }
            if (hour > 0)
                strTime = String.Format("{0} час {1} минут",hour, minuts);

            else
                strTime =  Convert.ToString(minuts + " минут");

            return strTime;
        }




    }
    } 

    