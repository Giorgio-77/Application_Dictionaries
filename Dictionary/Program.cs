using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Dictionaries
{
    class Program
    {
        public static void BinaryRead(DictCollection dictCollection)
        {
            using (FileStream fileStream = new FileStream(dictCollection.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    while (binaryReader.PeekChar() > -1)
                    {
                        string tmp_name = binaryReader.ReadString();
                        string tmp_path = binaryReader.ReadString();
                        if (File.Exists(tmp_path) && (new FileInfo(tmp_path).Length != 0))
                        {
                            Dictionary tmpDict = (Dictionary)XmlSerDeser.Deserialize("Dictionary", tmp_path);
                            tmpDict.Sort();
                            dictCollection.Add(tmpDict);
                        }
                        else
                            dictCollection.Add(new Dictionary(tmp_name));
                    }
                }
            }
        }

        public static void BinaryWrite(DictCollection dictCollection)
        {
            using (FileStream fileStream = new FileStream(dictCollection.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    foreach (var item in dictCollection.DictList)
                    {
                        binaryWriter.Write(item.Name);
                        binaryWriter.Write(item.Path);
                    }

                }
            }
        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            // Console.OutputEncoding = System.Text.Encoding.Default; - або так

            // Створюємо об*єкти
            DictCollection coll = new DictCollection();
            Log log = new Log();

            // Перевіряємо наявність файлів і виконуємо дії
            if (!Directory.Exists("Datas"))
            {
                Directory.CreateDirectory("Datas");
                File.Create(coll.Path).Close();
                File.Create(log.Path).Close();
            }
            else
            {
                if (!File.Exists(coll.Path))
                    File.Create(coll.Path).Close();
                else if (new FileInfo(coll.Path).Length != 0)
                    BinaryRead(coll);

                if (!File.Exists(log.Path))
                    File.Create(log.Path).Close();
                else if (new FileInfo(log.Path).Length != 0)
                    log = (Log)XmlSerDeser.Deserialize("Log", log.Path);
            }

            int inpInt = 0;
            string inpStr;
            int selMain = -1, selSub = -1;
            int d_pos, w_pos, v_pos;

            bool mainFlag, subFlag;

            do
            {
                mainFlag = subFlag = false;
                Console.Write("МЕНЮ ПЕРЕКЛАДАЧА\n");
                Console.WriteLine(new string('-', 25));
                Console.WriteLine("1 - Створити словник");
                Console.WriteLine("2 - Зайти в словник");
                Console.WriteLine("3 - Історія пошуку");
                Console.WriteLine("4 - Очистити історію");
                Console.WriteLine("0 - Вийти");
                Console.WriteLine(new string('-', 25));
                Console.Write(">>> ");
                selMain = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
                switch (selMain)
                {
                    case 0:
                        mainFlag = true;
                        break;
                    case 1:     // Створити словник
                        while (true)
                        {
                            Console.Write("\tНазва словника: ");
                            inpStr = Console.ReadLine();
                            if (!Regex.IsMatch(inpStr, new string(@"[a-zA-Zа-яА-Я*]")))
                            {
                                Console.WriteLine("\n\t\tНекоректий ввід, тільки букви!\n");
                                continue;
                            }
                            else
                                break;
                        }
                        if (inpStr == "*")
                        {
                            Console.Clear();
                            break;
                        }
                            
                        Console.WriteLine("\tУСПІШНО! Словник додано в колекцію, XML файл створено!\n");
                        coll.DictList.Add(new Dictionary(inpStr));
                        BinaryWrite(coll);
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case 2:     // Зайти в словник
                        while (true)
                        {
                            subFlag = false;
                            // Заходимо в словник
                            Console.Clear();
                            Console.WriteLine("Доступні словники\n" + new string('-', 20));
                            for (int i = 0; i < coll.Size; i++)
                            {
                                Console.Write($"{i+1}. {coll.DictList[i].Name}");
                                if (coll.DictList[i].ListWord.Count == 0)
                                    Console.WriteLine(" (empty)");
                                else
                                    Console.WriteLine();
                            }
                            Console.WriteLine("---\n0 - КРОК НАЗАД");
                            Console.WriteLine(new string('-', 20));
                            while (true)
                            {
                                Console.Write(">>> ");
                                inpStr = Console.ReadLine();
                                if (!Regex.IsMatch(inpStr, new string(@"[0-9]")))
                                {
                                    Console.WriteLine("\n\tНекоректий ввід, тільки цифри!\n");
                                    continue;
                                }
                                else
                                    break;
                            }
                            inpInt = Convert.ToInt32(inpStr);
                            if (inpInt == 0)
                            {
                                Console.Clear();
                                break;
                            }
                            d_pos = --inpInt;

                            // коли ми в словнику
                            while (true)
                            {
                                Console.Clear();
                                Console.Write($"МЕНЮ СЛОВНИКА {coll[d_pos].Name}\n") ;
                                Console.WriteLine(new string('-', 30));
                                Console.WriteLine("1 - Друкувати словник");
                                Console.WriteLine("2 - Додати слово");
                                Console.WriteLine("3 - Видалити слово");
                                Console.WriteLine("4 - Видалити переклад");
                                Console.WriteLine("5 - Шукати переклад");
                                Console.WriteLine("6 - Сортувати за зростанням");
                                Console.WriteLine("7 - Сортувати за спаданням");
                                Console.WriteLine("0 - КРОК НАЗАД");
                                Console.WriteLine(new string('-', 30));
                                Console.Write(">>> ");
                                selSub = Convert.ToInt32(Console.ReadLine());

                                switch (selSub)
                                {
                                    case 0:
                                        subFlag = true;
                                        Console.Clear();
                                        break;
                                    case 1:     //Друк словника
                                        Console.WriteLine();
                                        coll[d_pos].Sort();
                                        coll[d_pos].PrintGrouped();
                                        Console.Write("\n\nPress any key to continue...");
                                        Console.ReadKey();
                                        break;
                                    
                                    case 2:     // Додавання нового слова
                                        Word tmpWord = new Word();
                                        int tmpCount = 0;
                                        Console.Write("\n\tСлово: ");
                                        if ((inpStr = Console.ReadLine()) == "*")
                                            break;
                                        tmpWord.Key = inpStr;

                                        while (true)
                                        {
                                            ++tmpCount;
                                            Console.Write($"\tПереклад {tmpCount}: ");
                                            if ((inpStr = Console.ReadLine()) == "*")
                                                break;
                                            tmpWord.ListValue.Add(inpStr);
                                        }
                                        coll[d_pos].AddWord(tmpWord);
                                        coll[d_pos].Sort();
                                        XmlSerDeser.Serialize(coll[d_pos], coll[d_pos].Path);
                                        Console.WriteLine("\n\tУСПІШНО! Слово і його переклади додані в словник!\n");
                                        Console.Write("\nPress any key to continue...");
                                        Console.ReadKey();
                                        break;

                                    case 3:     // Видалити слово
                                        while (true)
                                        {
                                            Console.Write("\n\tСлово: ");
                                            inpStr = Console.ReadLine();
                                            if (!Regex.IsMatch(inpStr, new string(@"[a-zA-Zа-яА-Я*]")))
                                            {
                                                Console.WriteLine("\n\t\tНекоректий ввід, тільки букви!\n");
                                                continue;
                                            }
                                            else
                                                break;
                                        }
                                        if (inpStr == "*")
                                        {
                                            Console.Clear();
                                            break;
                                        }
                                        w_pos = coll[d_pos].ListWord.FindIndex(w => w.Key == inpStr);
                                        if (w_pos == -1)
                                        {
                                            Console.WriteLine("\n\tСлово не знайдено!\n");
                                            Console.Write("\nPress any key to continue...");
                                            Console.ReadKey();
                                            break;
                                        }
                                        coll[d_pos].DellWord(w_pos);

                                        XmlSerDeser.Serialize(coll[d_pos], coll[d_pos].Path);
                                        Console.WriteLine("\n\tУСПІШНО! Слово і його переклади видалені зі словника!\n");
                                        Console.Write("\nPress any key to continue...");
                                        Console.ReadKey();
                                        break;

                                    case 4:     // Видалити переклад
                                        while (true)
                                        {
                                            Console.Write("\n\tСлово: ");
                                            inpStr = Console.ReadLine();
                                            if (!Regex.IsMatch(inpStr, new string(@"[a-zA-Zа-яА-Я*]")))
                                            {
                                                Console.WriteLine("\n\t\tНекоректий ввід, тільки букви!\n");
                                                continue;
                                            }
                                            else
                                                break;
                                        }
                                        if (inpStr == "*")
                                        {
                                            Console.Clear();
                                            break;
                                        }
                                        w_pos = coll[d_pos].ListWord.FindIndex(w => w.Key == inpStr);
                                        if (w_pos == -1)
                                        {
                                            Console.WriteLine("\n\tСлово не знайдено!\n");
                                            Console.Write("\nPress any key to continue...");
                                            Console.ReadKey();
                                            break;
                                        }
                                        Console.WriteLine("\n\t" + coll[d_pos].ListWord[w_pos].Key + "\n\t" + new string('-', 20));
                                        for (int i = 0; i < coll[d_pos].ListWord[w_pos].ListValue.Count; i++)
                                        {
                                            Console.WriteLine($"\t {i+1}. {coll[d_pos].ListWord[w_pos].ListValue[i]}");
                                        }
                                        Console.WriteLine("\t" + new string('-', 20));
                                        while (true)
                                        {
                                            Console.Write("\t>>> ");
                                            inpStr = Console.ReadLine();
                                            if (!Regex.IsMatch(inpStr, new string(@"[0-9*]")))
                                            {
                                                Console.WriteLine("\n\t\tНекоректий ввід, тільки цифри!\n");
                                                continue;
                                            }
                                            else
                                                break;
                                        }
                                        if (inpStr == "*")
                                        {
                                            Console.Clear();
                                            break;
                                        }
                                        if (coll[d_pos].ListWord[w_pos].ListValue.Count == 1)
                                        {
                                            coll[d_pos].DellWord(w_pos);
                                            XmlSerDeser.Serialize(coll[d_pos], coll[d_pos].Path);
                                            Console.WriteLine("\n\tУСПІШНО! Слово і його єдиний переклад видалені зі словника!\n");
                                        }
                                        else
                                        {
                                            v_pos = Convert.ToInt32(inpStr) - 1;
                                            coll[d_pos].ListWord[w_pos].ListValue.RemoveAt(v_pos);
                                            XmlSerDeser.Serialize(coll[d_pos], coll[d_pos].Path);
                                            Console.WriteLine("\n\tУСПІШНО! Один з перекладів успішно видалений!\n");
                                        }
                                        Console.Write("\nPress any key to continue...");
                                        Console.ReadKey();
                                        break;

                                    case 5:     // шукати слово
                                        while (true)
                                        {
                                            Console.Write("\n\tСлово: ");
                                            inpStr = Console.ReadLine();
                                            if (!Regex.IsMatch(inpStr, new string(@"[a-zA-Zа-яА-Я*]")))
                                            {
                                                Console.WriteLine("\n\t\tНекоректий ввід, тільки букви!");
                                                continue;
                                            }
                                            else
                                                break;
                                        }
                                        if (inpStr == "*")
                                        {
                                            Console.Clear();
                                            break;
                                        }

                                        // Log
                                        log.Add(new LogString(DateTime.Now, inpStr, coll[d_pos].Name));
                                        XmlSerDeser.Serialize(log, log.Path);

                                        var result = from item in coll[d_pos].ListWord
                                                     where item.Key == inpStr
                                                     select item;
                                        if (result.Count() == 0)
                                            Console.WriteLine("\n\tНЕ ЗНАЙДЕНО!\n");

                                        foreach (var item in result)
                                        {
                                            Console.WriteLine("\n\b\b" + item + "\n");
                                        }

                                        Console.Write("\nPress any key to continue...");
                                        Console.ReadKey();
                                        break;
                                    case 6:
                                        result = from item in coll[d_pos].ListWord
                                                 orderby item
                                                 select item;

                                        Console.WriteLine();

                                        Console.WriteLine($"\tDictionary {coll[d_pos].Name}\n" + "\t" + new string('-', 40));
                                        char tmp = '0';
                                        foreach (var item in result)
                                        {
                                            if (item == result.First() || tmp != item.Key[0])
                                            {
                                                tmp = item.Key[0];
                                                Console.WriteLine("\t  " + tmp.ToString().ToUpper());
                                            }

                                            Console.WriteLine(item.ToString());
                                        }

                                        Console.Write("\n\nPress any key to continue...");
                                        Console.ReadKey();
                                        break;
                                    case 7:
                                        result = from item in coll[d_pos].ListWord
                                                 orderby item descending
                                                 select item;

                                        Console.WriteLine();

                                        Console.WriteLine($"\tDictionary {coll[d_pos].Name}\n" + "\t" + new string('-', 40));
                                        tmp = '0';
                                        foreach (var item in result)
                                        {
                                            if (item == result.First() || tmp != item.Key[0])
                                            {
                                                tmp = item.Key[0];
                                                Console.WriteLine("\t  " + tmp.ToString().ToUpper());
                                            }

                                            Console.WriteLine(item.ToString());
                                        }

                                        Console.Write("\n\nPress any key to continue...");
                                        Console.ReadKey();
                                        break;
                                }
                                BinaryWrite(coll);

                                if (subFlag)
                                    break;
                            }
                        }
                        break;

                    case 3:
                        log.Print();
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 4:
                        log.List.Clear();
                        File.WriteAllText(log.Path, String.Empty);
                        Console.WriteLine("\tУСПІШНО! Iторія пошуку очищена!\n");
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }

                Console.WriteLine();

                if (mainFlag)
                    break;

            } while (true);

            //Console.Write("Press any key to continue...");
            //Console.ReadKey();
        }
    }
}
